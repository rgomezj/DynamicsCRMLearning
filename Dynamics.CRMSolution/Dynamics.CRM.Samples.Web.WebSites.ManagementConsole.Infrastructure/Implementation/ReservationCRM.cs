using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using CrmToolkit;
using Pavliks.WAM.ManagementConsole.Helpers;

namespace Pavliks.WAM.ManagementConsole.Infrastructure.Implementation
{
    public class ReservationCRM : IReservationRepository
    {

        public Guid CreateReservation(Reservation reservation)
        {
            DataManager DataManager = new DataManager();
            ReservationMapper reservationMapper = new ReservationMapper(DataManager.ConnectionOnpremise());
            return DataManager.Create(reservationMapper.DomainToEntity(reservation));
        }

        public List<Reservation> GetReservationsByRegistration(Guid registrationId)
        {
            //Creates an crm connection.
            DataManager DataManager = new DataManager();
            ReservationMapper reservationMapper = new ReservationMapper(DataManager.ConnectionOnpremise());
            List<Reservation> reservations = new List<Reservation>();

            //Now I will search the order  that is associated with the registration.
            QueryExpression query = new QueryExpression("dm_reservation")
            {
                ColumnSet = new ColumnSet(true)
            };

            query.LinkEntities.Add(new LinkEntity("dm_reservation", "dm_class", "dm_sessionid", "dm_classid", JoinOperator.Inner));
            query.LinkEntities[0].EntityAlias = "Classes";
            query.LinkEntities[0].Columns.AddColumns("dm_subject", "dm_id");
            //we get just the activated registration.
            ConditionExpression statusCondition = new ConditionExpression("statuscode", ConditionOperator.Equal, 1);
            ConditionExpression registrationCondition = new ConditionExpression("dm_reservedforid", ConditionOperator.Equal, registrationId);
            query.Criteria.AddCondition(statusCondition);
            query.Criteria.AddCondition(registrationCondition);
            EntityCollection reservationCollection = DataManager.RetrieveMultiple(query);

            foreach (var reservation in reservationCollection.Entities)
            {
                //Converts the registration to a registration domain.
                reservations.Add(reservationMapper.EntityToDomain(reservation));
            }


            return reservations;
        }
    }
}
