
using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.BL
{
    public class WaitlistBL
    {
        #region Attributes
        private IWaitlistRepository _WaitlistRepository;
        private ISalesOrderRepository _salesOrderRepository;
        private IOrderCoursesRepository _orderCoursesRepository;

        #endregion
        #region Properties
        #endregion
        #region Methods

        public List<Waitlist> GetAllTheWaistlist()
        {
            return _WaitlistRepository.GetAllTheWaistlist();
        }

        public List<Waitlist> GetWaistlists(Guid eventId)
        {
            return _WaitlistRepository.GetWaistlists(eventId);
        }

        #endregion

        #region Constructor

        public WaitlistBL(IWaitlistRepository _WaitlistRepository, ISalesOrderRepository _salesOrderRepository,IOrderCoursesRepository _orderCoursesRepository)
        {
            this._WaitlistRepository = _WaitlistRepository;
            this._salesOrderRepository = _salesOrderRepository;
            this._orderCoursesRepository = _orderCoursesRepository;
        }

        public Waitlist GenerateLink(Waitlist waitlist)
        {
            #region Order Creation
            SalesOrder order = null;
            if (waitlist.SalesOrder != null && waitlist.SalesOrder.Id != null)
            {
                order = _salesOrderRepository.GetSalesOrderById(waitlist.SalesOrder.Id.ToString());
            }
            if (string.IsNullOrEmpty(waitlist.RegistrationLink))
            {
                SalesOrder salesOrder = new SalesOrder();
                if (order != null && order.Customer != null)
                {
                    salesOrder.Customer = order.Customer;
                }
                else
                {
                    salesOrder.Customer = waitlist.Contact;
                }
                salesOrder.Waitlist = waitlist;
                //Takes the price list from the product.
                //salesOrder.PriceList = product.PriceList;
                salesOrder.Name = string.Format("Order for {0} - {1}", waitlist.Contact.FullName, DateTime.Today);
                Guid orderId = _salesOrderRepository.CreateOrder(salesOrder);
                salesOrder.Id = orderId;
                //waitlist.SalesOrder = salesOrder;
                //Create an Order Courses
                OrderCourses orderCourses = new OrderCourses();
                orderCourses.Event = waitlist.EventCRM;
                orderCourses.SalesOrder = salesOrder;
                orderCourses.Id = _orderCoursesRepository.CreateOrderCourses(orderCourses);
                //Change the whole number to run a workflow
                waitlist.RegistrationLink = _WaitlistRepository.GenerateLink(waitlist, false);
            }
            return waitlist;
            #endregion
        }

        public Waitlist SendLink(Waitlist waitlist)
        {
            #region Order Creation
            if (!string.IsNullOrEmpty(waitlist.RegistrationLink))
            {
                _WaitlistRepository.SendEmail(waitlist, new Random().Next(100));
            }
            return waitlist;
            #endregion
        }
        #endregion
    }
}
