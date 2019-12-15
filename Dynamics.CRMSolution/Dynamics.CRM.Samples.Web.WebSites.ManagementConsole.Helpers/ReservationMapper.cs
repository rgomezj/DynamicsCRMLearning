using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Helpers
{
    /// <summary>
    ///  Mapper class that converts a product entity to a product domain.
    /// </summary>
    public class ReservationMapper
    {

        private IOrganizationService _organizationService;
        public ReservationMapper(IOrganizationService service)
        {

            this._organizationService = service;
        }


        /// <summary>
        /// Mapper that converts a reservation entity to a reservation domain.
        /// </summary>
        /// <param name="reservationEntity">Registration as entity.</param>
        /// <returns>Registration converts as sales order domain.</returns>
        public Reservation EntityToDomain(Entity reservationEntity)
        {
            Reservation reservation = new Reservation();
            reservation.Id = reservationEntity.Id;
            if (reservationEntity.Contains("dm_name"))
            {
                reservation.Name= (string)reservationEntity["dm_name"];
            }

            if (reservationEntity.Contains("dm_sessionid"))
            {
                reservation.CourseClass = getSession((EntityReference)reservationEntity["dm_sessionid"]);
            }

            string attribute = string.Empty;
            object valueAttribute = null;

            attribute = Mapping.GetAttributeName(reservationEntity, "statuscode");

            if (reservationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                reservation.StatusReason = (StatusReason)((OptionSetValue)valueAttribute).Value;
            }

            attribute = Mapping.GetAttributeName(reservationEntity, "dm_sessionid");

            if (reservationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                reservation.CourseClass = new CourseClass() { Id = ((EntityReference)valueAttribute).Id };

                attribute = Mapping.GetAttributeName(reservationEntity, "Classes.dm_subject");

                if (reservationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
                {
                    reservation.CourseClass.dm_subject = ((AliasedValue)valueAttribute).Value.ToString();
                }

                attribute = Mapping.GetAttributeName(reservationEntity, "Classes.dm_id");

                if (reservationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
                {
                    reservation.CourseClass.dm_id = ((AliasedValue)valueAttribute).Value.ToString();
                }
            }

            return reservation;
        }

        /// <summary>
        /// Method that gets the user associated to the reservation.
        /// </summary>
        /// <param name="ownerReference"></param>
        /// <returns> The user with all its attributes. </returns>
        public User getOwner(EntityReference ownerReference)
        {

            Entity owner = _organizationService.Retrieve(ownerReference.LogicalName, ownerReference.Id, new ColumnSet(true));

            UserMapper userMapper = new UserMapper();

            return userMapper.EntityToDomain(owner);
        }

        /// <summary>
        /// Method that gets the session associated to the reservation.
        /// </summary>
        /// <param name="sessionReference"></param>
        /// <returns> The user with all its attributes. </returns>
        public CourseClass getSession(EntityReference sessionReference)
        {

            Entity session = _organizationService.Retrieve(sessionReference.LogicalName, sessionReference.Id, new ColumnSet(true));

            CourseClassMapper sessionMapper = new CourseClassMapper();

            return sessionMapper.EntityToDomain(session);
        }




        /// <summary>
        /// Mapper that converts a reservation domain to a reservation entity.
        /// </summary>
        /// <param name="reservation">Event option as domain.</param>
        /// <returns>Event option converts as event option entity.</returns>
        public Entity DomainToEntity(Reservation reservation)
        {
            Entity reservationEntity = new Entity("dm_reservation");
            reservationEntity.Id = reservation.Id;

            reservationEntity["dm_name"] = reservation.Name;
            if (reservation.Owner!=null)
            {
                reservationEntity["ownerid"] = new EntityReference("systemuser", reservation.Owner.Id);
            }

            if (reservation.Registration != null)
            {
                reservationEntity["dm_reservedforid"] = new EntityReference("dm_registration", reservation.Registration.Id);
            }

            if (reservation.Course != null)
            {
                reservationEntity["dm_courseid"] = new EntityReference("dm_course", reservation.Course.Id);
            }

            if (reservation.CourseClass != null)
            {
                reservationEntity["dm_sessionid"] = new EntityReference("dm_class", reservation.CourseClass.Id);
            }

            if (reservation.StatusReason != StatusReason.None)
            {
                reservationEntity["statuscode"] = new OptionSetValue((int)reservation.StatusReason);
            }

            return reservationEntity;
        }
    }
}
