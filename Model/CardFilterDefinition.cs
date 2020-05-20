using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employees.Model;

namespace employees.Model
{
    /// <summary>
    /// Класс EmployeeFilterDefinition, вспомогательный класс,
    /// использующийся для хранения информации о критериях выбора
    /// данных списка карточек.
    /// </summary>
    public class CardFilterDefinition
    {
        public bool IsBySumWorkTime { get; set; }
        public bool IsByDatePass { get; set; }
        public bool IsByEmployee { get; set; }
        public int EmployeeId { get; set; }
        public int? SumWorkTimeLowBound { get; set; }
        public int? SumWorkTimeHighBound { get; set; }
        public DateTime? DatePassLowBound { get; set; }
        public DateTime? DatePassHighBound { get; set; }
    }
}
