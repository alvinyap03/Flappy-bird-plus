using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System.IO;
using System.Linq;
using MyGameNamespace;

public class DatabaseManager : MonoBehaviour
{
    private SQLiteConnection _connection;

    void Start()
    {
        string dbPath = Path.Combine(Application.persistentDataPath, "GameDatabase.db");
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

        // Create a table if not exists
        _connection.CreateTable<User>();

        // Add sample data
        _connection.Insert(new User() { UserName = "player1", Password = "password123", SinglePlayerHighScore = 100, MultiplayerScore = 200 });

        // Read data
        var users = _connection.Table<User>().ToList();
        foreach (var user in users)
        {
            Debug.Log($"User: {user.UserName}, High Score: {user.SinglePlayerHighScore}, Multiplayer Score: {user.MultiplayerScore}");
        }
    }
}
