// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-28-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-28-2016
// ***********************************************************************
// <copyright file="MovieCommand.cs" company="KCSPL">
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
    /// Class MovieCommand.
    /// </summary>
    public partial class MovieCommand : IMovieCommand
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context.
        /// </summary>
        private MovieDataContext objDataContext = null;

        /// <summary>
        /// Gets all movie for drop down.
        /// </summary>
        /// <returns>List&lt;SelectListItem  &gt;.</returns>
        public List<SelectListItem> GetAllMovieForDropDown()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            List<SelectListItem> objMovieList = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new MovieDataContext())
                {
                    objMovieList.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<GetMovieAllResult> objMovieResultList = this.objDataContext.GetMovieAll().ToList();
                    if (objMovieResultList != null && objMovieResultList.Count > 0)
                    {
                        foreach (var item in objMovieResultList)
                        {
                            objMovieList.Add(new SelectListItem { Text = item.MovieName, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objMovieList;
        }

        /// <summary>
        /// Gets the movie by movie identifier.
        /// </summary>
        /// <param name="lgMovieId">The long movie identifier.</param>
        /// <returns>Movie Model.</returns>
        public MovieModel GetMovieByMovieId(long lgMovieId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            MovieModel objMovieModel = new MovieModel();
            try
            {
                using (this.objDataContext = new MovieDataContext())
                {
                    GetMovieByIdResult item = this.objDataContext.GetMovieById(lgMovieId).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objMovieModel.Id = item.Id;
                        objMovieModel.MovieName = item.MovieName;
                        objMovieModel.MovieCategory = item.MovieCategory;
                        objMovieModel.MovieRating = item.MovieRating;
                        objMovieModel.MovieLanguage = item.MovieLanguage;
                        objMovieModel.MovieContentType = item.MovieContentType;
                        objMovieModel.MovieType = item.MovieType;
                        objMovieModel.UploadImage = item.UploadedImage;
                        objMovieModel.IsActive = (bool)item.IsActive;
                        objMovieModel.AppDescription = item.AppDescription;
                        objMovieModel.MobileImage = item.MobileImage;
                        objMovieModel.BeforeMinutes = item.EntryBefore;
                        objMovieModel.AfterMinutes = item.EntryAfter;
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objMovieModel;
        }

        /// <summary>
        /// Saves the movie.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <param name="objMovieTheatreShowList">The object movie theatre show list.</param>
        /// <returns>Returns System.INT64.</returns>
        public long SaveMovie(MovieModel objSave, List<MovieModel> objMovieTheatreShowList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                long movieId = 0;
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new MovieDataContext())
                    {
                        if (objSave.BeforeMinutes == 0 || objSave.BeforeMinutes == null)
                        {
                            objSave.BeforeMinutes = 60;
                        }
                        if (objSave.AfterMinutes == 0 || objSave.AfterMinutes == null)
                        {
                            objSave.AfterMinutes = 60;
                        }
                        var result = this.objDataContext.InsertOrUpdateMovie(objSave.Id, objSave.MovieName, objSave.MovieCategory, objSave.MovieRating, objSave.MovieLanguage, objSave.MovieContentType, objSave.MovieType, objSave.UploadImage, objSave.IsActive, MySession.Current.UserId, PageMaster.Movie, objSave.AppDescription, objSave.AfterMinutes, objSave.BeforeMinutes, objSave.MobileImage).FirstOrDefault();
                        if (result != null)
                        {
                            movieId = result.InsertedId;
                            if (objMovieTheatreShowList != null && objMovieTheatreShowList.Count > 0)
                            {
                                foreach (var item in objMovieTheatreShowList)
                                {
                                    if (item.IsDeleted && item.MovieTheatreShowId > 0)
                                    {
                                        this.objDataContext.DeleteMovieTheatreShow(item.MovieTheatreShowId.ToString(), MySession.Current.UserId, PageMaster.Movie);
                                    }
                                    else if (!item.IsDeleted)
                                    {
                                        this.objDataContext.InsertOrUpdateMovieTheatreShow(item.MovieTheatreShowId, item.MovieTheatreShowName, item.ShowKeyDate, item.MovieTheatreId, item.MovieShowId, movieId, item.MemberAmount, item.GuestAmount, MySession.Current.UserId, PageMaster.Movie);
                                    }
                                }
                            }
                        }
                    }

                    scope.Complete();
                }

                return movieId;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return 0;
            }
        }

        /// <summary>
        /// Deletes the movie.
        /// </summary>
        /// <param name="strMovieIdList">The string movie identifier list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>DeleteMovie Result.</returns>
        public DeleteMovieResult DeleteMovie(string strMovieIdList, long lgDeletedBy)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            DeleteMovieResult result = new DeleteMovieResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new MovieDataContext())
                    {
                        result = this.objDataContext.DeleteMovie(strMovieIdList, lgDeletedBy, PageMaster.Movie).ToList().FirstOrDefault();
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
        /// Determines whether [is movie exists] [the specified long movie identifier].
        /// </summary>
        /// <param name="lgMovieId">The long movie identifier.</param>
        /// <param name="strMovieName">Name of the string movie.</param>
        /// <returns><c>true</c> if [is movie exists] [the specified long movie identifier]; otherwise, <c>false</c>.</returns>
        public bool IsMovieExists(long lgMovieId, string strMovieName)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new MovieDataContext())
                {
                    if (this.objDataContext.Movies.Where(x => x.Id != lgMovieId && x.MovieName == strMovieName && x.IsDeleted == false).Count() > 0)
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
        /// Determines whether [is movie show exists] [the specified LG movie identifier].
        /// </summary>
        /// <param name="lgMovieId">The LG movie identifier.</param>
        /// <param name="movieShowId">The movie show identifier.</param>
        /// <param name="showDate">The show date.</param>
        /// <param name="theatreId">The theatre identifier.</param>
        /// <returns><c>true</c> if [is movie show exists] [the specified LG movie identifier]; otherwise, <c>false</c>.</returns>
        public bool IsMovieShowExists(long lgMovieId, int movieShowId, DateTime showDate, int theatreId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new MovieDataContext())
                {
                    if (this.objDataContext.MovieTheatreShows.Where(x => x.Id != lgMovieId && x.ShowKeyDate == showDate && x.MovieTheatreId == theatreId && x.IsDeleted == false).Count() > 0)
                    {
                        string id = this.objDataContext.MovieTheatreShows.Where(x => x.Id != lgMovieId && x.ShowKeyDate == showDate && x.MovieTheatreId == theatreId && x.IsDeleted == false).FirstOrDefault().Id.ToString();
                        var data = this.objDataContext.GetMovieTheatreShowById(Convert.ToInt32(id));
                        if (data != null)
                        {
                            long i = data.FirstOrDefault().Id;
                            if (i != 0)
                            {
                                return true;
                            }
                        }

                        return false;
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
        /// Searches the movie.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <param name="isActive">The is Active Movie.</param>
        /// <returns>List&lt;SearchMovieResult  &gt;.</returns>
        public List<SearchMovieResult> SearchMovie(int inRow, int inPage, string strSearch, string strSort, bool isActive)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new MovieDataContext())
                {
                    List<SearchMovieResult> objSearchMovieList = this.objDataContext.SearchMovie(inRow, inPage, strSearch, strSort, isActive).ToList();
                    return objSearchMovieList;
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
        /// Saves the movie theatre show.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        public long SaveMovieTheatreShow(MovieModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new MovieDataContext())
                    {
                        var result = this.objDataContext.InsertOrUpdateMovieTheatreShow(objSave.MovieTheatreShowId, objSave.MovieTheatreShowName, objSave.StrShowKeyDate.DateNullSafe(), objSave.MovieTheatreId, objSave.MovieShowId, objSave.Id, objSave.MemberAmount, objSave.GuestAmount, MySession.Current.UserId, PageMaster.Movie).FirstOrDefault();
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
        /// Deletes the movie theatre show.
        /// </summary>
        /// <param name="strMovieTheatreShowIdList">The string movie theatre show identifier list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>DeleteMovieTheatreShow Result.</returns>
        public DeleteMovieTheatreShowResult DeleteMovieTheatreShow(string strMovieTheatreShowIdList, long lgDeletedBy)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            DeleteMovieTheatreShowResult result = new DeleteMovieTheatreShowResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new MovieDataContext())
                    {
                        result = this.objDataContext.DeleteMovieTheatreShow(strMovieTheatreShowIdList, lgDeletedBy, PageMaster.Movie).ToList().FirstOrDefault();
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
        /// Gets the movie theatre show by movie identifier.
        /// </summary>
        /// <param name="lgMovieId">The long movie identifier.</param>
        /// <returns>List&lt;MovieModel  &gt;.</returns>
        public List<MovieModel> GetMovieTheatreShowByMovieId(long lgMovieId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            List<MovieModel> objMovieTheatreShowList = new List<MovieModel>();
            try
            {
                using (this.objDataContext = new MovieDataContext())
                {
                    List<GetMovieTheatreShowByMovieIdResult> objMovieTheatreShowResultList = this.objDataContext.GetMovieTheatreShowByMovieId(lgMovieId).ToList();
                    long lgCount = 1;
                    if (objMovieTheatreShowResultList != null && objMovieTheatreShowResultList.Count > 0)
                    {
                        foreach (var item in objMovieTheatreShowResultList)
                        {
                            var objMovieModel = new MovieModel();
                            objMovieModel.MovieTheatreShowId = item.Id;
                            objMovieModel.MovieTheatreShowName = item.MovieTheatreShowName;
                            objMovieModel.ShowKeyDate = item.ShowKeyDate;
                            objMovieModel.StrShowKeyDate = item.ShowKeyDate != null ? item.ShowKeyDate.Value.ToString(Functions.DateFormat) : string.Empty;
                            objMovieModel.MovieTheatreId = item.MovieTheatreId.Value;
                            objMovieModel.MovieShowId = item.MovieShowId.Value;
                            objMovieModel.MemberAmount = item.MemberAmount.Value;
                            objMovieModel.GuestAmount = item.GuestAmount.Value;
                            objMovieModel.Id = lgCount;
                            objMovieTheatreShowList.Add(objMovieModel);
                            lgCount++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objMovieTheatreShowList;
        }

        /// <summary>
        /// Gets the movie theatre show by movie identifier.
        /// </summary>
        /// <param name="movieId">The long movie identifier.</param>
        /// <returns>List of Movie Shows while InActivating Movie.</returns>
        public List<GetMovieShowForInActiveMovieResult> GetMovieShowForInActiveMovie(int movieId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            List<GetMovieShowForInActiveMovieResult> MovieShowForInActiveMovieResult = new List<GetMovieShowForInActiveMovieResult>(); //= this.objDataContext.GetMovieTheatreShowByMovieId(lgMovieId).ToList();
            try
            {
                using (this.objDataContext = new MovieDataContext())
                {

                    MovieShowForInActiveMovieResult = objDataContext.GetMovieShowForInActiveMovie(movieId).ToList();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            if (MovieShowForInActiveMovieResult == null)
            {
                MovieShowForInActiveMovieResult = new List<GetMovieShowForInActiveMovieResult>();
            }
            return MovieShowForInActiveMovieResult;
        }
    }
}
