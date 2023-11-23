// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-23-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-23-2016
// ***********************************************************************
// <copyright file="CMSConfigCommand.cs" company="string.Empty">
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
    /// Class CMSConfigCommand.
    /// </summary>
    public partial class CMSConfigCommand : ICMSConfigCommand
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context.
        /// </summary>
        private CMSConfigDataContext objDataContext = null;

        /// <summary>
        /// Gets all CMS configuration for drop down.
        /// </summary>
        /// <returns>List&lt; SelectList Item  &gt;.</returns>
        public List<SelectListItem> GetAllCMSConfigForDropDown()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            List<SelectListItem> objCMSConfigList = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new CMSConfigDataContext())
                {
                    objCMSConfigList.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<MS_GetCMSConfigAllResult> objCMSConfigResultList = this.objDataContext.MS_GetCMSConfigAll().ToList();
                    if (objCMSConfigResultList != null && objCMSConfigResultList.Count > 0)
                    {
                        foreach (var item in objCMSConfigResultList)
                        {
                            objCMSConfigList.Add(new SelectListItem { Text = item.PersonName, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objCMSConfigList;
        }

        /// <summary>
        /// Gets the CMS configuration by CMS configuration identifier.
        /// </summary>
        /// <param name="lgCMSConfigId">The long CMS configuration identifier.</param>
        /// <returns>CMSConfig  Model.</returns>
        public CMSConfigModel GetCMSConfigByCMSConfigId(long lgCMSConfigId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            CMSConfigModel objCMSConfigModel = new CMSConfigModel();
            try
            {
                using (this.objDataContext = new CMSConfigDataContext())
                {
                    MS_GetCMSConfigByIdResult item = this.objDataContext.MS_GetCMSConfigById(lgCMSConfigId).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objCMSConfigModel.Id = item.Id;
                        objCMSConfigModel.Module = item.Module;
                        objCMSConfigModel.PersonName = item.PersonName;
                        objCMSConfigModel.EmailID = item.EmailID;
                        objCMSConfigModel.MobileNo = item.MobileNo;
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objCMSConfigModel;
        }

        /// <summary>
        /// Saves the CMS configuration.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        public long SaveCMSConfig(CMSConfigModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new CMSConfigDataContext())
                    {
                        var result = this.objDataContext.MS_InsertOrUpdateCMSConfig(objSave.Id, objSave.Module, objSave.PersonName, objSave.EmailID, objSave.MobileNo, MySession.Current.UserId, PageMaster.CMSConfig).FirstOrDefault();
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
        /// Deletes the CMS configuration.
        /// </summary>
        /// <param name="strCMSConfigIdList">The string CMS configuration identifier list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>MS_Delete CMSConfig  Result.</returns>
        public MS_DeleteCMSConfigResult DeleteCMSConfig(string strCMSConfigIdList, long lgDeletedBy)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            MS_DeleteCMSConfigResult result = new MS_DeleteCMSConfigResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new CMSConfigDataContext())
                    {
                        result = this.objDataContext.MS_DeleteCMSConfig(strCMSConfigIdList, lgDeletedBy, PageMaster.CMSConfig).ToList().FirstOrDefault();
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
        /// Determines whether [is CMS configuration exists] [the specified long CMS configuration identifier].
        /// </summary>
        /// <param name="lgCMSConfigId">The long CMS configuration identifier.</param>
        /// <param name="strCMSConfigName">Name of the string CMS configuration.</param>
        /// <returns><c>true</c> if [is CMS configuration exists] [the specified long CMS configuration identifier]; otherwise, <c>false</c>.</returns>
        public bool IsCMSConfigExists(long lgCMSConfigId, string strCMSConfigName)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (this.objDataContext = new CMSConfigDataContext())
                {
                    if (this.objDataContext.CMSConfigs.Where(x => x.Id != lgCMSConfigId && x.PersonName == strCMSConfigName && x.IsDeleted == false).Count() > 0)
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
        /// Searches the CMS configuration.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List&lt;MS_Search CMSConfig Result  &gt;.</returns>
        public List<MS_SearchCMSConfigResult> SearchCMSConfig(int inRow, int inPage, string strSearch, string strSort)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new CMSConfigDataContext())
                {
                    List<MS_SearchCMSConfigResult> objSearchCMSConfigList = this.objDataContext.MS_SearchCMSConfig(inRow, inPage, strSearch, strSort).ToList();
                    return objSearchCMSConfigList;
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
