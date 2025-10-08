using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int currentLevel = 0;
    public int totalScore = 0;
    public PlayerStats playerStats;
    
    public List<GameObject> balls;
    
    private void Awake()
    { 
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (playerStats is not null)
        {
            playerStats.lives = 10;
        }
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }

    public void Update()
    {
        if (playerStats.lives <= 0 && SceneManager.GetActiveScene().buildIndex > 1)
        {
            LoadLevel(1);
        }

        if (balls.Count <= 0 && SceneManager.GetActiveScene().buildIndex > 1)
        {
            if (currentLevel < SceneManager.sceneCountInBuildSettings-1)
            {
                playerStats.lives += 5;
                currentLevel++;
                LoadLevel(currentLevel);
            }
            else
            {
                currentLevel = 1;
                LoadLevel(currentLevel);
            }
            
        }
    }
}
