// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-23-2016
// ***********************************************************************
// <copyright file="BanquetCommand.cs" company="string.Empty">
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
    /// Class BanquetCommand.
    /// </summary>
    public partial class BanquetCommand : IBanquetCommand
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context.
        /// </summary>
        private BanquetDataContext objDataContext = null;

        /// <summary>
        /// Gets all banquet for drop down.
        /// </summary>
        /// <returns>List&lt;Select List Item  &gt;.</returns>
        public List<SelectListItem> GetAllBanquetForDropDown()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            List<SelectListItem> objBanquetList = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new BanquetDataContext())
                {
                    objBanquetList.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<MS_GetBanquetAllResult> objBanquetResultList = this.objDataContext.MS_GetBanquetAll().ToList();
                    if (objBanquetResultList != null && objBanquetResultList.Count > 0)
                    {
                        foreach (var item in objBanquetResultList)
                        {
                            objBanquetList.Add(new SelectListItem { Text = item.BanquetName, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objBanquetList;
        }

        /// <summary>
        /// Gets the banquet by banquet identifier.
        /// </summary>
        /// <param name="lgBanquetId">The long banquet identifier.</param>
        /// <returns>Banquet  Model.</returns>
        public BanquetModel GetBanquetByBanquetId(long lgBanquetId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            BanquetModel objBanquetModel = new BanquetModel();
            try
            {
                using (this.objDataContext = new BanquetDataContext())
                {
                    MS_GetBanquetByIdResult item = this.objDataContext.MS_GetBanquetById(lgBanquetId).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objBanquetModel.Id = item.Id;
                        objBanquetModel.BanquetName = item.BanquetName;
                        objBanquetModel.Description = item.Description;
                        objBanquetModel.MinPersonCapcity = item.MinPersonCapcity;
                        objBanquetModel.MaxPersonCapcity = item.MaxPersonCapcity;
                        objBanquetModel.ProfilePic = item.ProfilePic;
                        objBanquetModel.Terms = item.Terms;
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objBanquetModel;
        }

        /// <summary>
        /// Saves the banquet.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <param name="objBanquetDetailList">The object banquet detail list.</param>
        /// <returns>Returns System.INT64.</returns>
        public long SaveBanquet(BanquetModel objSave, List<BanquetModel> objBanquetDetailList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                long banquetId = 0;
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new BanquetDataContext())
                    {
                        var result = this.objDataContext.MS_InsertOrUpdateBanquet(objSave.Id, objSave.BanquetName.Trim(), objSave.Description.Trim(), objSave.MinPersonCapcity, objSave.MaxPersonCapcity, objSave.ProfilePic, objSave.Terms, MySession.Current.UserId, PageMaster.Banquet).FirstOrDefault();
                        if (result != null)
                        {
                            banquetId = result.InsertedId;
                            if (objBanquetDetailList != null && objBanquetDetailList.Count > 0)
                            {
                                foreach (var item in objBanquetDetailList)
                                {
                                    if (item.IsDeleted && item.BanquetDetailId > 0)
                                    {
                                        this.objDataContext.MS_DeleteBanquetDetail(item.BanquetDetailId.ToString(), MySession.Current.UserId, PageMaster.Banquet);
                                    }
                                    else if (!item.IsDeleted)
                                    {
                                        this.objDataContext.MS_InsertOrUpdateBanquetDetail(item.BanquetDetailId, banquetId, item.GallaryImage, MySession.Current.UserId, PageMaster.Banquet);
                                    }
                                }
                            }
                        }
                    }

                    scope.Complete();
                }

                return banquetId;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return 0;
            }
        }

        /// <summary>
        /// Deletes the banquet.
        /// </summary>
        /// <param name="strBanquetIdList">The string banquet identifier list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>MS_Delete Banquet  Result.</returns>
        public MS_DeleteBanquetResult DeleteBanquet(string strBanquetIdList, long lgDeletedBy)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            MS_DeleteBanquetResult result = new MS_DeleteBanquetResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new BanquetDataContext())
                    {
                        result = this.objDataContext.MS_DeleteBanquet(strBanquetIdList, lgDeletedBy, PageMaster.Banquet).ToList().FirstOrDefault();
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
        /// Determines whether [is banquet exists] [the specified long banquet identifier].
        /// </summary>
        /// <param name="lgBanquetId">The long banquet identifier.</param>
        /// <param name="strBanquetName">Name of the string banquet.</param>
        /// <returns><c>true</c> if [is banquet exists] [the specified long banquet identifier]; otherwise, <c>false</c>.</returns>
        public bool IsBanquetExists(long lgBanquetId, string strBanquetName)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new BanquetDataContext())
                {
                    if (this.objDataContext.Banquets.Where(x => x.Id != lgBanquetId && x.BanquetName == strBanquetName && x.IsDeleted == false).Count() > 0)
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
        /// Searches the banquet.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List&lt;MS_Search Banquet Result  &gt;.</returns>
        public List<MS_SearchBanquetResult> SearchBanquet(int inRow, int inPage, string strSearch, string strSort)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new BanquetDataContext())
                {
                    List<MS_SearchBanquetResult> objSearchBanquetList = this.objDataContext.MS_SearchBanquet(inRow, inPage, strSearch, strSort).ToList();
                    return objSearchBanquetList;
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
        /// Saves the banquet detail.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        public long SaveBanquetDetail(BanquetModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new BanquetDataContext())
                    {
                        var result = this.objDataContext.MS_InsertOrUpdateBanquetDetail(objSave.BanquetDetailId, objSave.Id, objSave.GallaryImage, MySession.Current.UserId, PageMaster.Banquet).FirstOrDefault();
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
        /// Deletes the banquet detail.
        /// </summary>
        /// <param name="strBanquetDetailIdList">The string banquet detail identifier list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>MS_DeleteBanquet Detail  Result.</returns>
        public MS_DeleteBanquetDetailResult DeleteBanquetDetail(string strBanquetDetailIdList, long lgDeletedBy)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            MS_DeleteBanquetDetailResult result = new MS_DeleteBanquetDetailResult();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new BanquetDataContext())
                    {
                        result = this.objDataContext.MS_DeleteBanquetDetail(strBanquetDetailIdList, lgDeletedBy, PageMaster.Banquet).ToList().FirstOrDefault();
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
        /// Gets the banquet detail by banquet identifier.
        /// </summary>
        /// <param name="lgBanquetId">The long banquet identifier.</param>
        /// <returns>List&lt; Banquet Model  &gt;.</returns>
        public List<BanquetModel> GetBanquetDetailByBanquetId(long lgBanquetId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            List<BanquetModel> objBanquetDetailList = new List<BanquetModel>();
            try
            {
                using (this.objDataContext = new BanquetDataContext())
                {
                    List<MS_GetBanquetDetailByBanquetIdResult> objBanquetDetailResultList = this.objDataContext.MS_GetBanquetDetailByBanquetId(lgBanquetId).ToList();
                    long lgCount = 1;
                    if (objBanquetDetailResultList != null && objBanquetDetailResultList.Count > 0)
                    {
                        foreach (var item in objBanquetDetailResultList)
                        {
                            var objBanquetModel = new BanquetModel();
                            objBanquetModel.BanquetDetailId = item.Id;
                            objBanquetModel.Id = lgCount;
                            objBanquetModel.GallaryImage = item.GallaryImage;
                            objBanquetDetailList.Add(objBanquetModel);
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

            return objBanquetDetailList;
        }
    }
}
