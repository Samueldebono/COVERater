using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using COVERater.API.Entities;
using COVERater.API.Models;
using COVERater.API.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;

namespace COVERater.API.Services
{
    public class CoveraterRepository : ICoveraterRepository, IDisposable
    {
        private readonly CoveraterContext _context;

        public CoveraterRepository(CoveraterContext context, IOptions<AppSettings> appSettings)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }

        #region Role

        public AuthUsers CreateAuthUsers(AuthUsers authUsers)
        {
            if (authUsers == null)
            {
                throw new ArgumentNullException(nameof(authUsers));
            }

            var newAuthUser = _context.AuthUsers.Add(authUsers);
            _context.SaveChanges();
            return newAuthUser.Entity;
        }


        public AuthUsers? AuthUsers(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            var authUsers = _context.AuthUsers.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());

            return authUsers;
        }

        public AuthUsers GetAuthUsers(int roleId)
        {

            var authUsers = _context.AuthUsers.Include(x => x.UserStats)
                .ThenInclude(x => x.Guesses)
                .ThenInclude(x => x.SubImage)
                 .FirstOrDefault(x => x.RoleId == roleId);

            return authUsers;
        }
        public IEnumerable<AuthUsers> GetAuthUsers()
        {

            var authUsers = _context.AuthUsers.Include(x => x.UserStats)
                .ThenInclude(x => x.Guesses)
                .ThenInclude(x => x.SubImage).ToList();

            
            return authUsers;
        }

        #endregion

        #region User

        public UserStats GetUser(int userId)
        {
            var user = _context.UserStats
                .Include(x => x.Guesses)
                .FirstOrDefault(x => x.UserId == userId);

            if (user == null)
                throw new ArgumentNullException(nameof(userId));


            return user;
        }

        #region UserGuess
        public async Task<IEnumerable<UsersGuess>> GetUserGuesses()
        {
            var query = _context.UsersGuess.Include(x => x.SubImage) as IQueryable<UsersGuess>;
            return query.ToList<UsersGuess>();
        }

        public async Task<UsersGuess> CreateUserGuess(UsersGuess usersGuess)
        {
            if (usersGuess == null)
            {
                throw new ArgumentNullException(nameof(usersGuess));
            }

            try
            {
                var user = _context.UsersGuess.Add(usersGuess);
                await Save();
                return user.Entity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        #endregion

        #region Image

        public IEnumerable<Image> GetImages()
        {
            var query = _context.Image.Include(x => x.SubImages) as IQueryable<Image>;

            return query.ToList<Image>();
        }

        public Image GetImage(int imageId)
        {
            var query = _context.Image.Find(imageId);

            if (query == null)
                throw new ArgumentNullException(nameof(imageId));

            return query;
        }

        #endregion


        public Token CreateToken(Token token)
        {
            var newToken = _context.Tokens.Add(token);
            return newToken.Entity;
            //return token;
        }

        public Token GetToken(int id)
        {
            var newToken = _context.Tokens.Where(x => x.TokenId == id);
            return newToken.FirstOrDefault();
            //return token;
        }


        #endregion


        public async Task<VisitCounter> GetUpdateVisitCount()
        {
            var counter = _context.VisitCount.FirstOrDefault();
            if (counter != null)
            {
                counter.Count += 1;
            }
            else
            {
                counter = new VisitCounter
                {
                    Count = 1
                };
                _context.VisitCount.Add(counter);
            }

            await Save();
            return counter;
        }


        public async Task<bool> Save()
        {
            return (_context.SaveChanges() >= 0);
        }


        public async Task<bool> Log(Log log)
        {
            var results = _context.Logs.Add(log);
            _context.SaveChanges();
            return true;

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }
    }
}
