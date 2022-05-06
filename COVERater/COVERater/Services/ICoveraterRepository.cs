using COVERater.API.Models;

namespace COVERater.API.Services
{
    public interface ICoveraterRepository
    {

        IEnumerable<User> GetUsers();
        User GetUser(Guid userId);
        User GetUser(int userId);
        User CreateUser(User user);
        void UpdateUser(User user);
        bool UserExists(int userId);


        IEnumerable<UserEmails> GetUserEmails();
        UserEmails CreateUserEmail(UserEmails userEmail);


        #region UserGuess

        IEnumerable<UsersGuess> GetUserGuesses();
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

        AuthUsers Authenticate(string userName/*, string Password*/);
        Token CreateToken(Token token);
        Token GetToken(int tokenId);
        void UpdateToken(Token token);
        #endregion



        bool Save();
    }
}
