// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 10-08-2016
// ***********************************************************************
// <copyright file="IPAddressCommand.cs" company="string.Empty">
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
    using System.Linq;
    using System.Transactions;
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Common;

    /// <summary>
    /// Class IPAddressCommand.
    /// </summary>
    public partial class IPAddressCommand : IIPAddressCommand, IDisposable
    {
        /// <summary>
        /// Context for the object data.
        /// </summary>
        private readonly IPAddressDataContext objDataContext = new IPAddressDataContext();

        /// <summary>
        /// The disposed.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Gets all IP  address for drop down.
        /// </summary>
        /// <returns>List;SelectList Item;.</returns>
        public List<SelectListItem> GetAllIPAddressForDropDown()
        {
            List<SelectListItem> objIPAddressList = new List<SelectListItem>();
            try
            {
                objIPAddressList.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                List<GetIPAddressAllResult> objIPAddressResultList = this.objDataContext.GetIPAddressAll().ToList();
                if (objIPAddressResultList.Count > 0)
                {
                    foreach (var item in objIPAddressResultList)
                    {
                        objIPAddressList.Add(new SelectListItem { Text = item.IPAddressName, Value = item.Id.ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.IPAddress, MySession.Current.UserId);
            }

            return objIPAddressList;
        }

        /// <summary>
        /// Gets the IP  address by IP  address identifier.
        /// </summary>
        /// <param name="lgIPAddressId">The long IP  address identifier.</param>
        /// <returns>IP Address  Model.</returns>
        public IPAddressModel GetIPAddressByIPAddressId(long lgIPAddressId)
        {
            IPAddressModel objIPAddress = new IPAddressModel();
            GetIPAddressByIdResult item = this.objDataContext.GetIPAddressById(lgIPAddressId).ToList().FirstOrDefault();
            try
            {
                if (item != null)
                {
                    objIPAddress.Id = item.Id;
                    objIPAddress.IPAddressName = item.IPAddressName;
                    objIPAddress.Description = item.Description;
                    objIPAddress.IsActive = item.IsActive;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.IPAddress, MySession.Current.UserId);
            }

            return objIPAddress;
        }

        /// <summary>
        /// Saves the IP  address.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        public long SaveIPAddress(IPAddressModel objSave)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext)
                    {
                        var result = this.objDataContext.InsertOrUpdateIPAddress(objSave.Id, objSave.IPAddressName, objSave.Description, objSave.IsActive, MySession.Current.UserId, PageMaster.IPAddress).FirstOrDefault();
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.IPAddress, MySession.Current.UserId);
                return 0;
            }
        }

        /// <summary>
        /// Deletes the IP  address.
        /// </summary>
        /// <param name="strIPAddressIdList">The string IP  address identifier list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>Delete IPAddress  Result.</returns>
        public DeleteIPAddressResult DeleteIPAddress(string strIPAddressIdList, long lgDeletedBy)
        {
            DeleteIPAddressResult result = new DeleteIPAddressResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext)
                    {
                        result = this.objDataContext.DeleteIPAddress(strIPAddressIdList, lgDeletedBy, PageMaster.IPAddress).ToList().FirstOrDefault();
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.IPAddress, MySession.Current.UserId);
            }

            return result;
        }

        /// <summary>
        /// Determines whether [is IP address exists] [the specified long IP  address identifier].
        /// </summary>
        /// <param name="lgIPAddressId">The long IP  address identifier.</param>
        /// <param name="strIPAddressName">Name of the string IP  address.</param>
        /// <returns><c>true</c> if [is IP  address exists] [the specified long IP  address identifier]; otherwise, <c>false</c>.</returns>
        public bool IsIPAddressExists(long lgIPAddressId, string strIPAddressName)
        {
            try
            {
                if (this.objDataContext.IPAddresses.Where(x => x.Id != lgIPAddressId && x.IPAddressName == strIPAddressName && x.IsDeleted == false).Count() > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.IPAddress, MySession.Current.UserId);
                return false;
            }
        }

        /// <summary>
        /// Searches the IP  address.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List&lt;Search IPAddress Result  &gt;.</returns>
        public List<SearchIPAddressResult> SearchIPAddress(int inRow, int inPage, string strSearch, string strSort)
        {
            try
            {
                return this.objDataContext.SearchIPAddress(inRow, inPage, strSearch, strSort).ToList();
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.IPAddress, MySession.Current.UserId);
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
