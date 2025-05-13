using RatingAPI.Model;

namespace RatingAPI.Interfaces
{
    public interface IUserService
    {
        string Authenticate(string username,string password);
        //Task<IEnumerable<User>> GetAll();
        //Task<User?> GetById(int id);
        //Task<User?> AddAndUpdateUser(User userObj);
    }
}