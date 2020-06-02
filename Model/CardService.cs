using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.SqlServer.Server;
using NPOI.XSSF.Extractor;
using NPOI.XSSF.UserModel;

namespace employees.Model
{
    public class CardService
    {
        /// <summary>
        /// Класс-контекст, зависимость от Entity Framework 6
        /// </summary>
        private readonly ApplicationContext _applicationContext;

        public CardService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        /// <summary>
        /// Функция возвращает список записей о карточках
        /// отработанного времени
        /// </summary>
        /// <param name="searchString">Строка поиска</param>
        /// <param name="sortBy">Поле, по которому сортируется спиоск</param>
        /// <param name="sortDirection">Направление сортировки true - возрастание, false - убывание</param>
        /// <param name="filter">Критерии выбора данных</param>
        /// <param name="limit">Ограничение величины списка</param>
        /// <param name="offset">Смещение относительно начала списка</param>
        /// <returns>Список записей работников</returns>
        public List<Card> Get(string searchString, string sortBy, bool sortDirection,
            CardFilterDefinition filter, int limit, int offset)
        {
            var request = _applicationContext.Cards.AsQueryable().Where(x => x.DeletedAt == null);
            if (!string.IsNullOrEmpty(searchString))
            {
                request = request.Where(x => x.Id.ToString().Contains(searchString));
            }

            #region Применение фильтров

            if (filter.IsByEmployee && filter.EmployeeId != null)
                request = request.Where(x => x.EmployeeId == filter.EmployeeId);

            if (filter.IsBySumWorkTime)
            {
                if (filter.SumWorkTimeLowBound != null)
                {
                    request = request.Where(x => x.SumWorkLoadTime >= filter.SumWorkTimeLowBound);
                }

                if (filter.SumWorkTimeHighBound != null)
                {
                    request = request.Where(x => x.SumWorkLoadTime <= filter.SumWorkTimeHighBound);
                }
            }

            if (filter.IsByDatePass)
            {
                if (filter.DatePassLowBound != null)
                {
                    request = request.Where(x => x.DatePass >= filter.DatePassLowBound);
                }

                if (filter.DatePassHighBound != null)
                {
                    request = request.Where(x => x.DatePass <= filter.DatePassHighBound);
                }
            }

            #endregion

            //Применение сортировки
            switch (sortBy)
            {
                case "WorkLoadTimeMonday":
                    request = sortDirection
                        ? request.OrderBy(x => x.WorkLoadTimeMonday)
                        : request.OrderByDescending(x => x.WorkLoadTimeMonday);
                    break;
                case "WorkLoadTimeTuesday":
                    request = sortDirection
                        ? request.OrderBy(x => x.WorkLoadTimeTuesday)
                        : request.OrderByDescending(x => x.WorkLoadTimeTuesday);
                    break;
                case "WorkLoadTimeWednesday":
                    request = sortDirection
                        ? request.OrderBy(x => x.WorkLoadTimeWednesday)
                        : request.OrderByDescending(x => x.WorkLoadTimeWednesday);
                    break;
                case "WorkLoadTimeThursday":
                    request = sortDirection
                        ? request.OrderBy(x => x.WorkLoadTimeThursday)
                        : request.OrderByDescending(x => x.WorkLoadTimeThursday);
                    break;
                case "WorkLoadTimeFriday":
                    request = sortDirection
                        ? request.OrderBy(x => x.WorkLoadTimeFriday)
                        : request.OrderByDescending(x => x.WorkLoadTimeFriday);
                    break;
                case "DatePass":
                    request = sortDirection
                        ? request.OrderBy(x => x.DatePass)
                        : request.OrderByDescending(x => x.DatePass);
                    break;
                case "Employee":
                    request = sortDirection
                        ? request.OrderBy(x => x.EmployeeId)
                        : request.OrderByDescending(x => x.EmployeeId);
                    break;
                case "Payment":
                    request = sortDirection
                        ? request.OrderBy(x => x.Payment)
                        : request.OrderByDescending(x => x.Payment);
                    break;
                default:
                    request = sortDirection
                        ? request.OrderBy(x => x.Id)
                        : request.OrderByDescending(x => x.Id);
                    break;
            }

            return request.Skip(offset).Take(limit).ToList();
        }

        /// <summary>
        /// Функция, возвращающая количество записей о карточках
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public long GetCount(string searchString, CardFilterDefinition filter)
        {
            var request = _applicationContext.Cards.AsQueryable().Where(x => x.DeletedAt == null);

            if (!string.IsNullOrEmpty(searchString))
            {
                request = request.Where(x => x.Id.ToString().Contains(searchString));
            }

            #region Применение фильтров

            if (filter.IsByEmployee && filter.EmployeeId != null)
                request = request.Where(x => x.EmployeeId == filter.EmployeeId);

            if (filter.IsBySumWorkTime)
            {
                if (filter.SumWorkTimeLowBound != null)
                {
                    request = request.Where(x => x.SumWorkLoadTime >= filter.SumWorkTimeLowBound);
                }

                if (filter.SumWorkTimeHighBound != null)
                {
                    request = request.Where(x => x.SumWorkLoadTime <= filter.SumWorkTimeHighBound);
                }
            }

            if (filter.IsByDatePass)
            {
                if (filter.DatePassLowBound != null)
                {
                    request = request.Where(x => x.DatePass >= filter.DatePassLowBound);
                }

                if (filter.DatePassHighBound != null)
                {
                    request = request.Where(x => x.DatePass <= filter.DatePassHighBound);
                }
            }

            #endregion

            return request.Count();
        }

        /// <summary>
        /// Функция, возвращает запись карточки по её идентификационному номеру
        /// </summary>
        /// <param name="id">Идентификационный номер</param>
        /// <returns></returns>s>
        public Card GetById(int id)
        {
            return this._applicationContext.Cards.Find(id);
        }

        /// <summary>
        /// Функция, добавляющая информацию о карточке загруженности в систему
        /// </summary>
        /// <param name="card">Карточка</param>
        public void Add(Card card)
        {
            card.CreatedAt = DateTime.Now;
            _applicationContext.Cards.Add(card);
            _applicationContext.SaveChangesAsync();
        }

        /// <summary>
        /// Функция, обновляющая информацию о карточке загруженности
        /// </summary>
        /// <param name="card">Карточка</param>
        public void Update(Card card)
        {
            _applicationContext.Entry(card).State = EntityState.Modified;
            _applicationContext.SaveChangesAsync();
        }

        /// <summary>
        /// Функция, удаляющая информацию о карточке загруженности
        /// </summary>
        /// <param name="id">Идентификационный номер</param>
        public void Remove(int id)
        {
            var card = GetById(id);
            card.DeletedAt = DateTime.Now;
            _applicationContext.Entry(card).State = EntityState.Modified;
            _applicationContext.SaveChangesAsync();
        }

        /// <summary>
        /// Функция, экспортирующая список карточек загруженности
        /// в файл MS Excel.
        /// </summary>
        /// <param name="searchString">Строка поиска</param>
        /// <param name="sortBy">Поле, по которому производится сортировка</param>
        /// <param name="sortDirection">Направление сортировки</param>
        /// <param name="filter">Критерии выбора данных</param>
        public void SaveExcelDocument(string searchString, string sortBy, bool sortDirection,
            CardFilterDefinition filter)
        {
            var cards = this.Get(searchString, sortBy, sortDirection, filter, int.MaxValue, 0);

            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet();
            workbook.SetSheetName(0, "Карточки загруженности");

            var header = sheet.CreateRow(0);
            //Создание строки заголовка
            var tableSheetHeader = new[]
            {
                "№", "Работник", "Дата сдачи", "Пн", "Вт",
                "Ср", "Чт", "Пт", "Отработно всего", "З/П в час", "З/П всего"
            };

            for (var i = 0; i < tableSheetHeader.Length; i++)
            {
                header.CreateCell(i).SetCellValue(tableSheetHeader[i]);
            }
            //Заполнение строк таблицы
            for (int i = 0; i < cards.Count; i++)
            {
                var row = sheet.CreateRow(sheet.PhysicalNumberOfRows);

                row.CreateCell(0).SetCellValue(cards[i].Id);
                row.CreateCell(1).SetCellValue(cards[i].Employee.Signature);
                row.CreateCell(2).SetCellValue(cards[i].DatePass.ToString("dd.MM.yyyy"));
                row.CreateCell(3).SetCellValue(cards[i].WorkLoadTimeMonday.ToString("0:00"));
                row.CreateCell(4).SetCellValue(cards[i].WorkLoadTimeTuesday.ToString("0:00"));
                row.CreateCell(5).SetCellValue(cards[i].WorkLoadTimeWednesday.ToString("0:00"));
                row.CreateCell(6).SetCellValue(cards[i].WorkLoadTimeThursday.ToString("0:00"));
                row.CreateCell(7).SetCellValue(cards[i].WorkLoadTimeFriday.ToString("0:00"));
                row.CreateCell(8).SetCellValue(cards[i].SumWorkLoadTime.ToString("0:00"));
                row.CreateCell(9).SetCellValue(cards[i].Payment.ToString(CultureInfo.GetCultureInfo("en-GB")));
                row.CreateCell(10).SetCellValue(cards[i].PaymentFull.ToString(CultureInfo.GetCultureInfo("en-GB")));
            }

            for (var i = 0; i < tableSheetHeader.Length; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            var dialog = new SaveFileDialog
            {
                InitialDirectory = @"~/Documents",
                Title = $"Путь к экспортируемой таблице карточек загруженности",
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
        /// Получение данных для построения диаграммы загруженности
        /// </summary>
        /// <param name="filter">Критерии выбора данных</param>
        /// <returns>Данные</returns>
        public List<WorkLoadData> GetWorkLoadData(WorkLoadFilterDefinition filter)
        {
            var data = new List<WorkLoadData>();

            var request = _applicationContext.Cards.AsQueryable();

            #region Применение фильтров

            if (filter.IsByEmployee)
            {
                if (filter.EmployeeId != null) request = request.Where(x => x.EmployeeId == filter.EmployeeId);
            }
            else
            {
                if (filter.IsByGender)
                    request = request.Where(x => x.Employee.Gender == filter.Gender);
                if (filter.IsByDateBirth)
                {
                    if (filter.DateBirthLowBound != null)
                    {
                        request = request.Where(x => x.Employee.DateBirth >= filter.DateBirthLowBound);
                    }

                    if (filter.DateBirthHighBound != null)
                    {
                        request = request.Where(x => x.Employee.DateBirth <= filter.DateBirthHighBound);
                    }
                }
                
            }

            if (filter.IsByDatePass)
            {
                if (filter.DatePassLowBound != null)
                {
                    request = request.Where(x => x.DatePass >= filter.DatePassLowBound);
                }

                if (filter.DatePassHighBound != null)
                {
                    request = request.Where(x => x.DatePass <= filter.DatePassHighBound);
                }
            }
            
            #endregion

            try
            {
                var minMonday = request.Min(x => x.WorkLoadTimeMonday);
                var minTuesday = request.Min(x => x.WorkLoadTimeTuesday);
                var minWednesday = request.Min(x => x.WorkLoadTimeWednesday);
                var minThursday = request.Min(x => x.WorkLoadTimeThursday);
                var minFriday = request.Min(x => x.WorkLoadTimeFriday);

                var maxMonday = request.Max(x => x.WorkLoadTimeMonday);
                var maxTuesday = request.Max(x => x.WorkLoadTimeTuesday);
                var maxWednesday = request.Max(x => x.WorkLoadTimeWednesday);
                var maxThursday = request.Max(x => x.WorkLoadTimeThursday);
                var maxFriday = request.Max(x => x.WorkLoadTimeFriday);

                #region Вычисление среднего, путём перевода в минуты, и обратного перевода в часы

                var avgMonday = (int) request
                    .Select(x => x.WorkLoadTimeMonday / 100 * 60 + x.WorkLoadTimeMonday % 100)
                    .Select(x=> (double) x)
                    .Average(x => x);
                var avgTuesday = (int)
                    request.Select(x => x.WorkLoadTimeTuesday / 100 * 60 + x.WorkLoadTimeTuesday % 100)
                        .Select(x => (double)x)
                        .Average(x => x);
                var avgWednesday = (int) request.Select(x => x.WorkLoadTimeWednesday / 100 * 60 + x.WorkLoadTimeWednesday % 100)
                    .Select(x => (double)x)
                    .Average(x => x);
                var avgThursday = (int) request.Select(x => x.WorkLoadTimeThursday / 100 * 60 + x.WorkLoadTimeThursday % 100)
                    .Select(x => (double)x)
                    .Average(x => x);
                var avgFriday = (int) request.Select(x => x.WorkLoadTimeFriday / 100 * 60 + x.WorkLoadTimeFriday % 100)
                    .Select(x => (double)x)
                    .Average(x => x);

                avgMonday = avgMonday / 60 * 100 + avgMonday % 60;
                avgTuesday = avgTuesday / 60 * 100 + avgTuesday % 60;
                avgWednesday = avgWednesday / 60 * 100 + avgWednesday % 60;
                avgThursday = avgThursday / 60 * 100 + avgThursday % 60;
                avgFriday = avgFriday / 60 * 100 + avgFriday % 60;

                #endregion

                data.AddRange(new[]
                {
                    new WorkLoadData {Min = minMonday, Average = avgMonday, Max = maxMonday},
                    new WorkLoadData {Min = minTuesday, Average = avgTuesday, Max = maxTuesday},
                    new WorkLoadData {Min = minWednesday, Average = avgWednesday, Max = maxWednesday},
                    new WorkLoadData {Min = minThursday, Average = avgThursday, Max = maxThursday},
                    new WorkLoadData {Min = minFriday, Average = avgFriday, Max = maxFriday},
                });
            }
            //Обработка ошибки в случае, если карточек с данными критериями не существует
            catch (InvalidOperationException e)
            {
                for (int i = 0; i < 5; i++)
                {
                    data.Add(new WorkLoadData());
                }
            }


            return data;
        }
    }
}