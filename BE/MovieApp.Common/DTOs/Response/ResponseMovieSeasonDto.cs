using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Common.DTOs.Response
{
    public class ResponseMovieSeasonDto
    {
        public long SeasonId { get; set; }

        public long MovieId { get; set; }

        public string SeasonName { get; set; }

        public string PosterUrl { get; set; }

        public int ReleaseYear { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }
    }
}
