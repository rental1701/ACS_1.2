using ACS.Model;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;

namespace ACS.Infrastructure
{
    static class Export
    {
        public static void ExportToExcel(List<LogDataIO> list, string path)
        {
            try
            {
                Excel.Application excelApp = new Excel.Application();
                
                excelApp.Workbooks.Add();
                Excel.Worksheet ws = (Excel.Worksheet)excelApp.ActiveSheet;
                ws.Cells[1, "A"] = "Фамилия";
                ws.Cells[1, "B"] = "Подразделение";
                ws.Cells[1, "C"] = "Время входа";
                ws.Cells[1, "D"] = "Время выхода";
                ws.Cells[1, "E"] = "Разница";
                int row = 1;
                foreach (LogDataIO l in list)
                {
                  
                    row++;
                    ws.Cells[row, "A"] = l.SurName;
                    ws.Cells[row, "B"] = l.Division;
                    ws.Cells[row, "C"] = l.FirstInput;
                    ws.Cells[row, "D"] = l.LastOutput;
                    ws.Cells[row, "E"] = l.Worktime.ToString();
                   
                    if (l.IsLateEntry)
                    {
                        ws.Range[ws.Cells[row, "C"], ws.Cells[row, "C"]].Interior.Color = 
                            XlRgbColor.rgbIndianRed;   
                    }
                    if(l.IsEarlyExit)
                    {
                        ws.Range[ws.Cells[row, "D"], ws.Cells[row, "D"]].Interior.Color =
                            XlRgbColor.rgbIndianRed;
                    }
                   
                }
                ws.Range["A1", "E1"].Font.Size = 12;
                ws.Range["A1", "E1"].Font.Bold = true;
                ws.Range["C1",$"E{row}"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                ws.Columns.AutoFit(); 
                ws.SaveAs(path);
                excelApp.Visible = true;
            }

            catch (Exception ex)
            {
                throw new Exception("Не удалось\n"+ ex.Message);
            }
        }

        public static void ExportPersonToExcel(Person person, List<LogData> logs, string path)
        {
            try
            {
                Excel.Application excelApp = new Excel.Application();
                excelApp.Workbooks.Add();
                Excel.Worksheet ws = (Excel.Worksheet)excelApp.ActiveSheet;

                ws.Cells[1, "A"] = $"{person.Name}";
                ws.Cells[1, "B"] = $"{person.FirstName}";
                ws.Cells[1, "C"] = $"{person.MidName}";
                ws.Cells[3, "A"] = "Время входа";
                ws.Cells[3, "B"] = "Время выхода";
                ws.Cells[3, "C"] = "Разница";

                ws.Range["A1", "C1"].Font.Size = 14;
                ws.Range["A3", "C3"].Font.Size = 12;
                ws.Range["A1", "C3"].Font.Bold = true;

                

                int row = 3;
                foreach (LogData log in logs)
                {
                    row++;
                    ws.Cells[row, "A"] = log.EntryTime;
                    ws.Cells[row, "B"] = log.ExitTime;
                    ws.Cells[row, "C"] = log.WorkTime.ToString();

                    if (log.IsLateEntry)
                    {
                        ws.Range[ws.Cells[row, "A"], ws.Cells[row, "A"]].Interior.Color =
                            XlRgbColor.rgbIndianRed;
                    }
                    if (log.IsEarlyExit)
                    {
                        ws.Range[ws.Cells[row, "B"], ws.Cells[row, "B"]].Interior.Color =
                           XlRgbColor.rgbIndianRed;
                    }
                }

                ws.Range["A1", $"E{row}"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                ws.Columns.AutoFit();
                ws.SaveAs(path);
                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось\n" + ex.Message);
            }
        }
    }
}
