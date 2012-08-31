using GasyTek.Lakana.Common.Attributes;

namespace Samples.GasyTek.Lakana.Mvvm
{
    public class Employee
    {
        public string Code { get; set; }
        public int Age { get; set; }
        public Rank Rank { get; set; }
        public int CountryId { get; set; }
    }

    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    // HOW TO : define a resource identifier that contains the user friendly (and localized) text
    // that corresponds to the enum member.
    public enum Rank
    {
        [LocalizationEnum("Boss_ResId")]
        Boss,

        [LocalizationEnum("Trainee_ResId")]
        Trainee
    }
}
