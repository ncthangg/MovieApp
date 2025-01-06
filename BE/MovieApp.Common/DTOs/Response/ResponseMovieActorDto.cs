using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Common.DTOs.Response
{
    public class ResponseMovieActorDto
    {
        public long MovieId { get; set; }
        public IEnumerable<ResponseActorDto> Actor { get; set; }
    }

    public class ResponseActorMovieDto
    {
        public long ActorId { get; set; }
        public IEnumerable<ResponseMovieDto> Movie { get; set; }
    }
}
