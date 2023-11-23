// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : pratikp
// Created          : 11-10-2016
//
// Last Modified By : pratikp
// Last Modified On : 11-10-2016
// ***********************************************************************
// <copyright file="ServiceSupportCommand.cs" company="KCS">
//     Copyright ©  2016
// </copyright>
// <summary>ServiceSupportCommand</summary>
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
    /// Class ServiceSupportCommand.
    /// </summary>
    public class ServiceSupportCommand : IServiceSupportCommand
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context.
        /// </summary>
        private ServiceSupportDataContext objDataContext = new ServiceSupportDataContext();

        /// <summary>
        /// Searches the service support.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List&lt;CRMSearch Service Support Result&gt;.</returns>
        public List<CRMSearchServiceSupportResult> SearchServiceSupport(int inRow, int inPage, string strSearch, string strSort)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (this.objDataContext = new ServiceSupportDataContext())
                {
                    List<CRMSearchServiceSupportResult> objSearchServiceSupportList = this.objDataContext.CRMSearchServiceSupport(inRow, inPage, strSearch, strSort).ToList();
                    return objSearchServiceSupportList;
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
        /// Gets the service support by identifier.
        /// </summary>
        /// <param name="lgServiceSupportById">The service support by identifier.</param>
        /// <returns>Service Support Model.</returns>
        public ServiceSupportModel GetServiceSupportById(long lgServiceSupportById)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            ServiceSupportModel objServiceSupportModel = new ServiceSupportModel();
            try
            {
                using (this.objDataContext = new ServiceSupportDataContext())
                {
                    CRMGetServiceSupportByIdResult item = this.objDataContext.CRMGetServiceSupportById(lgServiceSupportById).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objServiceSupportModel.Id = item.Id;
                        objServiceSupportModel.StatusId = item.StatusId;
                        objServiceSupportModel.ServiceSupportTypeId = item.ServiceSupportTypeId;
                        objServiceSupportModel.TypeName = item.TypeName;
                        objServiceSupportModel.MemberCode = item.MemberCode;
                        objServiceSupportModel.MemberName = item.MemberName;
                        objServiceSupportModel.MemberId = item.MemberId;
                        objServiceSupportModel.Description = item.Description;
                        objServiceSupportModel.Response = item.Response;
                        objServiceSupportModel.ResponseBy = item.ResponseBy;
                        objServiceSupportModel.CreatedOn = item.CreatedOn;
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objServiceSupportModel;
        }

        /// <summary>
        /// Saves the service support.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Return SaveServiceSupport.</returns>
        public long SaveServiceSupport(ServiceSupportModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new ServiceSupportDataContext())
                    {
                        var result = this.objDataContext.CRMUpdateServiceSupport(objSave.Id, objSave.ServiceSupportTypeId, objSave.StatusId, objSave.Description, objSave.Response, objSave.ResponseBy, MySession.Current.UserId, PageMaster.ServiceSupport).FirstOrDefault();
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
        /// Gets the status for drop down.
        /// </summary>
        /// <returns>List&lt;Select List Item&gt;.</returns>
        public List<SelectListItem> GetStatusForDropDown()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            List<SelectListItem> objStatusList = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new ServiceSupportDataContext())
                {
                    objStatusList.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<CRMGetStatusAllResult> objStatusResultList = this.objDataContext.CRMGetStatusAll().ToList();
                    if (objStatusResultList.Count > 0)
                    {
                        foreach (var item in objStatusResultList)
                        {
                            objStatusList.Add(new SelectListItem { Text = item.StatusDesc, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objStatusList;
        }

        /// <summary>
        /// Gets the support type for drop down.
        /// </summary>
        /// <returns>List&lt;Select List Item&gt;.</returns>
        public List<SelectListItem> GetSupportTypeForDropDown()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            List<SelectListItem> objSupportTypeList = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new ServiceSupportDataContext())
                {
                    objSupportTypeList.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<MS_GetServiceSupportTypeResult> objSupportTypeResultList = this.objDataContext.MS_GetServiceSupportType().ToList();
                    if (objSupportTypeResultList.Count > 0)
                    {
                        foreach (var item in objSupportTypeResultList)
                        {
                            objSupportTypeList.Add(new SelectListItem { Text = item.TypeName, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objSupportTypeList;
        }

        /// <summary>
        /// Gets the service report by identifier.
        /// </summary>
        /// <param name="lgServiceSupportById">The service support by identifier.</param>
        /// <returns>Service Support Model.</returns>
        public ServiceSupportModel GetServiceReportById(long lgServiceSupportById)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            ServiceSupportModel objServiceSupportModel = new ServiceSupportModel();
            try
            {
                using (this.objDataContext = new ServiceSupportDataContext())
                {
                    CRMGetServiceReportByIdResult item = this.objDataContext.CRMGetServiceReportById(lgServiceSupportById).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objServiceSupportModel.Id = item.Id;
                        objServiceSupportModel.TypeName = item.TypeName;
                        objServiceSupportModel.MemberCode = item.MemberCode;
                        objServiceSupportModel.MemberName = item.MemberName;
                        objServiceSupportModel.Description = item.Description;
                        objServiceSupportModel.ResponseBy = item.ResponseBy;
                        objServiceSupportModel.CompanyName = item.CompanyName;
                        objServiceSupportModel.CreatedOn = item.CreatedOn.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objServiceSupportModel;
        }

        /// <summary>
        /// Gets the service support report by identifier.
        /// </summary>
        /// <param name="lgServiceSupportById">The service support by identifier.</param>
        /// <returns>Get result .</returns>
        public long GetServiceSupportReportById(long lgServiceSupportById)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            long result = 0;
            try
            {
                ServiceSupportDataContext objDataContext = new ServiceSupportDataContext();
                result = this.objDataContext.CRMGetServiceReportById(lgServiceSupportById).FirstOrDefault().Id;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return 0;
            }

            return result;
        }
    }
}