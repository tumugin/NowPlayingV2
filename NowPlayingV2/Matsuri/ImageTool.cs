using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NowPlayingV2.Matsuri
{
    public static class ImageTool
    {
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public static BitmapSource ToImageSource(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }

        public static string GetFileTypeFromBytes(byte[] image)
        {
            if (image.Take(3).ToArray().Equals(new byte[] { 0xFF, 0xD8, 0xFF }))
            {
                return "jpeg";
            }
            if (image.Take(8).ToArray().Equals(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }))
            {
                return "png";
            }
            if (image.Take(4).ToArray().Equals(new byte[] { 0x47, 0x49, 0x46 }))
            {
                return "gif";
            }
            if (image.Take(2).ToArray().Equals(new byte[] { 0x4D, 0x42 }))
            {
                return "bmp";
            }
            return "";
        }
    }
}
