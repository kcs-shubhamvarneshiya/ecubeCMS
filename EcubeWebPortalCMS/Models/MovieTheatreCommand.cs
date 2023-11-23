// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-28-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-28-2016
// ***********************************************************************
// <copyright file="MovieTheatreCommand.cs" company="string.Empty">
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
    /// Class MovieTheatreCommand.
    /// </summary>
    public partial class MovieTheatreCommand : IMovieTheatreCommand
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context.
        /// </summary>
        private MovieTheatreDataContext objDataContext = null;

        /// <summary>
        /// Gets all movie theatre for drop down.
        /// </summary>
        /// <returns>List&lt;SelectList Item &gt;.</returns>
        public List<SelectListItem> GetAllMovieTheatreForDropDown()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            List<SelectListItem> objMovieTheatreList = new List<SelectListItem>();
            try
            {
                objMovieTheatreList.Add(new SelectListItem { Text = "Select Screen", Value = string.Empty });
                using (this.objDataContext = new MovieTheatreDataContext())
                {
                    List<GetMovieTheatreAllResult> objMovieTheatreResultList = this.objDataContext.GetMovieTheatreAll().ToList();
                    if (objMovieTheatreResultList != null && objMovieTheatreResultList.Count > 0)
                    {
                        foreach (var item in objMovieTheatreResultList)
                        {
                            objMovieTheatreList.Add(new SelectListItem { Text = item.MovieTheatreName, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objMovieTheatreList;
        }

        /// <summary>
        /// Gets the movie theatre by movie theatre identifier.
        /// </summary>
        /// <param name="lgMovieTheatreId">The long movie theatre identifier.</param>
        /// <returns>MovieTheatre Model.</returns>
        public MovieTheatreModel GetMovieTheatreByMovieTheatreId(long lgMovieTheatreId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            MovieTheatreModel objMovieTheatreModel = new MovieTheatreModel();
            try
            {
                using (this.objDataContext = new MovieTheatreDataContext())
                {
                    GetMovieTheatreByIdResult item = this.objDataContext.GetMovieTheatreById(lgMovieTheatreId).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objMovieTheatreModel.Id = item.Id;
                        objMovieTheatreModel.MovieTheatreName = item.MovieTheatreName;
                        objMovieTheatreModel.TheatreFloor = item.TheatreFloor;
                        objMovieTheatreModel.ServiceId = item.ServiceId;
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objMovieTheatreModel;
        }

        /// <summary>
        /// Saves the movie theatre.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        public long SaveMovieTheatre(MovieTheatreModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new MovieTheatreDataContext())
                    {
                        var result = this.objDataContext.InsertOrUpdateMovieTheatre(objSave.Id, objSave.MovieTheatreName, objSave.TheatreFloor, MySession.Current.UserId, PageMaster.MovieTheatre).FirstOrDefault();
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
        /// Deletes the movie theatre.
        /// </summary>
        /// <param name="strMovieTheatreIdList">The string movie theatre identifier list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>DeleteMovieTheatre Result.</returns>
        public DeleteMovieTheatreResult DeleteMovieTheatre(string strMovieTheatreIdList, long lgDeletedBy)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            DeleteMovieTheatreResult result = new DeleteMovieTheatreResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new MovieTheatreDataContext())
                    {
                        result = this.objDataContext.DeleteMovieTheatre(strMovieTheatreIdList, lgDeletedBy, PageMaster.MovieTheatre).ToList().FirstOrDefault();
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
        /// Determines whether [is movie theater exists] [the specified long movie theater identifier].
        /// </summary>
        /// <param name="lgMovieTheatreId">The long movie theater identifier.</param>
        /// <param name="strMovieTheatreName">Name of the string movie theater.</param>
        /// <returns><c>true</c> if [is movie theater exists] [the specified long movie theater identifier]; otherwise, <c>false</c>.</returns>
        public bool IsMovieTheatreExists(long lgMovieTheatreId, string strMovieTheatreName)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (this.objDataContext = new MovieTheatreDataContext())
                {
                    if (this.objDataContext.MovieTheatres.Where(x => x.Id != lgMovieTheatreId && x.MovieTheatreName == strMovieTheatreName && x.IsDeleted == false).Count() > 0)
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
        /// Searches the movie theatre.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List&lt;SearchMovieTheatre Result &gt;.</returns>
        public List<SearchMovieTheatreResult> SearchMovieTheatre(int inRow, int inPage, string strSearch, string strSort)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (this.objDataContext = new MovieTheatreDataContext())
                {
                    List<SearchMovieTheatreResult> objSearchMovieTheatreList = this.objDataContext.SearchMovieTheatre(inRow, inPage, strSearch, strSort).ToList();
                    return objSearchMovieTheatreList;
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
        /// GETSUBSERVICES this instance.
        /// </summary>
        /// <returns>List Select List Item.</returns>
        public List<SelectListItem> GetSubService(int serviceId, int serviceFor)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            List<SelectListItem> subServiceList = new List<System.Web.Mvc.SelectListItem>();
            try
            {
                using (this.objDataContext = new MovieTheatreDataContext())
                {
                    List<GetSubServiceByServiceIdandServiceForResult> subserviceListResult = this.objDataContext.GetSubServiceByServiceIdandServiceFor(serviceId, serviceFor).ToList();
                    if (subserviceListResult != null && subserviceListResult.Count > 0)
                    {
                        subServiceList.Add(new System.Web.Mvc.SelectListItem { Text = "Select", Value = "0" });
                        foreach (var item in subserviceListResult)
                        {
                            subServiceList.Add(new System.Web.Mvc.SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return subServiceList;
        }

        /// <summary>
        /// GETSUBSERVICES this instance.
        /// </summary>
        /// <returns>List Select List Item.</returns>
        public List<SelectListItem> GetService()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            List<SelectListItem> serviceList = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new MovieTheatreDataContext())
                {
                    List<CRM_GetServiceListResult> serviceListResult = this.objDataContext.CRM_GetServiceList().ToList();
                    if (serviceListResult != null && serviceListResult.Count > 0)
                    {
                        serviceList.Add(new System.Web.Mvc.SelectListItem { Text = "Select", Value = "0" });
                        foreach (var item in serviceListResult)
                        {
                            serviceList.Add(new System.Web.Mvc.SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return serviceList;
        }

        /// <summary>
        /// Gets Movie Theatre Class By theatre for drop down.
        /// </summary>
        /// <returns>List&lt;SelectList Item &gt;.</returns>
        public List<SelectListItem> GetMovieTheatreClassByTheatreIdForDropDown(int movieTheatreId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            List<SelectListItem> movieTheatreClassList = new List<SelectListItem>();
            try
            {
                movieTheatreClassList.Add(new SelectListItem { Text = "Select Class", Value = string.Empty });
                using (this.objDataContext = new MovieTheatreDataContext())
                {
                    List<GetMovieTheatreClassByTheatreIdResult> movieTheatreClassResultList = this.objDataContext.GetMovieTheatreClassByTheatreId(movieTheatreId, 0).ToList();
                    if (movieTheatreClassResultList != null && movieTheatreClassResultList.Count > 0)
                    {
                        foreach (var item in movieTheatreClassResultList)
                        {
                            movieTheatreClassList.Add(new SelectListItem { Text = item.ClassName, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return movieTheatreClassList;
        }

        /// <summary>
        /// Gets the movie theatre by movie theatre identifier.
        /// </summary>
        /// <param name="movieTheatreId">The long movie theatre identifier.</param>
        /// <returns>MovieTheatre Model.</returns>
        public List<GetMovieTheatreClassByTheatreIdResult> GetMovieTheatreClassByTheatreId(int movieTheatreId, int movieShowPeriodId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            List<GetMovieTheatreClassByTheatreIdResult> movieTheatreClassList = new List<GetMovieTheatreClassByTheatreIdResult>();
            try
            {
                using (this.objDataContext = new MovieTheatreDataContext())
                {
                    movieTheatreClassList = this.objDataContext.GetMovieTheatreClassByTheatreId(movieTheatreId, movieShowPeriodId).ToList();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return movieTheatreClassList;
        }

        /// <summary>
        /// Update Movie Theatre Class.
        /// </summary>
        /// <param name="movieTheatreClassModel">The long movie theatre identifier.</param>
        /// <returns>MovieTheatre Model.</returns>
        public void UpdateMovieTheatreClass(MovieTheatreClassModel movieTheatreClassModel)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new MovieTheatreDataContext())
                    {
                        this.objDataContext.UpdateMovieTheatreClassById(movieTheatreClassModel.Id, movieTheatreClassModel.MemberSubServiceId, movieTheatreClassModel.GuestSubServiceId, MySession.Current.UserId, PageMaster.MovieTheatre);
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }
        }
    }
}
