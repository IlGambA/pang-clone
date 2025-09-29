using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class TextColorChanger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] 
    private TMP_Text targetText;
    
    [SerializeField] 
    private Color pressedColor = Color.red;
    
    private Color _baseColor;
    private bool _isPressed = false;

    private void Start()
    {
        if (targetText is not null)
           _baseColor = targetText.color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (targetText is null) return;
        targetText.color = pressedColor;
        _isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (targetText is null) return;
        targetText.color = _baseColor;
        _isPressed = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isPressed || targetText is null) return;
        targetText.color = _baseColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isPressed || targetText is null) return;
        targetText.color = _baseColor;
    }
}