// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 08-10-2016
// ***********************************************************************
// <copyright file="SMSLogController.cs" company="string.Empty">
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
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using EcubeWebPortalCMS.Common;
    using EcubeWebPortalCMS.Models;
    using Serilog;

    /// <summary>
    /// Class SMSLogController.
    /// </summary>
    public class SMSLogController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// Zero-based index of the SMS log.
        /// </summary>
        private readonly ISMSLogCommand objISMSLogCommand = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SMSLogController" /> class.
        /// </summary>
        /// <param name="iSMSLogCommand">The i SMS log command.</param>
        public SMSLogController(ISMSLogCommand iSMSLogCommand)
        {
            this.objISMSLogCommand = iSMSLogCommand;
        }

        /// <summary>
        /// SMSs the log view.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult SMSLogView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            ExportExcel objExportExcel = new ExportExcel();
            try
            {
                UserCommand userCommand = new UserCommand();
                GetPageRightsByUserIdResult getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.BanquetRoomSMSLog));
                if (getPageRights == null || getPageRights.PageId == 0 || getPageRights.ViewAll == false || getPageRights.ViewDetail == false)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }

                ////objExportExcel.Status = string.Empty;
                ////objExportExcel.AllItems = new List<SelectListItem>()
                ////{
                ////    new SelectListItem() { Text = "--ALL--", Value = "0", Selected = true },
                ////    new SelectListItem { Text = "Success",  Value = "Success" },
                ////    new SelectListItem { Text = "Failed",  Value = "Failed" }
                ////};

                return this.View(objExportExcel);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objExportExcel);
            }
        }

        /// <summary>
        /// Binds the SMS log grid.
        /// </summary>
        /// <param name="sidx">Parameter SIDX.</param>
        /// <param name="sord">Parameter SORD.</param>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="search">The search.</param>
        /// <param name="bookingType">Type of the booking.</param>
        /// <param name="fromDate">From date parameter.</param>
        /// <param name="toDate">To date parameter.</param>
        /// <param name="status">The status parameter.</param>
        /// <returns>ActionResult bind SMS log grid.</returns>
        public ActionResult BindSMSLogGrid(string sidx, string sord, int page, int rows, string filters, string search, string bookingType, string fromDate, string toDate, string status)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                var relaventId = 0;
                var moduleId = 0;
                if (bookingType != string.Empty)
                {
                    moduleId = Convert.ToInt32(bookingType.Split('_')[0].ToString());
                    relaventId = Convert.ToInt32(bookingType.Split('_')[1].ToString());
                }
                else
                {
                    moduleId = 1;
                    relaventId = 1;
                }

                List<SearchSMSLogResult> objSMSLogList = this.objISMSLogCommand.SearchSMSLog(rows, page, search, sidx + " " + sord, Convert.ToInt32(relaventId), Convert.ToInt32(moduleId), fromDate, toDate, status == "0" ? string.Empty : status);

                ////this.ExportExcel(objSMSLogList, "SMSLOG");
                return this.FillGridSMSLog(page, rows, objSMSLogList);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Exports to excel.
        /// </summary>
        /// <param name="objExportExcel">The object export excel parameter.</param>
        /// <returns>ActionResult export to excel.</returns>
        public ActionResult ExportToExcel(ExportExcel objExportExcel)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                var relaventId = 0;
                var moduleId = 0;
                if (!string.IsNullOrEmpty(objExportExcel.BookingType))
                {
                    moduleId = Convert.ToInt32(objExportExcel.BookingType.Split('_')[0].ToString());
                    relaventId = Convert.ToInt32(objExportExcel.BookingType.Split('_')[1].ToString());
                }
                else
                {
                    moduleId = 1;
                    relaventId = 1;
                }

                List<SearchSMSLogResult> objSMSLogList = this.objISMSLogCommand.SearchSMSLog(50000, 1, string.IsNullOrEmpty(objExportExcel.Filters) ? string.Empty : objExportExcel.Filters, "SentOn desc", Convert.ToInt32(relaventId), Convert.ToInt32(moduleId), objExportExcel.FromDate, objExportExcel.ToDate, objExportExcel.Status == "0" ? string.Empty : objExportExcel.Status);

                objSMSLogList = objSMSLogList != null ? objSMSLogList : new List<SearchSMSLogResult>();

                var objSMSList = objSMSLogList.Select(s => new
                {
                    ModuleName = s.ModuleName,
                    MobileNo = s.MobileNo,
                    Text = s.text,
                    SentOn = s.SentOn,
                    Status = s.Status
                }).ToList();

                GridView gridView1 = new GridView();
                gridView1.AllowPaging = false;
                gridView1.DataSource = objSMSList;
                gridView1.DataBind();
                this.Response.ClearContent();
                this.Response.Buffer = true;
                this.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", string.Empty + "SMSLOG_" + DateTime.Now.ToString("ddMMMyyyyhhmmss") + ".xls"));
                this.Response.ContentType = "application/ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gridView1.HeaderRow.Style.Add("background-color", "#FFFFFF");
                for (int i = 0; i < gridView1.HeaderRow.Cells.Count; i++)
                {
                    gridView1.HeaderRow.Cells[i].Style.Add("background-color", "#507CD1");
                }

                int j = 1;
                ////This loop is used to apply stlye to cells based on particular row
                foreach (GridViewRow gvrow in gridView1.Rows)
                {
                    if (j <= gridView1.Rows.Count)
                    {
                        if (j % 2 != 0)
                        {
                            for (int k = 0; k < gvrow.Cells.Count - 1; k++)
                            {
                                gvrow.Cells[k].Style.Add("background-color", "#EFF3FB");
                            }
                        }
                    }

                    j++;
                }

                gridView1.RenderControl(htw);
                this.Response.Write(sw.ToString());
                this.Response.End();
                return this.View("SMSLogView");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("System.OutOfMemoryException"))
                {
                    this.Response.Close();
                }

                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        // Get bookingType

        /// <summary>
        /// Get bookings the type.
        /// </summary>
        /// <returns>The JSON  Result.</returns>
        public JsonResult GetbookingType()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                ErrorLogCommand objSMSLog = new ErrorLogCommand();
                return this.Json(objSMSLog.GetBanquetType(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Fills the grid SMS log.
        /// </summary>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="objSMSLogList">The object SMS log list.</param>
        /// <returns>Action  Result.</returns>
        private ActionResult FillGridSMSLog(int page, int rows, List<SearchSMSLogResult> objSMSLogList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objSMSLogList != null && objSMSLogList.Count > 0 ? objSMSLogList[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objSMSLog in objSMSLogList
                            select new
                            {
                                RelaventId = objSMSLog.RelaventId != null ? objSMSLog.RelaventId.Value.ToString() : string.Empty,
                                ModuleId = objSMSLog.ModuleId.ToString(),
                                ModuleName = objSMSLog.ModuleName,
                                MobileNo = objSMSLog.MobileNo,
                                text = objSMSLog.text,
                                SentOn = objSMSLog.SentOn.ToString(Functions.DateTimeFormat),
                                Id = objSMSLog.Id.ToString().Encode(),
                                Status = objSMSLog.Status
                            }).ToArray()
                };
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }
    }
}