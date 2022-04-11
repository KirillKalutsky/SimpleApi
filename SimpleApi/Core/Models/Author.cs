using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleApi.Core
{
    public class Author:Person
    {
        [Key]
        public Guid Id { get; set; }
        public List<Article> Works { get; set; }

        public Author(DateTime dateOfBirth, string name, string surname, string patronymic) 
            :base(dateOfBirth, name, surname, patronymic)
        {
            Id = Guid.NewGuid();
            Works = new List<Article>();
        }
    }
}