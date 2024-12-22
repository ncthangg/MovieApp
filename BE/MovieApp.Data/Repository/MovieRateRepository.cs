using Microsoft.EntityFrameworkCore;
using MovieApp.Data.DBContext;
using MovieApp.Data.Models;
using MovieApp.Data.Repository.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Data.Repository
{
    public class MovieRateRepository : GenericRepository<MovieRate>
    {
        public MovieRateRepository()
        {
        }
        public MovieRateRepository(MovieAppDBContext context) => _context = context;

    }
}
