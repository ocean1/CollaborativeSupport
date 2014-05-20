using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CommonUtils.Video
{
    [Serializable]
    public class VideoFragment
    {
        public byte[] screenShot;
        public Rectangle diffRectangle;

        public VideoFragment(byte[] screenShot, Rectangle diffRectangle)
        {
            // TODO: Complete member initialization
            this.screenShot = screenShot;
            this.diffRectangle = diffRectangle;
        }

    }
}
