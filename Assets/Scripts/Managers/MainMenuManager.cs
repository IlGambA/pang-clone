using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
  
    [SerializeField]
    private Button startButton;
    
    [SerializeField]
    private Button quitButton;
    
    [SerializeField]
    private int nextSceneBuildIndex;
    
   // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (startButton)
        {
            GameManager.instance.playerStats.lives = 10;
            startButton.onClick.AddListener(StartGame);
            Handheld.Vibrate();
        }
        
        if (quitButton)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    private void StartGame()
    {
        Debug.Log("Quit Game!");
        GameManager.instance.currentLevel = nextSceneBuildIndex;
        GameManager.instance.LoadLevel(nextSceneBuildIndex);
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
