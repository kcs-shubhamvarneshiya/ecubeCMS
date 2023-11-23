// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-28-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-28-2016
// ***********************************************************************
// <copyright file="MovieTheatreController.cs" company="KCSPL">
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
    /// Class MovieTheatreController.
    /// </summary>
    public class MovieTheatreController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object i movie theater command.
        /// </summary>
        private readonly IMovieTheatreCommand objIMovieTheatreCommand = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieTheatreController"/> class.
        /// </summary>
        /// <param name="iMovieTheatreCommand">The i movie theatre command.</param>
        public MovieTheatreController(IMovieTheatreCommand iMovieTheatreCommand)
        {
            this.objIMovieTheatreCommand = iMovieTheatreCommand;
        }
        private UserCommand userCommand;
        private GetPageRightsByUserIdResult getPageRights;

        /// <summary>
        /// Movies the theatre view.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult MovieTheatreView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.MovieTheatre));
                if (getPageRights == null || getPageRights.PageId == 0 || getPageRights.ViewAll == false || getPageRights.ViewDetail == false)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }

                this.ViewData["blAddRights"] = getPageRights.Add;
                this.ViewData["blEditRights"] = getPageRights.Edit;
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
        /// Movies the theatre.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult MovieTheatre()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.MovieTheatre));

                MovieTheatreModel objMovieTheatreModel = new MovieTheatreModel();
                long lgMovieTheatreId = 0;
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["iFrame"] != null)
                    {
                        if (getPageRights.Add == false)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objMovieTheatreModel.HdnIFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (getPageRights.Edit == false || string.IsNullOrEmpty(Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgMovieTheatreId = Request.QueryString.ToString().Decode().LongSafe();
                        objMovieTheatreModel = this.objIMovieTheatreCommand.GetMovieTheatreByMovieTheatreId(lgMovieTheatreId);
                    }
                }
                else
                {
                    if (getPageRights.Add == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                objMovieTheatreModel.LstService = this.objIMovieTheatreCommand.GetService();
                return this.View(objMovieTheatreModel);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        /// <summary>
        /// Movies the theatre.
        /// </summary>
        /// <param name="objMovieTheatre">The object movie theatre.</param>
        /// <returns>Action  Result.</returns>
        [HttpPost]
        public ActionResult MovieTheatre(MovieTheatreModel objMovieTheatre)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.MovieTheatre));
                if (objMovieTheatre.Id == 0)
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

                if (objMovieTheatre.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = this.objIMovieTheatreCommand.IsMovieTheatreExists(objMovieTheatre.Id, objMovieTheatre.MovieTheatreName);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Movie Theatre", MessageType.AlreadyExist);
                }
                else
                {
                    string strErrorMsg = this.ValidateMovieTheatre(objMovieTheatre);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        if (objMovieTheatre.Id > 0)
                        {
                            this.ViewData["Message"] = Functions.AlertMessage("Movie Theatre", MessageType.Success);
                        }

                        objMovieTheatre.Id = this.objIMovieTheatreCommand.SaveMovieTheatre(objMovieTheatre);
                        if (objMovieTheatre.Id > 0)
                        {
                            this.ViewData["Success"] = "1";
                            if (this.ViewData["Message"] == null || this.ViewData["Message"].ToString() == string.Empty)
                            {
                                this.ViewData["Message"] = Functions.AlertMessage("Movie Theatre", MessageType.Success);
                            }

                            ////objMovieTheatre.LstMemberSubService = this.objIMovieTheatreCommand.GetSubService();
                            ////objMovieTheatre.LstGuestSubService = this.objIMovieTheatreCommand.GetSubService();
                            return this.View(objMovieTheatre);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("Movie Theatre", MessageType.Fail);
                        }
                    }
                }

                ////objMovieTheatre.LstMemberSubService = this.objIMovieTheatreCommand.GetSubService();
                ////objMovieTheatre.LstGuestSubService = this.objIMovieTheatreCommand.GetSubService();
                return this.View(objMovieTheatre);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Movie Theatre", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objMovieTheatre);
            }
        }

        /// <summary>
        /// Binds the movie theatre grid.
        /// </summary>
        /// <param name="sidx">Parameter SIDX.</param>
        /// <param name="sord">Parameter SORD.</param>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="search">The search.</param>
        /// <returns>Action  Result.</returns>
        public ActionResult BindMovieTheatreGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                List<SearchMovieTheatreResult> objMovieTheatreList = this.objIMovieTheatreCommand.SearchMovieTheatre(rows, page, search, sidx + " " + sord);
                if (objMovieTheatreList != null && objMovieTheatreList.Count > 0)
                {
                    return this.FillGridMovieTheatre(page, rows, objMovieTheatreList);
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

        /// <summary>
        /// Gets the movie theatre.
        /// </summary>
        /// <returns>JSON  Result.</returns>
        public JsonResult GetMovieTheatre()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                return this.Json(this.objIMovieTheatreCommand.GetAllMovieTheatreForDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Validates the movie theatre.
        /// </summary>
        /// <param name="objMovieTheatre">The object movie theatre.</param>
        /// <returns>Return System.String.</returns>
        private string ValidateMovieTheatre(MovieTheatreModel objMovieTheatre)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objMovieTheatre.MovieTheatreName))
                {
                    strErrorMsg += Functions.AlertMessage("Movie Theatre Name", MessageType.InputRequired);
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
        /// Fills the grid movie theatre.
        /// </summary>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="objMovieTheatreList">The object movie theatre list.</param>
        /// <returns>Action  Result.</returns>
        private ActionResult FillGridMovieTheatre(int page, int rows, List<SearchMovieTheatreResult> objMovieTheatreList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                int pageSize = rows;
                int totalRecords = objMovieTheatreList != null && objMovieTheatreList.Count > 0 ? (int)objMovieTheatreList[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objMovieTheatre in objMovieTheatreList
                            select new
                            {
                                MovieTheatreName = objMovieTheatre.MovieTheatreName,
                                TheatreFloor = objMovieTheatre.TheatreFloor,
                                Id = objMovieTheatre.Id.ToString().Encode()
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

        /// <summary>
        /// Gets the movie theatre.
        /// </summary>
        /// <returns>JSON  Result.</returns>
        public JsonResult GetSubServiceList(int serviceId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                return this.Json(new { MemberServiceList = this.objIMovieTheatreCommand.GetSubService(serviceId, 1), GuestServiceList = this.objIMovieTheatreCommand.GetSubService(serviceId, 2) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Gets the movie theatre.
        /// </summary>
        /// <returns>JSON  Result.</returns>
        public JsonResult GetMovieTheatreClass(int theaterId, int movieShowPeriodId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                return this.Json(this.objIMovieTheatreCommand.GetMovieTheatreClassByTheatreId(theaterId, movieShowPeriodId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveMovieTheatre(MovieTheatreModel movieTheatreModel, List<MovieTheatreClassModel> movieTheatreClassModel)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {

                bool blExists = this.objIMovieTheatreCommand.IsMovieTheatreExists(movieTheatreModel.Id, movieTheatreModel.MovieTheatreName);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Movie Theatre", MessageType.AlreadyExist);
                }
                else
                {
                    string strErrorMsg = this.ValidateMovieTheatre(movieTheatreModel);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                        return this.Json(strErrorMsg, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (movieTheatreModel.Id > 0)
                        {
                            this.ViewData["Message"] = Functions.AlertMessage("Movie Theatre", MessageType.Success);
                        }

                        movieTheatreModel.Id = this.objIMovieTheatreCommand.SaveMovieTheatre(movieTheatreModel);
                        if (movieTheatreModel.Id > 0)
                        {
                            foreach (var model in movieTheatreClassModel)
                            {
                                this.objIMovieTheatreCommand.UpdateMovieTheatreClass(model);
                            }
                            if (this.ViewData["Message"] == null || this.ViewData["Message"].ToString() == string.Empty)
                            {
                                this.ViewData["Message"] = Functions.AlertMessage("Movie Theatre", MessageType.Success);
                            }
                            return this.Json(Functions.AlertMessage("Movie Theatre", MessageType.Success), JsonRequestBehavior.AllowGet);
                            //return this.View(movieTheatreModel);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("Movie Theatre", MessageType.Fail);
                            return this.Json(Functions.AlertMessage("Movie Theatre", MessageType.Fail), JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                //return this.View(movieTheatreModel);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Movie Theatre", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                //return this.View(movieTheatreModel);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
