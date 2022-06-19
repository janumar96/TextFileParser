using System;
using System.Text;
using FileParser;
using Models;

namespace Assignment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                string? path = "";
                Console.WriteLine("Enter file path along with Name");
                path = Console.ReadLine();
                if (!string.IsNullOrEmpty(path))
                {
                    //Parsing File data to get Students information to display as report...
                    List<Student> students = Parser.ReadDelimitedFile(path);

                    //Display students report in format specified in file... 
                    PrintReport(students);
                }
                else
                {
                    Console.WriteLine("Please provide file path");
                }
            }
            catch (IOException ioexp)
            {
                Console.WriteLine("Input outpu exception thrown: " + ioexp.Message);
            }
            catch (Exception ex) //Just in case if any unexpected(General) exception occurred...
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void PrintReport(List<Student> students)
        {
            Console.WriteLine("\t\t\t\t Report \t\t\t\t\t\t\t\t\t");
            Console.WriteLine("--------------------------------------------------------------------------------------------");
            StringBuilder report = new StringBuilder();
            //Iterate over each student to display their results having course, instructors and marks details...
            foreach (Student student in students)
            {
                //Iterate over each result of student that is to be displayed...
                foreach (ExamResult result in student.ExamResults)
                {
                    report.Clear();

                    report.AppendFormat("Student \"{0}\" was taught the course \"{1}\" by instructors ", student.Name, result.Course.Title);
                    //List all the instructors that taught the course to perticulat student...
                    foreach (Teacher teacher in result.Instructors)
                    {
                        report.AppendFormat("\"{0}\" and", teacher.Name);
                    }
                    report.Remove(report.Length - 3, 3);
                    var examstatus = ((double)((result.Marks / result.Course.TotalMarks) * 100)) < 60 ? "failed" : "passed";
                    report.AppendFormat(". He \"{0}\" the exam by getting a score of \"{1}\" out of \"{2}\".", examstatus, result.Marks, result.Course.TotalMarks);
                    Console.WriteLine(report.ToString());
                }           
            }
        }
    }
}