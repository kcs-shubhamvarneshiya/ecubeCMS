// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-28-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-28-2016
// ***********************************************************************
// <copyright file="IMovieShowCommand.cs" company="string.Empty">
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
    /// Interface IMovieShowCommand.
    /// </summary>
    public interface IMovieShowCommand
    {
        /// <summary>
        /// Gets all movie show for drop down.
        /// </summary>
        /// <returns>List&lt; SelectListItem  &gt;.</returns>
        List<SelectListItem> GetAllMovieShowForDropDown();

        /// <summary>
        /// Gets the movie show by movie show identifier.
        /// </summary>
        /// <param name="lgMovieShowId">The long movie show identifier.</param>
        /// <returns>Movie Show Model.</returns>
        MovieShowModel GetMovieShowByMovieShowId(long lgMovieShowId);

        /// <summary>
        /// Saves the movie show.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        long SaveMovieShow(MovieShowModel objSave);

        /// <summary>
        /// Deletes the movie show.
        /// </summary>
        /// <param name="strMovieShowList">The string movie show list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>Delete Movie Show Result.</returns>
        DeleteMovieShowResult DeleteMovieShow(string strMovieShowList, long lgDeletedBy);

        /// <summary>
        /// Determines whether [is movie show exists] [the specified LG movie show identifier].
        /// </summary>
        /// <param name="lgMovieShowId">The LG movie show identifier.</param>
        /// <param name="strMovieShowName">Name of the string movie show.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns><c>true</c> if [is movie show exists] [the specified LG movie show identifier]; otherwise, <c>false</c>.</returns>
        bool IsMovieShowExists(long lgMovieShowId, string strMovieShowName, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Searches the movie show.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List; Search Movie Show Result;.</returns>
        List<SearchMovieShowResult> SearchMovieShow(int inRow, int inPage, string strSearch, string strSort);
    }
}
