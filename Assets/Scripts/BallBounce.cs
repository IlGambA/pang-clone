using UnityEngine;

public class BallBounce : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float gravity = -15f;
    [SerializeField] private float maxHeight = 10f;
    
    [Header("Ball Splitting")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float splitSpeed = 6f;
    [SerializeField] private bool canSplit = true; 
    
    private Vector3 velocity;
    private float startY;
    private bool _invulnerable = false;
    private float _timeInvulnerable = 1f;
    private void Start()
    {
        velocity = new Vector3(speed, speed, 0f);
        startY = transform.position.y;
      
    }
    
    private void Update()
    {
        if (_invulnerable)
        {
            _timeInvulnerable -= Time.deltaTime;
            if ( _timeInvulnerable < 0 )
            {
                _invulnerable = false;
            }
        }
        
        transform.position += velocity * Time.deltaTime;
        velocity.y += gravity * Time.deltaTime;
        ClampHeight();
    }
    
    void ClampHeight()
    {
        float currentY = transform.position.y;
        float maxAllowedY = startY + maxHeight;
        
        if (currentY > maxAllowedY)
        {
            transform.position = new Vector3(transform.position.x, maxAllowedY, transform.position.z);
            velocity.y = -Mathf.Abs(velocity.y);
            Debug.Log("Hit max height!");
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            BounceHorizontal();
        }
        else if (other.CompareTag("Ground"))
        {
            BounceVertical();
        }
        else if (other.CompareTag("Obstacle"))
        {
            BounceFromObstacle(other);
        }
        else if (other.CompareTag("Rope"))
        {
            if(_invulnerable)return;
            DestroyAndSplit();
        }
    }
    
    void BounceHorizontal()
    {
        velocity.x = -velocity.x;
        Debug.Log("Bounce horizontal");
    }
    
    void BounceVertical()
    {
        velocity.y = Mathf.Abs(velocity.y);
        if (velocity.y < speed * 0.5f)
        {
            velocity.y = speed * 0.8f;
        }
        Debug.Log("Bounce vertical");
    }
    
    void BounceFromObstacle(Collider obstacle)
    {
        Vector3 ballPos = transform.position;
        Vector3 obstaclePos = obstacle.transform.position;
        
        float deltaX = ballPos.x - obstaclePos.x;
        float deltaY = ballPos.y - obstaclePos.y;
        
        if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
        {
            BounceHorizontal();
        }
        else
        {
            velocity.y = -velocity.y;
        }
        
        Debug.Log("Bounce from obstacle");
    }
    
    void DestroyAndSplit()
    {
        Debug.Log("Ball destroyed by rope!");
        
        if (canSplit && ballPrefab != null)
        {
            SpawnChildBalls();
        }
        
        Destroy(gameObject);
    }
    
    void SpawnChildBalls()
    {
        Vector3 currentPos = transform.position;
        
        // Left Ball
        GameObject leftBall = Instantiate(ballPrefab, currentPos, Quaternion.identity);
        BallBounce leftScript = leftBall.GetComponent<BallBounce>();
        
        if (leftScript != null)
        {
            leftScript.SetSpeed(-splitSpeed);
            leftScript.maxHeight = maxHeight; 
            leftScript.startY = currentPos.y;
            leftScript._invulnerable = true;
        }
        
        // Right Ball
        GameObject rightBall = Instantiate(ballPrefab, currentPos, Quaternion.identity);
        BallBounce rightScript = rightBall.GetComponent<BallBounce>();
        if (rightScript != null)
        {
            rightScript.SetSpeed(splitSpeed);
            rightScript.maxHeight = maxHeight;
            rightScript.startY = currentPos.y;
            rightScript._invulnerable = true;
        }
        
        Debug.Log("Spawned 2 child balls!");
    }
    
    private void SetSpeed(float _speed)
    {
        speed = _speed;
    }
    private void SetMaxHeight(float _maxHeight)
    {
        maxHeight = _maxHeight;
    }
}
