using System;
using System.Collections.Generic;
using System.Linq;
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

        public CoveraterRepository(CoveraterContext context,  IOptions<AppSettings> appSettings )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
           
        }

        #region Role
        public AuthUsers Authenticate(string username)
        {
            var user = _context.AuthUsers.Include(x=>x.UserStats).FirstOrDefault(x => x.UserName == username);

            if (user == null)
                throw new ArgumentNullException(nameof(username));
            
            return user;

        }

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

        public AuthUsers ResetPassword(AuthUsers authUsers)
        {
            if (authUsers == null)
            {
                throw new ArgumentNullException(nameof(authUsers));
            }

            var user = _context.AuthUsers.FirstOrDefaultAsync(x=>x.Email == authUsers.Email);

            if (user != null)
            {
                user.Result.Password = authUsers.Password;
                _context.SaveChanges();
            }
            
            return user.Result;
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
                .ThenInclude(x=>x.Guesses)
                .ThenInclude(x=>x.SubImage)
                 .FirstOrDefault(x => x.RoleId == roleId);
            
            return authUsers;
        }

        #endregion

        #region User


        public IEnumerable<UserStats> GetUsers()
        {
            var query = _context.UserStats.Include(x => x.Guesses) as IQueryable<UserStats>;
            
            return query.ToList<UserStats>();
        }        
        public UserStats GetUser(Guid userId)
        {
            //var user = _context.UserStats.Include(x => x.Guesses).FirstOrDefault(x=>x.UserId == userId) ;

            //if(user == null)
            //    throw new ArgumentNullException(nameof(userId));

            //return user;
            return new UserStats();
        }
       
        public UserStats GetUser(int userId)
        {
            var user = _context.UserStats
                .Include(x => x.Guesses)
                .FirstOrDefault(x=>x.UserId == userId);
            
            if(user == null)
                throw new ArgumentNullException(nameof(userId));


            return user;
        }

        public UserStats CreateUser(UserStats user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var newUser = _context.UserStats.Add(user);
            return newUser.Entity;
        }

        public void UpdateUser(UserStats user)
        {
           // throw new NotImplementedException();
        }

        public bool UserExists(int userId)
        {
            if (userId == 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return _context.UserStats.Any(x => x.UserId == userId);
        }

        public IEnumerable<UserEmails> GetUserEmails()
        {
            var query = _context.UserEmails as IQueryable<UserEmails>;

            return query.ToList<UserEmails>();
        }
        public UserEmails CreateUserEmail(UserEmails userEmail)
        {
            if (userEmail == null)
            {
                throw new ArgumentNullException(nameof(userEmail));
            }

            var newUser = _context.UserEmails.Add(userEmail);
            return newUser.Entity;
        }

        #region UserGuess
        public async Task<IEnumerable<UsersGuess>> GetUserGuesses()
        {
            var query = _context.UsersGuess.Include(x=>x.SubImage) as IQueryable<UsersGuess>;
            return query.ToList<UsersGuess>();
        }

        public UsersGuess CreateUserGuess(UsersGuess usersGuess)
        {
            if (usersGuess == null)
            {
                throw new ArgumentNullException(nameof(usersGuess));
            }

            try
            {
                var user = _context.UsersGuess.Add(usersGuess);
                return user.Entity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
          
        }

        public void UpdateUserGuess(UsersGuess usersGuess)
        {
          
        }


        #endregion

        #region Image

        public IEnumerable<Image> GetImages()
        {
            var query = _context.Image.Include(x=>x.SubImages) as IQueryable<Image>;

            return query.ToList<Image>();
        }

        public Image GetImage(int imageId)
        {
            var query = _context.Image.Find(imageId);

            if(query == null)
                throw new ArgumentNullException(nameof(imageId));

            return query;
        }

        public void AddImage(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }
            _context.Image.Add(image);
        }

        public void UpdateImage(Image image)
        {

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
            var newToken = _context.Tokens.Where(x=>x.TokenId == id);
            return newToken.FirstOrDefault();
            //return token;
        }

        public void UpdateToken(Token token)
        {

        }

        #endregion

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
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
