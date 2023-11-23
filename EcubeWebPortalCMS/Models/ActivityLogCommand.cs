// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 10-08-2016
// ***********************************************************************
// <copyright file="ActivityLogCommand.cs" company="string.Empty">
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
    /// Class ActivityLogCommand.
    /// </summary>
    public partial class ActivityLogCommand : IActivityLogCommand, IDisposable
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context.
        /// </summary>
        private readonly ActivityLogDataContext objDataContext = new ActivityLogDataContext();

        /// <summary>
        /// The disposed.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityLogCommand" /> class.
        /// </summary>
        /// <param name="objDataContext">The object data context.</param>
        public ActivityLogCommand(ActivityLogDataContext objDataContext)
        {
            this.objDataContext = objDataContext;
        }

        /// <summary>
        /// Saves the activity log.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        public long SaveActivityLog(ActivityLogModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext)
                    {
                        var result = this.objDataContext.InsertOrUpdateActivityLog(objSave.Id, objSave.UserId, objSave.PageId, objSave.AuditComments, objSave.TableName, objSave.RecordId, MySession.Current.UserId, PageMaster.ActivityLog).FirstOrDefault();
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
        /// Searches the activity log.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List&lt;Search Activity Log Result  &gt;.</returns>
        public List<SearchActivityLogResult> SearchActivityLog(int inRow, int inPage, string strSearch, string strSort)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                return this.objDataContext.SearchActivityLog(inRow, inPage, strSearch, strSort).ToList();
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
