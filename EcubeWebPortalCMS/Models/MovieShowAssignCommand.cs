// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 10-08-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 06-13-2017
// ***********************************************************************
// <copyright file="MovieShowAssignCommand.cs" company="KCSPL">
//     Copyright (c) KCSPL. All rights reserved.
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
    /// Class MovieShowAssignCommand.
    /// </summary>
    public partial class MovieShowAssignCommand : IMovieShowAssignCommand
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context.
        /// </summary>
        private MovieShowAssignDataContext objDataContext = null;

        /// <summary>
        /// Gets the movie period.
        /// </summary>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="page">Parameter page.</param>
        /// <param name="search">The search.</param>
        /// <param name="sord">Parameter SORD.</param>
        /// <returns>List&lt;MS_GetMovieShowPeriodResult  &gt;.</returns>
        public List<MS_GetMovieShowPeriodResult> GetMoviePeriod(int rows, int page, string search, string sord, bool displayPastRecord)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            MovieShowAssignModel objMovieShowModel = new MovieShowAssignModel();
            try
            {
                using (this.objDataContext = new MovieShowAssignDataContext())
                {
                    List<MS_GetMovieShowPeriodResult> item = this.objDataContext.MS_GetMovieShowPeriod(rows, page, search, sord, displayPastRecord).ToList();
                    return item;
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
        /// Insert or the update movie show period.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Returns System.INT32.</returns>
        public int InsertorUpdateMovieShowPeriod(MovieShowAssignModel obj)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            MovieShowAssignModel objMovieShowModel = new MovieShowAssignModel();
            try
            {
                using (this.objDataContext = new MovieShowAssignDataContext())
                {
                    int item = this.objDataContext.MS_InsertorUpdateMovieShowPeriod(obj.Id, obj.StartDate, obj.EndDate, obj.CreatedBy);
                    return item;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return 0;
            }
        }

        /// <summary>
        /// Deletes the movie show period.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Returns System.INT32.</returns>
        public int DeleteMovieShowPeriod(MovieShowAssignModel obj)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            MovieShowAssignModel objMovieShowModel = new MovieShowAssignModel();
            try
            {
                using (this.objDataContext = new MovieShowAssignDataContext())
                {
                    int item = this.objDataContext.MS_DeleteMovieShowPeriod(obj.Id, obj.CreatedBy);
                    return item;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return 0;
            }
        }

        /// <summary>
        /// Gets the movie show details.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="movieTheatreId">The movie theatre identifier.</param>
        /// <returns>List MovieShow Details for Period.</returns>
        public List<MovieShowDetailsforPeriod> GetMovieShowDetails(DateTime startDate, DateTime endDate, int movieTheatreId, int movieShowPeriodId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            MovieShowAssignModel objMovieShowModel = new MovieShowAssignModel();

            try
            {
                using (this.objDataContext = new MovieShowAssignDataContext())
                {
                    List<MovieShowDetailsforPeriod> list = new List<MovieShowDetailsforPeriod>();
                    var result =
                  this.objDataContext.ExecuteQuery<MovieShowDetailsforPeriod>("exec MS_GetMovieShowDetailsByPeriodId {0}, {1},{2},{3}", startDate, endDate, movieTheatreId, movieShowPeriodId);
                    foreach (var d in result)
                    {
                        MovieShowDetailsforPeriod obj = new MovieShowDetailsforPeriod();
                        obj.MovieName = d.MovieName;
                        obj.MovieId = d.MovieId;
                        obj.S1 = d.S1;
                        obj.S2 = d.S2;
                        obj.S3 = d.S3;
                        obj.S4 = d.S4;
                        obj.S5 = d.S5;
                        obj.S6 = d.S6;
                        obj.S7 = d.S7;
                        obj.S8 = d.S8;
                        obj.S9 = d.S9;
                        obj.S10 = d.S10;
                        obj.S11 = d.S11;
                        obj.S12 = d.S12;
                        list.Add(obj);
                    }

                    return list;
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
        /// Determines whether [is exists dates] [the specified start date].
        /// </summary>
        /// <param name="startDate">The start date PARAM.</param>
        /// <param name="endDate">The end date PARAM.</param>
        /// <param name="movieTheatreId">The movie theatre identifier.</param>
        /// <returns>If otherwise.</returns>
        public bool IsExistsDates(DateTime startDate, DateTime endDate, int movieTheatreId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            bool isExists = false;
            try
            {
                using (this.objDataContext = new MovieShowAssignDataContext())
                {
                    var d = this.objDataContext.MovieShowPeriods.Where(x => x.IsDeleted == false && x.MovieTheatreId == movieTheatreId && ((startDate >= x.StartDate && startDate <= x.EndDate) || (endDate >= x.StartDate && endDate <= x.EndDate))).ToList();
                    if (d.Count > 0)
                    {
                        isExists = true;
                    }

                    return isExists;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return false;
            }
        }

        /// <summary>
        /// Assigns the shows.
        /// </summary>
        /// <param name="xML">Parameter xml.</param>
        /// <param name="movieTheatreId">The movie theatre identifier.</param>
        /// <returns>Return System.String.</returns>
        public int AssignShows(string xML, int movieTheatreId, int movieShowPeriodId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            int showid = 0;
            try
            {
                using (this.objDataContext = new MovieShowAssignDataContext())
                {
                    this.objDataContext.CommandTimeout = 0;
                    var result = this.objDataContext.MS_AssignMovieShows(xML, movieTheatreId, movieShowPeriodId).ToList();
                    if (result.Count > 0)
                    {
                        showid = result[0].MovieShowPeriodId;
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }
            return showid;
        }

        /// <summary>
        /// Deletes the assign movie shows.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>MS_DeleteAssignMovieShows Result.</returns>
        public MS_DeleteAssignMovieShowsResult DeleteAssignMovieShows(int id)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new MovieShowAssignDataContext())
                {
                    return this.objDataContext.MS_DeleteAssignMovieShows(id).FirstOrDefault();
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
        /// Checks the movie show exists.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="showId">The show identifier.</param>
        /// <param name="theatreId">The theatre identifier.</param>
        /// <param name="showDate">The show date.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool CheckMovieShowExists(DateTime startDate, DateTime endDate, int movieId, int theatreId, int showId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (this.objDataContext = new MovieShowAssignDataContext())
                {
                    if (this.objDataContext.CheckMovieExistByShowTime(startDate, endDate, movieId, theatreId, showId).FirstOrDefault().ShowCount > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return false;
            }
        }

        /// <summary>
        /// Get Movie Period by id.
        /// </summary>
        /// <param name="id">The id parameter.</param>
        /// <returns>The returns movie show Period.</returns>
        public MovieShowPeriod GetMoviePeriodbyid(int id)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new MovieShowAssignDataContext())
                {
                    MovieShowPeriod movieShowPeriod = this.objDataContext.MovieShowPeriods.Where(x => x.id == id).FirstOrDefault();
                    if (movieShowPeriod.MovieTheatreId > 0)
                    {
                        return movieShowPeriod;
                    }

                    return new MovieShowPeriod();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return new MovieShowPeriod();
            }
        }

        /// <summary>
        /// Check Exist Movie Booking.
        /// </summary>
        /// <param name="startDate">The start date PARAM.</param>
        /// <param name="endDate">The end date PARAM.</param>
        /// <param name="movieTheatreId">The movie theatre identifier.</param>
        /// <returns>If otherwise.</returns>
        public bool CheckExistMovieBooking(int showId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            bool isExists = false;
            try
            {
                using (this.objDataContext = new MovieShowAssignDataContext())
                {
                    var d = this.objDataContext.GetMovieBookingCount(showId).FirstOrDefault();
                    if (d.BookingRecord > 0)
                    {
                        isExists = true;
                    }

                    return isExists;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return false;
            }
        }

        /// <summary>
        /// Check Exist Movie Booking.
        /// </summary>
        /// <param name="startDate">The start date PARAM.</param>
        /// <param name="endDate">The end date PARAM.</param>
        /// <param name="movieTheatreId">The movie theatre identifier.</param>
        /// <returns>If otherwise.</returns>
        public int SaveMovieShowRate(MovieShowRateModel movieShowRateModel)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            int resultId = 0;
            try
            {
                using (this.objDataContext = new MovieShowAssignDataContext())
                {
                    var result = this.objDataContext.SaveMovieShowRate(movieShowRateModel.Id, movieShowRateModel.MovieShowPeriodId, movieShowRateModel.ClassId, movieShowRateModel.MemberRate, movieShowRateModel.GuestRate, MySession.Current.UserId, PageMaster.MovieShow).FirstOrDefault();
                    if (result.InsertedId > 0)
                    {
                        resultId = result.InsertedId;
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }
            return resultId;
        }
    }
}