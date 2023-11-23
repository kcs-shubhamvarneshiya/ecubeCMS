// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : karan.shah
// Created          : 09-15-2017
//
// Last Modified By : karan.shah
// Last Modified On : 09-15-2017
// ***********************************************************************
// <copyright file="NotificationCommand.cs" company="KCSPL">
//     Copyright ©  2016
// </copyright>
// <summary>NotificationCommand.cs</summary>
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
    /// Class NotificationCommand.
    /// </summary>
    public class NotificationCommand : INotification
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context.
        /// </summary>
        private NotificationDataContext objDataContext = new NotificationDataContext();

        /// <summary>
        /// Gets the notification list.
        /// </summary>
        /// <returns>List&lt;NotificationModel&gt; get notification list.</returns>
        public List<NotificationModel> GetNotificationList()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            List<NotificationModel> notificationList = new List<NotificationModel>();
            try
            {
                using (this.objDataContext = new NotificationDataContext())
                {
                    notificationList = this.objDataContext.Notifications.Select(x => new NotificationModel { Id = x.Id, NotificationTitle = x.NotificationTitles, Description = x.Description }).ToList();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }

            return notificationList;
        }

        /// <summary>
        /// Searches the information.
        /// </summary>
        /// <param name="inRow">The in row Parameter.</param>
        /// <param name="inPage">The in page Parameter.</param>
        /// <param name="strSearch">The string search Parameter.</param>
        /// <param name="strSort">The string sort Parameter.</param>
        /// <returns>List&lt;SearchInformationResult&gt; search information.</returns>
        public List<SearchNotificationResult> SearchNotification(int inRow, int inPage, string strSearch, string strSort)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new NotificationDataContext())
                {
                    List<SearchNotificationResult> objSearchNotificationList = this.objDataContext.SearchNotification(inRow, inPage, strSearch, strSort).ToList();
                    return objSearchNotificationList;
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
        /// Saves the notification.
        /// </summary>
        /// <param name="objSave">The object save Parameter.</param>
        /// <returns>System save notification.</returns>
        public int SaveNotification(NotificationModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int notificationId = 0;
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new NotificationDataContext())
                    {
                        var result = this.objDataContext.InsertNotification(objSave.Id, objSave.NotificationTitle, objSave.Description, MySession.Current.UserId, PageMaster.Information).FirstOrDefault();
                        if (result != null)
                        {
                            notificationId = result.InsertedId;
                        }
                    }

                    scope.Complete();
                }

                return notificationId;
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