using Microsoft.Xrm.Sdk.Client;
using System;
using System.ServiceModel.Description;

namespace Tests
{

    public class CRMConnection
    {
        //public const string connDev = "https://wamdev.bizxrm.com/XRMServices/2011/Organization.svc";
        public const string connDev = "https://wamdev.bizxrm.com/XRMServices/2011/Organization.svc";
        public const string connDev1B = "https://wamdev1b.pavliks.com/XRMServices/2011/Organization.svc";
        public const string connQA = "https://wamqa.pavliks.com/XRMServices/2011/Organization.svc";

        /// <summary>
        /// Obtain CRM connection given the organization service
        /// </summary>
        /// <param name="crmInstance">Connection instance</param>
        /// <returns>Servicio de coneción</returns>
        public static OrganizationServiceProxy GetCRMConnection(string crmInstance, string user, string password)
        {
            try
            {
                OrganizationServiceProxy service;
                ClientCredentials credentials = new ClientCredentials();
                credentials.UserName.UserName = user;
                credentials.UserName.Password = password;
                service = new OrganizationServiceProxy(new Uri(crmInstance), null, credentials, null);
                service.Authenticate();

                return service;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.ToString());
            }

        }
    }
}

