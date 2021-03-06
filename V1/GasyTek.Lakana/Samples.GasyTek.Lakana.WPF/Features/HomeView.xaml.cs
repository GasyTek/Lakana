﻿using GasyTek.Lakana.WPF.Common;
using GasyTek.Lakana.WPF.Services;

namespace Samples.GasyTek.Lakana.WPF.Features
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : IPresentable
    {
        private readonly IUIMetadata _uiMetadata;

        public HomeView()
        {
            InitializeComponent();

            _uiMetadata = new UIMetadata {LabelProvider = () => "Home"};
        }

        public IUIMetadata UIMetadata
        {
            get { return _uiMetadata; }
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            contentRun.Text = GetDescription();
        }

        public string GetDescription()
        {
            return "This demo application will show you the main features offered by Lakana.\r\n" +
                   "It covers the following : \r\n" +
                   "\r\n > Animated transition between views" +
                   "\r\n > MDI-like navigation" +
                   "\r\n > Modal windows" +
                   "\r\n > Message boxes" +
                   "\r\n > Application shutdown management" +
                   "\r\n > Use of MVVM and non-MVVM screens" +
                   "\r\n\r\n Project website : http://lakana.codeplex.com " +
                   "\r\n My blog (in french) : http://gasytek.wordpress.com " +
                   "\r\n Twitter : @gasytek, #lakana" +
                   "\r\n Mail : gasytek@gmail.com";
        }
    }
}
