using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser
{
    public class Parser
    {
        // Initialize list to formulate data from file in structured manner.
        static List<Teacher> teachers = new List<Teacher>();
        static List<Course> courses = new List<Course>();
        static List<Student> students = new List<Student>();
        public static List<Student> ReadDelimitedFile(string docPath)
        {           
            try
            {
                string[] all = File.ReadAllLines(docPath);


                // Read the file and display it line by line.
                using (var file = new StreamReader(docPath))
                {
                    //Iterate over file line by line to fetch data...
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        //Check and ignore comments i.e. lines starting with % or empty lines...
                        if (!(line.StartsWith("%") || line.Equals("")))
                        {
                            //Split line string on the basis of tab to check hierarchy. 
                            var segments = line.Split('\t', StringSplitOptions.None);
                            //If no tab involved i.e. single segment that means Heading at root hierarchy...
                            if (segments.Length == 1)
                            {
                                //Get heading to read perticular info from File...
                                string detailFor = segments[0];
                                if (detailFor.Equals("Teachers:"))
                                    teachers = GetTeachers(file);
                                if (detailFor.Equals("Courses:"))
                                    courses = GetCourses(file);
                                if (detailFor.Equals("Students:"))
                                    students = GetStudents(file);
                            }
                        }
                    }
                    //Close file that is opened...
                    file.Close();
                }
                //Fill out instructor info 
                foreach (Student student in students)
                {
                    foreach (ExamResult result in student.ExamResults)
                    {
                        try
                        {
                            Teacher teacher = result.Instructors.Where(inst => inst.Name == null).First();
                            teacher.Name = students[teacher.StaffID - 1].Name;
                            teacher.StaffID = students[teacher.StaffID - 1].Id;
                        }
                        catch (InvalidOperationException)
                        {
                            continue;
                        }
                    }
                }
            }
            catch(FileNotFoundException fileexp)
            {
                throw fileexp;
            }
            
            catch (Exception exp)
            { 
                throw exp;
            }
            return students;
        }

        public static List<Teacher> GetTeachers(StreamReader file)
        {
            //List to get teachers(instructors) details...
            List<Teacher> teachers = new List<Teacher>();
            string line;
            Teacher? teacher = null;
            try
            {
                while (!string.IsNullOrEmpty((line = file.ReadLine())))
                {
                    //Split line read from file to check hierarchy...
                    var segments = line.Split('\t', StringSplitOptions.None);
                    if (segments.Length == 2)
                    {

                        if (teacher != null)
                            teachers.Add(teacher);

                        teacher = new Teacher();
                    }
                    if (segments.Length == 3)
                    {
                        //Split line on the basis of : to get key value of teacher i.e. attribute and value...
                        string[] attrval = segments.Last().Split(":");
                        if (attrval[0].Equals("StaffID"))
                            teacher.StaffID = int.Parse(attrval[1]);
                        else if (attrval[0].Equals("Name"))
                            teacher.Name = attrval[1].TrimStart();
                        //Get list of qualifications against perticular teacher...
                        else if (attrval[0].Equals("Qualifications"))
                        {
                            teacher.Qualifications = (attrval[1].Replace("[", "").Replace("]", "")).Split(",").ToList<string>();
                        }
                    }
                }
                teachers.Add(teacher);
                return teachers;
            }
            catch (IOException ioexp)
            {
                throw ioexp;
            }
            catch (OutOfMemoryException memexp)
            {
                throw memexp;
            }
                   
        }

        public static List<Course> GetCourses(StreamReader file)
        {

            List<Course> courses = new List<Course>();
            string line;
            Course? course = null;
            try
            {
                while (!string.IsNullOrEmpty((line = file.ReadLine())))
                {
                    var segments = line.Split('\t', StringSplitOptions.None);

                    if (segments.Length == 2)
                    {
                        if (course != null)
                            courses.Add(course);

                        course = new Course();
                    }
                    if (segments.Length == 3)
                    {
                        string[] attrval = segments.Last().Split(":");
                        //KeyValuePair<String,String> teacherattr = new KeyValuePair<String,String>();
                        if (attrval[0].Equals("ID"))
                            course.Id = attrval[1];
                        else if (attrval[0].Equals("Title"))
                            course.Title = attrval[1].TrimStart();
                        else if (attrval[0].Equals("TotalMarks"))
                        {
                            course.TotalMarks = double.Parse(attrval[1]);
                        }
                    }
                }
                courses.Add(course);

                return courses;

            }
            catch (IOException ioexp)
            {
                throw ioexp;
            }
            catch (OutOfMemoryException memexp)
            {
                throw memexp;
            }
        }

        public static List<Student> GetStudents(StreamReader file)
        {
           
            List<Student> students = new List<Student>();
            string line;
            Student? student = null;
            ExamResult? result = null;
            try
            {
                while (!string.IsNullOrEmpty((line = file.ReadLine())))
                {
                    var segments = line.Split('\t', StringSplitOptions.None);

                    if (segments.Length == 2)
                    {
                        if (result != null && result.IsAnyNull())
                            student.ExamResults.Add(result);
                        if (student != null)
                            students.Add(student);

                        student = new Student();
                        result = new ExamResult();

                    }
                    else if (segments.Length == 3)
                    {
                        string[] attrval = segments.Last().Split(":");
                        //KeyValuePair<String,String> teacherattr = new KeyValuePair<String,String>();
                        if (attrval[0].Equals("StudentID"))
                            student.Id = int.Parse(attrval[1]);
                        else if (attrval[0].Equals("Name"))
                            student.Name = attrval[1].TrimStart();
                    }
                    else if (segments.Length == 4)
                    {
                        if ((result != null) && (result.IsAnyNull()))
                            student.ExamResults.Add(result);
                        result = new ExamResult();
                    }
                    else if (segments.Length == 5)
                    {
                        string[] attrval = segments.Last().Split(":");
                        //KeyValuePair<String,String> teacherattr = new KeyValuePair<String,String>();
                        if (attrval[0].Equals("Marks"))
                            result.Marks = double.Parse(attrval[1]);
                        else if (attrval[0].Equals("Course"))
                        {
                            result.Course =  courses[(int.Parse(attrval[1].Split("/").Last<string>())) - 1];
                        }
                        else if (attrval[0].Equals("Instructors"))
                        {
                            result.Instructors = GetInstrustorInfo(attrval[1].Replace("[", "").Replace("]", ""));
                        }
                    }
                }
                student.ExamResults.Add(result);
                students.Add(student);

                return students;

            }
            catch (IOException ioexp)
            {
                throw ioexp;
            }
            catch (OutOfMemoryException memexp)
            {
                throw memexp;
            }
        }
        

        //Gets instructor id and returns instructor details againt student 
        public static List<Teacher> GetInstrustorInfo(string instructors)
        {
            List<Teacher> instructorsList = new List<Teacher>();
            try
            {
                string[] instrctrs = instructors.Split(',');
                foreach (var instrctr in instrctrs)
                {
                    string[] teachr = instrctr.Split('/');
                    if (teachr[1].Equals("Teachers"))
                    {
                        instructorsList.Add(teachers[int.Parse(teachr.Last()) - 1]);
                    }
                    else
                    {
                        Teacher newInstructor = new Teacher();
                        int id = int.Parse(teachr.Last());
                        try
                        {
                            newInstructor.Name = students[id - 1].Name;
                            newInstructor.StaffID = students[id - 1].Id;
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            newInstructor.StaffID = id;
                        }

                        instructorsList.Add(newInstructor);
                    }
                }
                return instructorsList;

            }
            catch (ArgumentException argexp)
            {
                throw argexp;
            }
            catch (OverflowException overflowexp)
            {
                throw overflowexp;
            }

        }
    }
}