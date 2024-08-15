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

    public List<Robotcommand> GetRobotCommands()
    {
        return _context.Robotcommands.ToList();
    }

    public void InsertRobotCommand(Robotcommand newCommand)
    {
        _context.Robotcommands.Add(newCommand);
        _context.SaveChanges();
    }

    public void UpdateRobotCommand(Robotcommand updatedCommand)
    {
        _context.Robotcommands.Update(updatedCommand);
        _context.SaveChanges();
    }

    public void DeleteRobotCommand(int id)
    {
        var command = _context.Robotcommands.Find(id);
        if (command != null)
        {
            _context.Robotcommands.Remove(command);
            _context.SaveChanges();
        }
    }
}