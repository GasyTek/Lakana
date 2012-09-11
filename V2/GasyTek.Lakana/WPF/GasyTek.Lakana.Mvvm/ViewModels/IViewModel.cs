using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GasyTek.Lakana.Common.UI;
using GasyTek.Lakana.Mvvm.Commands;
using GasyTek.Lakana.Mvvm.ViewModelProperties;

namespace GasyTek.Lakana.Mvvm.ViewModels
{
    /// <summary>
    /// Abstracts view models that supports view model properties.
    /// </summary>
    public interface IViewModel : IDisposable, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the UI metadata, meaning localized text, icons etc.
        /// </summary>
        /// <value>
        /// The UI metadata.
        /// </value>
        IUIMetadata UIMetadata { get; }

        /// <summary>
        /// Gets the registered properties. 
        /// Registered properties participates in automatically refreshing the <seealso cref="RegisteredCommands"/>
        /// </summary>
        ReadOnlyCollection<IViewModelProperty> RegisteredProperties { get; }

        /// <summary>
        /// Gets the registered commands. 
        /// Registered commands will be automatically refreshed when changes appear in <seealso cref="RegisteredProperties"/>
        /// </summary>
        ReadOnlyCollection<ISimpleCommand> RegisteredCommands { get; }
    }
}