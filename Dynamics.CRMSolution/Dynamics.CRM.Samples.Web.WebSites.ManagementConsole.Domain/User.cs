using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Domain
{
    public class User
    {

        #region Properties
        public Guid Id { get; set; }
        public string FullName { get; set; }
        #endregion
    }
}
