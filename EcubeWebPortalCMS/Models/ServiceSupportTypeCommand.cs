// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : darpan
// Created          : 11-10-2016
//
// Last Modified By : darpan
// Last Modified On : 11-10-2016
// ***********************************************************************
// <copyright file="ServiceSupportTypeCommand.cs" company="KCS">
//     Copyright ©  2016
// </copyright>
// <summary>ServiceSupportTypeCommand.cs</summary>
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
    using System.Web;
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Common;
    using Serilog;

    /// <summary>
    /// Class ServiceSupportTypeCommand.
    /// </summary>
    public partial class ServiceSupportTypeCommand : IServiceSupportTypeCommand
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context variable.
        /// </summary>
        private ServiceSupportTypeDataContext objDataContext = new ServiceSupportTypeDataContext();

        /// <summary>
        /// Gets the service support type by identifier.
        /// </summary>
        /// <param name="id">The identifier parameter.</param>
        /// <returns>ServiceSupportTypeModel get service support type by identifier.</returns>
        public ServiceSupportTypeModel GetServiceSupportTypeById(int id)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            ServiceSupportTypeModel objserviseSupportType = new ServiceSupportTypeModel();
            GetServicSupportTypeByIdResult item = this.objDataContext.GetServicSupportTypeById(id).FirstOrDefault();
            try
            {
                if (item != null)
                {
                    objserviseSupportType.Id = item.Id;
                    objserviseSupportType.TypeName = item.TypeName;
                    objserviseSupportType.Description = item.Description;
                    objserviseSupportType.IsActive = item.IsActive;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objserviseSupportType;
        }

        /// <summary>
        /// Saves the type of the service support.
        /// </summary>
        /// <param name="objSave">The object save parameter.</param>
        /// <returns>System.Integer64 save service support type.</returns>
        public long SaveServiceSupportType(ServiceSupportTypeModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new ServiceSupportTypeDataContext())
                    {
                        var result = this.objDataContext.CRM_InsertUpdateServiceSupportType(objSave.Id, objSave.TypeName, objSave.Description, objSave.IsActive, MySession.Current.UserId, 1);
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
        /// Searches the type of the service support.
        /// </summary>
        /// <param name="inRow">The in row parameter.</param>
        /// <param name="inPage">The in page parameter.</param>
        /// <param name="strSearch">The string search parameter.</param>
        /// <param name="strSort">The string sort parameter.</param>
        /// <returns>List&lt;CRMSearchServiceSupportTypeResult&gt; search service support type.</returns>
        public List<CRMSearchServiceSupportTypeResult> SearchServiceSupportType(int inRow, int inPage, string strSearch, string strSort)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new ServiceSupportTypeDataContext())
                {
                    List<CRMSearchServiceSupportTypeResult> objSearch = this.objDataContext.CRMSearchServiceSupportType(inRow, inPage, strSearch, strSort).ToList();
                    return objSearch;
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
        /// Deletes the type of the service support.
        /// </summary>
        /// <param name="strSupportIdList">The string support identifier list parameter.</param>
        /// <param name="lgDeletedBy">The deleted by parameter.</param>
        /// <returns>CRM_DeleteServiceSupportTypeResult delete service support type.</returns>
        public CRM_DeleteServiceSupportTypeResult DeleteServiceSupportType(string strSupportIdList, int lgDeletedBy)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            CRM_DeleteServiceSupportTypeResult result = new CRM_DeleteServiceSupportTypeResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new ServiceSupportTypeDataContext())
                    {
                        result = this.objDataContext.CRM_DeleteServiceSupportType(strSupportIdList, lgDeletedBy, Convert.ToInt32(PageMaster.ServiceSupportType)).ToList().FirstOrDefault();
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
        /// Determines whether [is type name exists] [the specified long name identifier].
        /// </summary>
        /// <param name="lgNameId">The long name identifier parameter.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <returns><c>true</c> if [is type name exists] [the specified long name identifier]; otherwise, <c>false</c>.</returns>
        public bool IsTypeNameExists(long lgNameId, string typeName)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (this.objDataContext.CRMServiceSupportTypes.Where(x => x.Id != lgNameId && x.TypeName == typeName).Count() > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return false;
            }
        }
    }
}