// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-28-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-28-2016
// ***********************************************************************
// <copyright file="MovieShowCommand.cs" company="string.Empty">
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
    /// Class MovieShowCommand.
    /// </summary>
    public partial class MovieShowCommand : IMovieShowCommand
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context.
        /// </summary>
        private MovieShowDataContext objDataContext = null;

        /// <summary>
        /// Gets all movie show for drop down.
        /// </summary>
        /// <returns>List&lt;SelectList Item &gt;.</returns>
        public List<SelectListItem> GetAllMovieShowForDropDown()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            List<SelectListItem> objMovieShowList = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new MovieShowDataContext())
                {
                    objMovieShowList.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<GetMovieShowAllResult> objMovieShowResultList = this.objDataContext.GetMovieShowAll().ToList();
                    if (objMovieShowResultList != null && objMovieShowResultList.Count > 0)
                    {
                        foreach (var item in objMovieShowResultList)
                        {
                            objMovieShowList.Add(new SelectListItem { Text = item.MovieShowName + " (" + item.ShowStartDate.ToString("hh:mm tt") + " - " + item.ShowEndDate.ToString("hh:mm tt") + ")", Value = item.Id.ToString() });
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
        /// Gets the movie show by movie show identifier.
        /// </summary>
        /// <param name="lgMovieShowId">The long movie show identifier.</param>
        /// <returns>MovieShow Model.</returns>
        public MovieShowModel GetMovieShowByMovieShowId(long lgMovieShowId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            MovieShowModel objMovieShowModel = new MovieShowModel();
            try
            {
                using (this.objDataContext = new MovieShowDataContext())
                {
                    GetMovieShowByIdResult item = this.objDataContext.GetMovieShowById(lgMovieShowId).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objMovieShowModel.Id = item.Id;
                        objMovieShowModel.MovieShowName = item.MovieShowName;
                        objMovieShowModel.StrShowStartDate = item.ShowStartDate.ToString(Functions.DateTimeFormat.Split(' ')[1] + " " + Functions.DateTimeFormat.Split(' ')[2]);
                        objMovieShowModel.StrShowEndDate = item.ShowEndDate.ToString(Functions.DateTimeFormat.Split(' ')[1] + " " + Functions.DateTimeFormat.Split(' ')[2]);
                        objMovieShowModel.IsActive = item.IsActive;
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objMovieShowModel;
        }

        /// <summary>
        /// Saves the movie show.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        public long SaveMovieShow(MovieShowModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new MovieShowDataContext())
                    {
                        var result = this.objDataContext.InsertOrUpdateMovieShow(objSave.Id, objSave.MovieShowName, objSave.StrShowStartDate.ToString().DateSafe(), objSave.StrShowEndDate.DateSafe(), MySession.Current.UserId, PageMaster.MovieShow, objSave.IsActive).FirstOrDefault();
                        if (result != null)
                        {
                            objSave.Id = result.InsertedId;
                        }
                    }

                    scope.Complete();
                }

                return objSave.Id;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return 0;
            }
        }

        /// <summary>
        /// Deletes the movie show.
        /// </summary>
        /// <param name="strMovieShowIdList">The string movie show identifier list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>DeleteMovieShow Result.</returns>
        public DeleteMovieShowResult DeleteMovieShow(string strMovieShowIdList, long lgDeletedBy)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            DeleteMovieShowResult result = new DeleteMovieShowResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new MovieShowDataContext())
                    {
                        result = this.objDataContext.DeleteMovieShow(strMovieShowIdList, lgDeletedBy, PageMaster.MovieShow).ToList().FirstOrDefault();
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return result;
        }

        /// <summary>
        /// Determines whether [is movie show exists] [the specified LG movie show identifier].
        /// </summary>
        /// <param name="lgMovieShowId">The LG movie show identifier.</param>
        /// <param name="strMovieShowName">Name of the string movie show.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns><c>true</c> if [is movie show exists] [the specified LG movie show identifier]; otherwise, <c>false</c>.</returns>
        public bool IsMovieShowExists(long lgMovieShowId, string strMovieShowName, DateTime startDate, DateTime endDate)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (this.objDataContext = new MovieShowDataContext())
                {
                    if (this.objDataContext.MovieShow.Where(x => x.Id != lgMovieShowId && x.IsDeleted == false && (startDate == x.ShowStartDate) && (endDate == x.ShowEndDate)).Count() > 0)
                    {
                        return true;
                    }

                    return false;
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
        /// Searches the movie show.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List&lt;SearchMovieShow Result &gt;.</returns>
        public List<SearchMovieShowResult> SearchMovieShow(int inRow, int inPage, string strSearch, string strSort)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new MovieShowDataContext())
                {
                    List<SearchMovieShowResult> objSearchMovieShowList = this.objDataContext.SearchMovieShow(inRow, inPage, strSearch, strSort).ToList();
                    return objSearchMovieShowList;
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
