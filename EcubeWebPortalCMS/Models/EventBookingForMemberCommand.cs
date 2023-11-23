using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Transactions;
using EcubeWebPortalCMS.Common;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Serilog;

namespace EcubeWebPortalCMS.Models
{
    public partial class EventBookingForMemberCommand : IEventBookingForMemberCommand
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context.
        /// </summary>
        private EventDataContext objDataContext = null;
        public List<SelectListItem> GetMemberCodeDropdown()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            List<SelectListItem> objMemberCodeList = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    objMemberCodeList.Add(new SelectListItem { Text = "Select Member No", Value = "0" });
                    List<GetMemberCodeDropdownResult> objEventCategoryResultList = this.objDataContext.GetMemberCodeDropdown(string.Empty, false, false, false, false, false, false).ToList();
                    if (objEventCategoryResultList != null && objEventCategoryResultList.Count > 0)
                    {
                        foreach (var item in objEventCategoryResultList)
                        {
                            objMemberCodeList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objMemberCodeList;
        }

        public List<SelectListItem> GetRelationshipDropDown()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            List<SelectListItem> objRelationshipList = new List<SelectListItem>();
            objRelationshipList.Add(new SelectListItem { Text = "Select Relationship", Value = "0" });
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    List<GetRelationshipDropdownResult> lstRelationship = this.objDataContext.GetRelationshipDropdown().ToList();
                    if (lstRelationship != null && lstRelationship.Count > 0)
                    {
                        foreach (var item in lstRelationship)
                        {
                            objRelationshipList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objRelationshipList;
        }

        public List<SelectListItem> GetEventFeesDropDown(int eventId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            List<SelectListItem> objEventFeesList = new List<SelectListItem>();
            objEventFeesList.Add(new SelectListItem { Text = "Select Event Booking Type", Value = "0" });
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    List<GetEventFeesResult> lstEventFees = this.objDataContext.GetEventFees(eventId).ToList();
                    if (lstEventFees != null && lstEventFees.Count > 0)
                    {
                        foreach (var item in lstEventFees)
                        {
                            objEventFeesList.Add(new SelectListItem { Text = item.descriptions, Value = item.MemberFees.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objEventFeesList;
        }

        //******* Start Bind Member Grid ***********

        public List<GetMemberDetailsWithEventRateResult> SearchEvent(int memberid, int eventid, string bookingDate)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    List<GetMemberDetailsWithEventRateResult> objSearchEventList = this.objDataContext.GetMemberDetailsWithEventRate(memberid, eventid, bookingDate).ToList();
                    foreach (var item in objSearchEventList)
                    {
                        if (item.ImageName != null)
                        {
                            item.ImageName = item.ImageName.Replace("\\", "/");
                        }
                    }
                    return objSearchEventList;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return null;
            }
        }
        //******* End Bind Member Grid ***********

        public List<GetMemberDetailsForGuestWithEventRateResult> GetEventRate(int memberid, int eventid, string bookingDate, int Type)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    List<GetMemberDetailsForGuestWithEventRateResult> objSearchEventList = this.objDataContext.GetMemberDetailsForGuestWithEventRate(memberid, eventid, bookingDate, Type).ToList();
                    return objSearchEventList;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return null;
            }
        }

        public List<MS_GetEventDateByIDResult> GetEventDateByID(int EventId, int MemberId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    List<MS_GetEventDateByIDResult> objSearchEventList = this.objDataContext.MS_GetEventDateByID(EventId, MemberId).ToList();
                    return objSearchEventList;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return null;
            }
        }


        public List<EventList> GetAllEvent()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                List<EventList> ObjEventList = new List<EventList>();
                using (this.objDataContext = new EventDataContext())
                {
                    ObjEventList = this.objDataContext.MS_GetEventList().Select(x => new EventList
                    {
                        EventTitle = x.EventTitle,
                        EventPlace = x.EventPlace,
                        EventImage = x.EventImage,
                        EventDate = x.EventDate,
                        Id = x.Id
                    }).ToList();
                    return ObjEventList;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Saves the event Booking for Umed.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>System INT64.</returns>
        /// 

        public long SaveEventBookingForUmedMember(EventBookingForMemberModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {

                DataTable dtGuestList = new DataTable();
                dtGuestList.Columns.Add("MemberId", typeof(int));
                dtGuestList.Columns.Add("SubMemberId", typeof(int));
                dtGuestList.Columns.Add("Name", typeof(string));
                dtGuestList.Columns.Add("Image", typeof(string));
                dtGuestList.Columns.Add("Relation", typeof(string));
                dtGuestList.Columns.Add("Age", typeof(int));
                dtGuestList.Columns.Add("Amount", typeof(decimal));

                foreach (var item in objSave.MemberDataList)
                {
                    DataRow toInsert = dtGuestList.NewRow();



                    if (item.Relation == "self")
                    {
                        toInsert["MemberId"] = Convert.ToInt32(item.MemberID.ToString());
                        toInsert["SubMemberId"] = 0;
                    }
                    else
                    {
                        toInsert["MemberId"] = 0;
                        toInsert["SubMemberId"] = Convert.ToInt32(item.MemberID.ToString());
                    }

                    toInsert["Name"] = item.MemberName;
                    toInsert["Image"] = item.Image;
                    toInsert["Relation"] = item.Relation;
                    toInsert["Age"] = item.Age;
                    toInsert["Amount"] = Convert.ToDecimal(item.Amount);
                    dtGuestList.Rows.Add(toInsert);

                }

                int idr;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ECubeCMSConnectionString"].ToString();
                    SqlCommand command = new SqlCommand("dbo.[InsertEventBookingForUmedMember]", conn);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@MemberId", (int)objSave.MainMemberId));
                    command.Parameters.Add(new SqlParameter("@EventId", objSave.EventId));
                    command.Parameters.Add(new SqlParameter("@BookDate", objSave.BookDate));
                    command.Parameters.Add(new SqlParameter("@BalanceAmount", objSave.BalanceAmount));
                    command.Parameters.Add(new SqlParameter("@TotalAmount", objSave.TotalAmount));

                    command.Parameters.Add(new SqlParameter("@PaymentId", objSave.PaymentId));
                    command.Parameters.Add(new SqlParameter("@Date", objSave.Date));
                    command.Parameters.Add(new SqlParameter("@ChequeNo", objSave.ChequeNo));
                    command.Parameters.Add(new SqlParameter("@Branch", objSave.Branch));
                    command.Parameters.Add(new SqlParameter("@BankInfo", objSave.BankInfo));
                    command.Parameters.Add(new SqlParameter("@CardNo", objSave.CardNo));
                    command.Parameters.Add(new SqlParameter("@CardHolderName", objSave.CardHolderName));

                    command.Parameters.Add(new SqlParameter("@Id", 0));
                    command.Parameters.Add(new SqlParameter("@UserId", (int)(MySession.Current.UserId)));
                    command.Parameters.Add(new SqlParameter("@EventBookingDetailsMember", dtGuestList));


                    SqlParameter outputParam = new SqlParameter();
                    outputParam.ParameterName = "@OutID";
                    outputParam.SqlDbType = SqlDbType.Int;
                    outputParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outputParam);
                    // execute the stored procedure
                    conn.Open();
                    command.ExecuteNonQuery();
                    idr = (int)outputParam.Value;
                    command.CommandTimeout = 0;
                    conn.Close();
                }
                return idr;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return 0;
            }
        }


        public long SaveEventBookingForUmed(EventBookingForGuestModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {

                DataTable dtGuestList = new DataTable();
                dtGuestList.Columns.Add("Name", typeof(string));
                dtGuestList.Columns.Add("Relation", typeof(string));
                dtGuestList.Columns.Add("Image", typeof(string));
                dtGuestList.Columns.Add("AdultChild", typeof(string));
                dtGuestList.Columns.Add("Amount", typeof(decimal));

                foreach (var item in objSave.GuestList)
                {
                    DataRow toInsert = dtGuestList.NewRow();
                    toInsert["Name"] = item.GuestName;
                    toInsert["Relation"] = item.Relation;
                    toInsert["Image"] = item.GuestImage;
                    toInsert["AdultChild"] = item.IsAdult;
                    toInsert["Amount"] = item.Amount;

                    dtGuestList.Rows.Add(toInsert);

                }

                int idr;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ECubeCMSConnectionString"].ToString();
                    SqlCommand command = new SqlCommand("dbo.[InsertEventBookingForUmed]", conn);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@MemberId", (int)objSave.MemberId));
                    //command.Parameters.Add(new SqlParameter("@GuestName", objSave.GuestName));
                    //command.Parameters.Add(new SqlParameter("@Relation", objSave.Relation));
                    command.Parameters.Add(new SqlParameter("@EventId", objSave.EventId));
                    command.Parameters.Add(new SqlParameter("@Id", 0));

                    command.Parameters.Add(new SqlParameter("@BookDate", objSave.BookDate));
                    command.Parameters.Add(new SqlParameter("@BalanceAmount", objSave.BalanceAmount));
                    command.Parameters.Add(new SqlParameter("@TotalAmount", objSave.Amount));
                    command.Parameters.Add(new SqlParameter("@PaymentId", objSave.PaymentId));
                    command.Parameters.Add(new SqlParameter("@Date", objSave.Date));
                    command.Parameters.Add(new SqlParameter("@ChequeNo", objSave.ChequeNo));
                    command.Parameters.Add(new SqlParameter("@Branch", objSave.Branch));
                    command.Parameters.Add(new SqlParameter("@BankInfo", objSave.BankInfo));
                    command.Parameters.Add(new SqlParameter("@CardNo", objSave.CardNo));
                    command.Parameters.Add(new SqlParameter("@CardHolderName", objSave.CardHolderName));
                    command.Parameters.Add(new SqlParameter("@UserId", (int)(MySession.Current.UserId)));
                    command.Parameters.Add(new SqlParameter("@EventBookingDetails", dtGuestList));

                    SqlParameter outputParam = new SqlParameter();
                    outputParam.ParameterName = "@OutID";
                    outputParam.SqlDbType = SqlDbType.Int;
                    outputParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outputParam);
                    // execute the stored procedure
                    conn.Open();
                    command.ExecuteNonQuery();
                    idr = (int)outputParam.Value;
                    command.CommandTimeout = 0;
                    conn.Close();
                }

                return idr;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return 0;
            }
        }
        public List<SelectListItem> GetAffiliateDropDown()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            List<SelectListItem> objAffiliateList = new List<SelectListItem>();
            objAffiliateList.Add(new SelectListItem { Text = "Select Affiliate", Value = "0" });
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    List<GetAffiliateClubDropDownResult> lstAffiliate = this.objDataContext.GetAffiliateClubDropDown().ToList();
                    if (lstAffiliate != null && lstAffiliate.Count > 0)
                    {
                        foreach (var item in lstAffiliate)
                        {
                            objAffiliateList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objAffiliateList;
        }
        public List<SelectListItem> GetPaymentDropDown()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            List<SelectListItem> objAffiliateList = new List<SelectListItem>();
            objAffiliateList.Add(new SelectListItem { Text = "Select PaymentType", Value = "0" });
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    List<PaymentTypeDropdownResult> lstPayment = this.objDataContext.PaymentTypeDropdown("").ToList();
                    if (lstPayment != null && lstPayment.Count > 0)
                    {
                        foreach (var item in lstPayment)
                        {
                            objAffiliateList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objAffiliateList;
        }

        public string GetPaymentDropDown(string paymentFor)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            string Rtype = string.Empty;

            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    List<PaymentTypeDropdownResult> lstPayment = this.objDataContext.PaymentTypeDropdown(paymentFor).ToList();
                    if (lstPayment != null && lstPayment.Count > 0)
                    {
                        foreach (var item in lstPayment)
                        {
                            Rtype = item.Type;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return Rtype;
        }
        public long SaveEventBookingForUmedAffiliateClub(EventBookingForAffiliateModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {

                long idr;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ECubeCMSConnectionString"].ToString();
                    SqlCommand command = new SqlCommand("dbo.[InsertEventBookingForUmedAffiliateClub]", conn);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@MemberId", (int)objSave.MemberId));
                    command.Parameters.Add(new SqlParameter("@EventId", objSave.EventId));
                    command.Parameters.Add(new SqlParameter("@AffiliateMemberNo", objSave.AffiliateMemberCode));
                    command.Parameters.Add(new SqlParameter("@AffiliateMemberName", objSave.MemberName));
                    command.Parameters.Add(new SqlParameter("@AffiliateClubId", objSave.AffiliateMemberClubId));
                    command.Parameters.Add(new SqlParameter("@BookingDate", objSave.BookingDate));
                    command.Parameters.Add(new SqlParameter("@City", objSave.City));
                    command.Parameters.Add(new SqlParameter("@PaymentType", objSave.PaymentId));
                    command.Parameters.Add(new SqlParameter("@TotalAmount", objSave.Amount));
                    command.Parameters.Add(new SqlParameter("@Id", 0));
                    command.Parameters.Add(new SqlParameter("@Image", objSave.GuestImage));
                    command.Parameters.Add(new SqlParameter("@Date", objSave.Date));
                    command.Parameters.Add(new SqlParameter("@ChequeNo", objSave.ChequeNo));
                    command.Parameters.Add(new SqlParameter("@ChequeBranch", objSave.Branch));
                    command.Parameters.Add(new SqlParameter("@BankInfo", objSave.BankInfo));
                    command.Parameters.Add(new SqlParameter("@CreditCardNo", objSave.CardNo));
                    command.Parameters.Add(new SqlParameter("@CreditCardholder", objSave.CardHolderName));
                    command.Parameters.Add(new SqlParameter("@UserId", (int)(MySession.Current.UserId)));

                    SqlParameter outputParam = new SqlParameter();
                    outputParam.ParameterName = "@OutID";
                    outputParam.SqlDbType = SqlDbType.Int;
                    outputParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outputParam);
                    // execute the stored procedure
                    conn.Open();
                    command.ExecuteNonQuery();
                    idr = (int)outputParam.Value;
                    command.CommandTimeout = 0;
                    conn.Close();
                }

                return idr;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return 0;
            }
        }
        public long SaveEventBookingForUmedSponsor(EventBookingForSponsorModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {

                long idr;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ECubeCMSConnectionString"].ToString();
                    SqlCommand command = new SqlCommand("dbo.[InsertEventBookingForUmedSponsor]", conn);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@City", objSave.City));
                    command.Parameters.Add(new SqlParameter("@SponsorName", objSave.SponsorName));
                    command.Parameters.Add(new SqlParameter("@CompanyName", objSave.CompanyName));
                    command.Parameters.Add(new SqlParameter("@Image", objSave.GuestImage));
                    command.Parameters.Add(new SqlParameter("@EventId", objSave.EventId));
                    command.Parameters.Add(new SqlParameter("@Id", 0));
                    command.Parameters.Add(new SqlParameter("@BookDate", objSave.BookDate));
                    command.Parameters.Add(new SqlParameter("@UserId", (int)(MySession.Current.UserId)));

                    SqlParameter outputParam = new SqlParameter();
                    outputParam.ParameterName = "@OutID";
                    outputParam.SqlDbType = SqlDbType.Int;
                    outputParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outputParam);
                    // execute the stored procedure
                    conn.Open();
                    command.ExecuteNonQuery();
                    idr = (int)outputParam.Value;
                    command.CommandTimeout = 0;
                    conn.Close();
                }

                return idr;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return 0;
            }
        }
        public long SaveEventBookingForUmedParents(EventBookingForParentsModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {

                DataTable dtGuestList = new DataTable();
                dtGuestList.Columns.Add("Name", typeof(string));
                dtGuestList.Columns.Add("Relation", typeof(string));
                dtGuestList.Columns.Add("Image", typeof(string));
                dtGuestList.Columns.Add("AdultChild", typeof(string));
                dtGuestList.Columns.Add("Amount", typeof(decimal));

                foreach (var item in objSave.GuestList)
                {
                    DataRow toInsert = dtGuestList.NewRow();
                    toInsert["Name"] = item.ParentName;
                    toInsert["Relation"] = item.ParentRelation;
                    toInsert["Image"] = item.GuestImage;
                    toInsert["AdultChild"] = "";
                    toInsert["Amount"] = item.Amount;
                    dtGuestList.Rows.Add(toInsert);
                }
                long idr = 0;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ECubeCMSConnectionString"].ToString();
                    SqlCommand command = new SqlCommand("dbo.[InsertEventBookingForUmedParents]", conn);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@MemberId", (int)objSave.MemberId));
                    //command.Parameters.Add(new SqlParameter("@ParentName", objSave.ParentName));
                    //command.Parameters.Add(new SqlParameter("@Relation", objSave.ParentRelation));

                    command.Parameters.Add(new SqlParameter("@TotalAmount", objSave.Amount));
                    command.Parameters.Add(new SqlParameter("@PaymentId", objSave.PaymentId));
                    command.Parameters.Add(new SqlParameter("@ChequeNo", objSave.ChequeNo));
                    command.Parameters.Add(new SqlParameter("@Branch", objSave.Branch));
                    command.Parameters.Add(new SqlParameter("@BankInfo", objSave.BankInfo));
                    command.Parameters.Add(new SqlParameter("@CardNo", objSave.CardNo));
                    command.Parameters.Add(new SqlParameter("@CardHolderName", objSave.CardHolderName));
                    command.Parameters.Add(new SqlParameter("@EventId", objSave.EventId));
                    command.Parameters.Add(new SqlParameter("@Id", 0));
                    command.Parameters.Add(new SqlParameter("@Date", objSave.Date));
                    command.Parameters.Add(new SqlParameter("@BookingDate", objSave.BookingDate));
                    command.Parameters.Add(new SqlParameter("@UserId", (int)(MySession.Current.UserId)));
                    command.Parameters.Add(new SqlParameter("@EventBookingDetails", dtGuestList));


                    SqlParameter outputParam = new SqlParameter();
                    outputParam.ParameterName = "@OutID";
                    outputParam.SqlDbType = SqlDbType.Int;
                    outputParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outputParam);
                    // execute the stored procedure
                    conn.Open();
                    command.ExecuteNonQuery();
                    idr = (int)outputParam.Value;
                    command.CommandTimeout = 0;
                    conn.Close();
                }
                return idr;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return 0;
            }
        }
        public List<SelectListItem> GetTicketTypeDropDown()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            List<SelectListItem> objTicketTypeList = new List<SelectListItem>();
            objTicketTypeList.Add(new SelectListItem { Text = "Select TicketType", Value = "0" });
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    List<PaymentTypeDropdownResult> lstPayment = this.objDataContext.PaymentTypeDropdown("").ToList();
                    if (lstPayment != null && lstPayment.Count > 0)
                    {
                        foreach (var item in lstPayment)
                        {
                            objTicketTypeList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objTicketTypeList;
        }
        public List<EventBookingPrintTokenResult> SearchEventPrintToken(int memberno, int serialno, int tickettype, int EventBookingID, int EventEDID)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    List<EventBookingPrintTokenResult> objSearchEventPrintTokenList = this.objDataContext.EventBookingPrintToken(memberno, serialno, tickettype, EventBookingID, EventEDID).ToList();
                    List<EventBookingPrintTokenResult> objEventPrintTokenListCount = new List<EventBookingPrintTokenResult>();

                    foreach (var item in objSearchEventPrintTokenList.ToList())
                    {
                        objEventPrintTokenListCount.Add(new EventBookingPrintTokenResult { 
                            RECEIPTNo=item.RECEIPTNo,
                            Member_Name=item.Member_Name,
                            MemberNo=item.MemberNo,
                            Type=item.Type,
                            Relation=item.Relation,
                            Date=item.Date,
                            Photo=item.Photo,
                            mainMember=item.mainMember,
                            SeriolNo=item.SeriolNo,

                        });
                    }
                    return objEventPrintTokenListCount;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return null;
            }
        }
        public List<GetMemberDetailsForParentWithEventRateResult> GetParentEventRate(int memberid, int eventid, string bookingDate)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    List<GetMemberDetailsForParentWithEventRateResult> objSearchEventList = this.objDataContext.GetMemberDetailsForParentWithEventRate(memberid, eventid, bookingDate).ToList();
                    return objSearchEventList;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return null;
            }
        }

        public List<GetMemberDetailsForAffWithEventRateResult> GetAffiliateEventRate(int memberid, int eventid, string bookingDate)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    List<GetMemberDetailsForAffWithEventRateResult> objSearchEventList = this.objDataContext.GetMemberDetailsForAffWithEventRate(memberid, eventid, bookingDate).ToList();
                    return objSearchEventList;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return null;
            }
        }

        public List<MS_GetEventBookingDetailbyEventIDResult> GetEventBookingDetailResult(int id)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    List<MS_GetEventBookingDetailbyEventIDResult> objSearchEventList = this.objDataContext.MS_GetEventBookingDetailbyEventID(id).ToList();
                    return objSearchEventList;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return null;
            }
        }




    }
}