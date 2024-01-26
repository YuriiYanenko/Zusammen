using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Zusammen.Models;

// Rooms table object model.
[Table("rooms")]
public class rooms
{
    [Key]
    public int id { get; set; }
    public string name { get; set; }
    public int admin_id { get; set; }
    public int[] members_id { get; set; }

    public int film_id { get; set; }
}

// Films table object model.
[Table("films")]
public class films
{
    [Key]
    public int id { get; set; }
    public string name { get; set; }
    public string[] genre { get; set; }
    public string[] director { get; set; }
    public string[] actors { get; set; }
    public string description { get; set; }
    public int time { get; set; }
    public string video_path { get; set; }
    public string image_path { get; set; }
    public int year { get; set; }
    public string voice { get; set; }
    public string[] country { get; set; }
    public int quality { get; set; }
}

[Table("users")]
public class users
{
    public string nickname { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    
    public List<int>? rooms { get; set; }
    public string profile_description { get; set; }
    public string profile_image_path { get; set; }
    public string status { get; set; }
    [Key]
    public int id { get; set; }

    public users()
    {
        rooms = new List<int>();
    }
}

public class RoomAndFilm
{
    public rooms Room { get; set; }
    public string FilmPath { get; set; }
}

public class RegisterModel
{
    [Required]
    [StringLength(15, MinimumLength = 3)]
    public string name { get; set; }
    
    [Required]
    [EmailAddress]
    public string email { get; set; }
    
    [Required]
    [StringLength(10, MinimumLength = 6)]
    public string password { get; set; }
}

public class LoginModel
{
    public string name { get; set; }
    public string password { get; set; }
}

public class RegAndLogModel
{
    public RegisterModel register { get; set; }
    public LoginModel login { get; set; }
}