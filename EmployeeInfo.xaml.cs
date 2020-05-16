using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using employees.Model;
using Employees.Model;

namespace employees
{
    public class EmployeeInfoViewModel
    {
        public Employee Entity { get; set; }

        public EmployeeInfoViewModel()
        {
            this.Entity = new Employee
            {
                Id = 12,
                Name = "Василий",
                Surname = "Иванов",
                Patronymic = null,
                PhoneNumber = "80572523765",
                DateBirth = new DateTime(1985, 3, 4),
                Comment =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" +
                    " incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud " +
                    "exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure" +
                    " dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla " +
                    "pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui " +
                    "officia deserunt mollit anim id est laborum." +
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" +
                    " incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud " +
                    "exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure" +
                    " dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla " +
                    "pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui " +
                    "officia deserunt mollit anim id est laborum.",
                Cards = new List<Card>(
                    new[]
                    {
                        new Card
                        {
                            Id = 12,
                            WorkLoadTimeMonday = 845,
                            WorkLoadTimeTuesday = 845,
                            WorkLoadTimeFriday = 845,
                            WorkLoadTimeThursday = 845,
                            WorkLoadTimeWednesday = 845,
                            DatePass = DateTime.Now,
                            Comment = "Комментарий к карточке номер 12"
                        },
                        new Card
                        {
                            Id = 12,
                            WorkLoadTimeMonday = 845,
                            WorkLoadTimeTuesday = 845,
                            WorkLoadTimeFriday = 845,
                            WorkLoadTimeThursday = 845,
                            WorkLoadTimeWednesday = 845,
                            DatePass = DateTime.Now,
                            Comment = "Комментарий к карточке номер 12"
                        },
                        new Card
                        {
                            Id = 12,
                            WorkLoadTimeMonday = 845,
                            WorkLoadTimeTuesday = 845,
                            WorkLoadTimeFriday = 845,
                            WorkLoadTimeThursday = 845,
                            WorkLoadTimeWednesday = 845,
                            DatePass = DateTime.Now,
                            Comment = "Комментарий к карточке номер 12"
                        },
                        new Card
                        {
                            Id = 12,
                            WorkLoadTimeMonday = 845,
                            WorkLoadTimeTuesday = 845,
                            WorkLoadTimeFriday = 845,
                            WorkLoadTimeThursday = 845,
                            WorkLoadTimeWednesday = 845,
                            DatePass = DateTime.Now,
                            Comment = "Комментарий к карточке номер 12"
                        }
                    })
            };
        }
    }
}