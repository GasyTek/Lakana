using System;
using System.Collections.Generic;
using System.Linq;

namespace Samples.GasyTek.Lakana.Model
{
    /// <summary>
    /// A fake database
    /// </summary>
    public static class Database
    {
        private static List<Contact> _contacts;

        public static List<Contact> GetContacts()
        {
            return _contacts ?? (_contacts = new List<Contact>
                {
                    new Contact
                        {
                            Id = 1,
                            FirstName = "Friedrich",
                            LastName = "Nietzsche",
                            Sex = Sex.Male,
                            DateOfBirth = new DateTime(1844, 10, 15),
                            DateOfDeath = new DateTime(1900, 8, 25),
                            Email = "friedrich@philosopher.com"
                        },
                    new Contact
                        {
                            Id = 2,
                            FirstName = "Oscar",
                            LastName = "Wilde",
                            Sex = Sex.Male,
                            DateOfBirth = new DateTime(1854, 10, 16),
                            DateOfDeath = new DateTime(1900, 11, 30),
                            Email = "oscar@philosopher.net"
                        },
                    new Contact
                        {
                            Id = 3,
                            FirstName = "Ernest",
                            LastName = "Hemingway",
                            Sex = Sex.Male,
                            DateOfBirth = new DateTime(1899, 07, 21),
                            DateOfDeath = new DateTime(1961, 07, 2),
                            Email = "ernest@philosopher.com"
                        },
                    new Contact
                        {
                            Id = 3,
                            FirstName = "Arthur",
                            LastName = "Schopenhauer",
                            Sex = Sex.Male,
                            DateOfBirth = new DateTime(1788, 02, 22),
                            DateOfDeath = new DateTime(1860, 09, 21),
                            Email = "arthur@philosopher.co"
                        }
                });
        }

        public static Contact GetContactById(int id)
        {
            return _contacts.FirstOrDefault(c => c.Id == id);
        }
    }
}
