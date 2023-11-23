// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : pratikp
// Created          : 11-12-2016
//
// Last Modified By : pratikp
// Last Modified On : 11-12-2016
// ***********************************************************************
// <copyright file="CustomPageCommand.cs" company="KCS">
//     Copyright ©  2016
// </copyright>
// <summary>CustomPageCommand.</summary>
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
    /// Class CustomPageCommand.
    /// </summary>
    public class CustomPageCommand : ICustomPageCommand
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context.
        /// </summary>
        private VendorDataContext objDataContext = new VendorDataContext();

        /// <summary>
        /// Gets the vendor by identifier.
        /// </summary>
        /// <param name="pageId">The page identifier.</param>
        /// <param name="pageName">Name of the page.</param>
        /// <returns>Vendor Model.</returns>
        public CustomPageModel GetCustomPageById(int pageId, string pageName)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            CustomPageModel objVendorModel = new CustomPageModel();
            try
            {
                using (this.objDataContext = new VendorDataContext())
                {
                    CRMGetCustomPageByIdResult item = this.objDataContext.CRMGetCustomPageById(pageId, pageName).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objVendorModel.Id = item.Id;
                        objVendorModel.PageName = item.PageName;
                        objVendorModel.PageContent = item.PageContent;
                        objVendorModel.IsActive = item.IsActive;
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objVendorModel;
        }

        /// <summary>
        /// Saves the vendor.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>System long.</returns>
        public long SaveCustomPage(CustomPageModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new VendorDataContext())
                    {
                        var result = this.objDataContext.CRMInsertUpdateCustomPage(objSave.Id, objSave.PageName, objSave.PageContent, objSave.IsActive, MySession.Current.UserId, PageMaster.Vendors).FirstOrDefault();
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
    }
}