using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Tests;
using CA.SMSMessage;
//using CA.SMS.SendMessage;

namespace Test
{
    [TestClass]
    public class SMSTest
    {
        [TestMethod]
        public void SendMessage()
        {
            OrganizationServiceProxy service = CRMConnection.GetCRMConnection(CRMConnection.connDev, "rgomez@bizxrm.com", "Welcome2015");

            EntityReference emailReference = new EntityReference("dm_smsmessage", new Guid("875EE88C-6FC8-E611-80E6-0050568A586C")); ;

            SMSMessageSend sendMessage = new SMSMessageSend();
            sendMessage.Run(service, emailReference, "056500663939d967e7f94cbcd553c3b1", "http://SRV-CRM-FRONT/ManagementConsole", "AC4871905c391560002b9eb8fcbf00a405");
        }
    }
}
