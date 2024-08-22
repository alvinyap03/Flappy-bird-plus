using UnityEngine;
using UnityEngine.UI;
using SQLite4Unity3d;
using System.IO;
using MyGameNamespace; // Add this line to include the namespace
using TMPro;
using UnityEngine.SceneManagement;

public class AccountManager : MonoBehaviour
{
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;
    public Button registerButton;
    public Button loginButton;
    public Text feedbackText; // Add this line to reference the feedback text

    private SQLiteConnection _connection;

    void Start()
    {
        string dbPath = Path.Combine(Application.persistentDataPath, "GameDatabase.db");
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

        // Create the user table if it doesn't exist
        _connection.CreateTable<User>();

        registerButton.onClick.AddListener(RegisterUser);
        loginButton.onClick.AddListener(LoginUser);
    }

    void RegisterUser()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            // Check if the user already exists
            var existingUser = _connection.Table<User>().Where(u => u.UserName == username).FirstOrDefault();
            if (existingUser == null)
            {
                _connection.Insert(new User() { UserName = username, Password = password, SinglePlayerHighScore = 0, MultiplayerScore = 0 });
                Debug.Log("User registered successfully");
                feedbackText.text = "Account created successfully!";
            }
            else
            {
                Debug.Log("User already exists");
                feedbackText.text = "User already exists";
            }
        }
        else
        {
            Debug.Log("Username or password cannot be empty");
            feedbackText.text = "Username or password cannot be empty";
        }
    }

    void LoginUser()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            var user = _connection.Table<User>().Where(u => u.UserName == username && u.Password == password).FirstOrDefault();
            if (user != null)
            {
                Debug.Log("Login successful");
                feedbackText.text = "Login successful!";

                // Store the logged-in user's information
                PlayerPrefs.SetString("LoggedInUser", username);
                PlayerPrefs.SetInt("SinglePlayerHighScore", user.SinglePlayerHighScore);
                PlayerPrefs.SetInt("MultiplayerScore", user.MultiplayerScore);
                PlayerPrefs.Save();

                SceneManager.LoadScene(0); // Load the main menu scene
            }
            else
            {
                Debug.Log("Invalid username or password");
                feedbackText.text = "Invalid username or password";
            }
        }
        else
        {
            Debug.Log("Username or password cannot be empty");
            feedbackText.text = "Username or password cannot be empty";
        }
    }
}

public class User
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public int SinglePlayerHighScore { get; set; }
    public int MultiplayerScore { get; set; }
}
