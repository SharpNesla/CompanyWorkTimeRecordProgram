using System;
using System.Collections.Generic;
using System.Windows.Documents;
using employees.Model;

namespace Employees.Model
{
    public class Employee : Entity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string PassportSerial { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateBirth { get; set; }
        public string PhoneNumber{ get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public int SumWorkTime { get; set; }
        public Role Role { get; set; }
        public List<Card> Cards { get; set; }
    }
}