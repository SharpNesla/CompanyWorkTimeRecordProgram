using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employees.Model;

namespace employees.Model
{
    /// <summary>
    /// вспомогательный класс, использующийся для хранения информации
    /// о критериях выбора данных при построении диаграмм загруженности
    /// работников.
    /// </summary>
    public class WorkLoadFilterDefinition
    {
        public bool IsByGender { get; set; }
        public bool IsByDatePass { get; set; }
        public bool IsByEmployee { get; set; }
        public bool IsByDateBirth { get; set; }
        public Gender Gender { get; set; }
        public DateTime? DatePassLowBound { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? DatePassHighBound { get; set; }
        public DateTime? DateBirthLowBound { get; set; }
        public DateTime? DateBirthHighBound { get; set; }
    }
}
