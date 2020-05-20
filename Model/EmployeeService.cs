using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using Employees.Model;
using NPOI.XSSF.UserModel;

namespace employees.Model
{
    /// <summary>
    /// Класс операций над рабониками
    /// </summary>
    public class EmployeeService
    {
        /// <summary>
        /// Пользователь, вошедший в систему
        /// </summary>
        public Employee CurrentUser { get; private set; }

        /// <summary>
        /// Класс-контекст, зависимость от Entity Framework 6
        /// </summary>
        private readonly ApplicationContext _applicationContext;

        public EmployeeService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        /// <summary>
        /// Функция возвращает список записей работников
        /// </summary>
        /// <param name="searchString">Строка поиска</param>
        /// <param name="sortBy">Поле, по которому сортируется спиоск</param>
        /// <param name="sortDirection">Направление сортировки true - возрастание, false - убывание</param>
        /// <param name="filter">Критерии выбора данных</param>
        /// <param name="limit">Ограничение величины списка</param>
        /// <param name="offset">Смещение относительно начала списка</param>
        /// <returns>Список записей работников</returns>
        public List<Employee> Get(string searchString, string sortBy, bool sortDirection,
            EmployeeFilterDefinition filter, int limit, int offset)
        {
            IQueryable<Employee> request;

            //RAW (сырой) начало построения запроса
            //для применения текстового поиска
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

            #region Применение критериев выбора к запросу

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

            #endregion

            //Применение сортировки к запросу
            switch (sortBy)
            {
                case "Name":
                    request = sortDirection ? request.OrderBy(x => x.Name) : request.OrderByDescending(x => x.Name);
                    break;
                case "Surname":
                    request = sortDirection
                        ? request.OrderBy(x => x.Surname)
                        : request.OrderByDescending(x => x.Surname);
                    break;
                case "Patronymic":
                    request = sortDirection
                        ? request.OrderBy(x => x.Patronymic)
                        : request.OrderByDescending(x => x.Name);
                    break;
                case "DateBirth":
                    request = sortDirection
                        ? request.OrderBy(x => x.DateBirth)
                        : request.OrderByDescending(x => x.DateBirth);
                    break;
                default:
                    request = request.OrderBy(x => x.Id);
                    break;
            }

            return request.Skip(offset).Take(limit).ToList();
        }

        /// <summary>
        /// Функция, возвращает запись работника по его идентификационному номеру
        /// </summary>
        /// <param name="id">Идентификационный номер</param>
        /// <returns></returns>
        public Employee GetById(int id)
        {
            return this._applicationContext.Employees.Find(id);
        }

        /// <summary>
        /// Функция, возвращающая количество записей о работниках
        /// </summary>
        /// <param name="searchString">Строка поиска</param>
        /// <param name="filter">Критерии выбора данных</param>
        /// <returns>Количество записей</returns>
        public int GetCount(string searchString, EmployeeFilterDefinition filter)
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

            #region Применение критериев выбора к запросу

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

            #endregion

            return request.Count();
        }

        /// <summary>
        /// Процедура добавления информации о работнике в систему.
        /// </summary>
        /// <param name="employee">Добавляемый работник</param>
        public void Add(Employee employee)
        {
            employee.CreatedAt = DateTime.Now;
            _applicationContext.Employees.Add(employee);
            _applicationContext.SaveChanges();
        }

        /// <summary>
        /// Процедура, производит обновление информации о работнике.
        /// </summary>
        /// <param name="employee"></param>
        public void Update(Employee employee)
        {
            _applicationContext.Entry(employee).State = EntityState.Modified;
            _applicationContext.SaveChanges();
        }

        /// <summary>
        /// Процедура, производит удаление информации о работнике
        /// по идентификационному номеру.
        /// </summary>
        /// <param name="id">Id</param>
        public void Remove(int id)
        {
            var employee = GetById(id);
            employee.DeletedAt = DateTime.Now;
            _applicationContext.Entry(employee).State = EntityState.Modified;
            _applicationContext.SaveChanges();
        }

        /// <summary>
        /// Процедура, сохраняющая список работников в таблицу MS Excel.
        /// </summary>
        /// <param name="searchString">Строка поиска</param>
        /// <param name="sortBy">Поле, по которому производится сортировка</param>
        /// <param name="sortDirection">Направление сортировки</param>
        /// <param name="filter">Критерии выбора данных</param>
        public void SaveExcelDocument(string searchString, string sortBy, bool sortDirection,
            EmployeeFilterDefinition filter)
        {
            var cards = this.Get(searchString, sortBy, sortDirection, filter, int.MaxValue, 0);

            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet();
            workbook.SetSheetName(0, "Карточки загруженности");

            var header = sheet.CreateRow(0);

            var tableSheetHeader = new[]
            {
                "№", "Имя", "Фамилия", "Отчество", "Номер телефона",
                "Серия и номер паспорта", "Дата рождения", "Отработано всего", "Имя пользователя"
            };

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

        /// <summary>
        /// Функция авторизации работника в системе,
        /// при несовпадении пары имя пользователя-пароль или отсутствия информации о работнике
        /// с заданным именем пользователя в системе
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <returns>Работник</returns>
        public Employee Auth(string username, string password)
        {
            Employee user = null;

            try
            {
                user = _applicationContext.Employees.First(x => x.Username == username);
            }
            catch (Exception e)
            {
                throw new UnauthorizedAccessException("User with username is not found! ");
            }


            byte[] hashBytes = Convert.FromBase64String(user.PasswordHash);
            // Получение соли 
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            // Вычисление хэша вводимого пароля
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            // Сравнение хэшей
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    throw new UnauthorizedAccessException();
            CurrentUser = user;
            return user;
        }

        /// <summary>
        /// Задание хэша пароля для работника.
        /// </summary>
        /// <param name="employee">Работник</param>
        /// <param name="password">Пароль</param>
        /// <returns>Работник</returns>
        public static Employee SetPassword(Employee employee, string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string savedPasswordHash = Convert.ToBase64String(hashBytes);

            employee.PasswordHash = savedPasswordHash;

            return employee;
        }

        /// <summary>
        /// Функция, строящая регулярное выражение для
        /// поиска по подстрокам.
        /// </summary>
        /// <param name="searchString">Строка поиска</param>
        /// <returns>Строка регулярного выражения</returns>
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