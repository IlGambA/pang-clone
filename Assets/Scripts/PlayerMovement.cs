using UnityEngine;

[RequireComponent(typeof(Rigidbody) , typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float force = 1f;
    
    private Animator playerAnimator;
    private Rigidbody playerRigidBody;

    private bool isWalk;
    private int direction;
    
    private static readonly int Player_Animator_Walk = Animator.StringToHash("isWalk");
    private static readonly int Player_Animator_Direction = Animator.StringToHash("Direction");
    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        if (playerAnimator is null)
        {
            Debug.LogError("PlayerAnimator is null");
        }
        playerRigidBody = GetComponent<Rigidbody>();
        if (playerRigidBody is null)
        {
            Debug.LogError("PlayerRigidBody is null");
        }
        isWalk = false;
        direction = 0;
        playerRigidBody.maxLinearVelocity = 2f;
    }

    private void Update()
    {
        Debug.Log(playerRigidBody.linearVelocity);
    }
    private void FixedUpdate()
    {
        isWalk = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
        playerAnimator.SetBool(Player_Animator_Walk, isWalk);
        
        
        if (isWalk && Input.GetKey(KeyCode.A))
        {
            direction = 1;
            playerRigidBody.AddForce(Vector3.left * force,ForceMode.VelocityChange);
            playerAnimator.SetInteger(Player_Animator_Direction, direction);
        }
        else if (isWalk && Input.GetKey(KeyCode.D))
        {
            direction = 0;
            playerRigidBody.AddForce(Vector3.right * force,ForceMode.VelocityChange);
            playerAnimator.SetInteger(Player_Animator_Direction, direction);
        }
        
        
    }
}
