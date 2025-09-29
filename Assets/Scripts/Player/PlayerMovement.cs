using UnityEngine;

[RequireComponent(typeof(Rigidbody) , typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float force;
    
    private Animator _playerAnimator;
    private Rigidbody _playerRigidBody;

    private bool _isWalk;
    private int _direction;
    
    private static readonly int PlayerAnimatorWalk = Animator.StringToHash("isWalk");
    private static readonly int PlayerAnimatorDirection = Animator.StringToHash("Direction");
    private void Awake()
    {
        _isWalk = false;
        _direction = 0;
        
        _playerAnimator = GetComponent<Animator>();
        if (_playerAnimator is null)
        {
            Debug.LogError("PlayerAnimator is null");
            return;
        }
        _playerRigidBody = GetComponent<Rigidbody>();
        
        if (_playerRigidBody is null)
        {
            Debug.LogError("PlayerRigidBody is null");
            return;
        }
        _playerRigidBody.maxLinearVelocity = 3f;
    }
    
    private void FixedUpdate()
    {
        _isWalk = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
        _playerAnimator.SetBool(PlayerAnimatorWalk, _isWalk);
        
        if (_isWalk && Input.GetKey(KeyCode.A))
        {
            _direction = -1;
            _playerAnimator.SetInteger(PlayerAnimatorDirection, _direction);
            _playerRigidBody.AddForce(Vector3.left * force,ForceMode.Acceleration);
        }
        if (_isWalk && Input.GetKey(KeyCode.D))
        {
            _direction = 1;
            _playerAnimator.SetInteger(PlayerAnimatorDirection, _direction);
            _playerRigidBody.AddForce(Vector3.right * force,ForceMode.Acceleration);

        }
    }
}
