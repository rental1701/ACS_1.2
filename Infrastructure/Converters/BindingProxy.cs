using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ACS.Infrastructure.Converters
{
    internal class BindingProxy : Freezable
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(BindingProxy));
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }
        public object Value
        {
            get => GetValue(ValueProperty);
            set=>SetValue(ValueProperty, value);
        }
    }
}
