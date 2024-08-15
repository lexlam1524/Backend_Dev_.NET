using System;
using System.Collections.Generic;

namespace robot_controller_api.Models
{
    public  class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Passwordhash { get; set; } = null!;
        public string? Description { get; set; }
        public string? Role { get; set; }
        public DateTime Createddate { get; set; }
        public DateTime Modifieddate { get; set; }
        public User()
        {
            
        }   
        public User(int Id,string Email,string Firstname,string Lastname,string Passwordhash,string? Description,string? Role,DateTime Createddate,DateTime Modifieddate)
        {
            this.Id = Id;
            this.Email = Email;
            this.Firstname = Firstname;
            this.Lastname = Lastname;
            this.Passwordhash = Passwordhash;
            this.Description = Description;
            this.Role = Role;
            this.Createddate = Createddate;
            this.Modifieddate = Modifieddate;
        }
    }
    
}
