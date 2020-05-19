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
        private readonly ApplicationContext _applicationContext;

        public CardService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

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

            return request.OrderBy(x => x.Id).Skip(offset).Take(limit).ToList();
        }

        public long GetCount(string searchString, CardFilterDefinition filter)
        {
            return this._applicationContext.Cards.Count();
        }

        public Card GetById(int id)
        {
            return this._applicationContext.Cards.Find(id);
        }

        public void Add(Card card)
        {
            _applicationContext.Cards.Add(card);
            _applicationContext.SaveChangesAsync();
        }

        public void Update(Card card)
        {
            _applicationContext.Entry(card).State = EntityState.Modified;
            _applicationContext.SaveChangesAsync();
        }

        public void Remove(int id)
        {
            var card = GetById(id);
            card.DeletedAt = DateTime.Now;
            _applicationContext.Entry(card).State = EntityState.Modified;
            _applicationContext.SaveChangesAsync();
        }

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