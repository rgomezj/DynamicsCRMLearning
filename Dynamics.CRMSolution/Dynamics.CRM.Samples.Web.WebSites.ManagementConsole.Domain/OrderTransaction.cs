using CrmToolkit.Attributes;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Domain
{
    public class OrderTransaction
    {
        #region Properties
       
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public SalesOrder SalesOrder { get; set; }

        public string TransactionId { get; set; }

        public string CCNumber { get; set; }

        public decimal? TransactionAmount { get; set; }

        public TransactionType? TransactionType { get; set; }

        public PaymentTypeOrderTransaction? PaymentType { get; set; }

        public string TransactionComments { get; set; }

        public DateTime Date { get; set; }

        #endregion
    }

    public enum TransactionType
    {
        Sale = 602300000,
        Refund = 602300001

    }

    public enum PaymentTypeOrderTransaction
    {
        Visa = 602300000,
        Mastercard = 602300001,
        AmericanExpress = 602300002,
        Cheque = 602300003,
        ElectronicTransfer = 602300004,
        Cash = 602300005,
        OtherCard = 602300006

    }
}
