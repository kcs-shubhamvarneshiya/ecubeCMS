// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : pratikp
// Created          : 11-21-2016
//
// Last Modified By : pratikp
// Last Modified On : 11-22-2016
// ***********************************************************************
// <copyright file="DocumentLibraryCommand.cs" company="KCS">
//     Copyright ©  2016
// </copyright>
// <summary>DocumentLibraryCommand</summary>
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
    /// Class Document Library Command.
    /// </summary>
    public class DocumentLibraryCommand : IDocumentLibraryCommand
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context.
        /// </summary>
        private DocumentLibraryDataContext objDataContext = new DocumentLibraryDataContext();

        /// <summary>
        /// Searches the document.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List&lt;CRM Search Document Result&gt;.</returns>
        public List<CRMSearchDocumentResult> SearchDocument(int inRow, int inPage, string strSearch, string strSort)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new DocumentLibraryDataContext())
                {
                    List<CRMSearchDocumentResult> objSearch = this.objDataContext.CRMSearchDocument(inRow, inPage, strSearch, strSort).ToList();
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
        /// Saves the document library.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>System INT64.</returns>
        public long SaveDocumentLibrary(DocumentLibraryModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new DocumentLibraryDataContext())
                    {
                        var result = this.objDataContext.CRMInsertUpdateDocumentLibrary(objSave.DocumentId, objSave.DocumentName, objSave.Title, objSave.Remark, objSave.IsActive, MySession.Current.UserId, PageMaster.DocumentLibrary);
                    }

                    scope.Complete();
                }

                return objSave.DocumentId;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return 0;
            }
        }

        /// <summary>
        /// Gets the document by identifier.
        /// </summary>
        /// <param name="documentId">The document identifier.</param>
        /// <returns>Document Library Model.</returns>
        public DocumentLibraryModel GetDocumentById(long documentId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            DocumentLibraryModel objDocumentModel = new DocumentLibraryModel();
            CRMGetDocumentyIdResult item = this.objDataContext.CRMGetDocumentyId(documentId).FirstOrDefault();
            try
            {
                if (item != null)
                {
                    objDocumentModel.DocumentId = item.DocumentId;
                    objDocumentModel.DocumentName = item.DocumentName;
                    objDocumentModel.Title = item.Title;
                    objDocumentModel.Remark = item.Remark;
                    objDocumentModel.IsActive = item.IsActive.Value;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objDocumentModel;
        }

        /// <summary>
        /// Deletes the document.
        /// </summary>
        /// <param name="strSupportIdList">The string support identifier list.</param>
        /// <param name="lgDeletedBy">The deleted by.</param>
        /// <returns>CRMDelete Document Result.</returns>
        public CRMDeleteDocumentResult DeleteDocument(string strSupportIdList, int lgDeletedBy)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            CRMDeleteDocumentResult result = new CRMDeleteDocumentResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new DocumentLibraryDataContext())
                    {
                        result = this.objDataContext.CRMDeleteDocument(strSupportIdList, lgDeletedBy, Convert.ToInt32(PageMaster.ServiceSupportType)).ToList().FirstOrDefault();
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
        public bool IsTitleExists(long lgNameId, string typeName)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                if (this.objDataContext.CRMGeneralDocuments.Where(x => x.DocumentId != lgNameId && x.Title == typeName && x.IsDeleted == false).Count() > 0)
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