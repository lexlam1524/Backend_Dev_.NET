using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public interface IRobotCommandDataAccess
    {
        void DeleteRobotCommand(int id);
        List<Robotcommand> GetRobotCommands();
        void InsertRobotCommand(Robotcommand command);
        void UpdateRobotCommand(Robotcommand command);
    }
}