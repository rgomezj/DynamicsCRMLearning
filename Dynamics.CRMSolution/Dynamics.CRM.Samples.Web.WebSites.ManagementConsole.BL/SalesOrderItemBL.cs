using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pavliks.WAM.ManagementConsole.BL
{


    public class SalesOrderItemBL
    {

        #region Attributes
        private ISalesOrderItemRepository _SalesOrderItemRepository;
        private IRegistrationRepository _RegistrationRepository;
        private OrderManagementItemBL _OrderManagementItemBL;
        #endregion

        #region Methods

        public SalesOrderItemBL(ISalesOrderItemRepository salesOrderItemRepository, IRegistrationRepository registrationRepository)
        {
            this._SalesOrderItemRepository = salesOrderItemRepository;
            this._RegistrationRepository = registrationRepository;
        }

        public List<SalesOrderItem> GetAllSalesOrderItemByOrderId(string orderId)
        {
            List<Registration> registrations = _RegistrationRepository.GetRegistrationsByOrder(Guid.Parse(orderId));
            List<SalesOrderItem> salesOrderItems = new List<SalesOrderItem>();
            SalesOrderItem salesOrderItem;
            foreach (Registration registration in registrations)
            {
                if (registration.CourseFeeGross > 0)
                {
                    salesOrderItem = GetItemFromRegistration(registration, LineItemType.CourseFee);
                    salesOrderItem.Event = registration.Event;
                    salesOrderItem.SalesOrder = new SalesOrder() { Id = Guid.Parse(orderId) };
                    salesOrderItem.Refunded = registration.Refunded;
                    salesOrderItem.Deactivated = registration.Deactivated;
                    salesOrderItems.Add(salesOrderItem);
                }

                if (registration.AMCareFeeGross > 0)
                {
                    salesOrderItem = GetItemFromRegistration(registration, LineItemType.AmCare);
                    salesOrderItem.Event = registration.Event;
                    salesOrderItem.SalesOrder = new SalesOrder() { Id = Guid.Parse(orderId) };
                    salesOrderItem.Refunded = registration.Refunded;
                    salesOrderItem.Deactivated = registration.Deactivated;
                    salesOrderItems.Add(salesOrderItem);
                }

                if (registration.PMCareFeeGross > 0)
                {
                    salesOrderItem = GetItemFromRegistration(registration, LineItemType.PmCare);
                    salesOrderItem.Event = registration.Event;
                    salesOrderItem.SalesOrder = new SalesOrder() { Id = Guid.Parse(orderId) };
                    salesOrderItem.Refunded = registration.Refunded;
                    salesOrderItem.Deactivated = registration.Deactivated;
                    salesOrderItems.Add(salesOrderItem);
                }

                if (registration.SupervisedLunchGross > 0)
                {
                    salesOrderItem = GetItemFromRegistration(registration, LineItemType.SupervisedLunch);
                    salesOrderItem.Event = registration.Event;
                    salesOrderItem.SalesOrder = new SalesOrder() { Id = Guid.Parse(orderId) };
                    salesOrderItem.Refunded = registration.Refunded;
                    salesOrderItem.Deactivated = registration.Deactivated;
                    salesOrderItems.Add(salesOrderItem);
                }
                if (registration.ForCreditFeeGross > 0)
                {
                    salesOrderItem = GetItemFromRegistration(registration, LineItemType.CollegeCreditFee);
                    salesOrderItem.Event = registration.Event;
                    salesOrderItem.SalesOrder = new SalesOrder() { Id = Guid.Parse(orderId) };
                    salesOrderItem.Refunded = registration.Refunded;
                    salesOrderItem.Deactivated = registration.Deactivated;
                    salesOrderItems.Add(salesOrderItem);
                }

            }
            return salesOrderItems;
        }

        private SalesOrderItem GetItemFromRegistration(Registration registration, LineItemType lineItemType)
        {
            decimal total = 0;
            decimal pricePerUnit = 0;
            decimal manualDiscount = 0;
            decimal credit = 0;
            decimal scholarshipfund = 0;
            if (lineItemType == LineItemType.CourseFee)
            {
                total = registration.CourseFee;
                pricePerUnit = registration.CourseFeeGross;
                credit = registration.CourseCreditCourseFee;
                manualDiscount = pricePerUnit - total;

                total = registration.CourseFee - credit;

                if (registration.ContainsScholarpship)
                {
                    scholarshipfund = total;
                    total -= scholarshipfund;
                }

            }
            else if (lineItemType == LineItemType.AmCare)
            {
                total = registration.AmCare;

                pricePerUnit = registration.AMCareFeeGross;
                credit = registration.CourseCreditAmCare;
                manualDiscount = pricePerUnit - total;


                if (registration.ContainsScholarpship)
                {
                    scholarshipfund = total;
                }
            }
            else if (lineItemType == LineItemType.PmCare)
            {
                total = registration.PmCare;

                pricePerUnit = registration.PMCareFeeGross;
                credit = registration.CourseCreditPmCare;
                manualDiscount = pricePerUnit - total;


                if (registration.ContainsScholarpship)
                {
                    scholarshipfund = total;
                }
            }
            else if (lineItemType == LineItemType.SupervisedLunch)
            {
                total = registration.SupervisedLunch;

                pricePerUnit = registration.SupervisedLunchGross;
                credit = registration.CourseCreditSupervisedLunch;
                manualDiscount = pricePerUnit - total;


                if (registration.ContainsScholarpship)
                {
                    scholarshipfund = total;
                }
            }
            else if (lineItemType == LineItemType.CollegeCreditFee)
            {
                total = registration.ForCreditFee;

                pricePerUnit = registration.ForCreditFeeGross;
                credit = registration.CourseCreditAppliedToForCredit;
                manualDiscount = pricePerUnit - total;


            }
            return new SalesOrderItem() { Description = string.Format("Reg.: {0}, {1}", registration.Name, lineItemType), Registration = registration, LineItemType = lineItemType, Amount = total ,PricePerUnit=pricePerUnit ,ManualDiscount=manualDiscount,CourseCredit=credit,ScholarshipAmount=scholarshipfund };
        }

        #endregion




    }
}
