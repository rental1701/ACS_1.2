using ACS.Commands.BaseCommand;
using ACS.Infrastructure;
using ACS.Interfaces;
using ACS.Model;
using ACS.Model.DataContext;
using ACS.ViewModels.Base;
using ACS.ViewModels.WorkSchedules;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace ACS.ViewModels
{
    public class СheckpointVM : ViewModel
    {

        #region Активация/Деактивация кнопок
        private bool _IsEnableFormButton = true;
        /// <summary>Активация кнопки Кнопка формирования главной таблицы</summary>
        public bool IsEnableFormButton
        {
            get => _IsEnableFormButton;
            set => Set(ref _IsEnableFormButton, value);
        }


        private bool _IsEnableFormButtonPerson;
        private bool IsCancelQuery = true;

        /// <summary>Активация кнопки Кнопка формирования таблицы для одного сотрудника</summary>
        public bool IsEnableFormButtonPerson
        {
            get => _IsEnableFormButtonPerson;
            set => Set(ref _IsEnableFormButtonPerson, value);
        }
        #endregion

        #region Диалоговое окно
        private IDialogWindow Window;
        #endregion
        #region файловая система
        public WorkSchedulesVM Dir { get; } = new WorkSchedulesVM("Z:\\Отдел управления персоналом");//C:\\Users\\kiselev\\EF\\FirstEF\\ACS\\WorkPdf

        private WorkSchedulesVM? _SelectedDirectory;

        public WorkSchedulesVM? SelectedDirectory
        {
            get { return _SelectedDirectory; }
            set => Set(ref _SelectedDirectory, value);
        }


        private string? _SelectedPdfFile;

        public string? SelectedPdfFile
        {
            get { return _SelectedPdfFile; }
            set => Set(ref _SelectedPdfFile, value);
        }

        #endregion

        #region Группировка
        private List<string> _GroupList = null!;

        public List<string> GroupList
        {
            get => _GroupList;
            set
            {
                Set(ref _GroupList, value);
            }
        }


        #region Выбранная группа
        private string _SelectedGroup;

        public string SelectedGroup
        {
            get => _SelectedGroup;
            set
            {
                Set(ref _SelectedGroup, value);

                //if (SelectedGroup == "По подразделениям")
                //{

                //    IsVisiableListDivisions = true;
                //    SelectedDivision = Divisions?.First();
                //}
                //else
                //{
                //    IsVisiableListDivisions = false;
                //    SelectedDivision = null;
                //}

            }
        }

        private int _SelectedIndexItem = 0;

        public int SelectedIndexItem
        {
            get => _SelectedIndexItem;
            set
            {
                Set(ref _SelectedIndexItem, value);
                if (SelectedIndexItem == 1)
                {
                    IsVisiableListDivisions = true;
                    SelectedDivision = Divisions?.First();
                }
                else
                {
                    IsVisiableListDivisions &= false;
                    SelectedDivision = null;
                }
            }
        }

        #endregion

        #region Тригер для визуализации элементов
        private bool _IsVisiableListDivisions;

        public bool IsVisiableListDivisions
        {
            get => _IsVisiableListDivisions;
            set => Set(ref _IsVisiableListDivisions, value);
        }
        #endregion

        #region Подразделения
        private List<Division>? _Divisions;

        public List<Division>? Divisions
        {
            get => _Divisions;
            set => Set(ref _Divisions, value);
        }
        #endregion

        #region Выбранное подразделение
        private Division? _SelectedDivision;

        public Division? SelectedDivision
        {
            get => _SelectedDivision;
            set
            {
                if (Set(ref _SelectedDivision, value))
                {
                    if (SelectedGroup == "По подразделениям")
                    {
                        _CollectionPerson.View?.Refresh();
                    }
                    _CollectionPerson.View?.Refresh();
                }
            }
        }

        #endregion
        #region Выбранный сотрудник для общего поиска
        //private Person? _SelectedPersonFromDivison;

        //public Person? SelectedPersonFromDivison
        //{
        //    get => _SelectedPersonFromDivison;
        //    set
        //    {
        //        Set(ref _SelectedPersonFromDivison, value);
        //        if (SelectedPersonFromDivison != null)
        //            IsAllPerson = false;
        //    }
        //}

        #endregion

        #region Поиск всех сотрудников
        private bool _IsAllPerson = true;
        /// <summary>Определяет выбраны все сотрудники подразделения или нет</summary>
        public bool IsAllPerson
        {
            get => _IsAllPerson;
            set
            {
                Set(ref _IsAllPerson, value);
                if (IsAllPerson && !IsUpdateSelectedPerson)
                {
                    SelectedPerson = null;
                }
            }
        }
        #endregion

        #region Рабочее время
        private DateTime _StartTime = new DateTime(2023, 01, 01, 8, 0, 0);
        /// <summary>Время входа/summary>
        public DateTime StartTime
        {
            get => _StartTime;
            set
            {
                if (Set(ref _StartTime, value))
                {
                    if (ListLogData != null)
                    {
                        OnListListOfUndisciplinedCommandExecute(true);
                    }
                }
            }
        }
        private DateTime _ExitTime = new DateTime(2023, 01, 01, 17, 0, 0);
        /// <summary>Время выхода/summary>
        public DateTime ExitTime
        {
            get => _ExitTime;
            set
            {
                if (Set(ref _ExitTime, value))
                {
                    if (ListLogData != null)
                    {
                        OnListListOfUndisciplinedCommandExecute(true);
                    }
                }
            }
        }

        #endregion
        #endregion

        #region Диапозон дат
        private DateTime? _startDate = DateTime.Today;

        public DateTime? StartDate
        {
            get => _startDate;
            set => Set(ref _startDate, value);
        }

        private DateTime? _finishDate = DateTime.Now;

        public DateTime? FinishDate
        {
            get => _finishDate;
            set => Set(ref _finishDate, value);
        }
        #endregion

        #region Список LogDataIO
        private List<LogDataIO>? _ListLogData;

        public List<LogDataIO>? ListLogData
        {
            get => _ListLogData;
            set => Set(ref _ListLogData, value);
        }
        #endregion

        private bool _IsVisibleAllColumn = true;

        public bool IsVisibleAllColumn
        {
            get => _IsVisibleAllColumn;
            set => Set(ref _IsVisibleAllColumn, value);
        }

        private bool _IsVisibleColumnInput = true;
        /// <summary>Определяет видимость столбца входа в таблице</summary>
        public bool IsVisibleColumnInput
        {
            get => _IsVisibleColumnInput;
            set => Set(ref _IsVisibleColumnInput, value);
        }

        private bool _IsVisibleColumnOutput = false;
        /// <summary>Определяет видимость столбца выхода в таблице</summary>
        public bool IsVisibleColumnOutput
        {
            get => _IsVisibleColumnOutput;
            set => Set(ref _IsVisibleColumnOutput, value);
        }

        #region Выбранный сотрудник
        private Person? _SelectedPerson;

        public Person? SelectedPerson
        {
            get => _SelectedPerson;
            set
            {
                Set(ref _SelectedPerson, value);
                if (SelectedPerson != null)
                {
                    IsAllPerson = false;
                    OnFormSelectedPersonCommandExecute(false);
                }

                else
                    ListDataPerson = null;
            }
        }

        private Person? _SelectedPersonFromList;

        public Person? SelectedPersonFromList
        {
            get => _SelectedPersonFromList;
            set
            {
                if (Set(ref _SelectedPersonFromList, value))
                {
                    if (!IsUpdateSelectedPerson)
                    {
                        ListDataPerson = null;
                        SelectedPerson = SelectedPersonFromList;
                        IsAllPerson = false;
                        // OnFormSelectedPersonCommandExecute(false);
                    }
                    if (SelectedPersonFromList != null)
                    {
                        IsAllPerson = false;
                    }
                }
            }
        }


        private LogDataIO? _SelectedPersonIO;

        public LogDataIO? SelectedPersonIO
        {
            get => _SelectedPersonIO;
            set
            {

                if (Set(ref _SelectedPersonIO, value) && _SelectedPersonIO != null)
                {

                    if (SelectedPerson is null || !IsUpdateSelectedPerson)
                    {
                        Person e = new();

                        var person = Divisions?.SelectMany(d => d?.Persons)
                                               .FirstOrDefault(p => p.ID == SelectedPersonIO?.HozOrgan);
                       
                        e = (Person)person.Clone();
                        ListDataPerson = null;

                        SelectedPerson = e;
                        //OnFormSelectedPersonCommandExecute(false);
                    }
                }
            }
        }

        #region Диапозон дат для выбранного сотрудника
        private DateTime? _StartDatePerson = DateTime.Today;

        public DateTime? StartDatePerson
        {
            get => _StartDatePerson;
            set => Set(ref _StartDatePerson, value);
        }

        private DateTime? _FinishDatePerson = DateTime.Now;

        public DateTime? FinishDatePerson
        {
            get => _FinishDatePerson;
            set => Set(ref _FinishDatePerson, value);
        }
        #endregion

        private bool _IsUpdateSelectedPerson;
        /// <summary>
        /// Автоматическое обновление данных выбранного сотрудника
        /// </summary>
        public bool IsUpdateSelectedPerson
        {
            get => _IsUpdateSelectedPerson;
            set => Set(ref _IsUpdateSelectedPerson, value);
        }


        #region Данные событий
        private List<LogData>? _ListDataPerson;

        public List<LogData>? ListDataPerson
        {
            get => _ListDataPerson;
            set => Set(ref _ListDataPerson, value);
        }


        #endregion
        #region Рабочее время
        private DateTime _StartTimePerson = new DateTime(2023, 01, 01, 8, 0, 0);
        /// <summary>Начало рабочего дня сотрудника/summary>
        public DateTime StartTimePerson
        {
            get => _StartTimePerson;
            set => Set(ref _StartTimePerson, value);
        }
        private DateTime _ExitTimePerson = new DateTime(2023, 01, 01, 17, 0, 0);
        /// <summary>конец рабочего дня сотрудника/summary>
        public DateTime ExitTimePerson
        {
            get => _ExitTimePerson;
            set => Set(ref _ExitTimePerson, value);
        }


        #endregion

        #region Фильтрация сотрудников по фамилии
        private string? _SecondNameFilter;

        public string? SecondNameFilter
        {
            get => _SecondNameFilter;
            set
            {
                if (!Set(ref _SecondNameFilter, value))
                    return;


                _CollectionPerson.View?.Refresh();


            }
        }
        private void OnPersonFilterSelectedDivivision(object sender, FilterEventArgs e)
        {
            if (e.Item is not Person p)
            {
                e.Accepted = false;
                return;
            }
            if (SelectedDivision is null)
            {
                e.Accepted = false;
                return;
            }
            if (p.Name is null)
            {
                e.Accepted = false;
                return;
            }
            if (p.DivisionId == SelectedDivision?.ID)
                return;
            e.Accepted = false;
        }
        private void OnPersonFilter(object sender, FilterEventArgs e)
        {
            if (e.Item is not Person p)
            {
                e.Accepted = false;
                return;
            }

            var filter = _SecondNameFilter;
            if (string.IsNullOrEmpty(filter) && SelectedDivision != null && p.DivisionId == SelectedDivision.ID)
            {
                return;
            }
            if (string.IsNullOrEmpty(filter))
            {
                if (SelectedDivision != null && p.DivisionId != SelectedDivision.ID)
                {
                    e.Accepted = false;
                    return;
                }
                return;
            }


            if (p.Name is null)
            {
                e.Accepted = false;
                return;
            }


            if (SelectedDivision != null && p.DivisionId == SelectedDivision?.ID
                && p.Name.Contains(filter, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }


            if (SelectedDivision == null && p.Name.Contains(filter, StringComparison.OrdinalIgnoreCase))
                return;

            e.Accepted = false;


        }
        private readonly CollectionViewSource _CollectionPerson = new CollectionViewSource();
        public ICollectionView CollectionPerson => _CollectionPerson.View;

        #endregion
        #endregion

        #region Команды
        #region Формирование списка опоздавших
        private ICommand? _ListListOfUndisciplinedCommand;
        public ICommand? ListListOfUndisciplinedCommand => _ListListOfUndisciplinedCommand ??=
            new LambdaCommand(OnListListOfUndisciplinedCommandExecute, CanListListOfUndisciplinedCommandExecute);
        private bool CanListListOfUndisciplinedCommandExecute(object? arg) => ListLogData != null;

        private void OnListListOfUndisciplinedCommandExecute(object? obj)
        {
            List<LogDataIO> list = new();
            foreach (LogDataIO log in ListLogData!)
            {
                if (log.FirstInput is DateTime d1 && d1.TimeOfDay > StartTime.TimeOfDay)
                {
                    log.IsLateEntry = true;
                }
                else
                    log.IsLateEntry = false;
                if (log.LastOutput is DateTime d2 && d2.TimeOfDay < ExitTime.TimeOfDay)
                {
                    log.IsEarlyExit = true;
                }
                else
                    log.IsEarlyExit = false;
                list.Add(log);
            }
            ListLogData = list;
        }
        #endregion
        #region Формирование общей таблицы данных
        private ICommand? _FormCommand;
        public ICommand? FormCommand => _FormCommand ??= new LambdaCommand(OnFormCommandExecute, CanFormCommandExecute);

        private bool CanFormCommandExecute(object? arg) => true;


        private void OnFormCommandExecute(object? obj)
        {
            Task.Run(() =>
            {

                try
                {
                    IsEnableFormButton = false;
                    string queryOne = string.Empty;
                    string queryTwo = string.Empty;
                    if (SelectedIndexItem == 0)
                    {
                        if (SelectedGroup == GroupList[0])
                        {
                            if (IsAllPerson)
                            {
                                queryOne = $@"Select distinct i.HozOrgan Id, l.Name SurName, d.Name Division, MIN(i.TimeVal) over (partition by i.hozOrgan) input from pLogData i
                                      Join pList l on(l.ID = i.HozOrgan) 
                                      Join PDivision d on(d.ID = l.Section) 
                                      where i.Event = 32  and i.TimeVal >= '{StartDate}' AND  i.TimeVal <= '{FinishDate}' and i.Mode = 1";

                                queryTwo = $@"Select distinct o.HozOrgan Id,  MAX(o.TimeVal) over (partition by o.hozOrgan) output from pLogData o                                  
                                      where o.Event = 32  and o.TimeVal >= '{StartDate}' AND  o.TimeVal <= '{FinishDate}' and o.Mode = 2";

                                ListLogData = GetListLDIO(queryOne, queryTwo, isShort: true);
                            }
                            else if (SelectedPersonFromList != null)
                            {
                                queryOne = $@"Select distinct i.HozOrgan Id, l.Name SurName, d.Name Division, MIN(i.TimeVal) over (partition by i.hozOrgan) input from pLogData i
                                      Join pList l on(l.ID = i.HozOrgan) 
                                      Join PDivision d on(d.ID = l.Section) 
                                      where i.Event = 32  and i.TimeVal >= '{StartDate}' AND  i.TimeVal <= '{FinishDate}' and i.Mode = 1 and i.HozOrgan = {SelectedPersonFromList.ID}";

                                queryTwo = $@"Select distinct o.HozOrgan Id,  MAX(o.TimeVal) over (partition by o.hozOrgan) output from pLogData o                                  
                                      where o.Event = 32  and o.TimeVal >= '{StartDate}' AND  o.TimeVal <= '{FinishDate}' and o.Mode = 2 and o.HozOrgan = {SelectedPersonFromList.ID}";

                                ListLogData = GetListLDIO(queryOne, queryTwo, isShort: true);
                            }
                        }
                        else if (SelectedGroup == GroupList[1])
                        {

                            if (IsAllPerson)
                            {
                                queryOne = $@"Select p.HozOrgan Id, l.Name SurName, d.Name Division,  p.TimeVal input 
                                              From pLogData p 
                                              Join pList l on(l.ID = p.HozOrgan) 
                                              Join PDivision d on(d.ID = l.Section)
                                              Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}')
                                              AND p.Event = 32 AND p.Mode = 1";


                               // queryOne = @$"With input as(
                               //Select p.HozOrgan, p.mode, p.TimeVal, 
                               //Row_number() Over(Partition by Convert(date, p.TimeVal), p.HozOrgan Order by p.TimeVal asc) first 
                               //From pLogData p 
                               //Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}') AND p.Event = 32 AND p.Mode = 1) 

                               //SElect  i.HozOrgan Id,  l.Name SurName, d.Name Division,  i.TimeVal input From input i 
                               //Join pList l on(l.ID = i.HozOrgan) 
                               //Join PDivision d on(d.ID = l.Section) 
                               //Where i.first = 1 Order by i.TimeVal";
                                ListLogData = GetListLDIO(queryOne);
                            }
                            else if (SelectedPersonFromList != null)
                            {
                                queryOne = $@"Select p.HozOrgan Id, l.Name SurName, d.Name Division,  p.TimeVal input 
                                              From pLogData p 
                                              Join pList l on(l.ID = p.HozOrgan) 
                                              Join PDivision d on(d.ID = l.Section)
                                              Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}')
                                              AND p.Event = 32 AND p.Mode = 1  AND l.ID = {SelectedPersonFromList.ID}";

                                // queryOne = @$"With input as(
                                //Select p.HozOrgan, p.mode, p.TimeVal, 
                                //Row_number() Over(Partition by Convert(date, p.TimeVal), p.HozOrgan Order by p.TimeVal asc) first 
                                //From pLogData p 
                                //Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}') AND p.Event = 32 AND p.Mode = 1) 

                                //SElect  i.HozOrgan Id,  l.Name SurName, d.Name Division,  i.TimeVal input From input i 
                                //Join pList l on(l.ID = i.HozOrgan) 
                                //Join PDivision d on(d.ID = l.Section) 
                                //Where l.ID = {SelectedPersonFromList.ID} AND i.first = 1 Order by i.TimeVal";
                                ListLogData = GetListLDIO(queryOne);
                            }
                        }
                        else if (SelectedGroup == GroupList[2])
                        {
                            if (IsAllPerson)
                            {
                                queryOne = $@"Select p.HozOrgan Id, l.Name SurName, d.Name Division,  p.TimeVal output 
                                              From pLogData p 
                                              Join pList l on(l.ID = p.HozOrgan) 
                                              Join PDivision d on(d.ID = l.Section)
                                              Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}')
                                              AND p.Event = 32 AND p.Mode = 2";
                               // queryOne = @$"With output as(
                               //Select p.HozOrgan, p.mode, p.TimeVal, 
                               //Row_number() Over(Partition by Convert(date, p.TimeVal), p.HozOrgan Order by p.TimeVal desc) first 
                               //From pLogData p 
                               //Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}') AND p.Event = 32 AND p.Mode = 2) 

                               //SElect  o.HozOrgan Id,  l.Name SurName, d.Name Division,  o.TimeVal output From output o 
                               //Join pList l on(l.ID = o.HozOrgan) 
                               //Join PDivision d on(d.ID = l.Section) 
                               //Where o.first = 1 Order by o.TimeVal";
                                ListLogData = GetListLDIO(queryOne);
                            }
                            else if (SelectedPersonFromList != null)
                            {
                                queryOne = $@"Select p.HozOrgan Id, l.Name SurName, d.Name Division,  p.TimeVal output 
                                              From pLogData p 
                                              Join pList l on(l.ID = p.HozOrgan) 
                                              Join PDivision d on(d.ID = l.Section)
                                              Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}')
                                              AND p.Event = 32 AND p.Mode = 2  AND l.ID = {SelectedPersonFromList.ID}";
                                // queryOne = @$"With output as(
                                //Select p.HozOrgan, p.mode, p.TimeVal, 
                                //Row_number() Over(Partition by Convert(date, p.TimeVal), p.HozOrgan Order by p.TimeVal desc) first 
                                //From pLogData p 
                                //Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}') AND p.Event = 32 AND p.Mode = 2) 

                                //SElect  o.HozOrgan Id,  l.Name SurName, d.Name Division,  o.TimeVal output From output o 
                                //Join pList l on(l.ID = o.HozOrgan) 
                                //Join PDivision d on(d.ID = l.Section) 
                                //Where l.ID = {SelectedPersonFromList.ID} AND o.first = 1 Order by o.TimeVal";
                                ListLogData = GetListLDIO(queryOne);
                            }
                        }
                        else if (SelectedGroup == GroupList[3])
                        {
                            if (IsAllPerson)
                            {
                                queryOne = @$"With input as(
                               Select p.HozOrgan, p.mode, p.TimeVal, 
                               Row_number() Over(Partition by Convert(date, p.TimeVal), p.HozOrgan Order by p.TimeVal asc) first 
                               From pLogData p 
                               Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}') AND p.Event = 32 AND p.Mode = 1) 

                               SElect  i.HozOrgan Id,  l.Name SurName, d.Name Division,  i.TimeVal input From input i 
                               Join pList l on(l.ID = i.HozOrgan) 
                               Join PDivision d on(d.ID = l.Section) 
                               Where i.first = 1 Order by i.TimeVal";
                                queryTwo = @$"With output as(
                               Select p.HozOrgan, p.TimeVal,
                               Row_number() Over(Partition by Convert(date, p.TimeVal), p.HozOrgan Order by p.TimeVal desc) last 
                               From pLogData p 
                               Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}') AND p.Event = 32 AND p.Mode = 2) 

                               Select o.HozOrgan Id,  o.TimeVal output, l.Name SurName, d.Name Division  from output o 
                               Join pList l on(l.ID = o.HozOrgan) 
                               Join PDivision d on(d.ID = l.Section) 
                               Where o.last = 1 Order by o.TimeVal";

                                ListLogData = GetListLDIO(queryOne, queryTwo);
                            }
                            else if (SelectedPersonFromList != null)
                            {
                                queryOne = @$"With input as(
                               Select p.HozOrgan, p.mode, p.TimeVal, 
                               Row_number() Over(Partition by Convert(date, p.TimeVal), p.HozOrgan Order by p.TimeVal asc) first 
                               From pLogData p 
                               Join pList l On(p.HozOrgan = l.ID)
                               Join PDivision d On(l.Section = d.ID)
                               Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}') AND p.Event = 32 AND p.Mode = 1 
                               AND l.ID = {SelectedPersonFromList?.ID}) 

                               SElect  i.HozOrgan Id,  l.Name SurName, d.Name Division,  i.TimeVal input From input i 
                               Join pList l on(l.ID = i.HozOrgan) 
                               Join PDivision d on(d.ID = l.Section) 
                               Where i.first = 1 Order by i.TimeVal";
                                queryTwo = @$"With output as(
                               Select p.HozOrgan, p.TimeVal,
                               Row_number() Over(Partition by Convert(date, p.TimeVal), p.HozOrgan Order by p.TimeVal desc) last 
                               From pLogData p 
                               Join pList l On(p.HozOrgan = l.ID)
                               Join PDivision d On(l.Section = d.ID)
                               Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}') AND p.Event = 32 AND p.Mode = 2 
                               AND l.ID = {SelectedPersonFromList?.ID}) 

                               Select o.HozOrgan Id, o.TimeVal output from output o 
                               Where o.last = 1 Order by o.TimeVal";
                                ListLogData = GetListLDIO(queryOne, queryTwo);
                            }
                        }
                    }
                    else if (SelectedIndexItem == 1)
                    {
                        if (SelectedGroup == GroupList[0])
                        {
                            if (IsAllPerson)
                            {
                                queryOne = $@"Select distinct i.HozOrgan Id, l.Name SurName, d.Name Division, MIN(i.TimeVal) over (partition by i.hozOrgan) input from pLogData i
                                      Join pList l on(l.ID = i.HozOrgan) 
                                      Join PDivision d on(d.ID = l.Section) 
                                      Where i.Event = 32  and i.TimeVal >= '{StartDate}' AND  i.TimeVal <= '{FinishDate}' and i.Mode = 1 AND d.ID = {SelectedDivision?.ID}";

                                queryTwo = $@"Select distinct o.HozOrgan Id,  MAX(o.TimeVal) over (partition by o.hozOrgan) output from pLogData o 
                                            Join pList l on(l.ID = o.HozOrgan) 
                                            Join PDivision d on(d.ID = l.Section) 
                                            Where o.Event = 32  and o.TimeVal >= '{StartDate}' AND  o.TimeVal <= '{FinishDate}' and o.Mode = 2 AND d.ID = {SelectedDivision?.ID}";

                                ListLogData = GetListLDIO(queryOne, queryTwo, isShort: true);
                            }
                            else if (SelectedPersonFromList != null)
                            {
                                queryOne = $@"Select distinct i.HozOrgan Id, l.Name SurName, d.Name Division, MIN(i.TimeVal) over (partition by i.hozOrgan) input from pLogData i
                                      Join pList l on(l.ID = i.HozOrgan) 
                                      Join PDivision d on(d.ID = l.Section) 
                                      where i.Event = 32  and i.TimeVal >= '{StartDate}' AND  i.TimeVal <= '{FinishDate}' and i.Mode = 1 and i.HozOrgan = {SelectedPersonFromList.ID}";

                                queryTwo = $@"Select distinct o.HozOrgan Id,  MAX(o.TimeVal) over (partition by o.hozOrgan) output from pLogData o                                  
                                      where o.Event = 32  and o.TimeVal >= '{StartDate}' AND  o.TimeVal <= '{FinishDate}' and o.Mode = 2 and o.HozOrgan = {SelectedPersonFromList.ID}";

                                ListLogData = GetListLDIO(queryOne, queryTwo, isShort: true);
                            }
                        }
                        else if (SelectedGroup == GroupList[1])
                        {

                            if (IsAllPerson)
                            {
                                queryOne = $@"Select p.HozOrgan Id, l.Name SurName, d.Name Division,  p.TimeVal input 
                                              From pLogData p 
                                              Join pList l on(l.ID = p.HozOrgan) 
                                              Join PDivision d on(d.ID = l.Section)
                                              Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}')
                                              AND p.Event = 32 AND p.Mode = 1  AND d.ID = {SelectedDivision?.ID}";
                                //Запрос для первого входа
                                // queryOne = @$"With input as(
                                //Select p.HozOrgan, p.mode, p.TimeVal, 
                                //Row_number() Over(Partition by Convert(date, p.TimeVal), p.HozOrgan Order by p.TimeVal asc) first 
                                //From pLogData p 
                                //Join pList l on(l.ID = p.HozOrgan) 
                                //Join PDivision d on(d.ID = l.Section) 
                                //Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}') AND p.Event = 32 AND p.Mode = 1  AND d.ID = {SelectedDivision?.ID}) 

                                //SElect  i.HozOrgan Id,  l.Name SurName, d.Name Division,  i.TimeVal input From input i 
                                //Join pList l on(l.ID = i.HozOrgan) 
                                //Join PDivision d on(d.ID = l.Section) 
                                //Where i.first = 1 Order by i.TimeVal";
                                ListLogData = GetListLDIO(queryOne);
                            }
                            else if (SelectedPersonFromList != null)
                            {
                                queryOne = $@"Select p.HozOrgan Id, l.Name SurName, d.Name Division,  p.TimeVal input 
                                              From pLogData p 
                                              Join pList l on(l.ID = p.HozOrgan) 
                                              Join PDivision d on(d.ID = l.Section)
                                              Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}')
                                              AND p.Event = 32 AND p.Mode = 1  AND l.ID = {SelectedPersonFromList.ID}";

                                //Запрос для первого входа
                               // queryOne = @$"With input as(
                               //Select p.HozOrgan, p.mode, p.TimeVal, 
                               //Row_number() Over(Partition by Convert(date, p.TimeVal), p.HozOrgan Order by p.TimeVal asc) first 
                               //From pLogData p 
                               //Join pList l on(l.ID = p.HozOrgan) 
                               //Join PDivision d on(d.ID = l.Section) 
                               //Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}') AND p.Event = 32 AND p.Mode = 1  AND l.ID = {SelectedPersonFromList.ID}) 

                               //SElect  i.HozOrgan Id,  l.Name SurName, d.Name Division,  i.TimeVal input From input i 
                               //Join pList l on(l.ID = i.HozOrgan) 
                               //Join PDivision d on(d.ID = l.Section) 
                               //Where l.ID = {SelectedPersonFromList.ID} AND i.first = 1 Order by i.TimeVal";
                                ListLogData = GetListLDIO(queryOne);
                            }
                        }
                        else if (SelectedGroup == GroupList[2])
                        {

                            if (IsAllPerson)
                            {
                                queryOne = $@"Select p.HozOrgan Id, l.Name SurName, d.Name Division,  p.TimeVal output 
                                              From pLogData p 
                                              Join pList l on(l.ID = p.HozOrgan) 
                                              Join PDivision d on(d.ID = l.Section)
                                              Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}')
                                              AND p.Event = 32 AND p.Mode = 2  AND d.ID = {SelectedDivision?.ID}";


                                //Запрос для последнего выхода
                                // queryOne = @$"With output as(
                                //Select p.HozOrgan, p.mode, p.TimeVal, 
                                //Row_number() Over(Partition by Convert(date, p.TimeVal), p.HozOrgan Order by p.TimeVal desc) first 
                                //From pLogData p 
                                //Join pList l on(l.ID = p.HozOrgan) 
                                //Join PDivision d on(d.ID = l.Section) 
                                //Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}') AND p.Event = 32 AND p.Mode = 2  AND d.ID = {SelectedDivision?.ID}) 

                                //SElect  o.HozOrgan Id,  l.Name SurName, d.Name Division,  o.TimeVal output From output o 
                                //Join pList l on(l.ID = o.HozOrgan) 
                                //Join PDivision d on(d.ID = l.Section) 
                                //Where o.first = 1 Order by o.TimeVal";
                                ListLogData = GetListLDIO(queryOne);
                            }
                            else if (SelectedPersonFromList != null)
                            {
                                queryOne = $@"Select p.HozOrgan Id, l.Name SurName, d.Name Division,  p.TimeVal output 
                                              From pLogData p 
                                              Join pList l on(l.ID = p.HozOrgan) 
                                              Join PDivision d on(d.ID = l.Section)
                                              Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}')
                                              AND p.Event = 32 AND p.Mode = 2  AND l.ID = {SelectedPersonFromList.ID}";

                                //Запрос для последнего выхода
                                // queryOne = @$"With output as(
                                //Select p.HozOrgan, p.mode, p.TimeVal, 
                                //Row_number() Over(Partition by Convert(date, p.TimeVal), p.HozOrgan Order by p.TimeVal desc) first 
                                //From pLogData p 
                                //Join pList l on(l.ID = p.HozOrgan) 
                                //Join PDivision d on(d.ID = l.Section) 
                                //Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}') AND p.Event = 32 AND p.Mode = 2  AND l.ID = {SelectedPersonFromList.ID}) 

                                //SElect  o.HozOrgan Id,  l.Name SurName, d.Name Division,  o.TimeVal output From output o 
                                //Join pList l on(l.ID = o.HozOrgan) 
                                //Join PDivision d on(d.ID = l.Section) 
                                //Where l.ID = {SelectedPersonFromList.ID} AND o.first = 1 Order by o.TimeVal";
                                ListLogData = GetListLDIO(queryOne);
                            }
                        }
                        else if (SelectedGroup == GroupList[3])
                        {
                            if (IsAllPerson)
                            {
                                queryOne = @$"With input as(
                               Select p.HozOrgan, p.mode, p.TimeVal, 
                               Row_number() Over(Partition by Convert(date, p.TimeVal), p.HozOrgan Order by p.TimeVal asc) first 
                               From pLogData p 
                               Join pList l on(l.ID = p.HozOrgan) 
                               Join PDivision d on(d.ID = l.Section) 
                               Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}') AND p.Event = 32 AND p.Mode = 1 AND d.ID = {SelectedDivision?.ID}) 

                               SElect  i.HozOrgan Id,  l.Name SurName, d.Name Division,  i.TimeVal input From input i 
                               Join pList l on(l.ID = i.HozOrgan) 
                               Join PDivision d on(d.ID = l.Section) 
                               Where i.first = 1 Order by i.TimeVal";
                                queryTwo = @$"With output as(
                               Select p.HozOrgan, p.TimeVal,
                               Row_number() Over(Partition by Convert(date, p.TimeVal), p.HozOrgan Order by p.TimeVal desc) last 
                               From pLogData p 
                               Join pList l on(l.ID = p.HozOrgan) 
                               Join PDivision d on(d.ID = l.Section)
                               Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}') AND p.Event = 32 AND p.Mode = 2 AND d.ID = {SelectedDivision?.ID}) 

                               Select o.HozOrgan Id,  o.TimeVal output, l.Name SurName, d.Name Division  from output o 
                               Join pList l on(l.ID = o.HozOrgan) 
                               Join PDivision d on(d.ID = l.Section) 
                               Where o.last = 1 Order by o.TimeVal";

                                ListLogData = GetListLDIO(queryOne, queryTwo);
                            }
                            else if (SelectedPersonFromList != null)
                            {
                                queryOne = @$"With input as(
                               Select p.HozOrgan, p.mode, p.TimeVal, 
                               Row_number() Over(Partition by Convert(date, p.TimeVal), p.HozOrgan Order by p.TimeVal asc) first 
                               From pLogData p 
                               Join pList l On(p.HozOrgan = l.ID)
                               Join PDivision d On(l.Section = d.ID)
                               Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}') AND p.Event = 32 AND p.Mode = 1 
                               AND l.ID = {SelectedPersonFromList?.ID}) 

                               SElect  i.HozOrgan Id,  l.Name SurName, d.Name Division,  i.TimeVal input From input i 
                               Join pList l on(l.ID = i.HozOrgan) 
                               Join PDivision d on(d.ID = l.Section) 
                               Where i.first = 1 Order by i.TimeVal";
                                queryTwo = @$"With output as(
                               Select p.HozOrgan, p.TimeVal,
                               Row_number() Over(Partition by Convert(date, p.TimeVal), p.HozOrgan Order by p.TimeVal desc) last 
                               From pLogData p 
                               Join pList l On(p.HozOrgan = l.ID)
                               Join PDivision d On(l.Section = d.ID)
                               Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}') AND p.Event = 32 AND p.Mode = 2 
                               AND l.ID = {SelectedPersonFromList?.ID}) 

                               Select o.HozOrgan Id, o.TimeVal output from output o 
                               Where o.last = 1 Order by o.TimeVal";
                                ListLogData = GetListLDIO(queryOne, queryTwo);
                            }
                        }

                    }
                    else if (SelectedGroup == GroupList[1])
                    {

                        if (IsAllPerson && SelectedDivision != null)
                        {

                            queryOne = @$"With input as(
                               Select p.HozOrgan, p.mode, p.TimeVal, 
                               Row_number() Over(Partition by Convert(date, p.TimeVal), p.HozOrgan Order by p.TimeVal asc) first 
                               From pLogData p 
                               Join pList l On(p.HozOrgan = l.ID)
                               Join PDivision d On(l.Section = d.ID)
                               Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}') AND p.Event = 32 AND p.Mode = 1 AND d.ID = {SelectedDivision?.ID}) 

                               SElect  i.HozOrgan Id,  l.Name SurName, d.Name Division,  i.TimeVal input From input i 
                               Join pList l on(l.ID = i.HozOrgan) 
                               Join PDivision d on(d.ID = l.Section) 
                               Where i.first = 1 Order by i.TimeVal";
                            queryTwo = @$"With output as(
                               Select p.HozOrgan, p.TimeVal,
                               Row_number() Over(Partition by Convert(date, p.TimeVal), p.HozOrgan Order by p.TimeVal desc) last 
                               From pLogData p 
                               Join pList l On(p.HozOrgan = l.ID)
                               Join PDivision d On(l.Section = d.ID)
                               Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}') AND p.Event = 32 AND p.Mode = 2 AND d.ID = {SelectedDivision?.ID}) 

                               Select o.HozOrgan Id, o.TimeVal output from output o 
                               Where o.last = 1 Order by o.TimeVal";
                            ListLogData = GetListLDIO(queryOne, queryTwo);
                        }
                        else if (SelectedDivision != null && SelectedPersonFromList != null)
                        {
                            queryOne = @$"With input as(
                               Select p.HozOrgan, p.mode, p.TimeVal, 
                               Row_number() Over(Partition by Convert(date, p.TimeVal), p.HozOrgan Order by p.TimeVal asc) first 
                               From pLogData p 
                               Join pList l On(p.HozOrgan = l.ID)
                               Join PDivision d On(l.Section = d.ID)
                               Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}') AND p.Event = 32 AND p.Mode = 1 
                               AND l.ID = {SelectedPersonFromList?.ID}) 

                               SElect  i.HozOrgan Id,  l.Name SurName, d.Name Division,  i.TimeVal input From input i 
                               Join pList l on(l.ID = i.HozOrgan) 
                               Join PDivision d on(d.ID = l.Section) 
                               Where i.first = 1 Order by i.TimeVal";
                            queryTwo = @$"With output as(
                               Select p.HozOrgan, p.TimeVal,
                               Row_number() Over(Partition by Convert(date, p.TimeVal), p.HozOrgan Order by p.TimeVal desc) last 
                               From pLogData p 
                               Join pList l On(p.HozOrgan = l.ID)
                               Join PDivision d On(l.Section = d.ID)
                               Where (p.TimeVal >= '{StartDate}' AND p.TimeVal <= '{FinishDate}') AND p.Event = 32 AND p.Mode = 2 
                               AND l.ID = {SelectedPersonFromList?.ID}) 

                               Select o.HozOrgan Id, o.TimeVal output from output o 
                               Where o.last = 1 Order by o.TimeVal";
                            ListLogData = GetListLDIO(queryOne, queryTwo);
                        }
                    }

                    List<LogDataIO> GetListLDIO(string queryOne, string? queryTwo = null, bool isOutput = false, bool isShort = false)
                    {

                        DataTable data = SclDataConnection.GetData(queryOne).Result;
                        List<LogDataIO> list = LogDataIO.GetLogDataIO(data, StartTime.TimeOfDay);
                        if (queryTwo != null)
                        {
                            data = SclDataConnection.GetData(queryTwo).Result;
                            LogDataIO.InitialLastOutput(list, data, ExitTime.TimeOfDay, isShort);
                        }
                        return list;
                    }
                }
                catch (ArgumentException ex)
                {
                    ListLogData = null;
                    Window.ShowDialog(ex.Message);
                }
                catch (Exception ex)
                {
                    Window.ShowDialog(ex.Message);
                }
                finally
                {
                    IsEnableFormButton = true;
                }
            });
        }
        #endregion

        #region Формирование таблицы для выбранного сотрудника
        private ICommand? _FormSelectedPersonCommand;
        public ICommand? FormSelectedPersonCommand => _FormSelectedPersonCommand
            ??= new LambdaCommand(OnFormSelectedPersonCommandExecute, CanFormSelectedPersonCommandexecute);

        private bool CanFormSelectedPersonCommandexecute(object? arg) => SelectedPerson != null && IsCancelQuery;


        private void OnFormSelectedPersonCommandExecute(object? obj)
        {
            if (IsCancelQuery)
            {
                Task.Run(() =>
                     {
                         try
                         {
                             List<LogData> list = new();
                             IsCancelQuery = false;
                             IsEnableFormButtonPerson = false;
                             string queryOne = @$"With input as(
                               Select p.HozOrgan, p.TimeVal, 
                               Row_number() Over(Partition by Convert(date, p.TimeVal), p.HozOrgan Order by p.TimeVal asc) first 
                               From pLogData p 
                               Where (p.TimeVal >= '{StartDatePerson}' AND p.TimeVal <= '{FinishDatePerson}') 
                               AND p.Event = 32 AND p.Mode = 1 AND p.HozOrgan = {SelectedPerson?.ID}) 

                               SElect i.TimeVal input From input i                              
                               Where i.first = 1 Order by i.TimeVal";

                             string queryTwo = @$"With output as(
                               Select p.HozOrgan, p.TimeVal,
                               Row_number() Over(Partition by Convert(date, p.TimeVal), p.HozOrgan Order by p.TimeVal desc) last 
                               From pLogData p 
                               Where (p.TimeVal >= '{StartDatePerson}' AND p.TimeVal <= '{FinishDatePerson}') 
                               AND p.Event = 32 AND p.Mode = 2 AND p.HozOrgan = {SelectedPerson?.ID}) 

                               Select o.TimeVal output from output o 
                               Where o.last = 1 Order by o.TimeVal";

                             list = LogData.GetLogDataFromDataTable(queryOne, queryTwo);
                             foreach (var item in list)
                             {
                                 if (item.EntryTime is DateTime d1 && d1.TimeOfDay > StartTimePerson.TimeOfDay)
                                 {
                                     item.IsLateEntry = true;
                                 }
                                 if (item.ExitTime is DateTime d2 && d2.TimeOfDay < ExitTimePerson.TimeOfDay)
                                 {
                                     item.IsEarlyExit = true;
                                 }
                             }
                             ListDataPerson = list;
                         }
                         catch (Exception ex)
                         {
                             ListDataPerson = null;
                             Window.ShowDialog(ex.Message);
                         }
                         finally
                         {
                             IsCancelQuery = true;
                             IsEnableFormButtonPerson = true;
                         }
                     });
            }
        }
        #endregion
        #endregion

        public СheckpointVM()
        {
            GroupList = new List<string>
            {
            "Первый вход - Последний выход",
            "Только вход",
            "Только выход",
            "Вход - Выход за выбранный период"
            };
            _SelectedGroup = GroupList[0];
            Dir.Checkpoint = this;

            #region Инициализация подразделений
            using (DataUserContext db = new())
            {
                //Divisions = db.Divisions
                //    .Include(d => d.Persons)
                //    .ToList();

                List<Person> persons = db.Persons
                     .Include(p => p.Company)
                     .Include(p => p.Post)
                     .Include(d => d.Division)
                     .ToList();

                Divisions = db.Divisions.ToList();
                _CollectionPerson.Source = from p in persons select p;
            }
            #endregion
            _CollectionPerson.Filter += OnPersonFilter;

            Window = new DialogWindow();
        }
    }
}
