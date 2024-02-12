using EquipmentCalculating.Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EquipmentCalculating.Services
{
    internal static class ExcelReportService
    {
        public static void GenerateExcelReport(IList<Equipment> equipment)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "report.xlsx");

            using (var stream = File.Open(path, FileMode.OpenOrCreate))
            {
                var query = equipment.Select(e => new
                {
                    e.SerialNumber,
                    EquipmentName = e.Name,
                    EquipmentLocation = e.Location != null ? $"{e.Location.Number}|{e.Location.Name}" : "|",
                    EquipmentCategory = e.Category,
                    EquipmentCondition = e.GoodCondition
                });

                MiniExcelLibs.MiniExcel.SaveAs(stream, query);
            }
        }
    }
}
