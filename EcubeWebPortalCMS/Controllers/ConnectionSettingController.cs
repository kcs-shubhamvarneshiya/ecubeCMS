// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : Maulik Shah
// Created          : 03-Feb-2017
//
// Last Modified By : Maulik Shah
// Last Modified On : 02-03-2017
// ***********************************************************************
// <copyright file="ConnectionSettingController.cs" company="KCSPL">
//     Copyright ©  2016
// </copyright>
// <summary>ConnectionSettingController.cs</summary>
// ***********************************************************************

/// <summary>
/// The Controllers namespace.
/// </summary>
namespace EcubeWebPortalCMS.Controllers
{
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Common;
    using EcubeWebPortalCMS.Models;
    using Serilog;

    /// <summary>
    /// Class ConnectionSettingController.
    /// </summary>
    public class ConnectionSettingController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object connection setting variable. variable. variable.
        /// </summary>
        private ConnectionSettingModel objConnectionSetting = null;

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult index.</returns>
        public ActionResult Index()
        {

            this.objConnectionSetting = new ConnectionSettingModel();

            if (ConfigurationManager.AppSettings.Get("IsAllowToChangeConfig") != null && ConfigurationManager.AppSettings.Get("IsAllowToChangeConfig").BoolSafe())
            {
                foreach (System.Configuration.ConnectionStringSettings s in System.Web.Configuration.WebConfigurationManager.ConnectionStrings)
                {
                    if (s.Name != "LocalSqlServer")
                    {
                        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(s.ConnectionString);
                        this.objConnectionSetting.CatalogName = builder.InitialCatalog;
                        this.objConnectionSetting.ServerName = builder.DataSource;
                    }
                }

                this.objConnectionSetting.IsAllowToChangeConfig = true;
            }
            else
            {
                this.objConnectionSetting.IsAllowToChangeConfig = false;
            }

            return this.View(this.objConnectionSetting);
        }

        /// <summary>
        /// Changes the connection.
        /// </summary>
        /// <param name="connectionSetting">The connection setting parameter.</param>
        /// <returns>JSON Result change connection.</returns>
        public JsonResult ChangeConnection(ConnectionSettingModel connectionSetting)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                this.objConnectionSetting = new ConnectionSettingModel();
                this.objConnectionSetting = connectionSetting;

                foreach (System.Configuration.ConnectionStringSettings s in System.Web.Configuration.WebConfigurationManager.ConnectionStrings)
                {
                    if (s.Name != "LocalSqlServer")
                    {
                        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(s.ConnectionString);

                        builder.DataSource = this.objConnectionSetting.ServerName;
                        builder.InitialCatalog = this.objConnectionSetting.CatalogName;
                        builder.IntegratedSecurity = builder.IntegratedSecurity;

                        if (!builder.UserID.IsNullString())
                        {
                            builder.UserID = builder.UserID;
                        }

                        if (!builder.Password.IsNullString())
                        {
                            builder.Password = builder.Password;
                        }

                        var configuration = WebConfigurationManager.OpenWebConfiguration("~");

                        var section = (ConnectionStringsSection)configuration.GetSection("connectionStrings");

                        section.ConnectionStrings[s.Name].ConnectionString = builder.ConnectionString;

                        configuration.Save();
                    }
                }

                return this.Json("1111");
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Format("Connection strings can not change as got an error: {0}.", ex.Message.ToString()));
            }
        }
    }
}