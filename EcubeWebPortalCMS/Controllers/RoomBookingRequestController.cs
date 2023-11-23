// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-28-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 08-10-2016
// ***********************************************************************
// <copyright file="RoomBookingRequestController.cs" company="string.Empty">
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
    using System.Linq;
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Common;
    using EcubeWebPortalCMS.Models;
    using Serilog;

    /// <summary>
    /// Class RoomBookingRequestController.
    /// </summary>
    public class RoomBookingRequestController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object i room booking request command.
        /// </summary>
        private readonly IRoomBookingRequestCommand objIRoomBookingRequestCommand = null;

        private UserCommand userCommand;
        private GetPageRightsByUserIdResult getPageRights;


        /// <summary>
        /// Initializes a new instance of the <see cref="RoomBookingRequestController" /> class.
        /// </summary>
        /// <param name="iRoomBookingRequestCommand">The i room booking request command.</param>
        public RoomBookingRequestController(IRoomBookingRequestCommand iRoomBookingRequestCommand)
        {
            this.objIRoomBookingRequestCommand = iRoomBookingRequestCommand;
        }

        /// <summary>
        /// Rooms the booking request view.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult RoomBookingRequestView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.RoomRequest));
                if (getPageRights == null || getPageRights.PageId == 0 || getPageRights.ViewAll == false || getPageRights.ViewDetail == false)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }

                this.ViewData["blAddRights"] = getPageRights.Add;
                this.ViewData["blEditRights"] = getPageRights.Edit;
                this.ViewData["blDeleteRights"] = getPageRights.Delete;
                return this.View();
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        /// <summary>
        /// Rooms the booking request.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult RoomBookingRequest()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.RoomRequest));

                RoomBookingRequestModel objRoomBookingRequestModel = new RoomBookingRequestModel();
                long lgRoomBookingRequestId = 0;
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["iFrame"] != null)
                    {
                        if (getPageRights.Add == false)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objRoomBookingRequestModel.HdnIFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (getPageRights.Edit == false || string.IsNullOrEmpty(Request.QueryString.ToString()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgRoomBookingRequestId = Request.QueryString.ToString().LongSafe();
                        objRoomBookingRequestModel = this.objIRoomBookingRequestCommand.GetRoomBookingRequestByRoomBookingRequestId(lgRoomBookingRequestId);
                    }
                }
                else
                {
                    if (getPageRights.Add == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                return this.View(objRoomBookingRequestModel);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        /// <summary>
        /// Rooms the booking request.
        /// </summary>
        /// <param name="objRoomBookingRequest">The object room booking request.</param>
        /// <returns>Action  Result.</returns>
        [HttpPost]
        public ActionResult RoomBookingRequest(RoomBookingRequestModel objRoomBookingRequest)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.RoomRequest));

                if (objRoomBookingRequest.Id == 0)
                {
                    if (getPageRights.Add == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }
                else
                {
                    if (getPageRights.Edit == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                if (objRoomBookingRequest.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                string strErrorMsg = this.ValidateRoomBookingRequest(objRoomBookingRequest);
                if (!string.IsNullOrEmpty(strErrorMsg))
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = strErrorMsg;
                }
                else
                {
                    objRoomBookingRequest.Id = this.objIRoomBookingRequestCommand.SaveRoomBookingRequest(objRoomBookingRequest);
                    if (objRoomBookingRequest.Id > 0)
                    {
                        this.ViewData["Success"] = "1";
                        this.ViewData["Message"] = Functions.AlertMessage("Room Booking Request Status", MessageType.UpdateSuccess);
                        return this.View(objRoomBookingRequest);
                    }
                    else
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = Functions.AlertMessage("Room Booking Request Status", MessageType.Fail);
                    }
                }

                return this.View(objRoomBookingRequest);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Room Booking Request", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objRoomBookingRequest);
            }
        }

        /// <summary>
        /// Binds the room booking request grid.
        /// </summary>
        /// <param name="sidx">Parameter SIDX.</param>
        /// <param name="sord">Parameter SORD.</param>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="search">The search.</param>
        /// <param name="sortedby">The sorted by.</param>
        /// <param name="roomType">Type of the room.</param>
        /// <returns>Action  Result.</returns>
        public ActionResult BindRoomBookingRequestGrid(string sidx, string sord, int page, int rows, string filters, string search, string sortedby, string roomType)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                long lgRoomType = 0;
                if (roomType != string.Empty)
                {
                    lgRoomType = Convert.ToInt64(roomType);
                }
                else
                {
                    lgRoomType = 0;
                }

                List<MS_SearchRoomBookingRequestResult> objRoomBookingRequestList = this.objIRoomBookingRequestCommand.SearchRoomBookingRequest(rows, page, search, sidx + " " + sord, Convert.ToInt32(sortedby), Convert.ToInt64(lgRoomType));
                ////if (objRoomBookingRequestList != null && objRoomBookingRequestList.Count > 0)
                ////{
                return this.FillGridRoomBookingRequest(page, rows, objRoomBookingRequestList);
                ////}
                ////else
                ////{
                ////    return this.Json(string.Empty);
                ////}
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Deletes the room booking request.
        /// </summary>
        /// <param name="strRoomBookingRequestId">The string room booking request identifier.</param>
        /// <returns>JSON  Result.</returns>
        public JsonResult DeleteRoomBookingRequest(string strRoomBookingRequestId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                //string[] strRoomBookingRequest = strRoomBookingRequestId.Split(',');
                //strRoomBookingRequestId = string.Empty;
                //foreach (var item in strRoomBookingRequest)
                //{
                //    strRoomBookingRequestId += item.Decode() + ",";
                //}

                //strRoomBookingRequestId = strRoomBookingRequestId.Substring(0, strRoomBookingRequestId.Length - 1);
                MS_DeleteRoomBookingRequestResult result = this.objIRoomBookingRequestCommand.DeleteRoomBookingRequest(strRoomBookingRequestId, MySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Room Booking Request", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Room Booking Request", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Room Booking Request", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(Functions.AlertMessage("Room Booking Request", MessageType.DeleteFail));
            }
        }

        /// <summary>
        /// Gets the room booking request.
        /// </summary>
        /// <returns>The JSON  Result.</returns>
        public JsonResult GetRoomBookingRequest()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                return this.Json(this.objIRoomBookingRequestCommand.GetAllRoomBookingRequestForDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Gets the type of the room.
        /// </summary>
        /// <returns>JSON  Result.</returns>
        public JsonResult GetRoomType()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                RoomBookingRequestCommand objRoomType = new RoomBookingRequestCommand();
                return this.Json(objRoomType.GetRoomType().ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Validates the room booking request.
        /// </summary>
        /// <param name="objRoomBookingRequest">The object room booking request.</param>
        /// <returns>Return System.String.</returns>
        private string ValidateRoomBookingRequest(RoomBookingRequestModel objRoomBookingRequest)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string strErrorMsg = string.Empty;
                return strErrorMsg;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return ex.Message.ToString();
            }
        }

        /// <summary>
        /// Fills the grid room booking request.
        /// </summary>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="objRoomBookingRequestList">The object room booking request list.</param>
        /// <returns>Action  Result.</returns>
        private ActionResult FillGridRoomBookingRequest(int page, int rows, List<MS_SearchRoomBookingRequestResult> objRoomBookingRequestList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objRoomBookingRequestList != null && objRoomBookingRequestList.Count > 0 ? (int)objRoomBookingRequestList[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objRoomBookingRequest in objRoomBookingRequestList
                            select new
                            {
                                Name = objRoomBookingRequest.Name,
                                MemberId = objRoomBookingRequest.MemberId != null ? objRoomBookingRequest.MemberId.Value.ToString() : string.Empty,
                                InquiryFor = objRoomBookingRequest.InquiryFor != null ? objRoomBookingRequest.InquiryFor.Value.ToString() : string.Empty,
                                EMail = objRoomBookingRequest.EMail,
                                Address = objRoomBookingRequest.Address,
                                City = objRoomBookingRequest.City,
                                MobileNo = objRoomBookingRequest.MobileNo,
                                Member = objRoomBookingRequest.Member != null ? objRoomBookingRequest.Member.Value.ToString() : string.Empty,
                                Adults = objRoomBookingRequest.Adults != null ? objRoomBookingRequest.Adults.Value.ToString() : string.Empty,
                                Children = objRoomBookingRequest.Children != null ? objRoomBookingRequest.Children.Value.ToString() : string.Empty,
                                CheckedInDate = objRoomBookingRequest.CheckedInDate != null ? objRoomBookingRequest.CheckedInDate.Value.ToString(Functions.DateFormat) : string.Empty,
                                CheckedOutDate = objRoomBookingRequest.CheckedOutDate != null ? objRoomBookingRequest.CheckedOutDate.Value.ToString(Functions.DateFormat) : string.Empty,
                                RoomType = objRoomBookingRequest.RoomType,
                                Remarks = objRoomBookingRequest.Remarks,
                                StatusId = objRoomBookingRequest.StatusId,
                                Status = objRoomBookingRequest.Status,
                                AdminRemarks = objRoomBookingRequest.AdminRemarks,
                                Id = objRoomBookingRequest.Id.ToString()
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
