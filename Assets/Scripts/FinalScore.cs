using TMPro;
using UnityEngine;

public class FinalScore : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI pointsText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pointsText.text = GameManager.instance.totalScore.ToString();
    }
}
