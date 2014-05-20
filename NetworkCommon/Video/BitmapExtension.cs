using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.IO;

namespace CommonUtils.Video
{
    [StructLayout(LayoutKind.Explicit)]
    public struct PixelData
    {
        [FieldOffset(0)]
        public byte blue;
        [FieldOffset(1)]
        public byte green;
        [FieldOffset(2)]
        public byte red;
    }

    /// <summary>
    /// this are the extensions to the Bitmap class and
    /// provides a way to access unsafely (fast!) the bitmap data
    /// </summary>
    public static class BitmapExtensions
    {

        public static byte[] ToByteArray(this Bitmap bitmap, ImageFormat imageFormat)
        {
            byte[] byteBitmap;

            using (MemoryStream memoryStream = new MemoryStream())
            {

                //BinaryFormatter binaryFormatter = new BinaryFormatter();
                //binaryFormatter.Serialize(memoryStream, bitmapFragment);
                bitmap.Save(memoryStream, imageFormat);
                byteBitmap = memoryStream.ToArray();
            }
            return byteBitmap;
        }

        private static Point PixelSize(this Bitmap bitmap)
        {
                GraphicsUnit unit = GraphicsUnit.Pixel;
                RectangleF bounds = bitmap.GetBounds(ref unit);

                return new Point((int)bounds.Width, (int)bounds.Height);
        }

        public unsafe static void LockBitmap(this Bitmap bitmap, out BitmapData bitmapData, out Byte* pBase, out int width)
        {
            
            GraphicsUnit unit = GraphicsUnit.Pixel;
            RectangleF boundsF = bitmap.GetBounds(ref unit);
            Rectangle bounds = new Rectangle((int)boundsF.X,
            (int)boundsF.Y,
            (int)boundsF.Width,
            (int)boundsF.Height);

            // Figure out the number of bytes in a row
            // This is rounded up to be a multiple of 4
            // bytes, since a scan line in an image must always be a multiple of 4 bytes
            // in length.
            width = (int)boundsF.Width * Marshal.SizeOf(typeof(PixelData));
            if (width % 4 != 0)
            {
                width = 4 * (width / 4 + 1);
            }
            bitmapData = bitmap.LockBits(bounds, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            pBase = (Byte*)bitmapData.Scan0.ToPointer();

        }

        public unsafe static PixelData GetPixel(int x, int y, Byte* pBase, int width)
        {
            PixelData returnValue = *PixelAt(x, y,pBase,width);
            return returnValue;
        }

        public unsafe static void SetPixel(int x, int y, Byte* pBase, int width, PixelData colour)
        {
            PixelData* pixel = PixelAt(x, y,pBase,width);
            *pixel = colour;
        }

        public unsafe static void UnlockBitmap(this Bitmap bitmap, ref BitmapData bitmapData, ref Byte* pBase)
        {
            bitmap.UnlockBits(bitmapData);
            bitmapData = null;
            pBase = null;
        }

        public unsafe static PixelData* PixelAt(int x, int y, Byte* pBase, int width)
        {
            return (PixelData*)(pBase + y * width + x * sizeof(PixelData));
        }

    }

}

