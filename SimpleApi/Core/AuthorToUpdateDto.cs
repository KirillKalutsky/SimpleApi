using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleApi.Core
{
    public class AuthorToUpdateDto
    {
        public DateTime? DateOfDeath { get; set; }
        public string Surname { get; set; }
    }
}
