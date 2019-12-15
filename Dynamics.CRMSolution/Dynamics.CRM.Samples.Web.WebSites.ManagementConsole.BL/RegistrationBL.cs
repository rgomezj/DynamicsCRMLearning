using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pavliks.WAM.ManagementConsole.BL
{


    public class RegistrationBL
    {

        #region Attributes
        private IRegistrationRepository _registrationRepository;
        private IReservationRepository _reservationRepository;
        private ISalesOrderRepository _salesOrderRepository;
        private ISalesOrderItemRepository _salesOrderItemRepository;
        private IOrderTransactionRepository _OrderTransactionRepository;
        private OrderManagementItemBL _OrderManagementItemBL;
        
        #endregion
        #region Properties
        #endregion
        #region Methods

        public RegistrationBL(IRegistrationRepository registrationRepository, IReservationRepository reservationRepository, IOrderManagementItemRepository OrderManagementItemRepository, ISalesOrderRepository salesOrderRepository, ISalesOrderItemRepository salesOrderItemRepository, IOrderTransactionRepository _OrderTransactionRepository)
        {
            this._registrationRepository = registrationRepository;
            this._reservationRepository = reservationRepository;
            this._salesOrderRepository = salesOrderRepository;
            this._OrderManagementItemBL = new OrderManagementItemBL(OrderManagementItemRepository,salesOrderRepository,salesOrderItemRepository,registrationRepository,_OrderTransactionRepository);
        }

        public List<Registration> GetAllRegistration(string eventId)
        {
            return _registrationRepository.GetAllTheRegistration(eventId);
        }

        public Registration GetRegistrationById(Guid registrationId)
        {
            return _registrationRepository.GetRegistrationById(registrationId);
        }

        #endregion




    }
}
