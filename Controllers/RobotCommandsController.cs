using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Models;
using robot_controller_api.Persistence;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace robot_controller_api.Controllers;

[ApiController]
[Route("api/robot-commands")]
public class RobotCommandsController : ControllerBase
{

    private readonly IRobotCommandDataAccess _robotCommandsRepo;
    public RobotCommandsController(IRobotCommandDataAccess robotCommandsRepo)
    {
        _robotCommandsRepo = robotCommandsRepo;
    }
    [HttpGet]
    public IEnumerable<Robotcommand> GetAllRobotCommands()
    {
        return _robotCommandsRepo.GetRobotCommands();
    }
    


    [HttpGet("move"), Authorize(Policy = "UserOnly")]
    public IEnumerable<Robotcommand> GetMoveCommandsOnly()
    {
        var moveCommands = GetAllRobotCommands().Where(c => c.Ismovecommand);
        return moveCommands;
    }

    [HttpGet("{id}", Name = "GetRobotCommand")]
    public IActionResult GetRobotCommandById(int id)
    {
        var robotCommand = GetAllRobotCommands().FirstOrDefault(c => c.Id == id);
        if(robotCommand == null)
        {
            return NotFound();
        }
        return Ok(robotCommand);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public IActionResult AddRobotCommand(Robotcommand newCommand)
    {
        if (newCommand == null)
        {
            return NotFound();
        }
        if (GetAllRobotCommands().Any(c => c.Name == newCommand.Name))
        {
            return Conflict();
        }

        Robotcommand command = new Robotcommand(
            Id: newCommand.Id,
            name: newCommand.Name,
            isMoveCommand: newCommand.Ismovecommand,
            modifiedDate: DateTime.Now,
            createdDate: DateTime.Now,
            description: newCommand.Description
            );
        _robotCommandsRepo.InsertRobotCommand(command);
        return CreatedAtRoute("GetRobotCommand", new { id = command.Id }, command);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateRobotCommand(int id, Robotcommand updatedCommand)
    {
        var robotCommand = GetAllRobotCommands().FirstOrDefault(c => c.Id == id);
        if (robotCommand == null)
        {
            return NotFound();
        }
        if (updatedCommand == null)
        {
            return BadRequest();
        }
        robotCommand.Name = updatedCommand.Name;
        robotCommand.Description = updatedCommand.Description;
        robotCommand.Ismovecommand = updatedCommand.Ismovecommand;
        robotCommand.Modifieddate = DateTime.Now;
        _robotCommandsRepo.UpdateRobotCommand(robotCommand);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteRobotCommand(int id)
    {
        var robotCommand = GetAllRobotCommands().FirstOrDefault(c => c.Id == id);
        if (robotCommand == null)
        {
            return NotFound();
        }
        _robotCommandsRepo.DeleteRobotCommand(id);
        return NoContent();
    }


}
