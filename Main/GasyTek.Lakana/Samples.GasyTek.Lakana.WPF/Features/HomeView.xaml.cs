using GasyTek.Lakana.WPF.Common;
using GasyTek.Lakana.WPF.Services;

namespace Samples.GasyTek.Lakana.WPF.Features
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : IPresentable
    {
        private readonly IPresentationMetadata _presentationMetadata;

        public HomeView()
        {
            InitializeComponent();

            _presentationMetadata = new PresentationMetadata {LabelProvider = () => "Home"};
        }

        public IPresentationMetadata PresentationMetadata
        {
            get { return _presentationMetadata; }
        }
    }
}
