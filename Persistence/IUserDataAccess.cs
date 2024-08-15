using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public interface IUserDataAccess
    {
        void DeleteUser(int id);
        List<UserModel> GetUsers();
        UserModel GetUserByEmail(string email);
        void InsertUser(UserModel user);
        void UpdateUser(UserModel user);


    }
}