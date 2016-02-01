using System;
using System.Drawing;

using stdole;

namespace Loki.UI.Office
{
    internal class PictureDispImpl : IPictureDisp, IPicture
    {
        #region Init

        private readonly Bitmap image;

        private IntPtr handle;

        public PictureDispImpl(Bitmap image)
        {
            this.image = image;
        }

        ~PictureDispImpl()
        {
            if (handle != IntPtr.Zero)
            {
                NativeMethods.DeleteObject(handle);
            }
        }

        #endregion Init

        #region IPictureDisp Members

        public int Width
        {
            get { return image.Width; }
        }

        public int Height
        {
            get { return image.Height; }
        }

        public short Type
        {
            get { return 1; }
        }

        public int Handle
        {
            get
            {
                if (handle == IntPtr.Zero)
                {
                    handle = image.GetHbitmap();
                }

                return handle.ToInt32();
            }
        }

        public int hPal
        {
            get { return 0; }

            set { }
        }

        public void Render(int hdc, int x, int y, int cx, int cy, int xSrc, int ySrc, int cxSrc, int cySrc, IntPtr prcWBounds)
        {
            Graphics graphics = Graphics.FromHdc(new IntPtr(hdc));

            graphics.DrawImage(image, new Rectangle(x, y, cx, cy), xSrc, ySrc, cxSrc, cySrc, GraphicsUnit.Pixel);
        }

        #endregion IPictureDisp Members

        #region IPicture Members

        public int Attributes
        {
            get { return 0; }
        }

        public int CurDC
        {
            get { return 0; }
        }

        public bool KeepOriginalFormat
        {
            get { return false; }

            set { }
        }

        public void PictureChanged()
        {
        }

        public void SaveAsFile(IntPtr pstm, bool fSaveMemCopy, out int pcbSize)
        {
            pcbSize = 0;
        }

        public void SelectPicture(int hdcIn, out int phdcOut, out int phbmpOut)
        {
            phdcOut = 0;

            phbmpOut = 0;
        }

        public void SetHdc(int hdc)
        {
        }

        #endregion IPicture Members
    }
}