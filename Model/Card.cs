using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employees.Model;

namespace employees.Model
{
    /// <summary>
    /// Класс данных, хранит информацию о карточке отработанного времени.
    /// </summary>
    public class Card : Entity
    {
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public decimal Payment { get; set; }
        public decimal PaymentFull { get; set; }
        public DateTime DatePass { get; set; }
        public short WorkLoadTimeMonday { get; set; }
        public short WorkLoadTimeTuesday { get; set; }
        public short WorkLoadTimeWednesday { get; set; }
        public short WorkLoadTimeThursday { get; set; }
        public short WorkLoadTimeFriday { get; set; }
        public short SumWorkLoadTime { get; set; }
    }
}