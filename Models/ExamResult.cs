using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ExamResult
    {
        public Course Course { get; set; }
        public List<Teacher> Instructors { get; set; }
        public double Marks { get; set; }

        public bool IsAnyNull()
        {
            return this.Instructors != null && this.Marks != 0 && this.Course != null;
        }
    }
}
    

