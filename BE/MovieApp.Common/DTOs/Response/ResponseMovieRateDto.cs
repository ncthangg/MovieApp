using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Common.DTOs.Response
{
    public class ResponseMovieRateDto
    {
        public long Id { get; set; }

        public long MovieId { get; set; }

        public long UserId { get; set; }

        public long? Vote { get; set; }

        public string Comment { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
