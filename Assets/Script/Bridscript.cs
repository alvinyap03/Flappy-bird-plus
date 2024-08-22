using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System.IO;

public class Bridscript : MonoBehaviour
{
    public Rigidbody2D birdbody;
    public float flapstr;
    public logicscript logic;
    public bool birdisalive = true;
    public float birdmax = 17;
    public float birdmin = -17;

    private SQLiteConnection _connection;

    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("logic").GetComponent<logicscript>();

        // Initialize database connection
        string dbPath = Path.Combine(Application.persistentDataPath, "GameDatabase.db");
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

        // Create the user table if it doesn't exist
        _connection.CreateTable<User>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) == true && birdisalive == true)
        {
            birdbody.velocity = new Vector2(0, flapstr);
        }

        if (transform.position.y > birdmax || transform.position.y < birdmin)
        {
            HandleGameOver();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleGameOver();
    }

    void HandleGameOver()
    {
        if (birdisalive)
        {
            logic.gameOver();
            birdisalive = false;

            // Check if the user is logged in
            string username = PlayerPrefs.GetString("LoggedInUser", null);
            if (!string.IsNullOrEmpty(username))
            {
                int currentScore = logic.GetCurrentScore(); // Assuming this method returns the current game score
                var user = _connection.Table<User>().Where(u => u.UserName == username).FirstOrDefault();

                if (user != null && currentScore > user.SinglePlayerHighScore)
                {
                    // Update the high score in the database
                    user.SinglePlayerHighScore = currentScore;
                    _connection.Update(user);

                    // Update PlayerPrefs
                    PlayerPrefs.SetInt("SinglePlayerHighScore", currentScore);
                    PlayerPrefs.Save();
                }
            }
        }
    }
}
