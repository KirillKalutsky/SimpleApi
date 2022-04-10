using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleApi.Core
{
    public class ArticleToCreateDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
