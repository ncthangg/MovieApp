using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Common.DTOs.Response
{
    public class ResponseMovieEpisodeDto
    {
        public long EpisodeId { get; set; }

        public long SeasonId { get; set; }

        public string EpisodeName { get; set; }

        public string Description { get; set; }

        public string VideoUrl { get; set; }

        public string PosterUrl { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }
    }
}
