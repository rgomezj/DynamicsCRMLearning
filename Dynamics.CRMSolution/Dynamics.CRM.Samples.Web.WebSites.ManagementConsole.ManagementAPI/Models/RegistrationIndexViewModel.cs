using Pavliks.WAM.ManagementConsole.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI.Models
{
    public class RegistrationIndexViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SalesOrderId { get; set; }
        public string SalesOrderNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public string RegistrationStatus { get; set; }
        public string SalesOrderOutstandingBalance { get; set; }
        public string IntendedPaymentType { get; set; }
        public string GroupRegistration { get; set; }
        public string ContactId { get; set; }
        public string ContactFullName { get; set; }
        public string ConstituentFullName { get; set; }
        public string ContactJobTitle { get; set; }
        public bool IsBeingAdded { get; set; }
        public bool Refunded { get; set; }
        public string GroupRegistrationName;
    }
}