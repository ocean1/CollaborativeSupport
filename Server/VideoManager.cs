using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using CommonUtils;
using CommonUtils.Network.Packets;
using CommonUtils.Video;
using System.IO;
using Logger;

namespace Server
{
    public class VideoManager
    {

        public struct BitmapStatus
        {
            public Bitmap bitmap;
            public BitmapData bitmapData;
        }

        const int numOfSlices = 2;
        const int minSliceSize = 16;

        public delegate void SendDelegate(Packet Packet);
        public event EventHandler WindowClosedEvent;

        /// <summary>
        /// this delegate is for when the worker threads have completed and want to send the fragments list
        /// </summary>

        public SendDelegate SendPacket;

        private bool _disposed;
        private readonly object _lock = new object();

        private IntPtr hWnd = IntPtr.Zero; // contains the hwnd of the window (hWnd=0 -> get desktop)
        private Rectangle srcRect = Rectangle.Empty; // contain the rect for the source hdc, for selecting part of the desktop
        private BitmapStatus lastbs; // contains the last screenshot to make differences
        private BitmapStatus currentbs; // current screenshot
        private Size sliceSize;

        private Point mousePos;
        private int tasksPending;
        private List<Rectangle> videoFragmentsRegions;
        private PCQueue tasksQueue;

        public VideoManager(IntPtr hWnd)
        {
            this.hWnd = hWnd;

            if (hWnd != IntPtr.Zero)
            {
                NativeMethods.RECT r;
                bool result = NativeMethods.GetWindowRect(hWnd, out r);
                this.srcRect = r;

            }
            else
            {
                // se hWnd=null e non è stata selezionata alcuna rect seleziona l'intero schermo (primario)!
                if (srcRect == Rectangle.Empty) srcRect = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            }

            if (srcRect != Rectangle.Empty) sliceSize = CalculateSliceSize(srcRect);

            videoFragmentsRegions = new List<Rectangle>(16);

            tasksQueue = new PCQueue(4); // we want 4 workers

            lastbs.bitmap = null;
            currentbs.bitmap = null;
        }

        public VideoManager(IntPtr hWnd, Rectangle rect)
            : this(hWnd)
        {
            srcRect = rect;
            sliceSize = CalculateSliceSize(srcRect);
        }

        private static Size CalculateSliceSize(Rectangle srcRect)
        {
            // adding the remainder we are sure we won't have more than 16 slices :)
            int h = srcRect.Height / numOfSlices + srcRect.Height % numOfSlices;
            int w = srcRect.Width / numOfSlices + srcRect.Width % numOfSlices;

            // we want to force slices of at least 16x16 pixels
            // this way we avoid problems analyzing really small bitmaps
            // (smaller than 4x4 pixels where w/h could be 0)
            // and we avoid queueing a lot of "tasks" to our worker threads
            // with a really small area to analyze
            h = h > minSliceSize ? h : minSliceSize;
            w = w > minSliceSize ? w : minSliceSize;

            return new Size(w, h);
        }

        private Point GetRelativeMousePos()
        {

            Point position;

            //update the rect should the window have been moved
            if (hWnd != IntPtr.Zero && !IsWindowFocused(hWnd))
            {
                return Point.Empty;
            }

            position = System.Windows.Forms.Control.MousePosition;

            if ((position.X > srcRect.X && position.Y > srcRect.Y &&
                position.X < srcRect.Right && position.Y < srcRect.Bottom))
            {
                //normalize mouse position
                position.X -= srcRect.X;
                position.Y -= srcRect.Y;
            }
            else
            {
                position = Point.Empty;
            }

            return position;

        }

        public Bitmap GetScreenshot()
        {

            Bitmap bitmap = new Bitmap(srcRect.Width, srcRect.Height, PixelFormat.Format24bppRgb);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                #region commented out code
                /*
                //graphics.SmoothingMode = SmoothingMode.HighSpeed;
                //graphics.InterpolationMode = InterpolationMode.Low;
                //graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                //graphics.CompositingQuality = CompositingQuality.HighSpeed;

                // now take the source HDC from the hWnd
                //IntPtr srcHDC = NativeMethods.GetDC(hWnd);
                //IntPtr dstHDC = graphics.GetHdc();

                NativeMethods.BitBlt(
                    dstHDC,
                    0,
                    0,
                    srcRect.Width,
                    srcRect.Height,
                    srcHDC,
                    (hWnd != IntPtr.Zero) ? 0 : srcRect.X,
                    (hWnd != IntPtr.Zero) ? 0 : srcRect.Y,
                    NativeMethods.TernaryRasterOperations.SRCCOPY);
                 */
                #endregion

                if (hWnd != IntPtr.Zero)
                {
                    IntPtr hdcBitmap = graphics.GetHdc();
                    NativeMethods.PrintWindow(hWnd, hdcBitmap, 0);
                    graphics.ReleaseHdc(hdcBitmap);
                }
                else
                {
                    graphics.CopyFromScreen(srcRect.Location, new Point(0, 0), srcRect.Size);
                }

                //NativeMethods.ReleaseDC(hWnd, srcHDC);
                //graphics.ReleaseHdc(dstHDC);
            }

            return bitmap;
        }

        private bool IsWindowFocused(IntPtr hWnd)
        {
            // some times we could have problems with child windows since we have decided
            // to always get the parent window, find it and see if it has focus
            IntPtr fgHwnd = NativeMethods.GetForegroundWindow();

            while (true)
            {
                IntPtr fgHwnd2 = NativeMethods.GetParent(fgHwnd);
                if (fgHwnd2 == null || fgHwnd2 == IntPtr.Zero) break;
                fgHwnd = fgHwnd2;
            }

            return (hWnd == fgHwnd);

        }

        /// <summary>
        /// this method represents the task that is executed by the workers in the workers thread queue
        /// and is enqueued
        /// </summary>
        /// <param name="b1">the first bitmap</param>
        /// <param name="b2">the second bitmap</param>
        /// <param name="area">the area in which we are interested in to make the diff</param>
        /// <returns></returns>
        private unsafe void DiffAndCreateFragment(Rectangle area)
        {

            bool result = false;
            int left = int.MaxValue;
            int right = int.MinValue;
            int top = int.MaxValue;
            int bottom = int.MinValue;

            // after some initialization frenzy let's get to some actual businness
            // calculate the pointer to actual data

            // the stride represents the size of an actual row of the bitmap (padding included)
            // we need it to calculate the position of the next row

            unchecked
            {
                for (int y = area.Top; y < area.Bottom; y++)
                {
                    // we want some supa-optimized code, read the pixel as a 4 byte word
                    byte* p1 = (((byte*)lastbs.bitmapData.Scan0) + area.Left * 3 + (y * lastbs.bitmapData.Stride));
                    byte* p2 = (((byte*)currentbs.bitmapData.Scan0) + area.Left * 3 + (y * currentbs.bitmapData.Stride));
                    //PixelData* p1 = (PixelData*)(lastbs.bitmapData.Scan0 + (y * lastbs.bitmapData.Stride));
                    //PixelData* p2 = (PixelData*)(currentbs.bitmapData.Scan0 + (y * currentbs.bitmapData.Stride));
                    for (int x = area.Left; x < area.Right; x++)
                    {
                        //if( p1->blue != p2->blue || p1->red != p2->red || p1->green != p2->green )
                        if ((*(Int32*)(p1) & 0x00FFFFFF) != ((*(Int32*)p2) & 0x00FFFFFF))
                        {
                            if (x < left) left = x;
                            if (x > right) right = x;
                            if (y < top) top = y;
                            if (y > bottom) bottom = y;
                            result = true;
                        }

                        p1 += 3; // 3 bytes for 24bpp RGB format
                        p2 += 3;

                    }
                }
            }

            lock (_lock)
            {
                // if differences have been found create the new fragment and add it to the list
                if (result == true)
                {
                    Rectangle vfarea = new Rectangle(left, top, right - left + 1, bottom - top + 1);
                    videoFragmentsRegions.Add(vfarea);
                    //System.Diagnostics.Debug.Print("fragment added: " + vfarea.ToString());
                }

                // the task is ended
                tasksPending--;
                // if _tasksPending is 0 this was the last one and can send a new video packet with all the fragments
                if (tasksPending == 0)
                {
                    // unlocking bitmaps
                    lastbs.bitmap.UnlockBits(lastbs.bitmapData);
                    currentbs.bitmap.UnlockBits(currentbs.bitmapData);
                    lastbs.bitmapData = null;
                    currentbs.bitmapData = null;

                    List<VideoFragment> vfs = new List<VideoFragment>();
                    if (videoFragmentsRegions.Count > 0)
                    {

                        foreach (Rectangle region in videoFragmentsRegions)
                        {
                            using (Bitmap fragmentBitmap = new Bitmap(region.Width, region.Height))
                            using (Graphics fragmentGraphics = Graphics.FromImage(fragmentBitmap))
                            {
                                fragmentGraphics.SmoothingMode = SmoothingMode.HighSpeed;
                                fragmentGraphics.InterpolationMode = InterpolationMode.Low;
                                fragmentGraphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                                fragmentGraphics.CompositingQuality = CompositingQuality.HighSpeed;
                                // draw the portion of screenshot in the fragment we will send
                                fragmentGraphics.DrawImage(currentbs.bitmap, 0, 0, region, GraphicsUnit.Pixel);

                                VideoFragment vf = new VideoFragment(fragmentBitmap.ToByteArray(ImageFormat.Jpeg), region);
                                vfs.Add(vf);
                            }
                        }
                        VideoPacket vp = new VideoPacket(vfs.ToArray<VideoFragment>(), mousePos, false);
                        SendPacket(vp);
                    }
                    else
                    {
                        SendPacket(new VideoPacket(new VideoFragment[0], mousePos, false));
                        //System.Diagnostics.Debug.Print("sent only mouspos!");
                    }


                    lastbs.bitmap.Dispose(); // dispose the last screenshot we don't need it anymore
                    lastbs.bitmap = currentbs.bitmap; // substitute it by our new screen
                }
            }

        }

        public void ForceSendNewImage()
        {
            // null lastbs.bitmap this way we force the send of a new image
            lock (_lock)
            {
                lastbs.bitmap.Dispose();
                lastbs.bitmap = null;
            }
        }

        public bool TrySendVideo()
        {

            // se tutti i task sono stati eseguiti possiamo crearne di nuovi
            // altrimenti semplicemente questo frame viene ignorato
            lock (_lock)
            {
                if (tasksPending != 0) return false;

                //window closed? stop here!
                if (hWnd != IntPtr.Zero && !NativeMethods.IsWindow(hWnd))
                {
                    if(WindowClosedEvent!=null)
                        WindowClosedEvent(this, new EventArgs());
                    return true;
                }

                currentbs.bitmap = GetScreenshot();
                checkSourceRect();
                mousePos = GetRelativeMousePos();

                if (lastbs.bitmap != null)
                {

                    videoFragmentsRegions.Clear();

                    // otteniamo la region e finchè siamo nei bound della bitmap mettiamo in queue il task
                    GraphicsUnit unit = GraphicsUnit.Pixel;
                    RectangleF boundsF = lastbs.bitmap.GetBounds(ref unit);
                    Rectangle bounds = new Rectangle((int)boundsF.X, (int)boundsF.Y, (int)boundsF.Width, (int)boundsF.Height);

                    boundsF = currentbs.bitmap.GetBounds(ref unit);
                    Rectangle bounds2 = new Rectangle((int)boundsF.X, (int)boundsF.Y, (int)boundsF.Width, (int)boundsF.Height);

                    if (bounds.Height != bounds2.Height || bounds.Width != bounds2.Width)
                    {
                        //check needed when doing a windows resize!
                        lastbs.bitmap.Dispose();
                        lastbs.bitmap = null;
                        // a goto could also be used here
                    }
                    else
                    {
                        lastbs.bitmapData = lastbs.bitmap.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                        currentbs.bitmapData = currentbs.bitmap.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                        tasksPending = 0;

                        for (int x = 0; x < srcRect.Width; x += sliceSize.Width)
                        {
                            int w = sliceSize.Width;
                            if (x + w > srcRect.Width) w = srcRect.Width - x;
                            for (int y = 0; y < srcRect.Height; y += sliceSize.Height)
                            {

                                int h = sliceSize.Height;
                                if (y + h > srcRect.Height) h = srcRect.Height - y;

                                tasksPending++;
                                Rectangle area = new Rectangle(x, y, w, h);
                                tasksQueue.EnqueueItem(() => DiffAndCreateFragment(area));
                                //System.Diagnostics.Debug.Print("------- frammento da analizzare messo in coda: " + area);
                            }
                        }
                    }
                }

                if (lastbs.bitmap == null)
                {
                    using (Bitmap fragmentBitmap = new Bitmap(currentbs.bitmap))
                    using (Graphics fragmentGraphics = Graphics.FromImage(fragmentBitmap))
                    {
                        GraphicsUnit unit = GraphicsUnit.Pixel;
                        fragmentGraphics.SmoothingMode = SmoothingMode.HighSpeed;
                        fragmentGraphics.InterpolationMode = InterpolationMode.Low;
                        fragmentGraphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                        fragmentGraphics.CompositingQuality = CompositingQuality.HighSpeed;
                        
                        // draw the portion of screenshot in the fragment we will send
                        fragmentGraphics.DrawImage(currentbs.bitmap, 0, 0, currentbs.bitmap.GetBounds(ref unit), GraphicsUnit.Pixel);
                        VideoFragment vf = new VideoFragment(currentbs.bitmap.ToByteArray(ImageFormat.Jpeg), srcRect);
                        SendPacket(new VideoPacket(vf, mousePos, true));
                    }

                    lastbs.bitmap = currentbs.bitmap;
                }

            }

            return true;
        }

        private void checkSourceRect()
        {
            if (hWnd != IntPtr.Zero)
            {
                NativeMethods.RECT r;
                bool result = NativeMethods.GetWindowRect(hWnd, out r);

                Rectangle newRect = r;
                if (srcRect.Width != newRect.Width || srcRect.Height != newRect.Height)
                {
                    //force the sending of new image if bounds have changed
                    lastbs.bitmap.Dispose();
                    lastbs.bitmap = null;
                }

                srcRect = newRect; // assign to srcRect so relative mouse position will always be right
            }
        }


        #region IDisposable implementation

        public void Dispose()
        {
            Dispose(true); // dispose managed and unmanaged rosources alike!
            GC.SuppressFinalize(this); // avoid to execute finalization of this class from happening two times
            //base.Dispose();
        }

        /// <summary>
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If disposing equals false, the method has been called by the 
        /// runtime from inside the finalizer and you should not reference
        /// other objects. Only unmanaged resources can be disposed.
        /// </summary>
        /// <param name="disposing">tells if the method has been called directly (true)
        /// or indirectly by the runtime (false)</param>
        protected void Dispose(bool disposing)
        {
            lock (_lock)
            {
                if (_disposed == false)
                {
                    // If disposing equals true, dispose all managed 
                    // and unmanaged resources.
                    if (disposing)
                    {
                        // clean up managed resources here
                    }

                    tasksQueue.Close(true);
                }
                _disposed = true;
            }
        }

        ~VideoManager()
        {
            Dispose(false);
        }
        #endregion



    }
}
