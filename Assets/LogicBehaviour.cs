using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicBehaviour : MonoBehaviour
{
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI healthUI;
    public TextMeshProUGUI levelUI;
    public GameObject gameoverUI;
    public GameObject frasteroid;
    public GameObject ship;
    public AudioSource music;

    enum GameState { LEVELSCREEN, RUNNING, GAMEOVER };

    private GameState gameState;
    private int score;
    private int level;

    private float levelScreenTimeout = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        ShowLevelScreen();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if we need to 
        if (gameState == GameState.LEVELSCREEN && Time.fixedTime > levelScreenTimeout)
        {
            StartLevel();
        }

        if (gameState == GameState.RUNNING)
        {
            CheckLevelVictory();
        }
    }

    private void UpdateUI()
    {
        if (gameState == GameState.LEVELSCREEN)
        {
            levelUI.text = "LEVEL " + level;
            levelUI.enabled = true;
            scoreUI.enabled = false;
            healthUI.enabled = false;
            gameoverUI.SetActive(false);
            music.Stop();
        }
        else if (gameState == GameState.RUNNING)
        {
            scoreUI.text = "Score: " + score;
            levelUI.enabled = false;
            scoreUI.enabled = true;
            healthUI.enabled = true;
            gameoverUI.SetActive(false);
            music.Play();
        }
        else if (gameState == GameState.GAMEOVER)
        {
            levelUI.enabled = false;
            scoreUI.enabled = true;
            healthUI.enabled = false;
            gameoverUI.SetActive(true);
            music.Stop();
        }
    }

    private void ShowLevelScreen()
    {
        gameState = GameState.LEVELSCREEN;
        UpdateUI();
        levelScreenTimeout = Time.fixedTime + 3.0f;
    }

    private void StartLevel()
    {
        gameState = GameState.RUNNING;
        UpdateUI();

        // Spawn ship
        Vector2 shipSpawnPosition = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2));
        Instantiate(ship, shipSpawnPosition, Quaternion.identity);

        // Spawn asteroids
        for (int i = 0; i < level; i++)
        {
            float spawnY = Random.Range
                (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
            float spawnX = Random.Range
                (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

            Vector2 spawnPosition = new Vector2(spawnX, spawnY);
            Instantiate(frasteroid, spawnPosition, Quaternion.identity);
        }
    }

    private void CheckLevelVictory()
    {
        if (GameObject.FindGameObjectsWithTag("Asteroid").Length == 0)
        {
            Cleanup();
            level++;
            ShowLevelScreen();
        }
    }

    private void Cleanup()
    {
        foreach (GameObject ship in GameObject.FindGameObjectsWithTag("Ship"))
        {
            Destroy(ship);
        }

        foreach (GameObject asteroid in GameObject.FindGameObjectsWithTag("Asteroid"))
        {
            Destroy(asteroid);
        }

        foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            Destroy(bullet);
        }
    }

    public void GameOver()
    {
        gameState = GameState.GAMEOVER;
        Cleanup();
        UpdateUI();
    }

    public void IncreaseScore(int scoreIncrease)
    {
        score += scoreIncrease;
        scoreUI.text = "Score: " + score;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateHealthUI(float health)
    {
        healthUI.text = "Health: " + Mathf.Floor(health);
    }
}
