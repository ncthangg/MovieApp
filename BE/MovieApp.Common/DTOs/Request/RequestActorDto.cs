using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Common.DTOs.Request
{
    public class RequestActorDto
    {
        public string ActorName { get; set; }

        public DateOnly? BirthDate { get; set; }

        public string Bio { get; set; }
    }
}
