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
    public class UserTokenRepository : GenericRepository<UserToken>
    {
        public UserTokenRepository()
        {
        }
        public UserTokenRepository(MovieAppDBContext context) => _context = context;

        public async Task<UserToken> GetByUserIdAndToken(long userId, string refreshToken)
        {
            return await _context.Set<UserToken>()
                .AsNoTracking()
                .FirstOrDefaultAsync(token => token.UserId == userId && token.RefreshToken == refreshToken);
        }

    }
}
