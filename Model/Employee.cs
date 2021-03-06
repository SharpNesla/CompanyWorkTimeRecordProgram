﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Documents;
using employees.Model;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Employees.Model
{
    /// <summary>
    /// Класс данных, хранит информацию о работнике
    /// </summary>
    public class Employee : Entity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string PassportSerial { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateBirth { get; set; }
        public string PhoneNumber{ get; set; }
        public string PassportDistributor { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; }
        public List<Card> Cards { get; set; }
        [NotMapped] public string Signature => $"{Id} {Surname} {Name} {Patronymic}";
    }
}