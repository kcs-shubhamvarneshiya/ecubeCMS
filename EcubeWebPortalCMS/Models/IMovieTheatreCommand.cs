// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-28-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-28-2016
// ***********************************************************************
// <copyright file="IMovieTheatreCommand.cs" company="string.Empty">
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
    using System.Web.Mvc;

    /// <summary>
    /// Interface IMovieTheaterCommand.
    /// </summary>
    public interface IMovieTheatreCommand
    {
        /// <summary>
        /// Gets all movie theater for drop down.
        /// </summary>
        /// <returns>List&lt; SelectList Item   &gt;.</returns>
        List<SelectListItem> GetAllMovieTheatreForDropDown();

        /// <summary>
        /// Gets the movie theater by movie theater identifier.
        /// </summary>
        /// <param name="lgMovieTheatreId">The long movie theater identifier.</param>
        /// <returns>Movie Theater Model.</returns>
        MovieTheatreModel GetMovieTheatreByMovieTheatreId(long lgMovieTheatreId);

        /// <summary>
        /// Saves the movie theatre.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        long SaveMovieTheatre(MovieTheatreModel objSave);

        /// <summary>
        /// Deletes the movie theater.
        /// </summary>
        /// <param name="strMovieTheatreList">The string movie theater list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>DeleteMovie theater  Result.</returns>
        DeleteMovieTheatreResult DeleteMovieTheatre(string strMovieTheatreList, long lgDeletedBy);

        /// <summary>
        /// Determines whether [is movie theater exists] [the specified long movie theater identifier].
        /// </summary>
        /// <param name="lgMovieTheatreId">The long movie theater identifier.</param>
        /// <param name="strMovieTheatreName">Name of the string movie theater.</param>
        /// <returns><c>true</c> if [is movie theater exists] [the specified long movie theater identifier]; otherwise, <c>false</c>.</returns>
        bool IsMovieTheatreExists(long lgMovieTheatreId, string strMovieTheatreName);

        /// <summary>
        /// Searches the movie theatre.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List&lt; SearchMovieTheatreResult  &gt;.</returns>
        List<SearchMovieTheatreResult> SearchMovieTheatre(int inRow, int inPage, string strSearch, string strSort);

        /// <summary>
        /// GETSUBSERVICES this instance.
        /// </summary>
        /// <returns>List Select List Item.</returns>
        List<SelectListItem> GetSubService(int serviceId, int serviceFor);

        /// <summary>
        /// GetService this instance.
        /// </summary>
        /// <returns>List Select List Item.</returns>
        List<SelectListItem> GetService();

        /// <summary>
        /// Gets Movie Theatre Class By theatre for drop down.
        /// </summary>
        /// <returns>List&lt;SelectList Item &gt;.</returns>
        List<SelectListItem> GetMovieTheatreClassByTheatreIdForDropDown(int movieTheatreId);

        /// <summary>
        /// Get Movie Theatre Class By TheatreId.
        /// </summary>
        /// <param name="movieTheatreId">The movie Theatre Id.</param>
        /// <returns>List&lt; GetMovieTheatreClassByTheatreId  &gt;.</returns>
        List<GetMovieTheatreClassByTheatreIdResult> GetMovieTheatreClassByTheatreId(int movieTheatreId, int movieShowPeriodId);

        /// <summary>
        /// Update Movie Theatre Class.
        /// </summary>
        /// <param name="movieTheatreClassModel">The movie Theatre class model.</param>
        /// <returns>List&lt; Update Movie Theatre Class  &gt;.</returns>
        void UpdateMovieTheatreClass(MovieTheatreClassModel movieTheatreClassModel);
    }
}
