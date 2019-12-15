using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Domain
{
  public   class Contact
    {
        #region Attributes

        #endregion

        #region Properties

        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string BusinessPhone { get; set; }
        public string PreferedMethod { get; set; }


        #endregion
    }
}
