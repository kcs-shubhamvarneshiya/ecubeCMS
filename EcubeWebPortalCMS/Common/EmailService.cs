// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-19-2016
// ***********************************************************************
// <copyright file="EmailService.cs" company="string.Empty">
//     Copyright ©  2016
// </copyright>
// <summary>KCSPL</summary>
// ***********************************************************************

/// <summary>
/// The Common namespace.
/// </summary>
namespace EcubeWebPortalCMS.Common
{
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using EcubeWebPortalCMS.Models;
    using Microsoft.Exchange.WebServices.Data;
    using Serilog;

    /// <summary>
    /// Class EmailService.
    /// </summary>
    public class EmailService
    {
        public static readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// Send Email.
        /// </summary>
        /// <param name="toAddress">To address.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="message">The message.</param>
        /// <param name="toCCAddress">To cc address.</param>
        /// <param name="bccAddress">The BCC address.</param>
        /// <param name="attachmentPath">The attachment path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool SendEmail(string toAddress, string subject, string message, string toCCAddress = "", string bccAddress = "", string[] attachmentPath = null)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.EmailService + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                CommonDataContext objCommonData = new CommonDataContext();

                GetEmailConfigurationByProfileNameResult emailCredential = new GetEmailConfigurationByProfileNameResult();

                emailCredential = objCommonData.GetEmailConfigurationByProfileName("eCube").ToList().FirstOrDefault();
                if (emailCredential != null)
                {
                    string eCubeAdminBccEmail = Functions.GetSettings("eCubeAdminBccEmail");
                    string eCubeAdminCcEmail = Functions.GetSettings("eCubeAdminCcEmail");

                    if (subject != string.Empty)
                    {
                        subject = string.Format("{0} | {1}", subject, emailCredential.DisplayName);
                    }

                    message += message.Replace("[CompanyName]", emailCredential.DisplayName);

                    if (!emailCredential.IsExchange)
                    {
                        using (MailMessage mailmsg = new MailMessage(new MailAddress(emailCredential.UserName.Trim(), emailCredential.DisplayName), new MailAddress(toAddress)))
                        {
                            mailmsg.BodyEncoding = Encoding.Default;
                            mailmsg.Subject = subject.Trim();
                            mailmsg.Body = message;
                            mailmsg.IsBodyHtml = true;

                            if (!string.IsNullOrEmpty(eCubeAdminBccEmail))
                            {
                                foreach (string bcc in eCubeAdminBccEmail.Split(",".ToCharArray()))
                                {
                                    mailmsg.Bcc.Add(bcc.Trim());
                                }
                            }

                            if (!string.IsNullOrEmpty(eCubeAdminCcEmail))
                            {
                                foreach (string sCC in eCubeAdminCcEmail.Split(",".ToCharArray()))
                                {
                                    mailmsg.CC.Add(sCC.Trim());
                                }
                            }

                            if (emailCredential.ReplierEmail != string.Empty)
                            {
                                mailmsg.ReplyToList.Add(new MailAddress(emailCredential.ReplierEmail, "reply-to"));
                            }

                            string strAdminEmail = emailCredential.ReplierEmail;
                            if (strAdminEmail != string.Empty)
                            {
                                mailmsg.From = new MailAddress(strAdminEmail, emailCredential.DisplayName, System.Text.Encoding.UTF8);
                            }
                            else
                            {
                                mailmsg.From = new MailAddress(emailCredential.UserName.Trim(), emailCredential.DisplayName, System.Text.Encoding.UTF8);
                            }

                            using (var client = new SmtpClient(emailCredential.SMPTServer, emailCredential.Port))
                            {
                                client.Credentials = new NetworkCredential(emailCredential.UserName.Trim(), emailCredential.Password.Trim());
                                client.EnableSsl = emailCredential.EnableSSL;

                                client.Send(mailmsg);
                                return true;
                            }
                        }
                    }
                    else
                    {
                        ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP2);
                        service.Credentials = new NetworkCredential(emailCredential.UserName.Trim(), emailCredential.Password);
                        service.AutodiscoverUrl(emailCredential.UserName.Trim());

                        EmailMessage emailMessage = new EmailMessage(service);
                        emailMessage.Subject = subject.Trim();
                        emailMessage.Body = new MessageBody();

                        message = message.Contains("<body>") ? message : "<body>" + message + "</body>";
                        message = message.Contains("<html>") ? message : "<html><head><title></title></head>" + message + "</html>";

                        emailMessage.Body.Text = message;
                        emailMessage.Body.BodyType = BodyType.HTML;

                        if (!string.IsNullOrEmpty(toAddress))
                        {
                            emailMessage.ToRecipients.Add(new Microsoft.Exchange.WebServices.Data.EmailAddress(toAddress));
                        }

                        if (!string.IsNullOrEmpty(emailCredential.ReplierEmail))
                        {
                            emailMessage.ReplyTo.Add(new Microsoft.Exchange.WebServices.Data.EmailAddress(emailCredential.ReplierEmail));
                        }

                        if (!string.IsNullOrEmpty(eCubeAdminCcEmail))
                        {
                            foreach (string cc in eCubeAdminCcEmail.Split(",".ToCharArray()))
                            {
                                emailMessage.CcRecipients.Add(new Microsoft.Exchange.WebServices.Data.EmailAddress(cc.Trim()));
                            }
                        }

                        if (!string.IsNullOrEmpty(eCubeAdminBccEmail))
                        {
                            foreach (string bcc in eCubeAdminBccEmail.Split(",".ToCharArray()))
                            {
                                emailMessage.BccRecipients.Add(new Microsoft.Exchange.WebServices.Data.EmailAddress(bcc.Trim()));
                            }
                        }

                        if (attachmentPath != null)
                        {
                            foreach (string path in attachmentPath)
                            {
                                if (File.Exists(path))
                                {
                                    emailMessage.Attachments.AddFileAttachment(path);
                                }
                            }
                        }

                        emailMessage.SendAndSaveCopy();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return false;
            }
        }

        /// <summary>
        /// Email Log.
        /// </summary>
        /// <param name="releventId">The relevant identifier.</param>
        /// <param name="moduleId">The module identifier.</param>
        /// <param name="mailContent">Content of the mail.</param>
        /// <param name="mailTo">The mail to.</param>
        /// <param name="cc">Parameter cc.</param>
        /// <param name="bcc">Parameter  BCC.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="pageId">The page identifier.</param>
        public static void SaveEmailLog(long releventId, long moduleId, string mailContent, string mailTo, string cc, string bcc, long userId, long pageId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.EmailService + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                EmailLogDataContext objDataContext = new EmailLogDataContext();
                objDataContext.InsertOrUpdateEmailLog(0, releventId, moduleId, mailContent, mailTo, cc, bcc, DateTime.Now, userId, pageId);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }
        }
    }
}