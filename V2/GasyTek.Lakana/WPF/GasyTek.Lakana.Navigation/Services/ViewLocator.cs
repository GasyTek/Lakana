using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GasyTek.Lakana.Navigation.Attributes;

namespace GasyTek.Lakana.Navigation.Services
{
    /// <summary>
    /// Default implementation of <see cref="IViewLocator"/>
    /// </summary>
    internal class ViewLocator : IViewLocator
    {
        #region Fields

        private readonly Dictionary<string, Type> _viewMappings;

        #endregion

        #region Properties

        #endregion

        #region Constructor

        internal ViewLocator()
        {
            _viewMappings = new Dictionary<string, Type>();
        }

        #endregion

        public void RegisterApplicationViews()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var mappings = (from assembly in assemblies
                            from type in assembly.GetTypes()
                            from attr in type.GetCustomAttributes(typeof(ViewKeyAttribute), false).Cast<ViewKeyAttribute>()
                            select new Mapping  { ViewType = type, ViewAttribute = attr }).ToList();

            EnsuresAllKeysAreNotNullOrEmpty(mappings);

            EnsuresEachKeyIsUnique(mappings);

            foreach (var map in mappings)
            {
                _viewMappings.Add(map.ViewAttribute.UniqueKey, map.ViewType);
            }
        }

        public FrameworkElement GetViewInstance(string viewKey)
        {
            if (!_viewMappings.ContainsKey(viewKey))
                throw new ViewKeyNotFoundException(viewKey);

            var viewType = _viewMappings[viewKey];
            return Activator.CreateInstance(viewType) as FrameworkElement;
        }

        private void EnsuresAllKeysAreNotNullOrEmpty(IEnumerable<Mapping> mappings )
        {
            var nullKey = mappings.FirstOrDefault(mapping => string.IsNullOrEmpty(mapping.ViewAttribute.UniqueKey));
            if (nullKey != null)
                throw new ViewKeyNullOrEmptyException(nullKey.ViewType.FullName);
        }

        private void EnsuresEachKeyIsUnique(IEnumerable<Mapping> mappings)
        {
            var uniquekeyGroups = (from mapping in mappings
                                   group mapping by mapping.ViewAttribute.UniqueKey
                                      into g
                                      select new
                                      {
                                          NbKey = g.Count(),
                                          g.First().ViewAttribute.UniqueKey,
                                          ViewTypeNames = g.Select(u => u.ViewType.FullName)
                                      }).ToList();
            var duplicated = uniquekeyGroups.FirstOrDefault(ug => ug.NbKey > 1);
            if (duplicated != null)
            {
                throw new ViewKeyDuplicationException(duplicated.UniqueKey, duplicated.ViewTypeNames);
            }
        }

        #region Private class

        private class Mapping
        {
            public Type ViewType { get; set; }
            public ViewKeyAttribute ViewAttribute { get; set; }
        }

        #endregion
    }
}