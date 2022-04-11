using System;

namespace SimpleApi.Core
{
    public class Person
    {
        public DateTime DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }

        public Person(DateTime dateOfBirth, string name, string surname, string patronymic)
        {
            DateOfBirth = dateOfBirth;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
        }
    }
}