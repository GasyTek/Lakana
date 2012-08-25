using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GasyTek.Lakana.Common.Base;
using GasyTek.Lakana.Common.Extensions;
using GasyTek.Lakana.Common.UI;
using GasyTek.Lakana.Navigation.Services;
using Samples.GasyTek.Lakana.Navigation.Common;
using Samples.GasyTek.Lakana.Navigation.Data;

namespace Samples.GasyTek.Lakana.Navigation.Features
{
    public class ProductEditViewModel : NotifyPropertyChangedBase, IViewKeyAware, ICloseable, IPresentable
    {
        private readonly IUIMetadata _uiMetadata;
        private bool _isDirty;

        public Product Product { get; private set; }
        public ICommand SaveCommand { get; private set; }

        /// <summary>
        /// Used to simulate a dirty form.
        /// </summary>
        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                _isDirty = value;
                this.NotifyPropertyChanged(c => c.IsDirty);
            }
        }

        public string Description
        {
            get { return GetDescription(); }
        }

        public ProductEditViewModel(Product product)
        {
            _uiMetadata = new UIMetadata { LabelProvider = () => "Edit a product" };
            IsDirty = true;
            Product = product;

            SaveCommand = new SimpleCommand<object>(OnSaveCommandExecute);
        }

        private void OnSaveCommandExecute(object param)
        {
            if (!IsDirty)
            {
                Singletons.NavigationService.Close(ViewKey);
                return;
            }

            // Here is an example of a message box control.
            // The message box returns a Task<MessageBoxResult>, that means that you can use the C#5 async/await pattern to improve this code.
            // Notice that you have to use TaskScheduler.FromCurrentSynchronizationContext()
            // in order to use the UI thread synchronization context
            var messageBoxResult = Singletons.NavigationService.ShowMessageBox(ViewKey, "Save changes ?", MessageBoxImage.Question, MessageBoxButton.YesNoCancel);
            messageBoxResult.ContinueWith(r =>
                                              {
                                                  switch (r.Result)
                                                  {
                                                      case MessageBoxResult.Yes:

                                                          // put here logic that corresponds to Yes action

                                                          // close this view
                                                          Singletons.NavigationService.Close(ViewKey);
                                                          break;
                                                      case MessageBoxResult.No:
                                                          // put here logic that corresponds to No action

                                                          // close this view
                                                          Singletons.NavigationService.Close(ViewKey);
                                                          break;
                                                      case MessageBoxResult.Cancel:
                                                          // do nothing
                                                          break;
                                                  }
                                              }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private string GetDescription()
        {
            return "This view simulates a form.\r\n\r\n" +
                   "You can simulate a dirty form by checking the corresponding checkbox.\r\n" +
                   "If the form is dirty, attempting to save datas will show you a message box " +
                   "and attempting to shutdown the application will show the task manager view.\r\n" +
                   "From the task manager view, you can decide to force the application to close or fix problems before closing the app\r\n\r\n" +
                   "Features demonstrated : " +
                   "\r\n > Message boxes" +
                   "\r\n > Application shutdown management";
        }

        #region ICloseable members

        public bool CanClose()
        {
            // put here logic that determine if the this view can be closed
            return !IsDirty;
        }

        #endregion

        #region IPresentable members

        public IUIMetadata UIMetadata
        {
            get { return _uiMetadata; }
        }

        #endregion

        #region IViewKeyAware members

        public string ViewKey { get; set; }

        #endregion
    }
}
