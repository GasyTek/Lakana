using System;
using System.Collections.Generic;
using System.Linq;

namespace Samples.GasyTek.Lakana.Model
{
    public static class Database
    {
        private static List<Contact> _contacts;

        public static List<Contact> GetContacts()
        {
            if (_contacts == null)
            {
                _contacts = new List<Contact>
                                {
                                    new Contact
                                        {
                                            Id = 1,
                                            FirstName = "Friedrich",
                                            LastName = "Nietzsche",
                                            Sex = Sex.Male,
                                            DateOfBirth = new DateTime(1844, 10, 15),
                                            DateOfDeath = new DateTime(1900, 8, 25),
                                            PlaceOfBirth = "Röcken"
                                        },
                                    new Contact
                                        {
                                            Id = 2,
                                            FirstName = "Oscar",
                                            LastName = "Wilde",
                                            Sex = Sex.Male,
                                            DateOfBirth = new DateTime(1854, 10, 16),
                                            DateOfDeath = new DateTime(1900, 11, 30),
                                            PlaceOfBirth = "Dublin"
                                        },
                                    new Contact
                                        {
                                            Id = 3,
                                            FirstName = "Ernest",
                                            LastName = "Hemingway",
                                            Sex = Sex.Male,
                                            DateOfBirth = new DateTime(1899, 07, 21),
                                            DateOfDeath = new DateTime(1961, 07, 2),
                                            PlaceOfBirth = "Oak Park"
                                        }
                                };
            }

            return _contacts;
        }

        public static Contact GetContactById(int id)
        {
            return _contacts.FirstOrDefault(c => c.Id == id);
        }
    }
}
