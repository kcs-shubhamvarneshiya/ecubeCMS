// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-19-2016
// ***********************************************************************
// <copyright file="ClientInfo.cs" company="string.Empty">
//     Copyright ©  2016
// </copyright>
// <summary>KCSPL</summary>
// ***********************************************************************

/// <summary>
/// The Common namespace.
/// </summary>
namespace EcubeWebPortalCMS.Common
{
    using Serilog;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;

    /// <summary>
    /// Class ClientInfo.
    /// </summary>
    public class ClientInfo
    {
        public static readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The client identifier.
        /// </summary>
        private string clientID;

        /// <summary>
        /// The active time.
        /// </summary>
        private DateTime activeTime;

        /// <summary>
        /// The refresh time.
        /// </summary>
        private DateTime refreshTime;

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public long UserID { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>The client identifier.</value>
        public string ClientID
        {
            get
            {
                return this.clientID;
            }

            set
            {
                this.clientID = value;
            }
        }

        /// <summary>
        /// Gets or sets the active time.
        /// </summary>
        /// <value>The active time.</value>
        public DateTime ActiveTime
        {
            get
            {
                return this.activeTime;
            }

            set
            {
                this.activeTime = value;
            }
        }

        /// <summary>
        /// Gets or sets the refresh time.
        /// </summary>
        /// <value>The refresh time.</value>
        public DateTime RefreshTime
        {
            get
            {
                return this.refreshTime;
            }

            set
            {
                this.refreshTime = value;
            }
        }

        /// <summary>
        /// Gets the client information by client identifier.
        /// </summary>
        /// <param name="clientList">The client list.</param>
        /// <param name="strClientID">The string client identifier.</param>
        /// <returns>The ClientInfo.</returns>
        public static ClientInfo GetClinetInfoByClientID(List<ClientInfo> clientList, string strClientID)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.ClientInfo + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                if (clientList != null)
                {
                    for (int i = 0; i < clientList.Count; i++)
                    {
                        if (clientList[i].ClientID == strClientID)
                        {
                            return clientList[i];
                        }
                    }
                }

                return new ClientInfo();
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }
    }
}
