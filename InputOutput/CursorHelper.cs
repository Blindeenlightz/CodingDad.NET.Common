using Microsoft.Win32.SafeHandles;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brushes = System.Windows.Media.Brushes;
using FontFamily = System.Windows.Media.FontFamily;
using Point = System.Windows.Point;

namespace CodingDad.NET.Common.InputOutput
{
    /// <summary>
    /// Provides utility methods for creating custom cursors in WPF applications.
    /// </summary>
    public class CursorHelper
    {
        /// <summary>
        /// Creates a cursor containing a given string.
        /// </summary>
        /// <param name="cursorText">The string for the cursor to contain.</param>
        /// <param name="pixelPerDip">The result of calling VisualTreeHelper.GetDpi(this).PixelsPerDip.</param>
        /// <returns>A formatted cursor containing the given string.</returns>
        public static Cursor CreateStringCursor (string cursorText, double pixelPerDip)
        {
            // Text to render
            FormattedText fmtText = new(
                cursorText,
                new CultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface(
                    new FontFamily("Arial"),
                    FontStyles.Normal,
                    FontWeights.Normal,
                    new FontStretch()),
                12.0,  // FontSize
                Brushes.Black,
                pixelPerDip);

            // The Visual to use as the source of the RenderTargetBitmap.
            DrawingVisual drawingVisual = new();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawText(fmtText, new Point());
            drawingContext.Close();

            // The BitmapSource that is rendered with a Visual.
            RenderTargetBitmap rtb = new(
                (int)drawingVisual.ContentBounds.Width,
                (int)drawingVisual.ContentBounds.Height,
                96,   // dpiX
                96,   // dpiY
                PixelFormats.Pbgra32);
            rtb.Render(drawingVisual);

            // Encoding the RenderBitmapTarget into a bitmap (as PNG)
            PngBitmapEncoder encoder = new();
            encoder.Frames.Add(BitmapFrame.Create(rtb));

            using var ms = new MemoryStream();
            encoder.Save(ms);
            using var bmp = new System.Drawing.Bitmap(ms);
            return InternalCreateCursor(bmp);
        }

        /// <summary>
        /// Creates a cursor containing an image that represents the element being dragged.
        /// </summary>
        /// <param name="element">The UIElement to create an image representation in the cursor of.</param>
        /// <returns>A cursor containing an image of the given element.</returns>
        public static Cursor CreateUIElementCursor (UIElement element)
        {
            element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            element.Arrange(new Rect(new Point(), element.DesiredSize));

            RenderTargetBitmap rtb = new(
                (int)element.DesiredSize.Width,
                (int)element.DesiredSize.Height,
                96,   // dpiX
                96,   // dpiY
                PixelFormats.Pbgra32);

            rtb.Render(element);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));

            using var ms = new MemoryStream();
            encoder.Save(ms);
            using var bmp = new System.Drawing.Bitmap(ms);
            return InternalCreateCursor(bmp);
        }

        private static Cursor InternalCreateCursor (System.Drawing.Bitmap bmp)
        {
            var iconInfo = new NativeMethods.IconInfo();
            NativeMethods.GetIconInfo(bmp.GetHicon(), ref iconInfo);

            iconInfo.xHotspot = 0;
            iconInfo.yHotspot = 0;
            iconInfo.fIcon = false;

            SafeIconHandle cursorHandle = NativeMethods.CreateIconIndirect(ref iconInfo);
            return CursorInteropHelper.Create(cursorHandle);
        }

        private static class NativeMethods
        {
            [DllImport("user32.dll")]
            public static extern SafeIconHandle CreateIconIndirect (ref IconInfo icon);

            [DllImport("user32.dll")]
            public static extern bool DestroyIcon (nint hIcon);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetIconInfo (nint hIcon, ref IconInfo pIconInfo);

            public struct IconInfo
            {
                public bool fIcon;
                public nint hbmColor;
                public nint hbmMask;
                public int xHotspot;
                public int yHotspot;
            }
        }

        private class SafeIconHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            public SafeIconHandle ()
                : base(true)
            {
            }

            protected override bool ReleaseHandle ()
            {
                return NativeMethods.DestroyIcon(handle);
            }
        }
    }
}