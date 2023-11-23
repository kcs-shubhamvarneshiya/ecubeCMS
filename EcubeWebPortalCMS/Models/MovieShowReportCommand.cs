// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 10-08-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 10-08-2016
// ***********************************************************************
// <copyright file="MovieShowReportCommand.cs" company="KCSPL">
//     Copyright ©  2016
// </copyright>
// <summary>KCSPL</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace EcubeWebPortalCMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Transactions;
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Common;
    using Serilog;

    /// <summary>
    /// Class MovieShowReportCommand.
    /// </summary>
    public class MovieShowReportCommand
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context.
        /// </summary>
        private MovieShowReportDataContext objDataContext = null;

        /// <summary>
        /// Gets the movie show report.
        /// </summary>
        /// <param name="showId">The show identifier.</param>
        /// <returns>List&lt;MS_MovieShowReportResult  &gt;.</returns>
        public List<MS_MovieShowReportResult> GetMovieShowReport(int showId, int classId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new MovieShowReportDataContext())
                {
                    List<MS_MovieShowReportResult> objMovieShowReport = this.objDataContext.MS_MovieShowReport(showId, classId).ToList();
                    return objMovieShowReport;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return null;
            }
        }

        /// <summary>
        /// Gets the movie dates.
        /// </summary>
        /// <returns>List&lt;SelectListItem  &gt;.</returns>
        public List<SelectListItem> GetMovieDates()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            List<SelectListItem> objMovieShowList = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new MovieShowReportDataContext())
                {
                    objMovieShowList.Add(new SelectListItem { Text = "--Select Date--", Value = string.Empty });
                    List<MS_MovieShowDateResult> objMovieShowResultList = this.objDataContext.MS_MovieShowDate().ToList();
                    if (objMovieShowResultList != null && objMovieShowResultList.Count > 0)
                    {
                        foreach (var item in objMovieShowResultList)
                        {
                            objMovieShowList.Add(new SelectListItem { Text = Convert.ToDateTime(item.Date).ToString("dd/MM/yyyy"), Value = item.Date.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objMovieShowList;
        }

        /// <summary>
        /// Gets the movie list.
        /// </summary>
        /// <param name="date">Parameter date.</param>
        /// <returns>List&lt; SelectListItem  &gt;.</returns>
        public List<SelectListItem> GetMovieList(string date, int theatreId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            List<SelectListItem> objMovieShowList = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new MovieShowReportDataContext())
                {
                    objMovieShowList.Add(new SelectListItem { Text = "Select Movie", Value = string.Empty });
                    List<MS_MovieShowDateDetailResult> objMovieShowDateDetailResultList = this.objDataContext.MS_MovieShowDateDetail(Convert.ToDateTime(date)).ToList();
                    if (objMovieShowDateDetailResultList != null && objMovieShowDateDetailResultList.Count > 0)
                    {
                        foreach (var item in objMovieShowDateDetailResultList.Select(x => new { x.MovieId, x.MovieName, x.MovieLanguage, x.TheatreID }).Where(x => x.TheatreID == theatreId).Distinct())
                        {
                            if (!string.IsNullOrEmpty(item.MovieLanguage))
                            {
                                objMovieShowList.Add(new SelectListItem { Text = item.MovieName + " (" + item.MovieLanguage + ")", Value = item.MovieId.ToString() });
                            }
                            else
                            {
                                objMovieShowList.Add(new SelectListItem { Text = item.MovieName, Value = item.MovieId.ToString() });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objMovieShowList;
        }

        /// <summary>
        /// Gets the movie show list.
        /// </summary>
        /// <param name="date">Parameter date.</param>
        /// <param name="movieIdc">The movie identifier.</param>
        /// <returns>List&lt; SelectListItem  &gt;.</returns>
        public List<SelectListItem> GetMovieShowList(string date, int movieIdc, int theatreId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            List<SelectListItem> objMovieShowList = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new MovieShowReportDataContext())
                {
                    objMovieShowList.Add(new SelectListItem { Text = "Select Movie Show", Value = string.Empty });
                    List<MS_MovieShowDateDetailResult> objMovieShowDateDetailResultList = this.objDataContext.MS_MovieShowDateDetail(Convert.ToDateTime(date)).Where(x => x.MovieId == movieIdc && x.TheatreID == theatreId).OrderBy(x => x.StartTime.Substring(x.StartTime.Length - 2, 2)).ToList();
                    if (objMovieShowDateDetailResultList != null && objMovieShowDateDetailResultList.Count > 0)
                    {
                        foreach (var item in objMovieShowDateDetailResultList.Select(x => new { x.MovieShowId, x.StartTime }).Distinct())
                        {
                            objMovieShowList.Add(new SelectListItem { Text = item.StartTime, Value = item.MovieShowId.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objMovieShowList;
        }

        /// <summary>
        /// Gets the movie show identifier.
        /// </summary>
        /// <param name="date">Parameter date.</param>
        /// <param name="movieId">The movie identifier.</param>
        /// <param name="showId">The show identifier.</param>
        /// <returns>Return System.INT64.</returns>
        public long GetMovieShowId(string date, int theatreId, int movieId, int showId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            long movieShowId = 0;
            try
            {
                MovieDataContext objMovieData = new MovieDataContext();

                movieShowId = objMovieData.MovieTheatreShows.Where(x => x.MovieShowId == showId && x.MovieTheatreId == theatreId && x.ShowKeyDate.Value.Date == Convert.ToDateTime(date).Date && x.MovieId == movieId && (x.IsDeleted == false || x.IsDeleted == null)).FirstOrDefault().Id;

            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return movieShowId;
        }

        public List<GetMovieScannedTicketReportResult> MovieBookingReport(DateTime? movieDate, int movieId, int movieTheatreId, int showId, int ticketId, string membercode)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new MovieShowReportDataContext())
                {
                    List<GetMovieScannedTicketReportResult> objMovieShowReport = this.objDataContext.GetMovieScannedTicketReport(movieDate, movieId, movieTheatreId, showId, ticketId, membercode).ToList();
                    return objMovieShowReport;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return null;
            }
        }
    }
}