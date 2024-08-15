using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public interface IRobotCommandDataAccess
    {
        void DeleteRobotCommand(int id);
        List<RobotCommand> GetRobotCommands();
        void InsertRobotCommand(RobotCommand command);
        void UpdateRobotCommand(RobotCommand command);
    }
}