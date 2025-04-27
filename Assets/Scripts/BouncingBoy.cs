using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using static UnityEngine.ParticleSystem;

public class BouncingBoy : MonoBehaviour
{
    // vertical movement
    public const float gravity = -11;
    private const float bounceFactor = 10;
    private const float diveSpeed = -20;
    private const float playerGlideThreshold = -.5f; // if the player's downward velocity is stronger than this, then they may glide
    private const float glideGravityOffset = 0.8f; // higher number = stronger glide effect

    // horizontal movement
    private const float strafeAccelSpeed = 180;
    private const float maxStrafeSpeed = 10;
    private const float strafeDecelSpeed = 120;

    private Rigidbody rb;
    private Vector3 targetVelocity;
    public AudioClip collisionSound;
    public AudioClip trampolineSound;
    public AudioClip pickupSound;
    private AudioSource audioSource;
    private ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = collisionSound;
        particles = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // horizontal movement
        float moveX = 0;
        if (Input.GetKey(KeyCode.LeftArrow))
            moveX = -1;
        else if (Input.GetKey(KeyCode.RightArrow))
            moveX = 1;
        targetVelocity = new Vector3(moveX, 0, 0) * maxStrafeSpeed;

        // apply vertical movement
        rb.velocity += new Vector3(0, gravity * Time.deltaTime, 0);

        var isGliding = Input.GetKey(KeyCode.UpArrow) && rb.velocity.y < playerGlideThreshold;
        var isDiving = Input.GetKey(KeyCode.DownArrow) && rb.velocity.y < bounceFactor * .5;

        if (isGliding)
        {
            rb.velocity += new Vector3(0, gravity * -1 * glideGravityOffset * Time.deltaTime, 0);

            var s = particles.shape;
            s.rotation = new Vector3(90, 0, 0);
            if (!particles.isPlaying)
            {
                particles.Play();
            }
        }

        if (isDiving)
        {
            rb.velocity += new Vector3(0, diveSpeed * Time.deltaTime, 0);

            var s = particles.shape;
            s.rotation = new Vector3(-90, 0, 0);
            if (!particles.isPlaying)
            {
                particles.Play();
            }
        }

        if (!isDiving && !isGliding)
        {
            particles.Stop();
        }

        // Prevent the player from being pushed off position by collisions
        rb.position = new Vector3(rb.position.x, rb.position.y, 0);
    }

    void FixedUpdate()
    {
        // Compute current velocity
        var rb = GetComponent<Rigidbody>();
        Vector3 velocity = rb.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);

        // Accelerate or decelerate based on input
        // Apply acceleration if input is detected
        if (targetVelocity != Vector3.zero)
        {
            velocityChange.x = Mathf.Clamp(velocityChange.x, -strafeAccelSpeed * Time.fixedDeltaTime, strafeAccelSpeed * Time.fixedDeltaTime);
        }
        else
        {
            // Apply deceleration when no input is detected
            velocityChange.x = Mathf.Clamp(velocityChange.x, -strafeDecelSpeed * Time.fixedDeltaTime, strafeDecelSpeed * Time.fixedDeltaTime);
        }

        // Apply the velocity change to the Rigidbody
        rb.velocity += new Vector3(velocityChange.x, 0, velocityChange.z);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "platform")
        {
            rb.velocity = new Vector3(rb.velocity.x, bounceFactor, rb.velocity.z);

            audioSource.PlayOneShot(collisionSound);
        }

        if (col.gameObject.tag == "trampoline")
        {
            GameManager.Instance.TrampolineHit();
            audioSource.PlayOneShot(trampolineSound);
            // This velocity will be overridden by the setvelocity in gamemanager, but this is useful for one frame to prevent a double collision
            // (prevent the sound playing on multiple consecutive frames)
            rb.velocity = new Vector3(rb.velocity.x, bounceFactor, rb.velocity.z);
        }

        if (col.gameObject.tag == "pickup")
        {
            //GameManager.Instance.PickedupPickup();
            audioSource.PlayOneShot(pickupSound);
            Debug.Log("Pickeud up");
            col.gameObject.SetActive(false);
        }
    }
}
