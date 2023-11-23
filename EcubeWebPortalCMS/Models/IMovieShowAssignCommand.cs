// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 10-08-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 06-13-2017
// ***********************************************************************
// <copyright file="IMovieShowAssignCommand.cs" company="KCSPL">
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
    using System.Web.Mvc;

    /// <summary>
    /// Interface IMovieShowAssignCommand.
    /// </summary>
    public interface IMovieShowAssignCommand
    {
        /// <summary>
        /// Gets the movie show details.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="movieTheatreId">The movie theatre identifier.</param>
        /// <returns>List MovieShow Details FOR Period.</returns>
        List<MovieShowDetailsforPeriod> GetMovieShowDetails(DateTime startDate, DateTime endDate, int movieTheatreId, int movieShowPeriodId);

        /// <summary>
        /// Gets the movie period.
        /// </summary>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="page">Parameter page.</param>
        /// <param name="search">The search.</param>
        /// <param name="sord">Parameter SORD.</param>
        /// <returns>List MS_GetMovie Show Period Result.</returns>
        List<MS_GetMovieShowPeriodResult> GetMoviePeriod(int rows, int page, string search, string sord, bool displayPastRecord);

        /// <summary>
        /// INSERTORS the update movie show period.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>System INT32.</returns>
        int InsertorUpdateMovieShowPeriod(MovieShowAssignModel obj);

        /// <summary>
        /// Deletes the movie show period.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Returns System.INT32.</returns>
        int DeleteMovieShowPeriod(MovieShowAssignModel obj);

        /// <summary>
        /// Determines whether [is exists dates] [the specified start date].
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="movieTheatreId">The movie theatre identifier.</param>
        /// <returns>If otherwise.</returns>
        bool IsExistsDates(DateTime startDate, DateTime endDate, int movieTheatreId);

        /// <summary>
        /// Assigns the shows.
        /// </summary>
        /// <param name="xML">Parameter xml.</param>
        /// <param name="movieTheatreId">The movie theatre identifier.</param>
        /// <returns>Return System String.</returns>
        int AssignShows(string xML, int movieTheatreId, int movieShowPeriodId);

        /// <summary>
        /// Checks the movie show exists.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="showId">The show identifier.</param>
        /// <param name="theatreId">The theatre identifier.</param>
        /// <param name="showDate">The show date.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool CheckMovieShowExists(DateTime startDate, DateTime endDate, int movieId, int theatreId, int showId);

        /// <summary>
        /// Deletes the assign movie shows.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>MS_Delete Assign Movie Shows  Result.</returns>
        MS_DeleteAssignMovieShowsResult DeleteAssignMovieShows(int id);

        /// <summary>
        /// Get Movie Period by id.
        /// </summary>
        /// <param name="id">The movie period.</param>
        /// <returns>The returns object.</returns>
        MovieShowPeriod GetMoviePeriodbyid(int id);

        /// <summary>
        /// Determines whether [is exists dates] [the specified start date].
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="movieTheatreId">The movie theatre identifier.</param>
        /// <returns>If otherwise.</returns>
        bool CheckExistMovieBooking(int showId);

        /// <summary>
        /// Save Movie Show Rate.
        /// </summary>
        /// <param name="movieShowRateModel">The movie Show Rate Model.</param>
        /// <returns>If otherwise.</returns>
        int SaveMovieShowRate(MovieShowRateModel movieShowRateModel);
    }
}
