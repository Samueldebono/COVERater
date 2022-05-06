using COVERater.API.Entities;
using COVERater.API.Models;
using COVERater.API.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace COVERater.API.Services
{
    public class CoveraterRepository : ICoveraterRepository, IDisposable
    {
        private readonly CoveraterContext _context; 

        public CoveraterRepository(CoveraterContext context, IPropertyMappingService propertyMappingService, IOptions<AppSettings> appSettings )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
           
        }

        #region Role
        public AuthUsers Authenticate(string username/*, string password*/)
        {
            var user = _context.AuthUsers.SingleOrDefault(x => x.UserName == username /*&& x.Password == password*/);

            if (user == null)
                throw new ArgumentNullException(nameof(username));

            return user;

        }


        #endregion

        #region User


        public IEnumerable<User> GetUsers()
        {
            var query = _context.User.Include(x => x.Guesses) as IQueryable<User>;
            
            return query.ToList<User>();
        }        
        public User GetUser(Guid userId)
        {
            var user = _context.User.Include(x => x.Guesses).FirstOrDefault(x=>x.HashUser == userId) ;
            
            if(user == null)
                throw new ArgumentNullException(nameof(userId));

            return user;
        }
       
        public User GetUser(int userId)
        {
            var user = _context.User.Include(x => x.Guesses).FirstOrDefault(x=>x.UserId == userId);
            
            if(user == null)
                throw new ArgumentNullException(nameof(userId));


            return user;
        }


        public User CreateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var newUser = _context.User.Add(user);
            return newUser.Entity;
        }

        public void UpdateUser(User user)
        {
           // throw new NotImplementedException();
        }

        public bool UserExists(int userId)
        {
            if (userId == 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return _context.User.Any(x => x.UserId == userId);
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
        public IEnumerable<UsersGuess> GetUserGuesses()
        {
            var query = _context.UsersGuess.Include(x=>x.Image) as IQueryable<UsersGuess>;
            return query.ToList<UsersGuess>();
        }

        public UsersGuess CreateUserGuess(UsersGuess usersGuess)
        {
            if (usersGuess == null)
            {
                throw new ArgumentNullException(nameof(usersGuess));
            }
          var user =  _context.UsersGuess.Add(usersGuess);
          return user.Entity;
        }

        public void UpdateUserGuess(UsersGuess usersGuess)
        {
          
        }




        #endregion

        #region Image

        public IEnumerable<Image> GetImages()
        {
            var query = _context.Image as IQueryable<Image>;

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
