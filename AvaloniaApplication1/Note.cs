using SQLite;
using System;

namespace notes5;

public class Note
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime EditedAt { get; set; }

    public Note() { }

    public Note(string text)
    {
        Text = text;
        CreatedAt = DateTime.Now;
        EditedAt = DateTime.Now;
    }
    
}