using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody) , typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] 
    private float force;
    
    [SerializeField] 
    private Button leftButton;
    
    [SerializeField] 
    private Button rightButton;
    
    private Animator _playerAnimator;
    private Rigidbody _playerRigidBody;

    private bool _isWalk;
    private int _direction;
    private float _touchInput = 0f;
    
    private static readonly int PlayerAnimatorWalk = Animator.StringToHash("isWalk");
    private static readonly int PlayerAnimatorDirection = Animator.StringToHash("Direction");
    public readonly int PlayerAnimatorHurt = Animator.StringToHash("isHurt");
    
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

    private void Start()
    {
        SetupButtonEvents();
    }

    private void SetupButtonEvents()
    {
        if (leftButton != null)
        {
            var triggerLeft = leftButton.gameObject.GetComponent<EventTrigger>();
            if (triggerLeft == null)
                triggerLeft = leftButton.gameObject.AddComponent<EventTrigger>();

            triggerLeft.triggers.Clear();
            AddEventTrigger(triggerLeft, EventTriggerType.PointerDown, (data) => { _touchInput = -1f; });
            AddEventTrigger(triggerLeft, EventTriggerType.PointerUp, (data) => { if (_touchInput < 0) _touchInput = 0f; });
            AddEventTrigger(triggerLeft, EventTriggerType.PointerExit, (data) => { if (_touchInput < 0) _touchInput = 0f; });
        }

        if (rightButton != null)
        {
            var triggerRight = rightButton.gameObject.GetComponent<EventTrigger>();
            if (triggerRight == null)
                triggerRight = rightButton.gameObject.AddComponent<EventTrigger>();

            triggerRight.triggers.Clear();
            AddEventTrigger(triggerRight, EventTriggerType.PointerDown, (data) => { _touchInput = 1f; });
            AddEventTrigger(triggerRight, EventTriggerType.PointerUp, (data) => { if (_touchInput > 0) _touchInput = 0f; });
            AddEventTrigger(triggerRight, EventTriggerType.PointerExit, (data) => { if (_touchInput > 0) _touchInput = 0f; });
        }
    }
    
    void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, System.Action<BaseEventData> action)
    {
        var entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener(new UnityEngine.Events.UnityAction<BaseEventData>(action));
        trigger.triggers.Add(entry);
    }
    
    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        
        if (_touchInput != 0f)
            horizontal = _touchInput;

        _isWalk = Mathf.Abs(horizontal) > 0.01f;
        _playerAnimator.SetBool(PlayerAnimatorWalk, _isWalk);

        if (_isWalk)
        {
            _direction = horizontal > 0 ? 1 : -1;
            _playerAnimator.SetInteger(PlayerAnimatorDirection, _direction);
            _playerRigidBody.AddForce(Vector3.right * horizontal * force, ForceMode.Acceleration);
        }
        else
        {
            _direction = 0;
            _playerAnimator.SetInteger(PlayerAnimatorDirection, _direction);
        }
    }
}
