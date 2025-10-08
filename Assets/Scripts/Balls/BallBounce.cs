using UnityEngine;

public class BallBounce : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float horizontalSpeed = 5f;
    [SerializeField] private float bounceHeight = 8f;
    private readonly float _gravity = 20f;
    
    [Header("Split Settings")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private bool canSplit = true;

    private GameObject _ballParent;
    private BallScore _score;
    private bool _isBeingDestroyed = false;
    
    private float _horizontalDirection = 1f; 
    private float _verticalVelocity = 0f;

    private bool _invulnerable = false;
    private float _timeInvulnerable = 1f;
    
    private PlayerMovement _playerMovement;
    
    private void Start()
    {
        _playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        
        _ballParent = GameObject.FindWithTag("BallParent");
        _score = GetComponent<BallScore>();
        
        GameManager.instance.balls.Add(gameObject);
    }

    private void Update()
    {
        if (_isBeingDestroyed) return;

        if (_invulnerable)
        {
            _timeInvulnerable -= Time.deltaTime;
            if ( _timeInvulnerable < 0 )
            {
                _invulnerable = false;
            }
        }
        
        float moveX = _horizontalDirection * horizontalSpeed * Time.deltaTime;
        
        _verticalVelocity -= _gravity * Time.deltaTime;
        float moveY = _verticalVelocity * Time.deltaTime;
        
        transform.position += new Vector3(moveX, moveY, 0f);
    }
    private float CalculateJumpVelocity()
    {
        return Mathf.Sqrt(2f * _gravity * bounceHeight);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isBeingDestroyed) return;
        
        if (other.CompareTag("Rope"))
        {
            if(_invulnerable)return;
            DestroyAndSplit();
            return;
        }
        if (other.CompareTag("Wall"))
        {
            _horizontalDirection *= -1f;
        }
        else if (other.CompareTag("Ground"))
        {
            if (_verticalVelocity < 0)
            {
                _verticalVelocity = CalculateJumpVelocity();
            }
        }
        else if (other.CompareTag("Obstacle"))
        {
            HandleObstacleCollision(other);
        }
        else if (other.CompareTag("Player"))
        {
            if (GameManager.instance.playerStats == null) return;
            GameManager.instance.playerStats.lives-=1; 
            _playerMovement.GetComponent<Animator>().SetTrigger(_playerMovement.PlayerAnimatorHurt);
        }
    }

    private void HandleObstacleCollision(Collider obstacle)
    {
        Vector3 ballPos = transform.position;
        Vector3 obstaclePos = obstacle.transform.position;
        
        Collider ballCollider = GetComponent<Collider>();
        Vector3 contactPoint = obstacle.ClosestPoint(ballCollider.bounds.center);
        
        float deltaX = Mathf.Abs(ballCollider.bounds.center.x - contactPoint.x);
        float deltaY = Mathf.Abs(ballCollider.bounds.center.y - contactPoint.y);
        
        if (deltaX > deltaY)
        {
            _horizontalDirection *= -1f;
        }
        else
        {
            if ((_verticalVelocity < 0 && ballPos.y > obstaclePos.y) || (_verticalVelocity > 0 && ballPos.y < obstaclePos.y))
            { 
                _verticalVelocity = -_verticalVelocity * 0.9f;
            }
        }
    }

    private void DestroyAndSplit()
    {
        _isBeingDestroyed = true;
        GetComponent<Collider>().enabled = false;
        if (_score != null && GameManager.instance != null)
        {
            GameManager.instance.totalScore += _score.score;
        }
        if (canSplit && ballPrefab != null)
        {
            CreateChildBall(-1f);
            CreateChildBall(1f);
        }
        GameManager.instance.balls.Remove(gameObject);
        Destroy(gameObject);
    }

    private void CreateChildBall(float direction)
    {
        Vector3 spawnPosition = transform.position + Vector3.up * 0.2f;
        GameObject child = Instantiate(ballPrefab, spawnPosition, Quaternion.identity, _ballParent.transform);
        
        BallBounce childScript = child.GetComponent<BallBounce>();
        if (childScript != null)
        {
            childScript._horizontalDirection = direction;
            childScript._verticalVelocity = 0f;
            childScript._invulnerable = true;
        }
    }
}