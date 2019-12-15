using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pavliks.WAM.ManagementConsole.BL
{
    public class OrderManagementItemBL
    {
        #region Attributes

        private IOrderManagementItemRepository _orderManagementRepository;
        private ISalesOrderRepository _salesOrderRepository;
        private ISalesOrderItemRepository _salesOrderItemRepository;
        private IRegistrationRepository _registrationRepository;
        private IOrderTransactionRepository _OrderTransactionRepository;



        #endregion

        #region Methods

        public OrderManagementItemBL(IOrderManagementItemRepository orderManagementRepository, ISalesOrderRepository salesOrderRepository, ISalesOrderItemRepository salesOrderItemRepository, IRegistrationRepository registrationRepository, IOrderTransactionRepository _OrderTransactionRepository)
        {
            this._orderManagementRepository = orderManagementRepository;
            this._salesOrderRepository = salesOrderRepository;
            this._salesOrderItemRepository = salesOrderItemRepository;
            this._registrationRepository = registrationRepository;
            this._OrderTransactionRepository = _OrderTransactionRepository;

        }

        public void RemoveOrderManagementItemsById(Guid idItem)
        {
            _orderManagementRepository.DeleteOrderManagementItem(idItem);
        }

        public void RemoveOrderManagementItemsByOrderId(Guid salesOrder)
        {
            List<OrderManagementItem> orderManagementItems = _orderManagementRepository.GetOrderManagementItemBySalesOrder(salesOrder, StatusItem.InProgress);

            foreach (OrderManagementItem orderManagementItem in orderManagementItems)
            {
                _orderManagementRepository.DeleteOrderManagementItem(orderManagementItem.Id);
            }
        }

        public Guid CreateOrderManagementRegistration(Registration registration, Guid? salesOrderItemId, ActionItem actionItem, StatusItem statusItem)
        {
            OrderManagementItem orderManagementItem = new OrderManagementItem();
            orderManagementItem.ActionItem = actionItem;
            orderManagementItem.Price = 0; // Always 0 for registrations
            orderManagementItem.Quantity = 1; // Always 1 fro registrations
            orderManagementItem.Registration = registration;
            orderManagementItem.SalesOrder = registration.SalesOrder;
            orderManagementItem.SalesOrderItem = salesOrderItemId.HasValue ? salesOrderItemId.Value : Guid.Empty;
            orderManagementItem.StatusItem = statusItem;
            Guid idGenerated = _orderManagementRepository.CreateOrderManagementItem(orderManagementItem);
            return idGenerated;
        }

        public Guid CreateOrderManagementReservation(Reservation reservation, Guid? salesOrderItemId, ActionItem actionItem, StatusItem statusItem)
        {
            OrderManagementItem orderManagementItem = new OrderManagementItem();
            orderManagementItem.ActionItem = actionItem;
            orderManagementItem.Price = 0; // Always 0 for reservations
            orderManagementItem.Quantity = 1; // Always 1 for reservations
            orderManagementItem.Reservation = reservation;
            orderManagementItem.Registration = reservation.Registration;
            orderManagementItem.SalesOrder = reservation.Registration.SalesOrder;
            orderManagementItem.SalesOrderItem = salesOrderItemId.HasValue ? salesOrderItemId.Value : Guid.Empty;
            orderManagementItem.StatusItem = statusItem;
            Guid idGenerated = _orderManagementRepository.CreateOrderManagementItem(orderManagementItem);
            return idGenerated;
        }

        public Guid CreateOrderManagementItem(Registration registration, SalesOrder salesOrder, Guid? salesOrderItemId, Decimal quantity, Decimal amount, ActionItem actionItem, StatusItem statusItem, LineItemType lineitemtype)
        {
            OrderManagementItem orderManagementItem = new OrderManagementItem();
            orderManagementItem.ActionItem = actionItem;
            orderManagementItem.Price = amount; // Always 0 for eventOption
            orderManagementItem.Quantity = quantity; // Always 1 for eventOption
            orderManagementItem.Registration = null;
            orderManagementItem.SalesOrder = salesOrder;
            orderManagementItem.Registration = registration;
            orderManagementItem.SalesOrderItem = salesOrderItemId.HasValue ? salesOrderItemId.Value : Guid.Empty;
            orderManagementItem.StatusItem = statusItem;
            orderManagementItem.LineItemType = lineitemtype;
            Guid idGenerated = _orderManagementRepository.CreateOrderManagementItem(orderManagementItem);
            return idGenerated;
        }


        public Guid CreateOrderManagementProduct(Product product, SalesOrder salesOrder, Guid? salesOrderItemId, Contact contact, Decimal quantity, Decimal amount, ActionItem actionItem, StatusItem statusItem)
        {
            OrderManagementItem orderManagementItem = new OrderManagementItem();
            orderManagementItem.ActionItem = actionItem;
            orderManagementItem.Price = amount; // Always 0 for eventOption
            orderManagementItem.Quantity = quantity; // Always 1 for eventOption
            orderManagementItem.Registration = null;
            orderManagementItem.Product = product;
            orderManagementItem.SalesOrder = salesOrder;
            orderManagementItem.SalesOrderItem = salesOrderItemId.HasValue ? salesOrderItemId.Value : Guid.Empty;
            orderManagementItem.StatusItem = statusItem;
            orderManagementItem.Contact = contact;
            Guid idGenerated = _orderManagementRepository.CreateOrderManagementItem(orderManagementItem);
            return idGenerated;
        }

        public void UpdateManagementProduct(SalesOrderItem Oitem)
        {

            //_orderManagementRepository.UpdateOrderManagemertItem(Oitem);

        }
        public void UpdateOrderManagement(Guid? orderManagementItemId, decimal amount)
        {
            OrderManagementItem orderManagementItem = new OrderManagementItem();
            orderManagementItem.Id = orderManagementItemId.Value;
            orderManagementItem.Price = amount;
            _orderManagementRepository.UpdateOrderManagemertItem(orderManagementItem);

        }
        public void UpdateOrderManagementDeactivatedFlag(Guid? orderManagementItemId, bool flag)
        {
            OrderManagementItem orderManagementItem = new OrderManagementItem();
            orderManagementItem.Id = orderManagementItemId.Value;
            orderManagementItem.DeactivatedFlag = flag;
            _orderManagementRepository.UpdateOrderManagemertItemDeactivatedFlag(orderManagementItem);

        }

        public void ConfirmedOrderManagementItemsByOrderId(Guid salesOrder,bool isCheque, string chequeNumber)
        {
            List<OrderManagementItem> orderManagementItems = _orderManagementRepository.GetOrderManagementItemBySalesOrder(salesOrder, StatusItem.InProgress);
            SalesOrder so = _salesOrderRepository.GetSalesOrderById(salesOrder.ToString());
            OrderTransaction orderTransaction = _OrderTransactionRepository.GetLastRefundTransaction(salesOrder);

            if (orderManagementItems != null)
            {
                SalesOrder order = new SalesOrder();

                //Takes the price list from the product.
                //salesOrder.PriceList = product.PriceList;
                if (so.Currency != null)
                {
                    order.Currency = so.Currency;
                }
                if (so.Customer != null)
                {
                    order.Customer = so.Customer;
                }
                if (so.PriceList != null)
                {
                    order.PriceList = so.PriceList;
                }
                if (isCheque)
                {
                    order.paymentType = PaymentType.Check;
                    order.ChequeNumber = chequeNumber;
                }
                else
                {
                    order.paymentType = so.paymentType;
                }

                order.Name = string.Format("Order for {0} - {1}", "Refund ", DateTime.Today.ToShortDateString());
                order.typeOfOrder = TypeOrder.Refund;
                Guid orderId = _salesOrderRepository.CreateOrder(order);
                order.Id = orderId;

                List<Guid> registrations = orderManagementItems.Select(x => x.Registration.Id).Distinct().ToList();
                bool isScholarship = false;

                foreach (Guid reg in registrations)
                {
                    Registration registration = _registrationRepository.GetRegistrationById(reg);
                    if (registration.ContainsScholarpship)
                    {
                        isScholarship = true;
                    }
                    decimal value = orderManagementItems.Where(c => c.Registration.Id == reg && c.LineItemType == LineItemType.CourseFee).Sum(c => c.Price);

                    //insert course fee
                    if (value > 0)
                    {
                        CreateSalesOrderItem(registration, order, value, LineItemTypeInSalesOrder.CourseFee, messageByLineItemType(LineItemType.CourseFee));
                    }
                    //insert addons
                    value = orderManagementItems.Where(c => c.Registration.Id == reg && c.LineItemType == LineItemType.AmCare).Sum(c => c.Price);
                    if (value > 0)
                    {
                        CreateSalesOrderItem(registration, order, value, LineItemTypeInSalesOrder.AddonsFee, messageByLineItemType(LineItemType.AmCare));
                    }

                    value = orderManagementItems.Where(c => c.Registration.Id == reg && c.LineItemType == LineItemType.PmCare).Sum(c => c.Price);
                    if (value > 0)
                    {
                        CreateSalesOrderItem(registration, order, value, LineItemTypeInSalesOrder.AddonsFee, messageByLineItemType(LineItemType.PmCare));
                    }

                    value = orderManagementItems.Where(c => c.Registration.Id == reg && c.LineItemType == LineItemType.SupervisedLunch).Sum(c => c.Price);
                    if (value > 0)
                    {
                        CreateSalesOrderItem(registration, order, value, LineItemTypeInSalesOrder.AddonsFee, messageByLineItemType(LineItemType.SupervisedLunch));
                    }

                    value = orderManagementItems.Where(c => c.Registration.Id == reg && c.LineItemType == LineItemType.CollegeCreditFee).Sum(c => c.Price);
                    if (value > 0)
                    {
                        CreateSalesOrderItem(registration, order, value, LineItemTypeInSalesOrder.ForCreditFee, messageByLineItemType(LineItemType.CollegeCreditFee));
                    }

                    registration.Refunded = true;
                    _registrationRepository.UpdateRefundedRegistration(registration);
                    if (orderManagementItems.Any(c => c.Registration.Id == reg && c.LineItemType == LineItemType.CourseFee && c.DeactivatedFlag==true))
                    {
                        registration.Deactivated = true;
                        _registrationRepository.UpdateDeactivatedRegistration(registration);
                    }
                    List<OrderManagementItem> orderManagementItemsToUpdate = orderManagementItems.Where(c => c.Registration.Id == reg).ToList();
                    foreach (OrderManagementItem orderManagementItem in orderManagementItemsToUpdate)
                    {
                        _orderManagementRepository.ChangeStatusOrderManagementItem(orderManagementItem.Id, StatusItem.Confirmed, StateItem.Active);
                    }
                }
                if (isScholarship)
                {
                    _salesOrderRepository.isScholarpshipOrder(order.Id, isScholarship);
                }
                if (orderTransaction != null)
                {
                    orderTransaction.SalesOrder = order;
                    orderTransaction.Id = Guid.Empty;
                    orderTransaction.Id = _OrderTransactionRepository.CreateOrderTransaction(orderTransaction);
                }
                _salesOrderRepository.CloseOrder(order.Id);


            }
        }


        public void CreateSalesOrderItem(Registration registration, SalesOrder order, decimal amount, LineItemTypeInSalesOrder lineItemType, string description)
        {
            SalesOrderItem item = new SalesOrderItem();
            item.Registration = registration;
            item.Event = registration.Event;
            item.Contact = registration.Contact;
            item.Quantity = 1;
            item.ProductDescription = "Refund";
            item.SalesOrder = order;
            item.Amount = amount;
            item.LineItemTypeInsalesOrder = lineItemType;
            item.Description = description;
            _salesOrderItemRepository.CreateSalesOrderItem(item);
        }
        public string messageByLineItemType(LineItemType lineItemtype)
        {
            string message = "";
            if (lineItemtype == LineItemType.CourseFee)
            {
                message = "Refund Course Fee";
            }
            else if (lineItemtype == LineItemType.AmCare)
            {
                message = "Refund AM Care";
            }
            else if (lineItemtype == LineItemType.PmCare)
            {
                message = "Refund PM Care";
            }
            else if (lineItemtype == LineItemType.SupervisedLunch)
            {
                message = "Refund Supervised Lunch";
            }
            else if (lineItemtype == LineItemType.CollegeCreditFee)
            {
                message = "Refund College Credit Fee";
            }

            return message;
        }

        #endregion




    }
}
