using System;
using System.Collections.Generic;

namespace robot_controller_api.Models
{
    public  class Map
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? Columns { get; set; }
        public int? Rows { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Map()
        {
            
        }   
        public Map(int Id,string Name,string? Description,int? Columns,int? Rows,DateTime CreatedDate,DateTime ModifiedDate)
        {
            this.Id = Id;
            this.Name = Name;
            this.Description = Description;
            this.Columns = Columns;
            this.Rows = Rows;
            this.CreatedDate = CreatedDate;
            this.ModifiedDate = ModifiedDate;
        }
    }
    
}
