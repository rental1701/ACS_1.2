using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.Commands.BaseCommand
{
    internal class LambdaCommand : Command
    {
        private readonly Action<object?> _Execute;
        private readonly Func<object?, bool> _CanExecute;
        public override bool CanExecute(object? parameter)
       => _CanExecute?.Invoke(parameter) ?? true;

        public override void Execute(object? parameter)
       => _Execute?.Invoke(parameter);

        public LambdaCommand(Action<object?> execute, Func<object?, bool> canExecute)
        {
            _Execute = execute;
            _CanExecute = canExecute;
        }
    }
}
