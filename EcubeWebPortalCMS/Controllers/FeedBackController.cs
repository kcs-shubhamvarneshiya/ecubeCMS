using EcubeWebPortalCMS.Common;
using EcubeWebPortalCMS.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcubeWebPortalCMS.Controllers
{
    public class FeedBackController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        private readonly IFeedBackCommand iFeedBackCommand = new FeedBackCommand();

        public ActionResult FeedBack()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (this.Request.QueryString.Count > 0)
                {
                    ViewBag.QuestionnaireId = this.Request.QueryString.ToString().Decode().IntSafe();
                }
                return View();
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetFeedbackAnwserList(long questionnaireId, string memberCode, DateTime? fromDate, DateTime? toDate)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<GetFeedbackAnwserListResult> feedBackList = this.iFeedBackCommand.GetFeedbackAnwserList(questionnaireId, memberCode, fromDate, toDate).ToList();
                if (feedBackList.Count > 0)
                {
                    var jsonData = from feedback in feedBackList
                                   select new
                                   {
                                       QuestionnaireTitle = feedback.QuestionnaireTitle,
                                       AnswerId = feedback.AnswerId,
                                       AnswerDate = feedback.AnswerDate.ToString("dd/MM/yyyy hh:mm tt"),
                                       QuestionId = feedback.QuestionId,
                                       QuestionTitle = feedback.QuestionTitle,
                                       IsDisplayInDetail = feedback.IsDisplayInDetail,
                                       IsDisplayInSummary = feedback.IsDisplayInSummary,
                                       OptionId = feedback.OptionId,
                                       QuestionTypeId = feedback.QuestionTypeId,
                                       Detail = feedback.Detail,
                                       ImageHeight = feedback.ImageHeight,
                                       ImageWidth = feedback.ImageWidth,
                                   };

                    return this.Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
