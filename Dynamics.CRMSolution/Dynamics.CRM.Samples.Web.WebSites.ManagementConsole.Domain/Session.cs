using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrmToolkit.Attributes;
using CrmToolkit;

namespace Pavliks.WAM.ManagementConsole.Domain
{
    [Serializable]
    public class CourseClass
    {
        public Guid Id { get; set; }
        public string dm_id { get; set; }
        public string dm_subject { get; set; }
    }
}
