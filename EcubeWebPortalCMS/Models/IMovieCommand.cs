// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-28-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-28-2016
// ***********************************************************************
// <copyright file="IMovieCommand.cs" company="string.Empty">
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
    /// Interface IMovieCommand.
    /// </summary>
    public interface IMovieCommand
    {
        /// <summary>
        /// Gets all movie for drop down.
        /// </summary>
        /// <returns>List&lt; SelectList Item  &gt;.</returns>
        List<SelectListItem> GetAllMovieForDropDown();

        /// <summary>
        /// Gets the movie by movie identifier.
        /// </summary>
        /// <param name="lgMovieId">The long movie identifier.</param>
        /// <returns>Movie  Model.</returns>
        MovieModel GetMovieByMovieId(long lgMovieId);

        /// <summary>
        /// Saves the movie.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <param name="objMovieTheatreShow">The object movie theatre show.</param>
        /// <returns>Returns System.INT64.</returns>
        long SaveMovie(MovieModel objSave, List<MovieModel> objMovieTheatreShow);

        /// <summary>
        /// Deletes the movie.
        /// </summary>
        /// <param name="strMovieList">The string movie list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>Delete Movie  Result.</returns>
        DeleteMovieResult DeleteMovie(string strMovieList, long lgDeletedBy);

        /// <summary>
        /// Determines whether [is movie exists] [the specified long movie identifier].
        /// </summary>
        /// <param name="lgMovieId">The long movie identifier.</param>
        /// <param name="strMovieName">Name of the string movie.</param>
        /// <returns><c>true</c> if [is movie exists] [the specified long movie identifier]; otherwise, <c>false</c>.</returns>
        bool IsMovieExists(long lgMovieId, string strMovieName);

        /// <summary>
        /// Searches the movie.
        /// </summary>
        /// <param name="inRow">The in row parameter.</param>
        /// <param name="inPage">The in page parameter.</param>
        /// <param name="strSearch">The string search parameter.</param>
        /// <param name="strSort">The string sort parameter.</param>
        /// <param name="isActive">If set to <c>true</c> [is active].</param>
        /// <returns>List of SearchMovie Result search movie.</returns>
        List<SearchMovieResult> SearchMovie(int inRow, int inPage, string strSearch, string strSort, bool isActive);

        /// <summary>
        /// Saves the movie theatre show.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        long SaveMovieTheatreShow(MovieModel objSave);

        /// <summary>
        /// Deletes the movie theatre show.
        /// </summary>
        /// <param name="strMovieTheatreShowList">The string movie theatre show list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>Delete Movie Theater Show Result.</returns>
        DeleteMovieTheatreShowResult DeleteMovieTheatreShow(string strMovieTheatreShowList, long lgDeletedBy);

        /// <summary>
        /// Gets the movie theatre show by movie identifier.
        /// </summary>
        /// <param name="lgMovieId">The long movie identifier.</param>
        /// <returns>List&lt;Movie Model  &gt;.</returns>
        List<MovieModel> GetMovieTheatreShowByMovieId(long lgMovieId);

        /// <summary>
        /// Gets the movie theatre show by movie identifier.
        /// </summary>
        /// <param name="lgMovieId">The long movie identifier.</param>
        /// <returns>List&lt;Movie Model  &gt;.</returns>
        List<GetMovieShowForInActiveMovieResult> GetMovieShowForInActiveMovie(int movieId);

        /// <summary>
        /// Determines whether [is movie show exists] [the specified LG movie identifier].
        /// </summary>
        /// <param name="lgMovieId">The LG movie identifier.</param>
        /// <param name="movieShowId">The movie show identifier.</param>
        /// <param name="showDate">The show date.</param>
        /// <param name="theatreId">The theatre identifier.</param>
        /// <returns><c>true</c> if [is movie show exists] [the specified LG movie identifier]; otherwise, <c>false</c>.</returns>
        bool IsMovieShowExists(long lgMovieId, int movieShowId, DateTime showDate, int theatreId);
    }
}