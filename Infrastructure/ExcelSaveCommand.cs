using ACS.Commands.BaseCommand;
using ACS.Model;
using ACS.ViewModels.Base;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ACS.Infrastructure
{
    class ExcelSaveCommand : ViewModel
    {
        private bool _IsEnableButton;

        public bool IsEnableButton
        {
            get => _IsEnableButton;
            set => Set(ref _IsEnableButton, value);
        }

        private bool _IsEnableButtonPerson;

        public bool IsEnableButtonPerson
        {
            get => _IsEnableButtonPerson;
            set => Set(ref _IsEnableButtonPerson, value);
        }


        public ICommand SaveCommand { get; }

        private bool CanSaveCommandExecuted(object? arg)
        {
            IsEnableButton = arg is List<LogDataIO> list && list.Count > 0;
            return IsEnableButton;
        }


        private void OnSaveCommandExecuted(object? obj)
        {
            Task.Run(() =>
            {
                try
                {
                    IsEnableButton = false;
                    if (obj is List<LogDataIO> list)
                    {
                        string filePath = "Книга Excel.xlsx";
                        var save = new SaveFileDialog
                        {
                            Title = "Сохранение фала",
                            Filter = "Книга Excel(*.xlsx)|*.xlsx",
                            FileName = filePath,
                            RestoreDirectory = true
                        };

                        if (save.ShowDialog() != true)
                            return;
                        filePath = save.FileName;


                        Export.ExportToExcel(list, filePath);
                    }
                }
                catch (Exception ex)
                {
                    DialogWindow window = new();
                    window.ShowDialog(ex.Message);
                }
                finally { IsEnableButton = true; }
            });
        }


        public ICommand SavePersonDataCommand { get; }
        private bool CanSavePersonDataCommandExecuted(object? arg)
        {
            IsEnableButtonPerson = arg is object[] pd && pd[0] is List<LogData> && pd[1] is Person;
            return IsEnableButtonPerson;
        }


        private void OnSavePersonDataCommandExecuted(object? obj)
        {

            Task.Run(() =>
            {

                List<(int t, int)> a = new List<(int,int)>
                {
                    (1,3 ),
                    (2,4)
                };
                
                try
                {
                    IsEnableButtonPerson = false;
                    if (obj is object[] pd && pd[0] is List<LogData> logs && pd[1] is Person p)
                    {
                        string file = p.FullName + ".xlsx";
                        var saveWindow = new SaveFileDialog
                        {
                            Title = "Сохранение файла",
                            Filter = "Книга Escel(.xlsx)|.xlsx",
                            FileName = file,
                            RestoreDirectory = true
                        };

                        if (saveWindow.ShowDialog() != true)
                            return;

                        file = saveWindow.FileName;
                        Export.ExportPersonToExcel(p, logs, file);
                    }

                }
                catch (Exception ex)
                {
                    DialogWindow window = new();
                    window.ShowDialog(ex.Message);
                }
                finally
                {
                    IsEnableButtonPerson = true;
                }
            });
        }



        public ExcelSaveCommand()
        {
            SaveCommand = new LambdaCommand(OnSaveCommandExecuted, CanSaveCommandExecuted);

            SavePersonDataCommand = new LambdaCommand(OnSavePersonDataCommandExecuted, CanSavePersonDataCommandExecuted);
        }



    }
}
