using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingObject : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Delete object once it passes player
        if (rb.position.z < -50)
            Destroy(this);

        // move object towards player
        transform.Translate(new Vector3(0, 0, -1 * speed * Time.deltaTime));
    }
}
