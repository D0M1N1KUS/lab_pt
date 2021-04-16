using System;
using System.Threading;
using System.Windows.Input;

namespace Lab1.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
    private readonly Predicate<object> _canExecute;
        private EventHandler _requerySuggestedLocal;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute == null)
                    return;

                EventHandler eh = _requerySuggestedLocal;
                EventHandler ehTmp;
                do
                {
                    ehTmp = eh;
                    EventHandler valueTmp = (EventHandler)Delegate.Combine(ehTmp, value);
                    eh = Interlocked.CompareExchange(ref _requerySuggestedLocal, valueTmp, ehTmp);
                }
                while (eh != ehTmp);
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute == null)
                    return;

                EventHandler eventHandler = _requerySuggestedLocal;
                EventHandler eventHandler2;
                do
                {
                    eventHandler2 = eventHandler;
                    EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
                    eventHandler = Interlocked.CompareExchange(ref _requerySuggestedLocal, value2, eventHandler2);
                }
                while (eventHandler != eventHandler2);
                CommandManager.RequerySuggested -= value;
            }
        }

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));

            if (canExecute != null)
            {
                _canExecute = canExecute;
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
            {
                return _canExecute(parameter);
            }
            return true;
        }

        public virtual void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                _execute?.Invoke(parameter);
            }
        }
    }
}