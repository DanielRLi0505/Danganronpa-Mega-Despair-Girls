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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerReady)
        {
            if (initReadyScreen)
            {
                FreezePlayer(true);
                FreezeEnemies(true);
                screenMessageText.alignment = TextAlignmentOptions.Center;
                screenMessageText.alignment = TextAlignmentOptions.Top;
                screenMessageText.fontStyle = FontStyles.UpperCase;
                screenMessageText.fontSize = 24;
                screenMessageText.text = "\nREADY";
                initReadyScreen = false;
            }
            gamePlayerReadyTime -= Time.deltaTime;
            if (gamePlayerReadyTime < 0)
            {
                FreezePlayer(false);
                FreezeEnemies(false);
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

    void FreezePlayer(bool freeze)
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
    }

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
        FreezePlayer(true);
        FreezeEnemies(true);
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
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