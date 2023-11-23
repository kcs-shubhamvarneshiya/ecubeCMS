using EcubeWebPortalCMS.Common;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EcubeWebPortalCMS.Models
{
    public class EventTicketCommand : IEventTicketCommand
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        private EventTicketDataContext objDataContext = null;
        private readonly JavaScriptSerializer js = new JavaScriptSerializer();

        public List<MS_GetBookedEventBookingResult> GetBookedEventBookingResult(long? memberId, string eventBookingId, string memberCode)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            List<MS_GetBookedEventBookingResult> movieTheatreClassList = new List<MS_GetBookedEventBookingResult>();
            try
            {
                using (this.objDataContext = new EventTicketDataContext())
                {
                    movieTheatreClassList = this.objDataContext.MS_GetBookedEventBooking(memberId, eventBookingId, memberCode).ToList();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return movieTheatreClassList;
        }

        public List<EventTicketModel> GetEventBookingDetailResult(int? Id)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                List<EventTicketModel> movieTheatreClassList = new List<EventTicketModel>();
                string strBooking = GetEventBookingDetail(Id);
                if (strBooking != "0")
                {
                    List<EventTicketModel> deserializedMember = JsonConvert.DeserializeObject<List<EventTicketModel>>(strBooking).ToList();

                    if (deserializedMember != null && deserializedMember.Count > 0)
                    {
                        movieTheatreClassList = deserializedMember.ToList();
                    }
                }

                return movieTheatreClassList;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        public string GetEventBookingDetail(int? Id)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            List<MS_GetEventBookingDetailResult> movieTheatreClassList = new List<MS_GetEventBookingDetailResult>();
            try
            {
                using (this.objDataContext = new EventTicketDataContext())
                {
                    movieTheatreClassList = this.objDataContext.MS_GetEventBookingDetail(Id).ToList();
                    if (movieTheatreClassList != null && movieTheatreClassList.Count > 0)
                    {
                        return this.js.Serialize(movieTheatreClassList.ToList());
                    }
                    else
                    {
                        return "0";
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return "0";
            }
        }
        public string EventQRNAME(string bookingDetailId, DateTime? bookedDate = null, string qrFor = "/Event")
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                string tempPath = "";// HttpContext.Current.Server.MapPath("~/Event/QRCode/");

                HttpRequest request = HttpContext.Current.Request;
                string url = request.Url.Scheme + "://" + request.Url.Authority;
                tempPath = url + "/Event/QRCode/";

                string mainFileName = bookingDetailId + "_" + (bookedDate == null ? string.Empty : "_" + bookedDate.Value.ToString("ddMMyyyy")) + ".png";
                GenerateEventQRCode(bookingDetailId, bookedDate);

                return tempPath + mainFileName;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        public string GenerateEventQRCode(string bookingDetailId, DateTime? bookedDate = null, string qrFor = "/Event")
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            string codeImagePath = string.Empty;
            try
            {
                System.Net.WebClient webClient = new System.Net.WebClient();
                string tempPath = HttpContext.Current.Server.MapPath("~/" + qrFor + "/QRCode/");
                string mainFileName = bookingDetailId + "_" + (bookedDate == null ? string.Empty : "_" + bookedDate.Value.ToString("ddMMyyyy")) + ".png";

                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }

                if (!File.Exists(tempPath + mainFileName))
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    webClient.DownloadFile("https://api.qrserver.com/v1/create-qr-code/?size=250x250&data=" + bookingDetailId.EncryptCode(), tempPath + mainFileName);
                    webClient.Dispose();
                    webClient = null;
                }

                codeImagePath = qrFor + "/QRcode/" + mainFileName;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                //ErrorLog.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Common);
            }

            return codeImagePath;
        }

        public string GenerateEventBarCode(string bookingDetailId, DateTime? bookedDate)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            string codeImagePath = string.Empty;
            try
            {
                System.Net.WebClient webClient = new System.Net.WebClient();
                string tempPath = HttpContext.Current.Server.MapPath("~/Event/BarCode/");
                string fileName = bookingDetailId + "_" + (bookedDate == null ? string.Empty : "_" + bookedDate.Value.ToString());
                string mainFileName = fileName + ".png";

                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }

                using (Bitmap bitMap = new Bitmap(fileName.Length * 26, 80))
                {
                    using (Graphics graphics = Graphics.FromImage(bitMap))
                    {
                        Font font = new Font("IDAutomationHC39M", 16);
                        PointF point = new PointF(2f, 2f);
                        SolidBrush blackBrush = new SolidBrush(Color.Black);
                        SolidBrush whiteBrush = new SolidBrush(Color.White);
                        graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                        graphics.DrawString("*" + fileName + "*", font, blackBrush, point);
                    }

                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] byteImage = ms.ToArray();

                        File.WriteAllBytes(tempPath + mainFileName, byteImage);
                    }
                }

                codeImagePath = "/Event/BarCode/" + mainFileName;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);

            }

            return codeImagePath;
        }
    }
}
