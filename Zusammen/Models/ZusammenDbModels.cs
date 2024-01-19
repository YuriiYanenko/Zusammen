using System;

namespace Zusammen.Models;

// Rooms table object model. 
public class rooms
{
    public int id { get; set; }
    public string name { get; set; }
    public int admin_id { get; set; }
    public int[] members_id { get; set; }
    public int film_id { get; set; }
}

// Films table object model.
public class films
{
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

public class RoomAndFilm
{
    public rooms Room { get; set; }
    public string FilmPath { get; set; }
}