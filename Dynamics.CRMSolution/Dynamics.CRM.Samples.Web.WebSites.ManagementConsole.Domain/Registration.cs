using CrmToolkit.Attributes;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Domain
{
    public class Registration
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of Registration
        /// </summary>
        public string Name { get; set; }

        //Created On
        public DateTime CreatedOn { get; set; }

        public SalesOrder SalesOrder { get; set; }

        public dm_registrationstatus? RegistrationStatus { get; set; }

        public Contact Contact { get; set; }

        public Contact Constituent { get; set; }

        public Event Event { get; set; }

        //Amount of Registration
        public decimal AmountPaid { get; set; }

        public StatusReason StatusReason { get; set; }
        public decimal CourseFee { get; set; }
        public decimal AmCare { get; set; }
        public decimal PmCare { get; set; }
        public decimal SupervisedLunch { get; set; }
        public decimal AmountAddons { get; set; }

        public decimal DiscountCourseFee { get; set; }
        public decimal DiscountAmCare { get; set; }
        public decimal DiscountPmCare { get; set; }
        public decimal DiscountSupervisedLunch { get; set; }
        public decimal OtherPromotionsCourseFee { get; set; }
        public decimal CourseCreditCourseFee { get; set; }
        public decimal CourseCreditAmCare { get; set; }
        public decimal CourseCreditPmCare { get; set; }
        public decimal CourseCreditSupervisedLunch { get; set; }
        public decimal CourseFeeGross { get; set; }
        public decimal AddonsFeeGross { get; set; }
        public decimal CourseCreditAppliedToCourseFee { get; set; }
        public decimal CourseCreditAppliedToForCredit { get; set; }
        public decimal PMCareFeeGross { get; set; }
        public decimal AMCareFeeGross { get; set; }
        public decimal SupervisedLunchGross { get; set; }
        public decimal ForCreditFeeGross { get; set; }
        public decimal ForCreditFee { get; set; }
        public bool ContainsScholarpship { get; set; }
        public bool Refunded { get; set; }
        public bool Deactivated { get; set; }

        #endregion
    }

    public enum dm_registrationstatus
    {
        Paid = 602300000,
        PendingPayment = 602300001,
        Imported = 602300003
    }
}
