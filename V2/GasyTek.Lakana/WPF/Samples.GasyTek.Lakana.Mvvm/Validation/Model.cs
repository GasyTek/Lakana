using System;

namespace Samples.GasyTek.Lakana.Mvvm.Validation
{
    public class Model
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime BeginDistributionDate { get; set; }
        public DateTime EndDistributionDate { get; set; }
        public ProductCategory Category { get; set; }
    }

    public enum ProductCategory
    {
        Book,
        Dvd
    }
}