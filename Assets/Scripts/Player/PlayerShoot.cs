using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Button shootButton;
    
    [Header("Rope Settings")]
    [SerializeField] private int ropeSegmentsPool = 20;
    [SerializeField] private GameObject headPrefab;
    [SerializeField] private GameObject ropePrefab;
    [SerializeField] private float segmentHeight = 0.5f;
    [SerializeField] private float ropeSpeed = 10f;
    [SerializeField] private float ropeDuration = 1f;
    [SerializeField] private LayerMask obstacleLayer = -1;
    [SerializeField] private GameObject ropeParent;
    
    // Pools
    private GameObject[] _ropeSegments;
    private GameObject _headSegment;    
    
    private bool _isShooting = false;

    private void Start()
    {
        CreatePool(ropeParent);
        if (shootButton != null)
        {
            shootButton.onClick.AddListener(() => { if (!_isShooting) {
                    StartCoroutine(ShootRope());
                }
            });
        }
    }

    private void CreatePool(GameObject parent)
    {
        _ropeSegments = new GameObject[ropeSegmentsPool];
        
        _headSegment = Instantiate(headPrefab, parent.transform);
        _headSegment.SetActive(false);
        
        for (var i = 0; i < ropeSegmentsPool; i++)
        {
            _ropeSegments[i] = Instantiate(ropePrefab, parent.transform);
            _ropeSegments[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetButton("Jump") && !_isShooting)
        {
            StartCoroutine(ShootRope());
        }
    }

    private IEnumerator ShootRope()
    {
        _isShooting = true;
        
        Vector3 startPos = transform.position;
        startPos.y -= .2f;
        float currentLength = 0f;
        
        while (true)
        {
            currentLength += ropeSpeed * Time.deltaTime;
            int totalSegments = Mathf.CeilToInt(currentLength / segmentHeight);
            int ropeSegmentsNeeded = Mathf.Max(0, totalSegments - 1); 
            ropeSegmentsNeeded = Mathf.Min(ropeSegmentsNeeded, ropeSegmentsPool);
            
            UpdateAllSegments(startPos, ropeSegmentsNeeded);

            if (Physics.Raycast(startPos, Vector3.up, currentLength, obstacleLayer))
            {
                break;
            }
            
            yield return null;
        }
        
        yield return new WaitForSeconds(ropeDuration);
        HideAllSegments();
        _isShooting = false;
    }

    private void UpdateAllSegments(Vector3 startPos, int ropeCount)
    {
        for (int i = 0; i < ropeCount && i < ropeSegmentsPool; i++)
        {
            Vector3 pos = startPos + Vector3.up * (i * segmentHeight);
            _ropeSegments[i].transform.position = pos;
            _ropeSegments[i].SetActive(true);
        }
        
        for (int i = ropeCount; i < ropeSegmentsPool; i++)
        {
            _ropeSegments[i].SetActive(false);
        }

        Vector3 headPos = startPos + Vector3.up * (ropeCount * segmentHeight);
        _headSegment.transform.position = headPos;
        _headSegment.SetActive(true);
    }


    private void HideAllSegments()
    {
        for (int i = 0; i < ropeSegmentsPool; i++)
        {
            _ropeSegments[i].SetActive(false);
        }
        _headSegment.SetActive(false);
    }
}
