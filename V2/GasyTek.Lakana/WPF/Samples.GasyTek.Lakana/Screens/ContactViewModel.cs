using System.Threading.Tasks;
using System.Windows;
using GasyTek.Lakana.Common.UI;
using GasyTek.Lakana.Mvvm.Validation;
using GasyTek.Lakana.Mvvm.ViewModelProperties;
using GasyTek.Lakana.Mvvm.ViewModels;
using GasyTek.Lakana.Navigation.Services;
using Samples.GasyTek.Lakana.Model;

namespace Samples.GasyTek.Lakana.Screens
{
    public class ContactViewModel : EditableViewModelBase<Contact>, IViewKeyAware, ICloseable, IPresentable
    {
        public string ViewInstanceKey { get; set; }
        public IViewModelProperty FirstName { get; set; }
        public IViewModelProperty LastName { get; set; }
        public IViewModelProperty PhoneNumber { get; set; }
        public IViewModelProperty Sex { get; set; }
        public IViewModelProperty DateOfBirth { get; set; }
        public IViewModelProperty DateOfDeath { get; set; }
        public IViewModelProperty Email { get; set; }

        public ContactViewModel()
        {
            UIMetadata = new UIMetadata {LabelProvider = () => "Modify Contact Informations"};
        }

        protected override void OnCreateViewModelProperties()
        {
            FirstName = CreateValueProperty(Model.FirstName);
            LastName = CreateValueProperty(Model.LastName);
            PhoneNumber = CreateValueProperty(Model.PhoneNumber);
            Sex = CreateEnumProperty(Model.Sex);
            DateOfBirth = CreateValueProperty(Model.DateOfBirth);
            DateOfDeath = CreateValueProperty(Model.DateOfDeath);
            Email = CreateValueProperty(Model.Email);
        }

        protected override IValidationEngine OnCreateValidationEngine()
        {
            return new ContactViewModelValidation(this);
        }

        protected override void OnSave()
        {
            NavigationManager.Close(ViewInstanceKey);
        }

        protected override void OnCancel()
        {
            if(CanClose() == false)
            {
                var questionResult = NavigationManager.ShowMessageBox(ViewInstanceKey
                    , "Do you want to cancel and loose your current modifications ?"
                    , MessageBoxImage.Question
                    , MessageBoxButton.YesNo);
                questionResult.ContinueWith(result =>
                                                {
                                                    if (result.Result == MessageBoxResult.Yes)
                                                    {
                                                        NavigationManager.Close(ViewInstanceKey);
                                                    }
                                                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                NavigationManager.Close(ViewInstanceKey);
            }
        }

        public bool CanClose()
        {
            return HasChanged == false;
        }
    }
}