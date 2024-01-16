using System;

namespace Zusmmen.Models;

public class Rooms
{
    public int Id { get; set; }
    public char[] Name { get; set; }
    public int AdminId { get; set; }
    public int[] MembersId { get; set; }
    public int[] FilmsId { get; set; }
}

public class Films
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Genre { get; set; }
    public string Description { get; set; }
    public string Director { get; set; }
    public string Actors { get; set; }
    public string Tags { get; set; }
    public int Year { get; set; }
    public string Path { get; set; }
}