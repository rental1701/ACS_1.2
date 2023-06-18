using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ACS.View.MessageWindow
{
    /// <summary>
    /// Логика взаимодействия для WindowInfo.xaml
    /// </summary>
    public partial class WindowInfo : Window
    {
        //public static readonly DependencyProperty MessageProperty = DependencyProperty
        //    .Register(nameof(Message),
        //    typeof(string),
        //    typeof(WindowInfo), 
        //    new PropertyMetadata("Системное сообщение"));

        //public string Message
        //{
        //    get=> (string)GetValue(MessageProperty);
        //    set => SetValue(MessageProperty, value);
        //}

        public WindowInfo()
        {
            InitializeComponent();
        }
    }
}
