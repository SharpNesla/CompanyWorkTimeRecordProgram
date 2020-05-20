using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.SqlServer.Server;
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

            if (filter.IsByEmployee)
                request = request.Where(x => x.EmployeeId == filter.EmployeeId);

            if (filter.IsBySumWorkTime)
            {
                request = request.Where(x => x.SumWorkLoadTime >= filter.SumWorkTimeLowBound &&
                                             x.SumWorkLoadTime <= filter.SumWorkTimeHighBound);
            }

            if (filter.IsByDatePass)
            {
                request = request.Where(x => x.DatePass >= filter.DatePassLowBound &&
                                             x.DatePass <= filter.DatePassHighBound);
            }

            switch (sortBy)
            {
                case "WorkLoadTimeMonday":
                    request = sortDirection ? request.OrderBy(x => x.WorkLoadTimeMonday) :
                        request.OrderByDescending(x => x.WorkLoadTimeMonday);
                    break;
                case "WorkLoadTimeTuesday":
                    request = sortDirection ? request.OrderBy(x => x.WorkLoadTimeTuesday) :
                        request.OrderByDescending(x => x.WorkLoadTimeTuesday);
                    break;
                case "WorkLoadTimeWednesday":
                    request = sortDirection ? request.OrderBy(x => x.WorkLoadTimeWednesday) :
                        request.OrderByDescending(x => x.WorkLoadTimeWednesday);
                    break;
                case "WorkLoadTimeThursday":
                    request = sortDirection ? request.OrderBy(x => x.WorkLoadTimeThursday) :
                        request.OrderByDescending(x => x.WorkLoadTimeThursday);
                    break;
                case "WorkLoadTimeFriday":
                    request = sortDirection ? request.OrderBy(x => x.WorkLoadTimeFriday) :
                        request.OrderByDescending(x => x.WorkLoadTimeFriday);
                    break;
                case "DatePass":
                    request = sortDirection ? request.OrderBy(x => x.DatePass) :
                        request.OrderByDescending(x => x.DatePass);
                    break;
                case "Employee":
                    request = sortDirection ? request.OrderBy(x => x.EmployeeId) :
                        request.OrderByDescending(x => x.EmployeeId);
                    break;
                case "Payment":
                    request = sortDirection ? request.OrderBy(x => x.Payment) :
                        request.OrderByDescending(x => x.Payment);
                    break;
                default:
                    request = request.OrderBy(x => x.Id);
                    break;
            }

            return request.OrderBy(x => x.Id).Skip(offset).Take(limit).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public long GetCount(string searchString, CardFilterDefinition filter)
        {
            return this._applicationContext.Cards.Count();
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

            var tableSheetHeader = new[]
            {
                "№", "Работник", "Дата сдачи", "Пн", "Вт",
                "Ср", "Чт", "Пт", "Отработно всего", "З/П в час", "З/П всего"
            };

            for (var i = 0; i < tableSheetHeader.Length; i++)
            {
                header.CreateCell(i).SetCellValue(tableSheetHeader[i]);
            }

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
        /// <param name="filterDefinition">Критерии выбора данных</param>
        /// <returns>Данные</returns>
        public List<WorkLoadData> GetWorkLoadData(WorkLoadFilterDefinition filterDefinition)
        {
            var data = new List<WorkLoadData>();

            data.Add(new WorkLoadData
            {
                Min = 0,
                Average = 0,
                Max = 0,
            });
            data.Add(new WorkLoadData
            {
                Min = 0,
                Average = 0,
                Max = 0,
            });
            data.Add(new WorkLoadData
            {
                Min = 0,
                Average = 0,
                Max = 0,
            });
            data.Add(new WorkLoadData
            {
                Min = 0,
                Average = 0,
                Max = 0,
            });
            data.Add(new WorkLoadData
            {
                Min = 0,
                Average = 0,
                Max = 0,
            });

            return data;
        }
    }
}