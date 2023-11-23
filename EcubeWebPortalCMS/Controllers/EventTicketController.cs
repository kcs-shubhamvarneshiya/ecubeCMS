
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

    public class EventTicketController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        private readonly IEventTicketCommand objIEventTicketCommand = null;

        public EventTicketController(IEventTicketCommand iEventTicketCommand)
        {
            this.objIEventTicketCommand = iEventTicketCommand;
        }

        public ActionResult EventTicketView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                UserCommand userCommand = new UserCommand();
                GetPageRightsByUserIdResult getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.EventTicket));
                if (getPageRights == null || getPageRights.PageId == 0 || getPageRights.ViewAll == false || getPageRights.ViewDetail == false)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }
                EventTicketModel objEventList = new EventTicketModel();
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
        public ActionResult EventTicket(int? id)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                EventTicketCommand eventTicketCommand = new EventTicketCommand();
                var listEventBookingDetail = eventTicketCommand.GetEventBookingDetailResult(id);
                foreach (var item in listEventBookingDetail)
                {
                    if (item.IsQRCode)
                    {
                        item.QRCodePath = eventTicketCommand.GenerateEventQRCode(item.EventBookingId.ToString(), DateTime.Now);
                    }
                    else
                    {
                        item.QRCodePath = eventTicketCommand.GenerateEventBarCode(item.EventBookingId.ToString(), null);
                    }
                }
                return this.View(listEventBookingDetail);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);

                return this.Json(string.Empty);
            }
        }

        [HttpGet]
        public JsonResult EventTicketGrid(long? memberId, string eventBookingId, string memberCode)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (eventBookingId == "")
                {
                    eventBookingId = null;
                }
                if (memberCode == "")
                {
                    memberCode = null;
                }
                List<MS_GetBookedEventBookingResult> objEventList = new List<MS_GetBookedEventBookingResult>();
                objEventList = this.objIEventTicketCommand.GetBookedEventBookingResult(memberId, eventBookingId, memberCode);
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
