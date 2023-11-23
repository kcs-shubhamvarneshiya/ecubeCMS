
using EcubeWebPortalCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcubeWebPortalCMS.Controllers
{
    using EcubeWebPortalCMS.Common;
    using Serilog;
    using System.Configuration;
    using System.Data.SqlClient;

    public class MovieTicketController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        private readonly IMovieTicketCommand objIMovieTicketCommand = null;

        public MovieTicketController(IMovieTicketCommand iMovieTicketCommand)
        {
            this.objIMovieTicketCommand = iMovieTicketCommand;
        }

        public ActionResult MovieTicketView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                UserCommand userCommand = new UserCommand();
                GetPageRightsByUserIdResult getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.MovieTicket));
                if (getPageRights == null || getPageRights.PageId == 0 || getPageRights.ViewAll == false || getPageRights.ViewDetail == false)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }
                MovieTicketModel objEventList = new MovieTicketModel();
                return this.View(objEventList);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        [HttpGet]
        public ActionResult MovieTicket(int? id)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                MovieTicketModel objTicketDetails = new MovieTicketModel();
                if (id != null && id != 0)
                {
                    MovieTicketCommand objMovieBookingData = new MovieTicketCommand();
                    objTicketDetails = objMovieBookingData.GetMovieTicketDetails(id);
                    objTicketDetails.QRCodeImage = objMovieBookingData.GenerateEventQRCode(objTicketDetails.TicketId.ToString(), null, "Movie");
                }

                return this.View(objTicketDetails);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        [HttpGet]
        public JsonResult MovieTicketGrid(string id, string memberCode, string ticketNo)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<MS_GetMemberBookedMovieTicketListResult> objEventList = new List<MS_GetMemberBookedMovieTicketListResult>();
                if (id == "" || id == null)
                {
                    id = null;
                }
                if (memberCode == "" || memberCode == null)
                {
                    memberCode = null;
                }
                if (ticketNo == "" || ticketNo == null)
                {
                    ticketNo = null;
                }
                objEventList = this.objIMovieTicketCommand.GetMemberBookedMovieTicketList(id, memberCode, ticketNo);
                return Json(objEventList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

    }
}
