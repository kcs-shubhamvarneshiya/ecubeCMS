// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 10-08-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 10-08-2016
// ***********************************************************************
// <copyright file="MovieShowReportController.cs" company="KCSPL">
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
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Common;
    using EcubeWebPortalCMS.Models;
    using Serilog;

    /// <summary>
    /// Class MovieShowReportController.
    /// </summary>
    public class MovieShowReportController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object movie show.
        /// </summary>
        private MovieShowReportCommand objMovieShow = new MovieShowReportCommand();

        /// <summary>
        /// The i movie show command.
        /// </summary>
        private IMovieTheatreCommand iMovieTheatreCommand = new MovieTheatreCommand();

        /// <summary>
        /// Movies the show report.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult MovieShowReport()
        {
            UserCommand userCommand = new UserCommand();
            GetPageRightsByUserIdResult getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.MovieShowReport));
            if (getPageRights == null || getPageRights.PageId == 0 || getPageRights.ViewAll == false || getPageRights.ViewDetail == false)
            {
                return this.RedirectToAction("PermissionRedirectPage", "Home");
            }
            MovieShowReportModel obj = new MovieShowReportModel();
            List<SelectListItem> lst = new List<SelectListItem>();
            obj.LstMovieDate = this.objMovieShow.GetMovieDates();
            obj.LstMovieTheatreScreen = this.iMovieTheatreCommand.GetAllMovieTheatreForDropDown();
            obj.MovieTheatreId = Convert.ToInt16(obj.LstMovieTheatreScreen.Where(x => x.Value != "").FirstOrDefault().Value);
            obj.LstMovieTheatreClass = this.iMovieTheatreCommand.GetMovieTheatreClassByTheatreIdForDropDown(obj.MovieTheatreId);
            obj.MovieClassId = Convert.ToInt16(obj.LstMovieTheatreClass.Where(x => x.Value != "").FirstOrDefault().Value);
            obj.LstMovie = lst;
            obj.LstMovieShows = lst;
            obj.WindowsTitle = Functions.GetSettings("eCubePOSWindowTitle");
            return this.View(obj);
        }

        /// <summary>
        /// Gets the movie list by date.
        /// </summary>
        /// <param name="date">Parameter Date.</param>
        /// <returns>JSON  Result.</returns>
        public JsonResult GetMovieListByDate(string date, int theatreId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                return this.Json(this.objMovieShow.GetMovieList(date, theatreId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Gets the movie show list by date.
        /// </summary>
        /// <param name="date">The date .</param>
        /// <param name="movieId">The movie identifier.</param>
        /// <returns>JSON Result.</returns>
        public JsonResult GetMovieShowListByDate(string date, int movieId, int theatreId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                return this.Json(this.objMovieShow.GetMovieShowList(date, movieId, theatreId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Gets the movie show report.
        /// </summary>
        /// <param name="date">The date .</param>
        /// <param name="movieId">The movie identifier.</param>
        /// <param name="showId">The show identifier.</param>
        /// <returns>JSON Result.</returns>
        public JsonResult GetMovieShowReport(string date, int theatreId, int classId, int movieId, int showId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int movieShowId = Convert.ToInt32(this.objMovieShow.GetMovieShowId(date, theatreId, movieId, showId));
                return this.Json(this.objMovieShow.GetMovieShowReport(movieShowId, classId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Gets the movie list by date.
        /// </summary>
        /// <param name="date">Parameter Date.</param>
        /// <returns>JSON  Result.</returns>
        public JsonResult GetMovieClassListBytheaterId(int theatreId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                return this.Json(this.iMovieTheatreCommand.GetMovieTheatreClassByTheatreIdForDropDown(theatreId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        public ActionResult MovieBookingReport()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                UserCommand userCommand = new UserCommand();
                GetPageRightsByUserIdResult getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.MovieBookingReport));
                if (getPageRights == null || getPageRights.PageId == 0 || getPageRights.ViewAll == false || getPageRights.ViewDetail == false)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }
                MovieShowReportModel obj = new MovieShowReportModel();
                List<SelectListItem> lst = new List<SelectListItem>();
                obj.LstMovieDate = this.objMovieShow.GetMovieDates();
                obj.LstMovieTheatreScreen = this.iMovieTheatreCommand.GetAllMovieTheatreForDropDown();
                obj.MovieTheatreId = 0;
                obj.LstMovie = lst;
                obj.LstMovieShows = lst;
                obj.WindowsTitle = Functions.GetSettings("eCubePOSWindowTitle");
                return this.View(obj);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }
        public ActionResult BindMovieReportGrid(string sidx, string sord, int page, int rows, string movieDate, string movieId, string movieTheatreId, string showId, string ticketId, string memberCode)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<GetMovieScannedTicketReportResult> objMovieList = new List<GetMovieScannedTicketReportResult>();
                if (!string.IsNullOrEmpty(movieDate))
                {
                    objMovieList = this.objMovieShow.MovieBookingReport(Functions.ConvertDateTime(movieDate), movieId.IntSafe(), movieTheatreId.IntSafe(), showId.IntSafe(), ticketId.IntSafe(), memberCode);
                }
                else
                {
                    objMovieList = this.objMovieShow.MovieBookingReport(null, movieId.IntSafe(), movieTheatreId.IntSafe(), showId.IntSafe(), ticketId.IntSafe(), memberCode);
                }
                return this.MovieReportGrid(page, rows, objMovieList);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        private ActionResult MovieReportGrid(int page, int rows, List<GetMovieScannedTicketReportResult> objMovieBookingReport)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objMovieBookingReport.Count;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objMovie in objMovieBookingReport
                            select new
                            {
                                MovieName = objMovie.MovieName,
                                MovieShowName = objMovie.MovieShowName,
                                MemberName = objMovie.MemberName,
                                MCodeWithPrefix = objMovie.MCodeWithPrefix,
                                BookingId = objMovie.BookingId,
                                IsCheckedIn = objMovie.IsCheckedIn ? "Yes" : "No",
                                CheckedInTime = objMovie.CheckedInTime == null ? string.Empty : objMovie.CheckedInTime.Value.ToString("dd/MM/yyy hh:mm tt"),
                                CheckedInBy = objMovie.CheckedInBy
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
