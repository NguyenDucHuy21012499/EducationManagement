using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace COURSE
{
    public class Login
    {
        private string _password;
        private string _username;
        private string _email;

        public Login(string password, string username, string email)
        {
            _password = password;
            _username = username;
            _email = email;
        }
        public Login() { }

        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
    }

    public class Subject
    {
        private string subject_name;
        private string subject_code;
        private string credits;
        private string prerequisite_subject;

        public Subject(string subject_name, string subject_code, string credits, string prerequisite_subject)
        {
            this.subject_name = subject_name;
            this.subject_code = subject_code;
            this.credits = credits;
            this.prerequisite_subject = prerequisite_subject;
        }

        public Subject() { }

        public string SubjectName
        {
            get { return subject_name; }
            set { subject_name = value; }
        }

        public string SubjectCode
        {
            get { return subject_code; }
            set { subject_code = value; }
        }

        public string Credits
        {
            get { return credits; }
            set { credits = value; }
        }

        public string PrerequisiteSubject
        {
            get { return prerequisite_subject; }
            set { prerequisite_subject = value; }
        }
    }
    public class Major
    {
        private string major_name;
        private string major_code;
        private string major_year;
        private string major_credit;


        public Major(string major_name, string major_code, string major_year, string major_credit)
        {
            this.major_name = major_name;
            this.major_code = major_code;
            this.major_year = major_year;
            this.major_credit = major_credit;
        }

        public Major() { }

        public string MajorName
        {
            get { return major_name; }
            set { major_name = value; }
        }

        public string MajorCode
        {
            get { return major_code; }
            set { major_code = value; }
        }
        public string MajorYear
        {
            get { return major_year; }
            set { major_year = value; }
        }

        public string MajorCredit
        {
            get { return major_credit; }
            set { major_credit = value; }
        }
    }
    public class Block : Major
    {
        private string block_name;
        private string block_code;

        public Block() { }

        public Block(string block_name, string block_code)
        {
            this.block_name = block_name;
            this.block_code = block_code;
        }

        public string BlockName
        {
            get { return block_name; }
            set { block_name = value; }
        }

        public string BlockCode
        {
            get { return block_code; }
            set { block_code = value; }
        }
    }

    public class Semester:Major
    {
        private string semester_code;
        private string year;
        private string semester_number;

        public Semester(string semester_code, string year, string semester)
        {
            this.semester_code = semester_code;
            this.year = year;
            this.semester_number = semester;
        }

        public Semester() { }

        public string SemesterCode
        {
            get { return semester_code; }
            set { semester_code = value; }
        }
        public string Year
        {
            get { return year; }
            set { year = value; }
        }
        public string SemesterNumber
        {
            get { return semester_number; }
            set { semester_number = value; }
        }
    }
    public class Classes
    {
        private string classCode; 
        private string className;
        private string teacher;
        private string dayOfTheWeek;
        private string numberOfPeriods;
        private string classBegin;
        private string classEnd;
        private string classRoom;
        private string dayBegin;
        private string dayEnd;

        public Classes(string classCode, string className, string teacher,
                        string dayOfTheWeek, string numberOfPeriods,
                        string classBegin, string classEnd,
                        string classRoom, string dayBegin, string dayEnd)
        {
            this.classCode = classCode;
            this.className = className;
            this.teacher = teacher;
            this.dayOfTheWeek = dayOfTheWeek;
            this.numberOfPeriods = numberOfPeriods;
            this.classBegin = classBegin;
            this.classEnd = classEnd;
            this.classRoom = classRoom;
            this.dayBegin = dayBegin;
            this.dayEnd = dayEnd;
        }

        public Classes() { }

        public string ClassCode
        {
            get { return classCode; }
            set { classCode = value; }
        }

        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        public string Teacher
        {
            get { return teacher; }
            set { teacher = value; }
        }

        public string DayOfTheWeek
        {
            get { return dayOfTheWeek; }
            set { dayOfTheWeek = value; }
        }

        public string NumberOfPeriods
        {
            get { return numberOfPeriods; }
            set { numberOfPeriods = value; }
        }

        public string ClassBegin
        {
            get { return classBegin; }
            set { classBegin = value; }
        }

        public string ClassEnd
        {
            get { return classEnd; }
            set { classEnd = value; }
        }

        public string ClassRoom
        {
            get { return classRoom; }
            set { classRoom = value; }
        }

        public string DayBegin
        {
            get { return dayBegin; }
            set { dayBegin = value; }
        }

        public string DayEnd
        {
            get { return dayEnd; }
            set { dayEnd = value; }
        }
        
    }

}

