using Microsoft.EntityFrameworkCore;
using robot_controller_api.Models;
using robot_controller_api.Persistence;

public class RobotCommandEF : IRobotCommandDataAccess
{
    private readonly RobotContext _context;

    public RobotCommandEF(RobotContext context)
    {
        _context = context;
    }

    public List<RobotCommand> GetRobotCommands()
    {
        return _context.RobotCommands.ToList();
    }

    public void InsertRobotCommand(RobotCommand newCommand)
    {
        _context.RobotCommands.Add(newCommand);
        _context.SaveChanges();
    }

    public void UpdateRobotCommand(RobotCommand updatedCommand)
    {
        _context.RobotCommands.Update(updatedCommand);
        _context.SaveChanges();
    }

    public void DeleteRobotCommand(int id)
    {
        var command = _context.RobotCommands.Find(id);
        if (command != null)
        {
            _context.RobotCommands.Remove(command);
            _context.SaveChanges();
        }
    }
}