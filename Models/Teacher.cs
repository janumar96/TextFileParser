using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public  class Teacher
    {
        public int StaffID { get; set; }
        public string Name { get; set; }
        public List<string> Qualifications { get; set; }
    }
}
