// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-19-2016
// ***********************************************************************
// <copyright file="CommonController.cs" company="string.Empty">
//     Copyright ©  2016
// </copyright>
// <summary>KCSPL</summary>
// ***********************************************************************

/// <summary>
/// The Controllers namespace.
/// </summary>
namespace EcubeWebPortalCMS.Controllers
{
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Common;
    using Serilog;

    /// <summary>
    /// Class CommonController.
    /// </summary>
    public class CommonController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Checks the user active.
        /// </summary>
        /// <returns>JSON  Result.</returns>
        public JsonResult CheckUserActive()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (MySession.Current.UserId > 0)
                {
                    return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);

            }

            return this.Json("../Home/Login", JsonRequestBehavior.AllowGet);
        }
    }
}
