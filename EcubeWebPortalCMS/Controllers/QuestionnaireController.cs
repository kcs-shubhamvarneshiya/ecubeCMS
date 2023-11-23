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

    public class QuestionnaireController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        private readonly IQuestionnaireCommand iQuestionnaire = new QuestionnaireCommand();

        private UserCommand userCommand;
        private GetPageRightsByUserIdResult getPageRights;

        public ActionResult QuestionnaireView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.FeedbackQuestionnaire));
                if (getPageRights == null || getPageRights.PageId == 0 || getPageRights.ViewAll == false || getPageRights.ViewDetail == false)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }

                this.ViewData["blAddRights"] = getPageRights.Add;
                this.ViewData["blEditRights"] = getPageRights.Edit;
                this.ViewData["blDeleteRights"] = getPageRights.Delete;
                return this.View();
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        public ActionResult Questionnaire()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.FeedbackQuestionnaire));

                if (getPageRights.Add == false)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }

                this.ViewData["blAddRights"] = getPageRights.Add;
                this.ViewData["blEditRights"] = getPageRights.Edit;
                this.ViewData["blDeleteRights"] = getPageRights.Delete;
                int questionnaireId = 0;
                QuestionnaireModel objQuestionnaireModel = new QuestionnaireModel();
                if (this.Request.QueryString.Count > 0)
                {
                    if (this.Request.QueryString["iFrame"] != null)
                    {
                        if (getPageRights.Add == false)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objQuestionnaireModel.hdniFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (getPageRights.Edit == false || string.IsNullOrEmpty(this.Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        questionnaireId = this.Request.QueryString.ToString().Decode().IntSafe();
                        objQuestionnaireModel = this.iQuestionnaire.GetQuestionnaireByQuestionnaireId(questionnaireId);
                    }
                }
                else
                {
                    if (getPageRights.Add == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }
                objQuestionnaireModel.LstQuestionType = iQuestionnaire.GetQuestionTypeForDropDown();
                return View(objQuestionnaireModel);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return View();
            }
        }

        public ActionResult SaveQuestionnaire(QuestionnaireModel objQuestionnaire, List<QuestionModel> lstQuestion)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                if (this.iQuestionnaire.IsQuestionnaireExists(objQuestionnaire.Id, objQuestionnaire.QuestionnaireTitle))
                {
                    return this.Json(new { status = false, msg = Functions.AlertMessage("Questionnaire", MessageType.AlreadyExist, Convert.ToString(objQuestionnaire.QuestionnaireTitle)) }, JsonRequestBehavior.AllowGet);
                }

                string strPosition = string.Empty;

                long lgQuestionnaireId = this.iQuestionnaire.SaveQuestionnaire(objQuestionnaire, lstQuestion, ref strPosition);
                if (lgQuestionnaireId > 0)
                {
                    if (lstQuestion.Where(x => x.QuestionTypeId == 23).Count() > 0)
                    {
                        foreach (var item in lstQuestion.Where(x => x.QuestionTypeId == 23).ToList())
                        {
                            if (!string.IsNullOrEmpty(item.ImagePath))
                            {
                                string strOldFilePath = this.Request.MapPath("../CMSUpload/TempQuestions");
                                string strFilePath = this.Request.MapPath("../CMSUpload/Questions");
                                if (!System.IO.Directory.Exists(strFilePath))
                                {
                                    System.IO.Directory.CreateDirectory(strFilePath);
                                }

                                if (System.IO.File.Exists(System.IO.Path.Combine(strOldFilePath, item.ImagePath)))
                                {
                                    System.IO.File.Move(System.IO.Path.Combine(strOldFilePath, item.ImagePath), System.IO.Path.Combine(strFilePath, item.ImagePath));
                                }
                            }
                        }
                    }
                    return this.Json(new { status = true, msg = Functions.AlertMessage("Questionnaire", MessageType.Success) }, JsonRequestBehavior.AllowGet);
                }

                return this.Json(new { status = false, msg = Functions.AlertMessage("Questionnaire", MessageType.Fail) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(new { status = false, msg = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UploadFiles()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                string strFilePath = this.Request.MapPath("../CMSUpload/TempQuestions");
                string strFileNames = string.Empty;
                if (!System.IO.Directory.Exists(strFilePath))
                {
                    System.IO.Directory.CreateDirectory(strFilePath);
                }

                if (this.Request.Files.Count > 0)
                {
                    for (int i = 0; i < this.Request.Files.Count; i++)
                    {
                        HttpPostedFileBase file = this.Request.Files[i];
                        if (file != null && file.ContentLength > 0)
                        {
                            string fileName = DateTime.Now.ToString("ddMMyyHHmmss") + System.IO.Path.GetExtension(file.FileName);
                            file.SaveAs(System.IO.Path.Combine(strFilePath, fileName));
                            if (i == 0)
                            {
                                strFileNames = fileName;
                            }
                            else
                            {
                                strFileNames += "," + fileName;
                            }
                        }
                    }
                }

                return this.Json(new { status = true, msg = strFileNames }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(new { status = false, msg = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult BindQuestionnaireGrid(string sidx, string sord, int page, int rows, string search, bool isActive)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                List<SearchQuestionnaireResult> QuestionnaireList = this.iQuestionnaire.SearchQuestionnaire(rows, page, search, sidx + " " + sord, isActive);
                if (QuestionnaireList != null)
                {
                    return this.FillGridQuestionnaire(page, rows, QuestionnaireList);
                }
                else
                {
                    return this.Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        public JsonResult DeleteQuestionnaire(string questionnaireId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                string[] strQuestionnaire = questionnaireId.Split(',');
                questionnaireId = string.Empty;

                foreach (var item in strQuestionnaire)
                {
                    questionnaireId += item.Decode() + ",";
                }

                questionnaireId = questionnaireId.Substring(0, questionnaireId.Length - 1);

                return this.Json(Functions.AlertMessage("Questionnaire", MessageType.DeleteFail), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(Functions.AlertMessage("Questionnaire", MessageType.DeleteFail), JsonRequestBehavior.AllowGet);
            }
        }

        private ActionResult FillGridQuestionnaire(int page, int rows, List<SearchQuestionnaireResult> questionnaireList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                int pageSize = rows;
                int totalRecords = questionnaireList != null && questionnaireList.Count > 0 ? (int)questionnaireList[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objQuestionnaire in questionnaireList
                            select new
                            {
                                Description = objQuestionnaire.Description,
                                Link = Functions.GetSettings("eCubeWebPortalURL") + "Feedback/Index?QId=" + objQuestionnaire.Link,
                                QuestionnaireTitle = objQuestionnaire.QuestionnaireTitle,
                                Id = objQuestionnaire.Id.ToString().Encode(),
                                QuestionnaireId = objQuestionnaire.Id,
                                RecursionEmail = objQuestionnaire.RecursionEmail,
                                RecursionSMS = objQuestionnaire.RecursionSMS,
                                IsRecursionCreated = objQuestionnaire.IsRecursionCreated,
                                RecursionTypeId = objQuestionnaire.RecursionTypeId,
                                RecursionFrom = objQuestionnaire.RecursionFrom,
                                RecursionTo = objQuestionnaire.RecursionTo,
                                IsActive = objQuestionnaire.IsActive,
                                ActiveLink = (objQuestionnaire.IsActive == true ? "Active" : "Inactive"),
                                AnswerCount = objQuestionnaire.AnswerCount
                            }).ToArray()
                };
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateQuestionnaireQuestion(QuestionModel questionModel)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                if (this.iQuestionnaire.SaveQuestionnaireQuestion(questionModel) > 0)
                {
                    if (!string.IsNullOrEmpty(questionModel.ImagePath))
                    {
                        string strOldFilePath = this.Request.MapPath("../CMSUpload/TempQuestions");
                        string strFilePath = this.Request.MapPath("../CMSUpload/Questions");
                        if (!System.IO.Directory.Exists(strFilePath))
                        {
                            System.IO.Directory.CreateDirectory(strFilePath);
                        }

                        if (System.IO.File.Exists(System.IO.Path.Combine(strOldFilePath, questionModel.ImagePath)))
                        {
                            System.IO.File.Move(System.IO.Path.Combine(strOldFilePath, questionModel.ImagePath), System.IO.Path.Combine(strFilePath, questionModel.ImagePath));
                        }
                    }

                    return this.Json(new { status = true, msg = Functions.AlertMessage("Question", MessageType.Success) }, JsonRequestBehavior.AllowGet);
                }

                return this.Json(new { status = false, msg = Functions.AlertMessage("Question", MessageType.Fail) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(new { status = false, msg = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetAllRecursionType()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                return this.Json(this.iQuestionnaire.GetAllRecursionType().ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateRecursionDetailsByQuestionnaireId(QuestionnaireModel questionnaireModel)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                return this.Json(this.iQuestionnaire.UpdateRecursionDetailsByQuestionnaireId(questionnaireModel), JsonRequestBehavior.AllowGet);
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
