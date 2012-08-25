using System.Windows.Input;

namespace GasyTek.Lakana.Mvvm.Commands
{
    public interface ISimpleCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }

    public interface ISimpleCommand<TParam> : ISimpleCommand
    {
       
    }
}