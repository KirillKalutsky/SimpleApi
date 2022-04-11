using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleApi.Core
{
    public class ArticleDto
    {
        public string Title { get; set; }
        public IEnumerable<Guid> Creators { get; set; }
        public string Content { get; set; }
    }
}
