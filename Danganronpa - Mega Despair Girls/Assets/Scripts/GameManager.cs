using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{

    public static GameManager Instance = null;

    AudioClip music;

    bool isGameOver;
    bool playerReady;
    bool initReadyScreen;

    int playerScore;

    float gameRestartTime;
    public float gamePlayerReadyTime;

    public float gameRestartDelay = 5f;
    public float gamePlayerReadyDelay = 2f;

    TextMeshProUGUI playerScoreText;
    TextMeshProUGUI screenMessageText;
    public GameObject playerPrefab;

    // Start is called before the first frame update

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerReady)
        {
            if (initReadyScreen)
            {
                //GameObject.FindGameObjectWithTag("Player").SetActive(true);
                Messenger.Broadcast<bool>("Freeze", true);
                screenMessageText.alignment = TextAlignmentOptions.Center;
                screenMessageText.alignment = TextAlignmentOptions.Top;
                screenMessageText.fontStyle = FontStyles.UpperCase;
                screenMessageText.text = "\nREADY";
                initReadyScreen = false;
            }
            gamePlayerReadyTime -= Time.deltaTime;
            if (gamePlayerReadyTime < 0)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                Messenger.Broadcast<bool>("Freeze", false);
                if (player == null)
                {
                    player = Instantiate(playerPrefab);
                    player.transform.position = new Vector2(0, Camera.main.transform.position.y + Camera.main.orthographicSize);
                }
                //FreezeEnemies(false);
                TeleportPlayer(true);
                screenMessageText.text = "";
                playerReady = false;
            }
            return;
        }

        if (playerScoreText != null)
        {
            playerScoreText.text = String.Format("<mspace=\"{0}\">{1:0000000}</mspace>", playerScoreText.fontSize, playerScore);
        }

        if (!isGameOver)
        {

        }
        else
        {
            gameRestartTime -= Time.deltaTime;
            if (gameRestartTime < 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartGame();
    }

    private void StartGame()
    {
        isGameOver = false;
        playerReady = true;
        initReadyScreen = true;
        gamePlayerReadyTime = gamePlayerReadyDelay;
        playerScoreText = GameObject.Find("PlayerScore").GetComponent<TextMeshProUGUI>();
        screenMessageText = GameObject.Find("ScreenMessage").GetComponent<TextMeshProUGUI>();
        SoundManager.Instance.PlayMusic(SoundManager.Instance.GetAudioClip());
        

    }

    public void AddScorePoints(int points)
    {
        playerScore += points;
    }

    /*void FreezePlayer(bool freeze)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<Komaru>().FreezeInput(freeze);
            player.GetComponent<Komaru>().FreezePlayer(freeze);
        }
    }

    void FreezeEnemies(bool freeze)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().FreezeEnemy(freeze);
        }
    }*/

    void FreezeBullets(bool freeze)
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            bullet.GetComponent<Bullet>().FreezeBullet(freeze);
        }
    }

    public void PlayerDefeated()
    {
        isGameOver = true;
        gameRestartTime = gameRestartDelay;
        SoundManager.Instance.Stop();
        SoundManager.Instance.StopMusic();
        Messenger.Broadcast<bool>("Freeze", true);
        /*GameObject[] explosions = GameObject.FindGameObjectsWithTag("Explosion");
        foreach (GameObject explosion in explosions)
        {
            Destroy(explosion);
        }*/
    }

    void TeleportPlayer(bool teleport)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<Komaru>().teleport(teleport);
        }
    }
}