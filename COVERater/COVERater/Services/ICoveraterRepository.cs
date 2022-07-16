using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using COVERater.API.Models;

namespace COVERater.API.Services
{
    public interface ICoveraterRepository
    {

        IEnumerable<UserStats> GetUsers();
        UserStats GetUser(Guid userId);
        UserStats GetUser(int userId);
        UserStats CreateUser(UserStats user);
        void UpdateUser(UserStats user);
        bool UserExists(int userId);


        IEnumerable<UserEmails> GetUserEmails();
        UserEmails CreateUserEmail(UserEmails userEmail);


        #region UserGuess

        Task<IEnumerable<UsersGuess>> GetUserGuesses();
        UsersGuess CreateUserGuess(UsersGuess usersGuess);
        void UpdateUserGuess(UsersGuess usersGuess);

        #endregion



        #region Image

        IEnumerable<Image> GetImages();
        Image GetImage(int imageId);
        void AddImage(Image image);
        void UpdateImage(Image image);
        //bool AthleteExists(int memberId);

        #endregion

        #region Roles

        AuthUsers Authenticate(string userName);
        AuthUsers CreateAuthUsers(AuthUsers authUsers);
        AuthUsers ResetPassword(AuthUsers authUsers);
        AuthUsers? AuthUsers(string email);
        AuthUsers GetAuthUsers(int roleId);

        

        Token CreateToken(Token token);
        Token GetToken(int tokenId);
        void UpdateToken(Token token);
        #endregion



        bool Save();
    }
}
