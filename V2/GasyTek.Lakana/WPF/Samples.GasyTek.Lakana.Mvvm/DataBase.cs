using System.Collections.Generic;
using System.Linq;

namespace Samples.GasyTek.Lakana.Mvvm
{
    public static class Database
    {
        /// <summary>
        /// Countries provider.
        /// </summary>
        private static List<Country> _countries;
        public static List<Country> GetCountriesLookupValues()
        {
            return _countries ?? (_countries = new List<Country>
                                                   {
                                                       new Country {Id = 1, Name = "France"},
                                                       new Country {Id = 2, Name = "USA"},
                                                       new Country {Id = 3, Name = "China"},
                                                       new Country {Id = 4, Name = "Madagascar"},
                                                   });
        }

        public static Country GetCountry(int idCountry)
        {
            return GetCountriesLookupValues().FirstOrDefault(c => c.Id == idCountry);
        }
    }
}