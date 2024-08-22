using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using SQLite4Unity3d;
using System.IO;

public class MultiplayerBirdScript : MonoBehaviourPun, IPunObservable
{
    public Rigidbody2D birdbody;
    public float flapstr;
    public logicscript3 logic;
    public bool birdisalive = true;
    public float birdmax = 17;
    public float birdmin = -17;

    private Vector3 latestPos;
    private Quaternion latestRot;
    private SQLiteConnection _connection;
    private bool roundOver = false; // Flag to ensure one win per round

    void Start()
    {
        if (photonView.IsMine)
        {
            logic = GameObject.FindGameObjectWithTag("logic").GetComponent<logicscript3>();
            if (logic == null)
            {
                Debug.LogError("logicscript3 component not found!");
                return;
            }

            // Initialize database connection
            string dbPath = Path.Combine(Application.persistentDataPath, "GameDatabase.db");
            _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

            if (_connection == null)
            {
                Debug.LogError("Database connection could not be established.");
            }
            else
            {
                Debug.Log("Database connection established successfully.");
                // Create the user table if it doesn't exist
                _connection.CreateTable<User>();
            }
        }
        else
        {
            // Disable the rigidbody for the non-owned bird to prevent interference
            birdbody.isKinematic = true;
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space) && birdisalive)
            {
                birdbody.velocity = new Vector2(0, flapstr);
            }

            if (transform.position.y > birdmax || transform.position.y < birdmin)
            {
                HandleGameOver();
            }
        }
        else
        {
            // Smoothly move non-owned birds to their latest networked position
            transform.position = Vector3.Lerp(transform.position, latestPos, Time.deltaTime * 10);
            transform.rotation = Quaternion.Lerp(transform.rotation, latestRot, Time.deltaTime * 10);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (photonView.IsMine)
        {
            HandleGameOver();
        }
    }

    void HandleGameOver()
    {
        if (birdisalive && !roundOver)
        {
            birdisalive = false;
            logic.gameOver();

            roundOver = true; // Mark the round as over

            photonView.RPC("NotifyPlayerWon", RpcTarget.Others); // Notify other player to handle win
        }
    }

    [PunRPC]
    void NotifyPlayerWon()
    {
        StartCoroutine(DeclareWinAfterDelay(1));
    }

    IEnumerator DeclareWinAfterDelay(int delay)
    {
        yield return new WaitForSeconds(delay);

        if (!roundOver)
        {
            roundOver = true; // Mark the round as over

            logic.gameOver();
            birdisalive = false;

            // Check if the user is logged in
            string username = PlayerPrefs.GetString("LoggedInUser", null);
            Debug.Log($"Logged in as: {username}"); // Debug log

            if (!string.IsNullOrEmpty(username))
            {
                if (_connection == null)
                {
                    Debug.LogError("Database connection is not initialized.");
                    yield break;
                }

                var user = _connection.Table<User>().Where(u => u.UserName == username).FirstOrDefault();
                if (user != null)
                {
                    Debug.Log($"Current Multiplayer Score: {user.MultiplayerScore}"); // Debug log

                    // Update the multiplayer score in the database
                    user.MultiplayerScore += 1;
                    _connection.Update(user);

                    // Verify update
                    var updatedUser = _connection.Table<User>().Where(u => u.UserName == username).FirstOrDefault();
                    if (updatedUser != null)
                    {
                        Debug.Log($"Updated Multiplayer Score: {updatedUser.MultiplayerScore}"); // Debug log

                        // Update PlayerPrefs
                        PlayerPrefs.SetInt("MultiplayerScore", updatedUser.MultiplayerScore);
                        PlayerPrefs.Save();

                        Debug.Log($"PlayerPrefs Updated Multiplayer Score: {PlayerPrefs.GetInt("MultiplayerScore")}"); // Debug log
                    }
                    else
                    {
                        Debug.LogError("Failed to re-query user after update.");
                    }
                }
                else
                {
                    Debug.LogWarning("User not found in database."); // Debug log
                }
            }
            else
            {
                Debug.LogWarning("No user logged in."); // Debug log
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send data to other players
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(birdbody.velocity);
            stream.SendNext(birdisalive);
        }
        else
        {
            // Receive data from other players
            latestPos = (Vector3)stream.ReceiveNext();
            latestRot = (Quaternion)stream.ReceiveNext();
            birdbody.velocity = (Vector2)stream.ReceiveNext();
            birdisalive = (bool)stream.ReceiveNext();
        }
    }
}
