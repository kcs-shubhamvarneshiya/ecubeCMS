// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-28-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-28-2016
// ***********************************************************************
// <copyright file="MovieShowController.cs" company="KCSPL">
//     Copyright ©  2016
// </copyright>
// <summary>KCSPL</summary>
// ***********************************************************************

/// <summary>
/// The Controllers namespace.
/// </summary>
namespace EcubeWebPortalCMS.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Common;
    using EcubeWebPortalCMS.Models;
    using Serilog;

    /// <summary>
    /// Class MovieShowController.
    /// </summary>
    public class MovieShowController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object i movie show command.
        /// </summary>
        private readonly IMovieShowCommand objIMovieShowCommand = null;

        private UserCommand userCommand;
        private GetPageRightsByUserIdResult getPageRights;
        /// <summary>
        /// Initializes a new instance of the <see cref="MovieShowController"/> class.
        /// </summary>
        /// <param name="iMovieShowCommand">The i movie show command.</param>
        public MovieShowController(IMovieShowCommand iMovieShowCommand)
        {
            this.objIMovieShowCommand = iMovieShowCommand;
        }

        /// <summary>
        /// Movies the show view.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult MovieShowView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.MovieShow));
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

        /// <summary>
        /// Movies the show.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult MovieShow()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.MovieShow));

                MovieShowModel objMovieShowModel = new MovieShowModel();
                long lgMovieShowId = 0;
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["iFrame"] != null)
                    {
                        if (getPageRights.Add == false)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objMovieShowModel.HdnIFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (getPageRights.Edit == false || string.IsNullOrEmpty(Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgMovieShowId = Request.QueryString.ToString().Decode().LongSafe();
                        objMovieShowModel = this.objIMovieShowCommand.GetMovieShowByMovieShowId(lgMovieShowId);
                    }
                }
                else
                {
                    if (getPageRights.Add == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                return this.View(objMovieShowModel);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        /// <summary>
        /// Movies the show.
        /// </summary>
        /// <param name="objMovieShow">The object movie show.</param>
        /// <returns>Action  Result.</returns>
        [HttpPost]
        public ActionResult MovieShow(MovieShowModel objMovieShow)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                objMovieShow.StrShowStartDate = "01/01/2000 " + objMovieShow.StrShowStartDate;
                objMovieShow.StrShowEndDate = "01/01/2000 " + objMovieShow.StrShowEndDate;

                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.MovieShow));

                if (objMovieShow.Id == 0)
                {
                    if (getPageRights.Add == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }
                else
                {
                    if (getPageRights.Edit == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                if (objMovieShow.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = this.objIMovieShowCommand.IsMovieShowExists(objMovieShow.Id, objMovieShow.MovieShowName, Convert.ToDateTime(objMovieShow.StrShowStartDate), Convert.ToDateTime(objMovieShow.StrShowEndDate));
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Movie Show", MessageType.AlreadyExist);
                }
                else
                {
                    string strErrorMsg = this.ValidateMovieShow(objMovieShow);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        if (objMovieShow.Id > 0)
                        {
                            this.ViewData["Message"] = Functions.AlertMessage("Movie Show", MessageType.UpdateSuccess);
                        }

                        objMovieShow.Id = this.objIMovieShowCommand.SaveMovieShow(objMovieShow);
                        if (objMovieShow.Id > 0)
                        {
                            this.ViewData["Success"] = "1";
                            if (this.ViewData["Message"] == null || this.ViewData["Message"].ToString() == string.Empty)
                            {
                                this.ViewData["Message"] = Functions.AlertMessage("Movie Show", MessageType.Success);
                            }

                            return this.View(objMovieShow);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("Movie Show", MessageType.Fail);
                        }
                    }
                }

                return this.View(objMovieShow);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Movie Show", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objMovieShow);
            }
        }

        /// <summary>
        /// Binds the movie show grid.
        /// </summary>
        /// <param name="sidx">Parameter SIDX.</param>
        /// <param name="sord">Parameter SORD.</param>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="search">The search.</param>
        /// <returns>Action  Result.</returns>
        public ActionResult BindMovieShowGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<SearchMovieShowResult> objMovieShowList = this.objIMovieShowCommand.SearchMovieShow(rows, page, search, sidx + " " + sord);
                if (objMovieShowList != null && objMovieShowList.Count > 0)
                {
                    return this.FillGridMovieShow(page, rows, objMovieShowList);
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

        /// <summary>
        /// Deletes the movie show.
        /// </summary>
        /// <param name="strMovieShowId">The string movie show identifier.</param>
        /// <returns>JSON  Result.</returns>
        public JsonResult DeleteMovieShow(string strMovieShowId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string[] strMovieShow = strMovieShowId.Split(',');
                strMovieShowId = string.Empty;
                foreach (var item in strMovieShow)
                {
                    strMovieShowId += item.Decode() + ",";
                }

                strMovieShowId = strMovieShowId.Substring(0, strMovieShowId.Length - 1);
                DeleteMovieShowResult result = this.objIMovieShowCommand.DeleteMovieShow(strMovieShowId, MySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Movie Show", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Movie Show", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Movie Show", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(Functions.AlertMessage("Movie Show", MessageType.DeleteFail));
            }
        }

        /// <summary>
        /// Gets the movie show.
        /// </summary>
        /// <returns>JSON  Result.</returns>
        public JsonResult GetMovieShow()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                return this.Json(this.objIMovieShowCommand.GetAllMovieShowForDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Validates the movie show.
        /// </summary>
        /// <param name="objMovieShow">The object movie show.</param>
        /// <returns>Return System.String.</returns>
        private string ValidateMovieShow(MovieShowModel objMovieShow)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objMovieShow.StrShowStartDate))
                {
                    strErrorMsg += Functions.AlertMessage("Show Start Date", MessageType.SelectRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objMovieShow.StrShowEndDate))
                {
                    strErrorMsg += Functions.AlertMessage("Show End Date", MessageType.SelectRequired) + "<br/>";
                }

                return strErrorMsg;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return ex.Message.ToString();
            }
        }

        /// <summary>
        /// Fills the grid movie show.
        /// </summary>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="objMovieShowList">The object movie show list.</param>
        /// <returns>Action  Result.</returns>
        private ActionResult FillGridMovieShow(int page, int rows, List<SearchMovieShowResult> objMovieShowList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objMovieShowList != null && objMovieShowList.Count > 0 ? (int)objMovieShowList[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objMovieShow in objMovieShowList
                            select new
                            {
                                MovieShowName = objMovieShow.MovieShowName,
                                ShowStartDate = objMovieShow.ShowStartDate.ToString("hh:mm tt"),
                                ShowEndDate = objMovieShow.ShowEndDate.ToString("hh:mm tt"),
                                Id = objMovieShow.Id.ToString().Encode()
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
    }
}
