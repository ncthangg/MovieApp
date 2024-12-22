using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Common.DTOs.Request
{
    public class ResponseUserLikeDto
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long? MovieId { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
