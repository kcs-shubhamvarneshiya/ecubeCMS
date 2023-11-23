// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-19-2016
// ***********************************************************************
// <copyright file="Extension.cs" company="string.Empty">
//     Copyright ©  2016
// </copyright>
// <summary>KCSPL</summary>
// ***********************************************************************

/// <summary>
/// The Common namespace.
/// </summary>
namespace EcubeWebPortalCMS.Common
{
    using Serilog;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    /// <summary>
    /// Class Extension.
    /// </summary>
    public static class Extension
    {
        public static readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// Accepted string set to encrypt or decrypt string.
        /// </summary>
        private static readonly string AcceptedCharacters = "KRISH1234567891123456".ToUpper();

        /// <summary>
        /// The default image variable.
        /// </summary>
        private static readonly string DefaultImage = "../Images/Movies/DefaultMovie.jpg";

        /// <summary>
        /// The uploads directory variable.
        /// </summary>
        private static readonly string UploadsDirectory = "../Images/Movies/";

        /// <summary>
        /// Array of all date FORMATE.
        /// </summary>
        private static readonly string[] Formate =
        {
            "dd/MM/yyyy",
            "dd/M/yyyy",
            "dd/MMM/yyyy",
            "dd/MMM/yy",
            "dd/MMM/yyyy",
            "dd/MMMM/yy",
            "dd/MMMM/yyyy",
            "MM/dd/yyyy",
            "dddd, dd MMMM yyyy",
            "dddd, dd MMMM yyyy",
            "dddd, dd MMMM yyyy HH:mm",
            "dddd, dd MMMM yyyy hh:mm tt",
            "dddd, dd MMMM yyyy H:mm",
            "dddd, dd MMMM yyyy h:mm tt",
            "dddd, dd MMMM yyyy HH:mm:ss",
            "MM/dd/yyyy HH:mm",
            "MM/dd/yyyy hh:mm tt",
            "MM/dd/yyyy H:mm",
            "MM/dd/yyyy h:mm tt",
            "MM/dd/yyyy HH:mm:ss tt",
            "M/d/yyyy HH:mm:ss tt",
            "M/d/yyyy H:mm:ss tt",
            "M/d/yyyy h:m:s tt",
            "MM/dd/yyyy",
            "M/d/yyyy",
            "MMM/dd/yyyy HH:mm",
            "MMM/dd/yyyy hh:mm tt",
            "MMM/dd/yyyy H:mm",
            "MMM/dd/yyyy h:mm tt",
            "MMM/dd/yyyy HH:mm:ss tt",
            "MMM/d/yyyy HH:mm:ss tt",
            "MMM/d/yyyy H:mm:ss tt",
            "MMM/d/yyyy h:m:s tt",
            "MMMM/dd/yyyy HH:mm",
            "MMMM/dd/yyyy hh:mm tt",
            "MMMM/dd/yyyy H:mm",
            "MMMM/dd/yyyy h:mm tt",
            "MMMM/dd/yyyy HH:mm:ss tt",
            "MMMM/d/yyyy HH:mm:ss tt",
            "MMMM/d/yyyy H:mm:ss tt",
            "MMMM/d/yyyy h:m:s tt",
            "MMM/dd/yyyy",
            "MMMM/dd/yyyy",
            "yyyy/MM/dd HH:mm:ss tt",
            "yyyy/MM/dd hh:mm tt",
            "yyyy/MM/dd h:mm tt",
            "yyyy/MM/dd h:mm",
            "yyyy/MM/dd hh:mm:ss tt",
            "yyyy/MM/dd"
        };

        /// <summary>
        /// Convert String to long.
        /// </summary>
        /// <param name="longStr">The long string.</param>
        /// <returns>Returns System.INT64.</returns>
        public static long LongSafe(this string longStr)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Extension + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                long ret = 0;
                long.TryParse(longStr, out ret);
                return ret;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Parse Date from string. If invalid date then return current system date.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>Returns DateTime.</returns>
        public static DateTime DateSafe(this string str)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Extension + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                DateTime date;
                if (DateTime.TryParseExact(str, Formate, null, System.Globalization.DateTimeStyles.None, out date) == true)
                {
                    return date;
                }

                return date;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Parse Date from string. If invalid date then return null.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>System.NULL; DateTime;.</returns>
        public static DateTime? DateNullSafe(this string str)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Extension + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                DateTime date;
                if (DateTime.TryParseExact(str, Formate, null, System.Globalization.DateTimeStyles.None, out date) == true)
                {
                    return date;
                }

                return null;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Convert dynamic value to string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>Return System.String.</returns>
        public static string StrSafe(this string str)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Extension + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                return Convert.ToString(str);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Parse integer value from string. If invalid date then return 0.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>Returns System.INT32.</returns>
        public static int IntSafe(this string str)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Extension + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                if (str != null)
                {
                    str = str.Replace(",", string.Empty);
                }

                int ret = 0;
                int.TryParse(str, out ret);
                return ret;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Parse integer value from string. If invalid date then return null.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>System.NULL; System.INT32;.</returns>
        public static int? IntNullSafe(this string str)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Extension + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                if (str != null)
                {
                    str = str.Replace(",", string.Empty);
                }

                int ret = 0;
                int.TryParse(str, out ret);
                if (ret == 0)
                {
                    return null;
                }
                else
                {
                    return ret;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Parse decimal value from string. If invalid date then return 0.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>Returns System.Decimal.</returns>
        public static decimal DecSafe(this string str)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Extension + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                decimal ret = 0;
                str = str.Replace(",", string.Empty);
                decimal.TryParse(str, out ret);
                return ret;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Parse decimal value from string. If invalid date then return null.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>Returns System.NULL; System.Decimal;.</returns>
        public static decimal? DecNullSafe(this string str)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Extension + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                decimal ret = 0;
                str = str.Replace(",", string.Empty);
                decimal.TryParse(str, out ret);
                if (ret == 0)
                {
                    return null;
                }
                else
                {
                    return ret;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Parse BOOL value from string. If invalid BOOL then return false.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool BoolSafe(this string str)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Extension + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                bool bl = false;
                bool.TryParse(str, out bl);
                return bl;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Encode string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>Return System.String.</returns>
        public static string Encode(this string str)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Extension + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                byte[] encbuff = System.Text.Encoding.ASCII.GetBytes(str);
                string encode = Convert.ToBase64String(encbuff);
                return encode;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return string.Empty;
            }
        }

        /// <summary>
        /// Decode string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>Return System.String.</returns>
        public static string Decode(this string str)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Extension + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                str = str.Replace("%3d", "=");
                byte[] decbuff = Convert.FromBase64String(str);
                string decode = System.Text.Encoding.ASCII.GetString(decbuff);

                return decode;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return string.Empty;
            }
        }
       
        /// <summary>
        /// Encrypts the string.
        /// </summary>
        /// <param name="plainSourceStringToEncrypt">The plain source string to encrypt.</param>
        /// <returns>Return System.String.</returns>
        public static string EncryptString(this string plainSourceStringToEncrypt)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Extension + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                ////Set up the encryption objects
                using (AesCryptoServiceProvider acsp = GetProvider(Encoding.Default.GetBytes(AcceptedCharacters)))
                {
                    byte[] sourceBytes = Encoding.ASCII.GetBytes(plainSourceStringToEncrypt);
                    ICryptoTransform ictE = acsp.CreateEncryptor();

                    ////Set up stream to contain the encryption
                    MemoryStream msS = new MemoryStream();

                    ////Perform the encrpytion, storing output into the stream
                    CryptoStream csS = new CryptoStream(msS, ictE, CryptoStreamMode.Write);
                    csS.Write(sourceBytes, 0, sourceBytes.Length);
                    csS.FlushFinalBlock();

                    ////sourceBytes are now encrypted as an array of secure bytes
                    //// .ToArray() is important, don't mess with the buffer
                    byte[] encryptedBytes = msS.ToArray();

                    ////return the encrypted bytes as a BASE64 encoded string
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Decrypts the string.
        /// </summary>
        /// <param name="base64StringToDecrypt">The base64 string to decrypt.</param>
        /// <returns>Return System.String.</returns>
        public static string DecryptString(this string base64StringToDecrypt)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Extension + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                ////Set up the encryption objects
                using (AesCryptoServiceProvider acsp = GetProvider(Encoding.Default.GetBytes(AcceptedCharacters)))
                {
                    byte[] rawBytes = Convert.FromBase64String(base64StringToDecrypt);
                    ICryptoTransform ictD = acsp.CreateDecryptor();

                    ////RawBytes now contains original byte array, still in Encrypted state
                    ////Decrypt into stream
                    MemoryStream msD = new MemoryStream(rawBytes, 0, rawBytes.Length);
                    CryptoStream csD = new CryptoStream(msD, ictD, CryptoStreamMode.Read);
                    ////csD now contains original byte array, fully decrypted

                    ////return the content of msD as a regular string
                    return (new StreamReader(csD)).ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Gets the week number.
        /// </summary>
        /// <param name="date">Parameter date.</param>
        /// <returns>Returns System.INT32.</returns>
        public static int GetWeekNumber(this DateTime date)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Extension + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Get MonthName from month number.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <returns>Return System.String.</returns>
        public static string MonthName(this int month)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Extension + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                switch (month)
                {
                    case 1:
                        return "Jan";

                    case 2:
                        return "Feb";

                    case 3:
                        return "Mar";

                    case 4:
                        return "Apr";

                    case 5:
                        return "May";

                    case 6:
                        return "Jun";

                    case 7:
                        return "Jul";

                    case 8:
                        return "Aug";

                    case 9:
                        return "Sep";

                    case 10:
                        return "Oct";

                    case 11:
                        return "Nov";

                    case 12:
                        return "Dec";
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return string.Empty;
        }

        /// <summary>
        /// Determines whether [is null string] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is null string] [the specified value]; otherwise, <c>false</c>.</returns>
        public static bool IsNullString(this string value)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Extension + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Images the or default.
        /// </summary>
        /// <param name="filename">The filename parameter.</param>
        /// <returns>System.String image or default.</returns>
        public static string ImageOrDefault(this string filename)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Extension + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                var imagePath = UploadsDirectory + filename;
                var imageSrc = File.Exists(HttpContext.Current.Server.MapPath(imagePath))
                                   ? imagePath : DefaultImage;

                return imageSrc;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Strips the HTML.
        /// </summary>
        /// <param name="input">The input parameter.</param>
        /// <returns>System.String strip HTML.</returns>
        public static string StripHTML(this string input)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Extension + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                string stripHTML = Regex.Replace(input, "<.*?>", string.Empty);
                return stripHTML.Replace("&nbsp;", " ");
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <param name="key">Parameter key.</param>
        /// <returns>AES Crypts ServiceProvider.</returns>
        private static AesCryptoServiceProvider GetProvider(byte[] key)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Extension + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                AesCryptoServiceProvider result = new AesCryptoServiceProvider();
                result.BlockSize = 128;
                result.KeySize = 128;
                result.Mode = CipherMode.CBC;
                result.Padding = PaddingMode.PKCS7;

                result.GenerateIV();
                result.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                byte[] realKey = GetKey(key, result);
                result.Key = realKey;
                return result;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="suggestedKey">The suggested key.</param>
        /// <param name="p">Parameter P.</param>
        /// <returns>Returns System.Byte[].</returns>
        private static byte[] GetKey(byte[] suggestedKey, SymmetricAlgorithm p)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Extension + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                byte[] kraw = suggestedKey;
                List<byte> klist = new List<byte>();

                for (int i = 0; i < p.LegalKeySizes[0].MinSize; i += 8)
                {
                    klist.Add(kraw[(i / 8) % kraw.Length]);
                }

                byte[] k = klist.ToArray();
                return k;
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