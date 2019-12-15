using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrmToolkit;
using CrmToolkit.Attributes;

namespace Pavliks.WAM.ManagementConsole.Domain
{
    [Serializable]
    public class Event
    {
        #region Properties
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int dm_registrationlimit { get; set; }
        public DateTime dm_startdate { get; set; }
        public int dm_waitlist { get; set; }
        public bool dm_hybrid { get; set; }
        public int dm_totalregistrants { get; set; }
        public int SpacesAvailable { get; set; }

        #endregion
    }


    public enum oha_type
    {
        ConferencesAndSeminars = 1,
        DistanceLearning = 2,
        ContinuingEducation = 3,
        GovernanceCourses = 4,
        HealthAchieve = 5,
        Marketing = 6,
        BusinessOfHealth = 7,
        DatesToRemember = 8,
        BoardMeetings = 9
    }
}
