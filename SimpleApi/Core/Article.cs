using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleApi.Core
{
    public class Article
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public List<Author> Creators { get; set; }
        public string Content { get; set; }

        public Article()
        {
            Id = Guid.NewGuid();
            Creators = new List<Author>();
        }
    }
}
