using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using employees.Annotations;
using Employees.Model;
using LiveCharts.Dtos;
using NPOI.XSSF.UserModel;

namespace employees.Model
{
    public class EmployeeService
    {
        private readonly ApplicationContext _applicationContext;

        public EmployeeService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public List<Employee> Get(string searchString, string sortBy, bool sortDirection,
            EmployeeFilterDefinition filter, int limit, int offset)
        {
            IQueryable<Employee> request;
            if (!string.IsNullOrEmpty(searchString))
            {
                request = _applicationContext.Employees
                    .SqlQuery(
                        $"SELECT * FROM \"Employees\" " +
                        $"WHERE \"Employees\".\"Id\" || ' ' || \"Employees\".\"Name\" || ' ' || \"Employees\".\"Surname\" || ' ' ||" +
                        $" \"Employees\".\"Patronymic\" ~* '{MakeSearchRegexp(searchString)}'")
                    .AsQueryable();
            }
            else
            {
                request = _applicationContext.Employees.AsQueryable();
            }

            request = request.Where(x => x.DeletedAt == null);

            if (filter.IsByGender)
                request = request.Where(x => x.Gender == filter.Gender);

            if (filter.IsByDateBirth)
            {
                if (filter.DateBirthLowBound != null)
                {
                    request = request.Where(x => x.DateBirth >= filter.DateBirthLowBound);
                }

                if (filter.DateBirthHighBound != null)
                {
                    request = request.Where(x => x.DateBirth <= filter.DateBirthHighBound);
                }
            }

            if (filter.IsBySumWorkTime)
            {
                if (filter.SumWorkTimeLowBound != null)
                {
                    request = request.Where(x => x.SumWorkTime >= filter.SumWorkTimeLowBound);
                }

                if (filter.SumWorkTimeHighBound != null)
                {
                    request = request.Where(x => x.SumWorkTime <= filter.SumWorkTimeHighBound);
                }
            }


            if (filter.IsByRole)
                request = request.Where(x => x.Role == filter.Role);
            
            return request.OrderBy(x => x.Id).Skip(offset).Take(limit).ToList();
        }

        public Employee GetById(int id)
        {
            return this._applicationContext.Employees.Find(id);
        }

        public long GetCount(string searchString, EmployeeFilterDefinition filter)
        {
            IQueryable<Employee> request;

            if (!string.IsNullOrEmpty(searchString))
            {
                request = _applicationContext.Employees
                    .SqlQuery(
                        $"SELECT * FROM \"Employees\" " +
                        $"WHERE \"Employees\".\"Id\" || ' ' || \"Employees\".\"Name\" || ' ' || \"Employees\".\"Surname\" || ' ' ||" +
                        $" \"Employees\".\"Patronymic\" ~* '{MakeSearchRegexp(searchString)}'")
                    .AsQueryable();
            }
            else
            {
                request = _applicationContext.Employees.AsQueryable();
            }

            request = request.Where(x => x.DeletedAt == null);

            if (filter.IsByGender)
                request = request.Where(x => x.Gender == filter.Gender);

            if (filter.IsByDateBirth)
            {
                if (filter.DateBirthLowBound != null)
                {
                    request = request.Where(x => x.DateBirth >= filter.DateBirthLowBound);
                }

                if (filter.DateBirthHighBound != null)
                {
                    request = request.Where(x => x.DateBirth <= filter.DateBirthHighBound);
                }
            }

            if (filter.IsBySumWorkTime)
            {
                if (filter.SumWorkTimeLowBound != null)
                {
                    request = request.Where(x => x.SumWorkTime >= filter.SumWorkTimeLowBound);
                }

                if (filter.SumWorkTimeHighBound != null)
                {
                    request = request.Where(x => x.SumWorkTime <= filter.SumWorkTimeHighBound);
                }
            }


            if (filter.IsByRole)
                request = request.Where(x => x.Role == filter.Role);

            return request.Count();
        }

        public void Add(Employee employee)
        {
            _applicationContext.Employees.Add(employee);
            _applicationContext.SaveChanges();
        }

        public void Update(Employee employee)
        {
            _applicationContext.Entry(employee).State = EntityState.Modified;
            _applicationContext.SaveChanges();
        }

        public void Remove(int id)
        {
            var employee = GetById(id);
            employee.DeletedAt = DateTime.Now;
            _applicationContext.Entry(employee).State = EntityState.Modified;
            _applicationContext.SaveChanges();
        }

        public void SaveExcelDocument(string searchString, string sortBy, bool sortDirection,
            EmployeeFilterDefinition filter)
        {
            var cards = this.Get(searchString, sortBy, sortDirection, filter, int.MaxValue, 0);

            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet();
            workbook.SetSheetName(0, "Карточки загруженности");

            var header = sheet.CreateRow(0);

            var tableSheetHeader = new[] {"№", "Имя", "Фамилия", "Отчество", "Номер телефона",
                "Серия и номер паспорта", "Дата рождения", "Отработано всего", "Имя пользователя"};

            for (var i = 0; i < tableSheetHeader.Length; i++)
            {
                header.CreateCell(i).SetCellValue(tableSheetHeader[i]);
            }

            for (int i = 0; i < cards.Count; i++)
            {
                var row = sheet.CreateRow(sheet.PhysicalNumberOfRows);

                row.CreateCell(0).SetCellValue(cards[i].Id);
                row.CreateCell(1).SetCellValue(cards[i].Name);
                row.CreateCell(2).SetCellValue(cards[i].Surname);
                row.CreateCell(3).SetCellValue(cards[i].Patronymic);
                row.CreateCell(4).SetCellValue(cards[i].PhoneNumber);
                row.CreateCell(5).SetCellValue(cards[i].PassportSerial);
                row.CreateCell(6).SetCellValue(cards[i].DateBirth.ToString("dd.MM.yyyy"));
                row.CreateCell(7).SetCellValue(cards[i].SumWorkTime.ToString("0:00"));
                row.CreateCell(8).SetCellValue(cards[i].Username);
            }

            for (var i = 0; i < tableSheetHeader.Length; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            var dialog = new SaveFileDialog
            {
                InitialDirectory = @"~/Documents",
                Title = "Путь к экспортируемой таблице работников",
                AddExtension = true,
                Filter = "Файлы Excel 2007 (*.xlsx)|*.xlsx|Все остальные файлы (*.*)|*.*"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(dialog.FileName))
                {
                    File.Delete(dialog.FileName);
                }

                //запись в файл
                using (var fs = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(fs);
                }
            }
        }

        private static string MakeSearchRegexp(string searchString)
        {
            var searchChunks = searchString.Split(' ');

            var regex = @"^";

            foreach (var searchChunk in searchChunks)
            {
                regex += $"(?=.*{searchChunk})";
            }

            regex += @".*$";
            return regex;
        }
    }
}