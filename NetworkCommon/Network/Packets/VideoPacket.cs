using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;
using CommonUtils.Video;

namespace CommonUtils.Network.Packets
{
    [Serializable]
    public class VideoPacket : Packet
    {
        public VideoFragment[] _vFragments;
        private bool _newImage;

        // cursor position in the screenshot
        // if (-1,-1) cursor position is not valid
        public Point cursorPos = Point.Empty;

        public bool IsNewImage
        {
            get { return _newImage; }
        }

        public VideoFragment[] videoFragments{
            get {return _vFragments;}
        }

        private VideoPacket(Point cursorPos, bool newImage):base(Command.Video)
        {
            this.cursorPos = cursorPos;
            this._newImage = newImage;
        }

        /// <summary>
        /// This class provides support for the video packet
        /// </summary>
        /// <param name="screenshot">the screenshot in a byte array</param>
        /// <param name="diffRect">the rect which tells where this piece of screenshot is to be placed in the actual bitmap</param>
        /// <param name="newImage">tells if it is a differential packet or a whole new image</param>
        public VideoPacket(VideoFragment[] vFragments, bool newImage)
            : this(vFragments,Point.Empty, newImage)
        {
        }

        public VideoPacket(VideoFragment vFragment, bool newImage)
            : this(vFragment,Point.Empty,newImage)
        {
        }

        public VideoPacket(VideoFragment[] vFragments, Point cursorPos, bool newImage)
            : this(cursorPos, newImage)
        {
            _vFragments = vFragments;
        }


        public VideoPacket(VideoFragment vFragment, Point cursorPos, bool newImage)
            : this(cursorPos, newImage)
        {
            _vFragments = new VideoFragment[1];
            _vFragments[0] = vFragment;
        }

    }
}
