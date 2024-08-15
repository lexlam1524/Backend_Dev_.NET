using System;
using System.Collections.Generic;

namespace robot_controller_api.Models
{

    public partial class Robotcommand
    {
        public Robotcommand() { }


        public Robotcommand(int Id, string name, bool isMoveCommand, DateTime createdDate, DateTime modifiedDate, string? description)
        {
            this.Id = Id;
            this.Name = name;
            this.Ismovecommand = isMoveCommand;
            this.Createddate = createdDate;
            this.Modifieddate = modifiedDate;
            this.Description = description;
        }

        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public bool Ismovecommand { get; set; }

        public DateTime Createddate { get; set; }

        public DateTime Modifieddate { get; set; }
    }


}

