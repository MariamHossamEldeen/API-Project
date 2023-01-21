using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class Department : BaseEntity
    {
        public string Name { get; set; }

        // Navigation Property [Many]
        //public Employee Employee { get; set; }
    }
}
