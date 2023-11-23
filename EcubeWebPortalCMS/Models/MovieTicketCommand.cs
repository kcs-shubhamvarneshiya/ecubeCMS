using EcubeWebPortalCMS.Common;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EcubeWebPortalCMS.Models
{
    public class MovieTicketCommand : IMovieTicketCommand
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        private MovieTicketDataContext objDataContext = null;
        private readonly JavaScriptSerializer js = new JavaScriptSerializer();

        public List<MS_GetMemberBookedMovieTicketListResult> GetMemberBookedMovieTicketList(string id, string memberCode, string ticketNo)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            List<MS_GetMemberBookedMovieTicketListResult> movieTheatreClassList = new List<MS_GetMemberBookedMovieTicketListResult>();
            try
            {
                using (this.objDataContext = new MovieTicketDataContext())
                {
                    movieTheatreClassList = this.objDataContext.MS_GetMemberBookedMovieTicketList(id, memberCode, ticketNo).ToList();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return movieTheatreClassList;
        }

        public MovieTicketModel GetMovieTicketDetails(int? Id)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                MovieTicketModel movieTheatreClassList = new MovieTicketModel();
                string strBooking = GetMovieBookingDetail(Id);
                if (strBooking != "0")
                {
                    List<MovieTicketModel> deserializedMember = JsonConvert.DeserializeObject<List<MovieTicketModel>>(strBooking);

                    if (deserializedMember != null && deserializedMember.Count > 0)
                    {
                        movieTheatreClassList = deserializedMember.FirstOrDefault();
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
        public string GetMovieBookingDetail(int? movieBookingMasterId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            List<MS_GetMovieBookingDetailResult> movieTheatreClassList = new List<MS_GetMovieBookingDetailResult>();
            try
            {
                using (this.objDataContext = new MovieTicketDataContext())
                {
                    movieTheatreClassList = this.objDataContext.MS_GetMovieBookingDetail(movieBookingMasterId).ToList();
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

        public string GenerateEventQRCode(string bookingDetailId, DateTime? bookedDate = null, string qrFor = "Movie")
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
                string path = tempPath + mainFileName;
                if (!File.Exists(tempPath + mainFileName))
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    webClient.DownloadFile("https://api.qrserver.com/v1/create-qr-code/?size=250x250&data=" + bookingDetailId, path);
                    webClient.Dispose();
                    webClient = null;
                }

                codeImagePath = "/Movie/QRcode/" + mainFileName;
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