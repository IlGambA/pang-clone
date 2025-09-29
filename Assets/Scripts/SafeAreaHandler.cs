using UnityEngine;

public class SafeAreaHandler : MonoBehaviour
{
    private RectTransform _rectTransform;
    private ScreenOrientation _lastOrientation = ScreenOrientation.AutoRotation;
   private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        ApplySafeArea();
    }
    
   private void ApplySafeArea()
    {
        if (!_rectTransform) return;
        
        Rect safeArea = Screen.safeArea;
        
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        
        _rectTransform.anchorMin = anchorMin;
        _rectTransform.anchorMax = anchorMax;
        
        Debug.Log($"Safe Area applied: {safeArea}");
    }
   
    private void Update()
    {
        if (Screen.orientation != _lastOrientation)
        {
            _lastOrientation = Screen.orientation;
            ApplySafeArea();
        }
    }
}