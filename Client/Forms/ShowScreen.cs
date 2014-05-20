using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonUtils.Network.Packets;
using CommonUtils.Video;
using CommonUtils.Network;
using Logger;

namespace Client
{
    public partial class ShowScreen : Form
    {

        private Bitmap screenshot = null;
        private Bitmap screenshotWithCursor = null;

        public delegate void ShowVideoDelegate(VideoPacket vp);

        public void ShowVideo(VideoPacket vp)
        {

            bool IsNewImage = vp.IsNewImage;
            VideoFragment[] videoFragments = vp._vFragments;

            if (videoFragments == null)
            {
                MyLogger.Instance.AddError("Error, received video packet with null videofragment array", logLevel.ERROR);
                return;
            }

            if (this.screenPictureBox.InvokeRequired)
            {
                // This is a worker thread so delegate the task.
                this.screenPictureBox.BeginInvoke(new ConnectionManager.ShowVideoDelegate(this.ShowVideo), vp);
                return;
            }

            // This is the UI thread so perform the task.
            //this.screenPictureBox....
            if (IsNewImage)
            {
                using (MemoryStream memoryStream = new MemoryStream(videoFragments[0].screenShot))
                {
                    if (screenshot != null) screenshot.Dispose();
                    screenshot = new Bitmap(memoryStream);
                    int diffh = this.screenPictureBox.Size.Height - screenshot.Size.Height;
                    int diffw = this.screenPictureBox.Size.Width - screenshot.Size.Width;
                    this.screenPictureBox.Size = screenshot.Size;
                    this.ClientSize = screenshot.Size;
                }
            }
            else
            {
                // maybe we didn't receive the first screenshot during transmission
                if (screenshot != null)
                {
                    foreach (VideoFragment vf in videoFragments)
                    {
                        using (MemoryStream memoryStream = new MemoryStream(vf.screenShot))
                        using (Bitmap fragment = new Bitmap(memoryStream))
                        using (Graphics graphics = Graphics.FromImage(screenshot))
                        {
                            // il frammento viene disegnato sul rettangolo indicato (DiffRect), che avrà 
                            // dimensioni relative all'immagine totale, da dall'immagine mandata nel byte array (new Rect..)
                            graphics.DrawImage(
                                fragment,
                                vf.diffRectangle,
                                new Rectangle(0, 0, vf.diffRectangle.Width, vf.diffRectangle.Height),
                                GraphicsUnit.Pixel);
                        }

                    }
                }
            }

            // ensure we have already got the whole image at first
            // to update 
            if (screenshot != null)
            {
                if (screenshotWithCursor != null)
                    screenshotWithCursor.Dispose();

                screenshotWithCursor = new Bitmap(screenshot);
                using (Graphics graphics = Graphics.FromImage(screenshotWithCursor))
                {
                    //System.Diagnostics.Debug.Print("MousePos " + vp.cursorPos.ToString());
                    if (vp.cursorPos != Point.Empty && vp.cursorPos.X < screenshot.Width && vp.cursorPos.Y < screenshot.Height)
                        System.Windows.Forms.Cursors.Default.Draw(graphics, new Rectangle(vp.cursorPos, System.Windows.Forms.Cursor.Current.Size));
                }

                this.screenPictureBox.Invalidate();
            }
        }



        public ShowScreen()
        {
            InitializeComponent();
        }

        private void screenPictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (screenshotWithCursor != null)
            {
                if (screenPictureBox.Image != null)
                    screenPictureBox.Image.Dispose();
                screenPictureBox.Image = new Bitmap(screenshotWithCursor);
            }
        }

        private void ShowScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel=true;
        }
    }
}
