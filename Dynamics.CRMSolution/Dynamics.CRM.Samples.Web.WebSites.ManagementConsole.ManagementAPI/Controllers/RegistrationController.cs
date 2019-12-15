using AutoMapper;
using Pavliks.WAM.ManagementConsole.BL;
using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Implementation;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using Pavliks.WAM.ManagementConsole.ManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI.Controllers
{
    public class RegistrationController : ApiController
    {
        private RegistrationBL _registrationBL;
        private ProductBL _ProductBL;
        private EventBl _EventBL;
        private SessiontBL _SessionBL;
        private ReservationBL _ReservationBL;
        
        public RegistrationController(IRegistrationRepository _registrationRepository, IEventRepository _EventRepository, IProductRepository _ProductRepository, ISessionRepository _SessionRepository, IReservationRepository _ReservationRepository, IOrderManagementItemRepository _OrderManagementItemRepository, ISalesOrderRepository _salesOrderRepository, ISalesOrderItemRepository _salesOrderItemRepository, IOrderTransactionRepository _OrderTransactionRepository)
        {
            _registrationBL = new RegistrationBL(_registrationRepository, _ReservationRepository, _OrderManagementItemRepository, _salesOrderRepository,_salesOrderItemRepository,_OrderTransactionRepository);
            _ProductBL = new ProductBL(_ProductRepository);
            _EventBL = new EventBl(_EventRepository);
            _SessionBL = new SessiontBL(_SessionRepository);
            _ReservationBL = new ReservationBL(_ReservationRepository);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.ActionName("RegistrationByEvent")]
        public IEnumerable<RegistrationIndexViewModel> Registrations(string eventId)
        {

            List<Registration> registrations = _registrationBL.GetAllRegistration(eventId);
            Guid eventIdentifier = Guid.Parse(eventId);
            #region convertion
            //It converts the Registration domain record to Registration Index ViewModel
            AutoMapper.MapperConfiguration config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Registration, RegistrationIndexViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            var source = new Registration();
            var registrationsViewModel = mapper.Map<IEnumerable<Registration>, IEnumerable<RegistrationIndexViewModel>>(registrations);
            #endregion
            return registrationsViewModel;

        }

        [System.Web.Http.HttpGet]
        public Registration Registration(string id)
        {
            Registration registration = _registrationBL.GetRegistrationById(new Guid(id));
            return registration;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.ActionName("RegistrationAllInfo")]
        public RegistrationOptionsViewModel RegistrationAllInfo(string registrationId)
        {
            Registration registration = _registrationBL.GetRegistrationById(new Guid(registrationId));

            RegistrationOptionsViewModel registrationOptionsViewModel = new RegistrationOptionsViewModel();
            List<Reservation> reservations = _ReservationBL.GetReservationsByRegistration(registration.Id).ToList();
            var sessions = from p in reservations
                           select new CourseClass() { Id = p.CourseClass.Id, dm_id = p.CourseClass.dm_id, dm_subject = p.CourseClass.dm_subject };

            registrationOptionsViewModel.Classes = sessions.ToList();

            registrationOptionsViewModel.Event = registration.Event;
            registrationOptionsViewModel.Registration = registration;
            return registrationOptionsViewModel;
        }
    }
}