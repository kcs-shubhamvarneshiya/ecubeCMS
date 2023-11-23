// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : karan.shah
// Created          : 09-13-2017
//
// Last Modified By : karan.shah
// Last Modified On : 09-13-2017
// ***********************************************************************
// <copyright file="InformationCommand.cs" company="KCSPL">
//     Copyright ©  2016
// </copyright>
// <summary>InformationCommand.cs</summary>
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
    /// Class InformationCommand.
    /// </summary>
    public class InformationCommand : IInformation
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context.
        /// </summary>
        private InformationDataContext objDataContext = new InformationDataContext();

        /// <summary>
        /// Gets the information list.
        /// </summary>
        /// <returns>List&lt;Information&gt; get information list.</returns>
        public List<InformationModel> GetInformationList()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            List<InformationModel> informationList = new List<InformationModel>();
            try
            {
                using (this.objDataContext = new InformationDataContext())
                {
                    informationList = this.objDataContext.Informations.Where(x => !x.IsDeleted).Select(x => new InformationModel()
                    {
                        InformationId = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        ImagePath = x.ImagePath,
                        SequenceNo = x.SequenceNo.Value
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }

            return informationList;
        }

        /// <summary>
        /// Gets the information by identifier.
        /// </summary>
        /// <param name="id">The identifier Parameter.</param>
        /// <returns>InformationModel get information by identifier.</returns>
        public InformationModel GetInformationById(int id)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            InformationModel informationById = new InformationModel();
            try
            {
                using (this.objDataContext = new InformationDataContext())
                {
                    informationById = (from c in this.objDataContext.Informations
                                       where c.IsDeleted == false && c.Id == id
                                       select new InformationModel
                                       {
                                           InformationId = c.Id,
                                           Name = c.Name,
                                           Description = c.Description,
                                           ImagePath = c.ImagePath,
                                           SequenceNo = c.SequenceNo.Value
                                       }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }

            return informationById;
        }

        /// <summary>
        /// Searches the information.
        /// </summary>
        /// <param name="inRow">The in row Parameter.</param>
        /// <param name="inPage">The in page Parameter.</param>
        /// <param name="strSearch">The string search Parameter.</param>
        /// <param name="strSort">The string sort Parameter.</param>
        /// <returns>List&lt;SearchInformationResult&gt; search information.</returns>
        public List<SearchInformationResult> SearchInformation(int inRow, int inPage, string strSearch, string strSort)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (this.objDataContext = new InformationDataContext())
                {
                    List<SearchInformationResult> objSearchInformationList = this.objDataContext.SearchInformation(inRow, inPage, strSearch, strSort).ToList();
                    return objSearchInformationList;
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
        /// Deletes the information.
        /// </summary>
        /// <param name="strInformationIdList">The string information identifier list Parameter.</param>
        /// <param name="deletedBy">The deleted by Parameter.</param>
        /// <returns>DeleteInformationResult delete information.</returns>
        public DeleteInformationResult DeleteInformation(string strInformationIdList, int deletedBy)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            DeleteInformationResult result = new DeleteInformationResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new InformationDataContext())
                    {
                        result = this.objDataContext.DeleteInformation(strInformationIdList, deletedBy, PageMaster.Information).ToList().FirstOrDefault();
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
        /// Determines whether [is information exists] [the specified identifier].
        /// </summary>
        /// <param name="id">The identifier Parameter.</param>
        /// <param name="informationName">Name of the information.</param>
        /// <returns><c>true</c> if [is information exists] [the specified identifier]; otherwise, <c>false</c>.</returns>
        public bool IsInformationExists(int id, string informationName)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new InformationDataContext())
                {
                    if (this.objDataContext.Informations.Where(x => x.Id != id && x.Name == informationName && x.IsDeleted == false).Count() > 0)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return false;
            }
        }

        /// <summary>
        /// Saves the information.
        /// </summary>
        /// <param name="objSave">The object save Parameter.</param>
        /// <returns>System save information.</returns>
        public int SaveInformation(InformationModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int informationId = 0;
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new InformationDataContext())
                    {
                        var result = this.objDataContext.InsertOrUpdateInformation(objSave.InformationId, objSave.Name, objSave.Description, objSave.ImagePath, objSave.SequenceNo, MySession.Current.UserId, PageMaster.Information).FirstOrDefault();
                        if (result != null)
                        {
                            informationId = result.InsertedId;
                        }
                    }

                    scope.Complete();
                }

                return informationId;
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