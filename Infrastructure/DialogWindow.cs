using ACS.Interfaces;
using ACS.View.MessageWindow;
using ACS.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ACS.Infrastructure
{
    public class DialogWindow : IDialogWindow
    {
        public void ShowDialog(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                WindowInfo info = new WindowInfo();
                info.txtMessage.Text = message;
                info.Title = "Сообщение";
                info.Owner = Application.Current.MainWindow;
                info.ShowDialog();
            });

        }
    }
}
