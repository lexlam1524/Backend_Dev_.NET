using System;
using System.Collections.Generic;

namespace robot_controller_api.Models;

public partial class Map
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? Columns { get; set; }

    public int? Rows { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Modifieddate { get; set; }

    public Map() { }

    public Map(int id, string name, int columns, int rows, DateTime createDate,
    DateTime modifiedDate, string? description = null)
    {
        Id = id;
        Name = name;
        Description = description;
        Columns = columns;
        Rows = rows;
        this.Createddate = createDate;
        this.Modifieddate = modifiedDate;
    }
    public bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < Columns && y >= 0 && y < Rows;
    }
}
