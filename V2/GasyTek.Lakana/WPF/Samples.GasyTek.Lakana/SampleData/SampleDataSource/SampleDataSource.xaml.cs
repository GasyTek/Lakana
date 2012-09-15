//      *********    NE PAS MODIFIER CE FICHIER     *********
//      Ce fichier est régénéré par un outil de création.Modifier
// .     ce fichier peut provoquer des erreurs.
namespace Expression.Blend.SampleData.SampleDataSource
{
	using System; 

// To significantly reduce the sample data footprint in your production application, you can set
// the DISABLE_SAMPLE_DATA conditional compilation constant and disable sample data at runtime.
#if DISABLE_SAMPLE_DATA
	internal class SampleDataSource { }
#else

	public class SampleDataSource : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		public SampleDataSource()
		{
			try
			{
				System.Uri resourceUri = new System.Uri("/Samples.GasyTek.Lakana;component/SampleData/SampleDataSource/SampleDataSource.xaml", System.UriKind.Relative);
				if (System.Windows.Application.GetResourceStream(resourceUri) != null)
				{
					System.Windows.Application.LoadComponent(this, resourceUri);
				}
			}
			catch (System.Exception)
			{
			}
		}

		private ItemCollection _Collection = new ItemCollection();

		public ItemCollection Collection
		{
			get
			{
				return this._Collection;
			}
		}
	}

	public class Item : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private string _FirstName = string.Empty;

		public string FirstName
		{
			get
			{
				return this._FirstName;
			}

			set
			{
				if (this._FirstName != value)
				{
					this._FirstName = value;
					this.OnPropertyChanged("FirstName");
				}
			}
		}

		private string _LastName = string.Empty;

		public string LastName
		{
			get
			{
				return this._LastName;
			}

			set
			{
				if (this._LastName != value)
				{
					this._LastName = value;
					this.OnPropertyChanged("LastName");
				}
			}
		}

		private string _DateOfBirth = string.Empty;

		public string DateOfBirth
		{
			get
			{
				return this._DateOfBirth;
			}

			set
			{
				if (this._DateOfBirth != value)
				{
					this._DateOfBirth = value;
					this.OnPropertyChanged("DateOfBirth");
				}
			}
		}

		private string _DateOfDeath = string.Empty;

		public string DateOfDeath
		{
			get
			{
				return this._DateOfDeath;
			}

			set
			{
				if (this._DateOfDeath != value)
				{
					this._DateOfDeath = value;
					this.OnPropertyChanged("DateOfDeath");
				}
			}
		}

		private string _PlaceOfBirth = string.Empty;

		public string PlaceOfBirth
		{
			get
			{
				return this._PlaceOfBirth;
			}

			set
			{
				if (this._PlaceOfBirth != value)
				{
					this._PlaceOfBirth = value;
					this.OnPropertyChanged("PlaceOfBirth");
				}
			}
		}
	}

	public class ItemCollection : System.Collections.ObjectModel.ObservableCollection<Item>
	{ 
	}
#endif
}
