using UnityEngine;

public class PropelToNextLevel : MonoBehaviour
{
    public float targetHeight = 10.0f; // Desired height to reach
    public Rigidbody rb;
    public Collider coll;
    public bool isPropelling = false;

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        if (GetComponent<Collider>() == null)
        {
            coll = GetComponent<Collider>();
        }
    }

    private void Update()
    {
        if (isPropelling)
        {
            // TODO: this is sometimes triggered the instant the ball touches the tramp
            // Checking if velocity < 0 doesn't yield the proper result. Needs to be < -0.1.
            if (rb.velocity.y < -0.2)
            {
                Debug.Log("Propel deactivated, rb velocity y is " + rb.velocity.y);
                GetComponent<Collider>().enabled = true;
                isPropelling = false;
            }
        }
    }

    public void PropelTo(float targetHeight)
    {
        GetComponent<Collider>().enabled = false;
        isPropelling = true;
        PropelToHeight(targetHeight);
    }

    private void PropelToHeight(float targetHeight)
    {
        float initialVelocity = CalculateInitialVelocity(targetHeight, BouncingBoy.gravity);
        float force = CalculatePropellingForce(rb.mass, initialVelocity);

        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
    }

    float CalculateInitialVelocity(float height, float gravity)
    {
        return Mathf.Sqrt(2 * Mathf.Abs(gravity) * height);
    }

    float CalculatePropellingForce(float mass, float initialVelocity)
    {
        return mass * initialVelocity;
    }
}