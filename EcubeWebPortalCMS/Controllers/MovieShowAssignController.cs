// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 10-08-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 06-13-2017
// ***********************************************************************
// <copyright file="MovieShowAssignController.cs" company="KCSPL">
//     Copyright (c) KCSPL. All rights reserved.
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
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Common;
    using EcubeWebPortalCMS.Models;
    using Serilog;

    /// <summary>
    /// Class MovieShowAssignController.
    /// </summary>
    public class MovieShowAssignController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object i movie command.
        /// </summary>
        private readonly IMovieCommand objIMovieCommand = new MovieCommand();

        /// <summary>
        /// The object i movie command.
        /// </summary>
        private IMovieShowAssignCommand objIMovieShowAssignCommand = new MovieShowAssignCommand();

        /// <summary>
        /// The i movie show command.
        /// </summary>
        private IMovieShowCommand iMovieShowCommand = new MovieShowCommand();

        /// <summary>
        /// The i movie show command.
        /// </summary>
        private IMovieTheatreCommand iMovieTheatreCommand = new MovieTheatreCommand();

        /// <summary>
        /// The object movie show data.
        /// </summary>
        private MovieShowDataContext objMovieShowData = new MovieShowDataContext();

        /// <summary>
        /// Movies the show assign view.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult MovieShowAssignView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                MovieShowAssignModel objMoviesShow = new MovieShowAssignModel();
                int id = 0;
                int movieTheatreId = 0;
                DateTime sDate = Convert.ToDateTime("01/01/1900");
                DateTime eDate = Convert.ToDateTime("01/01/1900");
                DateTime dateToDisplay = Convert.ToDateTime("01/01/1900");
                string timeToDisplay = string.Empty;
                this.Session["XMLString"] = null;
                if (Request.QueryString.Count > 0)
                {
                    id = Convert.ToInt32(Request.QueryString["SID"].ToString().Decode());
                    var obj = this.objIMovieShowAssignCommand.GetMoviePeriod(1000, 1, string.Empty, string.Empty, Convert.ToBoolean(Request.QueryString["IsOld"].ToString())).Where(x => x.id == id).FirstOrDefault();

                    //id = Convert.ToInt32(this.Request.QueryString.ToString().Decode());
                    //bool IsOld = Convert.ToBoolean(false);
                    //var obj = this.objIMovieShowAssignCommand.GetMoviePeriod(1000, 1, string.Empty, string.Empty, IsOld).Where(x => x.id == id).FirstOrDefault();
                    sDate = obj.StartDate.DateSafe();
                    eDate = obj.EndDate.DateSafe();
                    dateToDisplay = obj.DateToDisplay.DateSafe();
                    timeToDisplay = obj.TimeToDisplay;
                    movieTheatreId = this.objIMovieShowAssignCommand.GetMoviePeriodbyid(id).MovieTheatreId ?? 0;
                    this.TempData["DisabledDates"] = "disabled";
                    objMoviesShow.Id = obj.id;
                    objMoviesShow.StrStartDate = sDate.ToString("dd/MM/yyyy");
                    objMoviesShow.StrEndDate = eDate.ToString("dd/MM/yyyy");
                    objMoviesShow.MovieTheatreId = movieTheatreId;
                    objMoviesShow.DateToDisplayMovie = dateToDisplay.ToString("dd/MM/yyyy");
                    objMoviesShow.TimeToDisplayMovie = timeToDisplay;
                }

                objMoviesShow.LstMovieTheatreScreen = this.iMovieTheatreCommand.GetAllMovieTheatreForDropDown();
                objMoviesShow.LstMovieShowDetails = this.objIMovieShowAssignCommand.GetMovieShowDetails(sDate, eDate, movieTheatreId, objMoviesShow.Id);
                return this.View(objMoviesShow);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Movies the show assign view.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Action  Result.</returns>
        [HttpPost]
        public ActionResult MovieShowAssignView(MovieShowAssignModel obj)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                MovieShowAssignModel objMoviesShow = new MovieShowAssignModel();
                DateTime sDate = Convert.ToDateTime("01/01/1900");
                DateTime eDate = Convert.ToDateTime("01/01/1900");

                objMoviesShow.LstMovieTheatreScreen = this.iMovieTheatreCommand.GetAllMovieTheatreForDropDown();
                objMoviesShow.LstMovieShowDetails = this.objIMovieShowAssignCommand.GetMovieShowDetails(sDate, eDate, obj.MovieTheatreId, objMoviesShow.Id);
                return this.View(objMoviesShow);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// ENS the code.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Return System.String.</returns>
        public string EnCode(string value)
        {
            return value.Encode();
        }

        /// <summary>
        /// Movies the show assign display.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult MovieShowAssignDisplay()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                UserCommand userCommand = new UserCommand();
                GetPageRightsByUserIdResult getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.MovieShowAssign));
                if (getPageRights == null || getPageRights.PageId == 0 || getPageRights.ViewAll == false || getPageRights.ViewDetail == false)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }

                this.ViewData["blAddRights"] = getPageRights.Add;
                this.ViewData["blEditRights"] = getPageRights.Edit;
                this.ViewData["blDeleteRights"] = getPageRights.Delete;
                MovieShowAssignModel objMoviesShow = new MovieShowAssignModel();
                return this.View(objMoviesShow);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
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
        public ActionResult BindMovieShowGrid(string sidx, string sord, int page, int rows, string filters, string search, bool displayPastRecord)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<MS_GetMovieShowPeriodResult> objMovieShowList = this.objIMovieShowAssignCommand.GetMoviePeriod(rows, page, search, sidx + " " + sord, displayPastRecord);
                if (objMovieShowList != null && objMovieShowList.Count > 0)
                {
                    return this.FillGridMovieShow(page, rows, objMovieShowList);
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
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Determines whether [is movie show exists] [the specified from date].
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">Parameter to date.</param>
        /// <param name="movieId">The movie identifier.</param>
        /// <param name="showId">The show identifier.</param>
        /// <returns>Return System.String.</returns>
        public string IsMovieShowExists(string fromDate, string toDate, int movieId, int theatreId, int showId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string message = string.Empty;
                bool exists = this.objIMovieShowAssignCommand.CheckMovieShowExists(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), movieId, theatreId, showId);
                if (exists)
                {
                    message = "Show is already exists and booked for other movie";
                }
                return message;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;

            }
        }

        /// <summary>
        /// Determines whether [is movie ticket booked] [the specified from date].
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">Parameter To date.</param>
        /// <param name="movieId">The movie identifier.</param>
        /// <param name="showId">The show identifier.</param>
        /// <returns>Return System.String.</returns>
        public string IsMovieTicketBooked(string fromDate, string toDate, int movieId, int theatreId, int showId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                string message = string.Empty;
                bool exists = this.objIMovieShowAssignCommand.CheckMovieShowExists(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), movieId, theatreId, showId);
                if (exists)
                {
                    message = "Tickets are already booked for selected movie and show time ";
                }
                return message;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Period Exists if Exists show or not.
        /// </summary>
        /// <param name="startDate">The startDate.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="movieTheaterId">The movie Theater movieTheatreId.</param>
        /// <returns>The Returns BOOL.</returns>
        public bool IsPeriodExists(string startDate, string endDate, int movieTheaterId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (Request.QueryString.Count > 0)
                {
                    return false;
                }
                else
                {
                    return this.objIMovieShowAssignCommand.IsExistsDates(Convert.ToDateTime(startDate), Convert.ToDateTime(endDate), movieTheaterId);
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
        /// Creates the session for save data.
        /// </summary>
        /// <param name="data">Parameter data.</param>
        /// <returns>Return System.String.</returns>
        public string CreateSessionforSaveData(string data)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (data.IndexOf("<ROOT>") > -1)
                {
                    this.Session["XMLString"] = null;
                    this.Session["XMLString"] = data;
                }
                else
                {
                    this.Session["XMLString"] = this.Session["XMLString"] + data;
                }

                return this.Session["XMLString"].ToString();
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Assigns the movie shows.
        /// </summary>
        /// <param name="showData">The show data.</param>
        /// <param name="movieTheatreId">The movie theatre identifier.</param>
        /// <param name="movieShowRateModel">The movie Show Rate.</param>
        /// <returns>Return System String.</returns>
        public ActionResult AssignMovieShows(string showData, int movieTheatreId, List<MovieShowRateModel> movieShowRateModel, int movieShowPeriodId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int showId = this.objIMovieShowAssignCommand.AssignShows(showData.Replace("%3C", "<").Replace("%3E", ">").Replace("%3A", ":").Replace("%20", string.Empty), movieTheatreId, movieShowPeriodId);
                if (showId > 0)
                {
                    if (movieShowRateModel != null)
                    {
                        foreach (var model in movieShowRateModel)
                        {
                            model.MovieShowPeriodId = showId;
                            if (this.objIMovieShowAssignCommand.SaveMovieShowRate(model) == 0)
                            {
                                showId = 0;
                            }
                        }
                    } 
                }
                return this.Json(showId, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Deletes the movie assign show.
        /// </summary>
        /// <param name="showPeriodId">The show period identifier.</param>
        /// <returns>JSON  Result.</returns>
        public JsonResult DeleteMovieAssignShow(string showPeriodId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string[] strMovie = showPeriodId.Split(',');
                showPeriodId = string.Empty;

                foreach (var item in strMovie)
                {
                    showPeriodId += item.Decode() + ",";
                }

                showPeriodId = showPeriodId.Substring(0, showPeriodId.Length - 1);
                MS_DeleteAssignMovieShowsResult result = this.objIMovieShowAssignCommand.DeleteAssignMovieShows(Convert.ToInt32(showPeriodId));
                if (result != null && result.Result == string.Empty)
                {
                    return this.Json(Functions.AlertMessage("Assign Movie Show", MessageType.DeleteSucess));
                }

                return this.Json(result.Result);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(Functions.AlertMessage("MovieAssign", MessageType.DeleteFail));
            }
        }

        /// <summary>
        /// Fills the grid movie show.
        /// </summary>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="objMovieShowList">The object movie show list.</param>
        /// <returns>Action  Result.</returns>
        private ActionResult FillGridMovieShow(int page, int rows, List<MS_GetMovieShowPeriodResult> objMovieShowList)
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
                                ShowStartDate = objMovieShow.StartDate,
                                ShowEndDate = objMovieShow.EndDate,
                                Id = objMovieShow.id.ToString().Encode(),
                                MovieTheatreName = objMovieShow.MovieTheatreName.ToString()
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
        /// Period Exists if Exists show or not.
        /// </summary>
        /// <param name="startDate">The startDate.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="movieTheaterId">The movie Theater movieTheatreId.</param>
        /// <returns>The Returns BOOL.</returns>
        public bool CheckExistMovieBooking(int showId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                return this.objIMovieShowAssignCommand.CheckExistMovieBooking(showId);
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
