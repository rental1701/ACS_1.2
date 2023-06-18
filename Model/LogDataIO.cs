using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using static ACS.Infrastructure.ParseSQLData;

namespace ACS.Model
{
    public class LogDataIO
    {
        /// <summary>Первый вход</summary>
        public DateTime? FirstInput { get; set; }

        /// <summary>Последний выход</summary>
        private DateTime? _LastOutput;

        public DateTime? LastOutput
        {
            get { return _LastOutput; }
            set
            {
                _LastOutput = value;
            }
        }

        public bool IsLateEntry { get; set; }
        public bool IsEarlyExit { get; set; }
        public TimeSpan? Worktime
        {
            get { return LastOutput - FirstInput; }
        }

        /// <summary>Id работника</summary>
        public int? HozOrgan { get; set; } = null!;

        private string? _SurName;
        /// <summary>Фамилия</summary>

        public string? SurName
        {
            get => _SurName != null ? Regex.Replace(_SurName, "\\d+_", "") : "Неизвестно";
            set => _SurName = value;
        }
        /// <summary>Подразделение</summary>
        public string? Division { get; set; } = null!;

        public static List<LogDataIO> GetLogDataIO(DataTable data, TimeSpan timeEntry)
        {
            if (data.Rows.Count != 0)
            {
                List<LogDataIO> logData = new();
                int indexRow = 0;
                while (indexRow < data.Rows.Count)
                {
                    logData.Add(new(data, indexRow, timeEntry));
                    indexRow++;
                }
                return logData;
            }
            else
                throw new ArgumentException("Данные за выбранный период отсутствуют!");

        }

        public static void InitialLastOutput(List<LogDataIO> logDatas, DataTable data, TimeSpan timeExit, bool isShort = false)
        {
            if (!isShort)
            {
                foreach (LogDataIO log in logDatas)
                {
                    int row = 0;
                    while (row < data.Rows.Count)
                    {
                        DateTime? date = ConvertToDateTime(data.Rows[row].ItemArray[1]);
                        if (date is DateTime d &&
                             TryParseIntBD(data.Rows[row].ItemArray[0]) == log.HozOrgan)
                        {
                            if (d.Date == log.FirstInput?.Date)
                            {
                                if (d.TimeOfDay < timeExit)
                                {
                                    log.IsEarlyExit = true;
                                }
                                log.LastOutput = d;
                                data.Rows[row].Delete();
                                data.AcceptChanges();
                                break;
                            }
                            //int c = Convert.ToInt32(d.DayOfYear - log.FirstInput?.DayOfYear);

                            //if (c == 1)
                            //{
                            //    if (d.TimeOfDay < timeExit)
                            //    {
                            //        log.IsEarlyExit = true;
                            //    }
                            //    log.LastOutput = d;
                            //    data.Rows[row].Delete();
                            //    data.AcceptChanges();
                            //    break;
                            //}
                        }
                        row++;
                    }
                }

                if (data.Rows.Count != 0)
                {
                    foreach (LogDataIO log in logDatas)
                    {
                        if (log.LastOutput == null)
                        {
                            int row = 0;
                            while (row < data.Rows.Count)
                            {
                                DateTime? date = ConvertToDateTime(data.Rows[row].ItemArray[1]);
                                if (date is DateTime d &&
                                     TryParseIntBD(data.Rows[row].ItemArray[0]) == log.HozOrgan)
                                {
                                    int c = Convert.ToInt32(d.DayOfYear - log.FirstInput?.DayOfYear);
                                    if (c == 1)
                                    {
                                        if (d.TimeOfDay < timeExit)
                                        {
                                            log.IsEarlyExit = true;
                                        }
                                        log.LastOutput = d;
                                        data.Rows[row].Delete();
                                        data.AcceptChanges();
                                        break;
                                    }  
                                }
                                row++;
                            }
                        }
                    }
                    if (data.Rows.Count != 0)
                    {
                        for (int i = 0; i < data.Rows.Count; i++)
                        {
                            int id = TryParseIntBD(data.Rows[i].ItemArray[0]);
                            DateTime? d = ConvertToDateTime(data.Rows[i].ItemArray[1]);
                            LogDataIO log = logDatas.FirstOrDefault(l => l.HozOrgan == id);
                            if (log != null)
                            {
                                logDatas.Add(new LogDataIO
                                {
                                    HozOrgan = log.HozOrgan,
                                    SurName = log.SurName,
                                    Division = log.Division,
                                    LastOutput = d,
                                    IsEarlyExit = d?.TimeOfDay < timeExit
                                });
                            }
                            else
                            {
                                LogDataIO temp = new(data, i, timeExit) { IsEarlyExit = d?.TimeOfDay < timeExit };
                                logDatas.Add(temp);
                            }
                        } 
                    }
                    //LogDataCompare comp = new();
                    //logDatas.Sort(comp);
                }
            }
            else
            {
                foreach (LogDataIO log in logDatas)
                {
                    foreach (DataRow row in data.Rows)
                    {
                        int id = TryParseIntBD(row.ItemArray[0]);
                        if (log.HozOrgan == id)
                        {
                            DateTime? date = ConvertToDateTime(row.ItemArray[1]);
                            log.LastOutput = date;
                            if (date?.TimeOfDay < timeExit)
                                log.IsEarlyExit = true;
                        }
                    }
                }

            }
        }
        public override bool Equals(object? obj)
        {
            if (obj is LogDataIO log)
            {
                return this.HozOrgan == log.HozOrgan;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public LogDataIO()
        {

        }
        public LogDataIO(DataTable data, int row, TimeSpan time)
        {
            int c = 0;
            while (c < data.Columns.Count)
            {
                switch (data.Columns[c].ColumnName)
                {
                    case "Id":
                        HozOrgan = TryParseIntBD(data.Rows[row].ItemArray[c]);
                        break;
                    case "SurName":
                        SurName = TryParseStringBD(data.Rows[row].ItemArray[c]);
                        break;
                    case "Division":
                        Division = TryParseStringBD(data.Rows[row].ItemArray[c]);
                        break;
                    case "input":
                        FirstInput = ConvertToDateTime(data.Rows[row].ItemArray[c]);
                        if (FirstInput?.TimeOfDay > time)
                            IsLateEntry = true;
                        break;
                    case "output":
                        LastOutput = ConvertToDateTime(data.Rows[row].ItemArray[c]);
                        if (LastOutput?.TimeOfDay < time)
                            IsEarlyExit = true;
                        break;
                }
                c++;
            }
        }
    }

    public class LogDataCompare : IComparer<LogDataIO>
    {
        public int Compare(LogDataIO? x, LogDataIO? y)
        {
            if (x?.FirstInput == null)
            {
                if (y?.FirstInput == null)
                {
                    return 0;
                }
                else
                    return 0;
            }
            else
            {
                if (y?.FirstInput == null)
                {
                    return 1;
                }
                else
                {
                    int? c = x.FirstInput?.CompareTo(y.FirstInput);
                    if (c != null)
                        return (int)c;
                    return 0;
                }
            }
        }
    }
}
