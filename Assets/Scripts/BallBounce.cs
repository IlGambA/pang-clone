using UnityEngine;

public class BallBounce : MonoBehaviour
{
    [Tooltip("widen or narrow the distance between the two rebounds")]
    [SerializeField] private float amplitude = 1f;
    
    [Tooltip("Change force bounce")]   
    [SerializeField] private float force = 1f;
    
    private Rigidbody rb;
    private float direction;
    private Vector3 currentPosition;
    
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb is null)
        {
            Debug.LogError(this.name + ": needs a Rigidbody");
        }
        direction = 1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Invert direction when collide with wall
        if (collision.collider.CompareTag("Wall"))
        {
            direction = -direction;
        }
        
        // Check if collide with ground
        if (collision.collider.CompareTag("Ground"))
        {
            Debug.Log(gameObject.name + " collided with " + collision.collider.name);

            //Get Normal
            Vector3 collisionNormal = collision.contacts[0].normal;
            Debug.DrawRay(collision.contacts[0].point, collisionNormal.normalized * 3, Color.red, 2f);

            //Add force for bounce
            rb.AddForce(collisionNormal * force, ForceMode.Impulse);
        }
    }
    private void FixedUpdate()
    {
        //Move ball along x position based of direction and amplitude physically
        rb.AddForce(Vector3.right * (amplitude*direction), ForceMode.Force);
    }
}