using System;

namespace Zusmmen.Models;

public class rooms
{
    public int Id { get; set; }
    public char[] Name { get; set; }
    public int AdminId { get; set; }
    public int[] MembersId { get; set; }
    public int[] FilmsId { get; set; }
}

public class films
{
    public int id { get; set; }
    public string name { get; set; }
    public string genre { get; set; }
    public string director { get; set; }
    public string actors { get; set; }
    public string description { get; set; }
    public string tags { get; set; }
    public string path { get; set; }
    public int year { get; set; }
}