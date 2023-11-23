// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : KCSPL
// Created          : 12-30-2016
//
// Last Modified By : KCSPL
// Last Modified On : 12-30-2016
// ***********************************************************************
// <copyright file="ImageHandler.cs" company="KCSPL">
//     Copyright ©  2016
// </copyright>
// <summary>ImageHandler.cs</summary>
// ***********************************************************************

/// <summary>
/// The Common namespace.
/// </summary>
namespace DreamSteam.Common
{
    using EcubeWebPortalCMS.Common;
    using Serilog;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Class ImageHandler.
    /// </summary>
    public class ImageHandler
    {
        public static readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// Saves the specified image.
        /// </summary>
        /// <param name="image">The image parameter.</param>
        /// <param name="maxWidth">The maximum width parameter.</param>
        /// <param name="maxHeight">The maximum height parameter.</param>
        /// <param name="quality">The quality parameter.</param>
        /// <param name="filePath">The file path parameter.</param>
        /// <param name="maintainRation">If set to <c>true</c> [maintain ration].</param>
        public static void Save(Bitmap image, int maxWidth, int maxHeight, int quality, string filePath, bool maintainRation)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.ImageHandler + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            Image finalImage = image;
            //// Get the image's original width and height
            int originalWidth = image.Width;
            int originalHeight = image.Height;

            ImageCodecInfo imgInfo = ImageCodecInfo.GetImageEncoders()
                                                   .Where(codecInfo =>
                                                                      codecInfo.MimeType == "image/png")
                                                   .First();

            // To preserve the aspect ratio
            float ratioX = (float)maxWidth / (float)originalWidth;
            float ratioY = (float)maxHeight / (float)originalHeight;
            float ratio = ratioX < ratioY ? ratioX : ratioY;

            // New width and height based on aspect ratio
            int newWidth = 1;
            int newHeight = 1;

            if (maintainRation)
            {
                newWidth = (int)(originalWidth * ratio);
                newHeight = (int)(originalHeight * ratio);
            }
            else
            {
                newWidth = (int)(maxWidth * 1);
                newHeight = (int)(maxHeight * 1);
            }

            // Convert other formats (including CMYK) to RGB.
            Bitmap newImage = new Bitmap(newWidth, newHeight);

            // Draws the image in the specified size with quality mode set to HighQuality
            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            finalImage = newImage;

            try
            {
                using (EncoderParameters encParams = new EncoderParameters(1))
                {
                    encParams.Param[0] = new EncoderParameter(Encoder.Quality, (long)100);
                    ////quality should be in the range 
                    ////[0..100] .. 100 for max, 0 for min (0 best compression)
                    finalImage.Save(filePath, imgInfo, encParams);
                    if (newImage != null)
                    {
                        newImage.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                if (newImage != null)
                {
                    newImage.Dispose();
                }
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }
        }

        /// <summary>
        /// Saves the center cropped image.
        /// </summary>
        /// <param name="image">The image parameter.</param>
        /// <param name="maxWidth">The maximum width parameter.</param>
        /// <param name="maxHeight">The maximum height parameter.</param>
        /// <param name="filePath">The file path parameter.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool SaveCenterCroppedImage(Image image, int maxWidth, int maxHeight, string filePath)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.ImageHandler + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            ImageCodecInfo imgInfo = ImageCodecInfo.GetImageEncoders()
                                                   .Where(codecInfo =>
                                                                      codecInfo.MimeType == "image/png")
                                                   .First();
            Image finalImage = image;
            System.Drawing.Bitmap bitmap = null;
            try
            {
                int left = 0;
                int top = 0;
                int srcWidth = maxWidth;
                int srcHeight = maxHeight;
                bitmap = new System.Drawing.Bitmap(maxWidth, maxHeight);
                double croppedHeightToWidth = (double)maxHeight / maxWidth;
                double croppedWidthToHeight = (double)maxWidth / maxHeight;

                if (image.Width > image.Height)
                {
                    srcWidth = (int)Math.Round(image.Height * croppedWidthToHeight);
                    if (srcWidth < image.Width)
                    {
                        srcHeight = image.Height;
                        left = (image.Width - srcWidth) / 2;
                    }
                    else
                    {
                        srcHeight = (int)Math.Round(image.Height * ((double)image.Width / srcWidth));
                        srcWidth = image.Width;
                        top = (image.Height - srcHeight) / 2;
                    }
                }
                else
                {
                    srcHeight = (int)Math.Round(image.Width * croppedHeightToWidth);
                    if (srcHeight < image.Height)
                    {
                        srcWidth = image.Width;
                        top = (image.Height - srcHeight) / 2;
                    }
                    else
                    {
                        srcWidth = (int)Math.Round(image.Width * ((double)image.Height / srcHeight));
                        srcHeight = image.Height;
                        left = (image.Width - srcWidth) / 2;
                    }
                }

                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height), new Rectangle(left, top, srcWidth, srcHeight), GraphicsUnit.Pixel);
                }

                finalImage = bitmap;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            try
            {
                using (EncoderParameters encParams = new EncoderParameters(1))
                {
                    encParams.Param[0] = new EncoderParameter(Encoder.Quality, (long)100);
                    ////quality should be in the range 
                    ////[0..100] .. 100 for max, 0 for min (0 best compression)
                    finalImage.Save(filePath, imgInfo, encParams);
                    if (bitmap != null)
                    {
                        bitmap.Dispose();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                if (bitmap != null)
                {
                    bitmap.Dispose();
                    information += " - " + ex.Message;
                    logger.Error(ex, information);
                }

                return false;
            }
        }

        /// <summary>
        /// Method to get encoder info for given image format.
        /// </summary>
        /// <param name="format">Image format.</param>
        /// <returns>Image code info.</returns>
        private static ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.ImageHandler + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                return ImageCodecInfo.GetImageDecoders().SingleOrDefault(c => c.FormatID == format.Guid);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }
    }
}