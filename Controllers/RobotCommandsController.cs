using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Models;
using robot_controller_api.Persistence;
using System;
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
    public IEnumerable<RobotCommand> GetAllRobotCommands()
    {
        return _robotCommandsRepo.GetRobotCommands();
    }
    


    [HttpGet("move")]
    public IEnumerable<RobotCommand> GetMoveCommandsOnly()
    {
        var moveCommands = GetAllRobotCommands().Where(c => c.IsMovecommand);
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
    [HttpPost]
    public IActionResult AddRobotCommand(RobotCommand newCommand)
    {
        if (newCommand == null)
        {
            return NotFound();
        }
        

        
        newCommand.ModifiedDate = DateTime.Now;
        newCommand.CreatedDate = DateTime.Now;
        _robotCommandsRepo.InsertRobotCommand(newCommand);
        return CreatedAtRoute("GetRobotCommand", new { id = newCommand.Id }, newCommand);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateRobotCommand(int id, RobotCommand updatedCommand)
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
        robotCommand.IsMovecommand = updatedCommand.IsMovecommand;
        robotCommand.ModifiedDate = DateTime.Now;
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
