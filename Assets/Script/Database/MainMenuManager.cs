using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Text usernameText;
    public Text singlePlayerScoreText;
    public Button logoutButton;
    public Text feedbackText; // Add a Text element to display feedback messages

    void Start()
    {
        // Retrieve the logged-in user's information
        string username = PlayerPrefs.GetString("LoggedInUser", null);
        int singlePlayerHighScore = PlayerPrefs.GetInt("SinglePlayerHighScore", 0);

        // Display the information in the UI if the user is logged in
        if (!string.IsNullOrEmpty(username))
        {
            usernameText.text = $"Username: {username}";
            singlePlayerScoreText.text = $"Single Player High Score: {singlePlayerHighScore}";
        }
        else
        {
            usernameText.text = "";
            singlePlayerScoreText.text = "";
        }

        logoutButton.onClick.AddListener(LogoutUser);
    }

    public void Loading()
    {
        string username = PlayerPrefs.GetString("LoggedInUser", null);

        if (!string.IsNullOrEmpty(username))
        {
            SceneManager.LoadSceneAsync(2);
        }
        else
        {
            feedbackText.text = "Multiplayer requires login. Please log in to proceed.";
            Debug.Log("Multiplayer requires login. Please log in to proceed.");
        }
    }

    void LogoutUser()
    {
        PlayerPrefs.DeleteKey("LoggedInUser");
        PlayerPrefs.DeleteKey("SinglePlayerHighScore");
        Debug.Log("User logged out");
        SceneManager.LoadScene("LoginScene"); // Load the login scene
    }
}
