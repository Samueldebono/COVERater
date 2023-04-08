using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using COVERater.API.Models;

namespace COVERater.API.Services
{
    public interface ICoveraterRepository
    {
        
        UserStats GetUser(int userId);

        #region UserGuess

        Task<IEnumerable<UsersGuess>> GetUserGuesses();
        Task<UsersGuess> CreateUserGuess(UsersGuess usersGuess);

        #endregion



        #region Image

        IEnumerable<Image> GetImages();
        Image GetImage(int imageId);

        #endregion

        #region Roles
        
        AuthUsers CreateAuthUsers(AuthUsers authUsers);
        AuthUsers? AuthUsers(string email);
        AuthUsers GetAuthUsers(int roleId);
        IEnumerable<AuthUsers> GetAuthUsers();

        

        Token CreateToken(Token token);
        Token GetToken(int tokenId);
        #endregion
        
        Task<VisitCounter> GetUpdateVisitCount();

        Task<bool> Save();
        Task<bool> Log(Log log);
    }
}
