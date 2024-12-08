using System;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace notes5;

public class Database
{
    private SQLiteConnection _connection;

    public Database(string dbPath)
    {
        _connection = new SQLiteConnection(dbPath);
        _connection.CreateTable<Note>(); // Создание таблицы, если ее нет
    }

    public Task<List<Note>> GetItemsAsync()
    {
        return Task.Run(() => 
            _connection.Table<Note>()
                .OrderByDescending(note => note.EditedAt)
                .ToList()
        );
    }

    public void AddItem(Note item)
    {
        _connection.Insert(item);
    }
    public void UpdateItem(int id, string newText)
    {
        var item = _connection.Find<Note>(id);
        if (item != null)
        {
            item.Text = newText; // Предполагается, что у класса Note есть свойство Name
            item.EditedAt = DateTime.Now;
            _connection.Update(item);
        }
    }
    public void DeleteItem(int id)
    {
        var item = _connection.Find<Note>(id);
        if (item != null)
        {
            _connection.Delete(item);
        }
    }


    
}