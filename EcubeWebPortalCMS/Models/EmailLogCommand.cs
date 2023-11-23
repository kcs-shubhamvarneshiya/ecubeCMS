// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 10-08-2016
// ***********************************************************************
// <copyright file="EmailLogCommand.cs" company="string.Empty">
//     Copyright �  2016
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
    using EcubeWebPortalCMS.Common;
    using Serilog;

    /// <summary>
    /// Class EmailLogCommand.
    /// </summary>
    public partial class EmailLogCommand : IEmailLogCommand, IDisposable
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// Context for the object data.
        /// </summary>
        private readonly EmailLogDataContext objDataContext = new EmailLogDataContext();

        /// <summary>
        /// The disposed.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Saves the email log.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        public long SaveEmailLog(EmailLogModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext)
                    {
                        var result = this.objDataContext.InsertOrUpdateEmailLog(objSave.Id, objSave.RelaventId, objSave.ModuleId, objSave.MailContent, objSave.MailTo, objSave.CC, objSave.BCC, objSave.SentOn, MySession.Current.UserId, PageMaster.EmailLog).FirstOrDefault();
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
        /// Searches the email log.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <param name="relaventId">The relevant identifier.</param>
        /// <param name="moduleId">The module identifier.</param>
        /// <param name="fromDate">From date parameter.</param>
        /// <param name="toDate">To date parameter.</param>
        /// <param name="status">The status parameter.</param>
        /// <returns>List&lt;SearchEmail Log Result  &gt;.</returns>
        public List<SearchEmailLogResult> SearchEmailLog(int inRow, int inPage, string strSearch, string strSort, int relaventId, int moduleId, string fromDate, string toDate, string status)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                return this.objDataContext.SearchEmailLog(inRow, inPage, strSearch, strSort, relaventId, moduleId, fromDate.DateNullSafe(), toDate.DateNullSafe(), status).ToList();
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return null;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            try
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch (Exception)
            {
                ////ErrorLog.Write(string.Format(this.GetType().Name, "Dispose"), ex, LogType.Critical);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (!this.disposed)
                {
                    if (disposing)
                    {
                        if (this.objDataContext != null)
                        {
                            this.objDataContext.Dispose();
                        }
                    }

                    this.disposed = true;
                }
            }
            catch (Exception)
            {
                ////ErrorLog.Write(string.Format(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name), ex, LogType.Critical);
            }
        }
    }
}