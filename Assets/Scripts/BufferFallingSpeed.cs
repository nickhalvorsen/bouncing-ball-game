using UnityEngine;

public class BufferFallSpeed : MonoBehaviour
{
    private float bufferHeight = 0; // Height above which to buffer the fall speed
    public float bufferedFallSpeed = 20.0f; // The fall speed while buffering
    private bool isBuffering = false;
    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody found on the object.");
        }
    }

    public void BufferUntilHeight(float height)
    {
        bufferHeight = height;
        isBuffering = true;
    }

    void Update()
    {
        if (!isBuffering) return;

        if (rb.transform.position.y < bufferHeight)
        {
            isBuffering = false;
            return;
        }

        // Only adjust fall speed if the object is above the buffer height
        if (transform.position.y > bufferHeight)
        {
            // Check if the object is falling
            if (rb.velocity.y < -bufferedFallSpeed)
            {
                Vector3 velocity = rb.velocity;
                // Buffer the fall speed to the specified value
                velocity.y = Mathf.Max(velocity.y, -bufferedFallSpeed);
                rb.velocity = velocity;
            }
        }
    }
}