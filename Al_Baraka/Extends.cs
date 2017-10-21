using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ResizeImageASPNETCore
{
    public static class Extends
    {
        public static Image Resize(this Image current, int maxWidth, int maxHeight)
        {
            int width, height;
            if (Array.IndexOf(current.PropertyIdList, 274) > -1)
                {
                    var orientation = (int)current.GetPropertyItem(274).Value[0];
                    switch (orientation)
                    {
                        case 1:
                            // No rotation required.
                            break;
                        case 2:
                            current.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            break;
                        case 3:
                            current.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                        case 4:
                            current.RotateFlip(RotateFlipType.Rotate180FlipX);
                            break;
                        case 5:
                            current.RotateFlip(RotateFlipType.Rotate90FlipX);
                            break;
                        case 6:
                            current.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case 7:
                            current.RotateFlip(RotateFlipType.Rotate270FlipX);
                            break;
                        case 8:
                            current.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                    }
                    // This EXIF data is now invalid and should be removed.
                    current.RemovePropertyItem(274);
                }
            #region reckon size
            if (current.Width > current.Height)
            {
                double onepercent = current.Width / 100;
                double howmuchpercent = maxWidth / onepercent;
                height = (int)(current.Height * (howmuchpercent / 100));
                width = maxWidth;
                //height = Convert.ToInt32(current.Height * maxHeight / (double)current.Width);
            }
            else
            {
                double onepercent = current.Height / 100;
                double howmuchpercent = maxHeight / onepercent;
                width = (int)(current.Width * (howmuchpercent / 100));
                //width = Convert.ToInt32(current.Width * maxWidth / (double)current.Height);
                height = maxHeight;
            }
            #endregion

            #region get resized bitmap
            var canvas = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(canvas))
            {
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                
                graphics.DrawImage(current, 0, 0, width, height);
            }

            return canvas;
            #endregion
        }

        public static byte[] ToByteArray(this Image current)
        {
            using (var stream = new MemoryStream())
            {
                current.Save(stream, current.RawFormat);
                return stream.ToArray();
            }
        }
    }
}