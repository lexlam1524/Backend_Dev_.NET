using System;
using System.Collections.Generic;

namespace robot_controller_api.Models
{
    public  class RobotCommand
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsMovecommand { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public RobotCommand()
        {
            
        }   
        public RobotCommand(int Id,string Name,string? Description,bool IsMovecommand,DateTime CreatedDate,DateTime ModifiedDate)
        {
            this.Id = Id;
            this.Name = Name;
            this.Description = Description;
            this.IsMovecommand = IsMovecommand;
            this.CreatedDate = CreatedDate;
            this.ModifiedDate = ModifiedDate;
        }
    }
    
}
