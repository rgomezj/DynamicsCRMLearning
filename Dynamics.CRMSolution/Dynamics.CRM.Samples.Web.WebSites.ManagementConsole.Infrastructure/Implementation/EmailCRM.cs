using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using CrmToolkit;
using Pavliks.WAM.ManagementConsole.Helpers;
using Microsoft.Crm.Sdk.Messages;
using System.IO;
using System.Net;
using System.Net.Cache;

namespace Pavliks.WAM.ManagementConsole.Infrastructure.Implementation
{
    public class EmailCRM : IEmailRepository
    {



        /// <summary>
        /// Method that allows you to generate the receipt report
        /// </summary>
        /// <param name="salesOrderId">sales order id</param>
        /// <returns>The report as web reponse.</returns>
        public byte[] ReceiptReport(Configuration configuration, string salesOrderId)
        {

            string serverURL = configuration.ReportServerURL;
            string reportPath = configuration.OrderReceiptReportPath;
            string user = configuration.UserNameReportServer;
            string password = configuration.PasswordReportServer;
            string format = WebConfigurationManager.AppSettings["format"];
            string param = String.Format("OrderGuid={0}", salesOrderId);

            HttpWebRequest request;
            String reportURL;
            if (!"EXCEL,PDF,XML,CSV".Split(',').ToList().Any(f => f.Equals(format.ToUpper())))
            {
                throw new Exception("The format configured for the report is not correct.");
            }
            reportURL = "{serverURL}/ReportServer?{reportPath}&rs:Command=Render&rs:Format={format}";
            reportURL = reportURL.Replace("{serverURL}", serverURL).Replace("{reportPath}", reportPath).Replace("{format}", format);
            if (!String.IsNullOrEmpty(param))
            {
                reportURL += "&" + param;
            }
            try
            {
                request = (HttpWebRequest)WebRequest.Create(reportURL);
                request.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                request.Credentials = new NetworkCredential(user, password);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                return ReadFully(response.GetResponseStream(), 32768);

               
            }
            catch 
            {
                throw new Exception("An error occurred during the report generation.");
            }
        }

        /// <summary>
        /// Method that allows you to get the report as a byte array .
        /// </summary>
        /// <param name="stream">stream that will let you to read the array.</param>
        /// <param name="initialLength">array length</param>
        /// <returns></returns>
        private static byte[] ReadFully(Stream stream, int initialLength)
        {
            if (initialLength < 1)
            {
                initialLength = 32768;
            }
            byte[] buffer = new byte[initialLength];
            int read = 0;
            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();
                    if (nextByte == -1)
                    {
                        return buffer;
                    }
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }

        /// <summary>
        /// Method that sends an email to a group of contacts.
        /// </summary>
        bool IEmailRepository.SendEmailReceipt(Email emailToSend)
        {


            try
            {
                //Validates if there are contacts to send the email.
                if (emailToSend.Contacts.Count > 0)
                {
                    DataManager DataManager = new DataManager();

                    #region email configuration
                    Entity email = new Entity("email");
                    //creates a collection that will work as list of recipients.
                    List<Entity> toEntities = new List<Entity>();
                    //creates a collection that will work as list of from.
                    List<Entity> fromEntities = new List<Entity>();
                    //Gets the contacts from the email class.To those contacts we will send an email to each one of them.
                    List<Contact> contacts = emailToSend.Contacts;
                    foreach (var contact in contacts)
                    {
                        //Creates this entity to set it on "to " email part.
                        Entity recipient = new Entity("activityparty");
                        recipient["partyid"] = new EntityReference("contact", contact.Id);
                        toEntities.Add(recipient);

                    }

                    //Creates this entity to set it on "from " email part.
                    Entity from = new Entity("activityparty");
                    Guid userGuid = emailToSend.Configuration.Emailsender.Id;
                    from["partyid"] = new EntityReference("systemuser", userGuid);
                    fromEntities.Add(from);
                    email.Attributes["to"] = new EntityCollection(toEntities);
                    email["from"] = new EntityCollection(fromEntities);
                    //Creates the email with its configurable fields filled out.
                    Guid emailId = DataManager.Create(email);

                    #region Attaches receipt report

                    Entity att = new Entity("activitymimeattachment");
                    //att.Attributes["activityid"] = new EntityReference(emailReference.LogicalName, emailReference.Id);
                    att.Attributes["objectid"] = new EntityReference("email", emailId);
                    att.Attributes["objecttypecode"] = "email";
                    att["subject"] = "WAM Course Registration Confirmation";
                    att["attachmentnumber"] = 1;
                    byte[] receiptReport = ReceiptReport(emailToSend.Configuration, emailToSend.SalesOrder.Id.ToString());
                    MemoryStream s = new MemoryStream(receiptReport);
                    // 4.0 code:
                    att["body"] = Convert.ToBase64String(s.ToArray());
                    att["filename"] = "Receipt report" + ".pdf";
                    att["mimetype"] = "application/pdf";
                    DataManager.Create(att);
#endregion
                    #region Sends the email
                    // Use the SendEmail message to send an e-mail message.
                    SendEmailRequest request = new SendEmailRequest();
                    request.EmailId = emailId;
                    request.IssueSend = true;
                    request.TrackingToken = "";
                    DataManager.Execute(request);

                    #endregion
                    #endregion
                    return true;
                }
                else
                {
                    return false;

                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }
}
