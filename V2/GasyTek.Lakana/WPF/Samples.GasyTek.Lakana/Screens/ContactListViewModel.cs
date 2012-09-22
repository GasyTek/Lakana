using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GasyTek.Lakana.Mvvm.Commands;
using GasyTek.Lakana.Mvvm.ViewModels;
using GasyTek.Lakana.Navigation.Services;
using Samples.GasyTek.Lakana.Model;
using Samples.GasyTek.Lakana.Utils;
using GasyTek.Lakana.Common.Extensions;

namespace Samples.GasyTek.Lakana.Screens
{
    public class ContactListViewModel : ViewModelBase
    {
        private Contact _selectedContact;

        public ObservableCollection<Contact> Contacts { get; private set; }

        public Contact SelectedContact
        {
            get { return _selectedContact; } 
            set { this.SetPropertyValueAndNotify(ref _selectedContact, value, vm => vm.SelectedContact); }
        }

        public ICommand EditContactCommand { get; private set; }

        public ContactListViewModel()
        {
            Contacts = new ObservableCollection<Contact>(Database.GetContacts());
            SelectedContact = Contacts.First();

            EditContactCommand = new SimpleCommand(param =>
                                                       {
                                                           if (SelectedContact == null) return;
                                                           var viewModel = new ContactViewModel { Model = SelectedContact };
                                                           var navigationInfo = NavigationInfo.CreateComplex(ScreenId.Contact,
                                                                                        ScreenId.ContactList,
                                                                                        viewModel);
                                                           Singletons.NavigationServiceInstance.NavigateTo<ContactView>(navigationInfo);
                                                       });
        }

        protected override void OnCreateViewModelProperties()
        {
            
        }
    }
}
