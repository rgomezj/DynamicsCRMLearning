using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Domain
{
  public  class Waitlist
    {
        #region Attributes

        private Contact contact;

        private Event eventCRM;

        private string eventName;

        #endregion

        #region Properties

        public Guid Id { get; set; }

        public string RegistrationLink { get; set; }


        public DateTime CreatedOn { get; set; }

        public string EventName
        {
            get { return eventName; }
            set { eventName = value; }
        }

        public Contact Contact
        {
            get { return contact; }
            set { contact = value; }
        }
        public Event EventCRM
        {
            get { return eventCRM; }
            set { eventCRM = value; }
        }

        public SalesOrder SalesOrder { get; set; }

        #endregion
    }
}
