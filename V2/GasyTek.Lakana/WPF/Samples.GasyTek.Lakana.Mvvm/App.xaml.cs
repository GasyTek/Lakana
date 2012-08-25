using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Windows;
using GasyTek.Lakana.Mvvm.Validation;

namespace Samples.GasyTek.Lakana.Mvvm
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private CompositionContainer _container;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // TODO : MEF doesn't work yet
            //var catalog = new AggregateCatalog();
            //catalog.Catalogs.Add(new AssemblyCatalog(typeof(App).Assembly));
            //catalog.Catalogs.Add(new AssemblyCatalog(typeof(IValidationEngine).Assembly));

            //_container = new CompositionContainer(catalog);
            //_container.ComposeParts();
        }
    }
}
