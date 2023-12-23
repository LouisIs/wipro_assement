using System;
using System.ComponentModel.DataAnnotations;

namespace wipro_assement.Models
{

    public class Student
    {
        [Key]
        public int Rollno { get; set; }
        public string Sname { get; set; }
        public string Std { get; set; }
    }
}

