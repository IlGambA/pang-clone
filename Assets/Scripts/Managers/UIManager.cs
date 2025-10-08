using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI livesText;
    
    // Update is called once per frame
    void Update()
    {
        scoreText.text = GameManager.instance.totalScore.ToString();
        livesText.text = GameManager.instance.playerStats.lives.ToString();
    }
}
