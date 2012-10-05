using GasyTek.Lakana.Common.UI;
using GasyTek.Lakana.Navigation.Services;

namespace Samples.GasyTek.Lakana.Screens
{
	/// <summary>
	/// Logique d'interaction pour HomeView.xaml
	/// </summary>
	public partial class HomeView : IPresentable
	{
	    private readonly IUIMetadata _uiMetadata;

		public HomeView()
		{
			InitializeComponent();

            _uiMetadata = new UIMetadata() { LabelProvider = () => "Home"};
		}

	    public IUIMetadata UIMetadata
	    {
            get { return _uiMetadata; }
	    }
	}
}