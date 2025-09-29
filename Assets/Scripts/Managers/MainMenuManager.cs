using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
  
    [SerializeField]
    private Button startButton;
    
    [SerializeField]
    private Button quitButton;
    
    [SerializeField]
    private int nextSceneBuildIndex;
    
    private GameManager gameManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (startButton)
        {
            startButton.onClick.AddListener(StartGame);
        }
        
        if (quitButton)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
        
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void StartGame()
    {
        Debug.Log("Quit Game!");
        gameManager.currentLevel = nextSceneBuildIndex;
        gameManager.LoadLevel(nextSceneBuildIndex);
    }
    
    
    private void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
