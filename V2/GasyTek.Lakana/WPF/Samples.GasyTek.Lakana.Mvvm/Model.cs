using System;
using GasyTek.Lakana.Common.UI;
using Samples.GasyTek.Lakana.Mvvm.Resources;

namespace Samples.GasyTek.Lakana.Mvvm
{
    public class Employee
    {
        public string Code { get; set; }
        public int Age { get; set; }
        public Rank Rank { get; set; }
        public Country Country { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfHire { get; set; }
        public DateTime DateOfDeath { get; set; }
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
        Boss,
        Trainee
    }

    public static class RankEnumUIMetadataProvider
    {
        public static UIMetadata GetUIMetadata(Rank rank)
        {
            var result = new UIMetadata();
            switch (rank)
            {
                case Rank.Boss:
                    result.LabelProvider = () => Labels.Boss;
                    break;
                case Rank.Trainee:
                    result.LabelProvider = () => Labels.Trainee;
                    break;
            }
            return result;
        }
    }
}
