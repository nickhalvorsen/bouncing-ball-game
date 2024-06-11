using UnityEngine;

public class DebugCube : MonoBehaviour
{
    private GameObject cube;

    void Start()
    {
        // Create a new cube GameObject
        GameObject newCube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Set the position of the new cube
        newCube.transform.position = new Vector3(0, 1, 0);

        // Set the scale of the new cube
        newCube.transform.localScale = new Vector3(.5f,.5f,.5f);

        // Add a Rigidbody component to the new cube (optional)
        Rigidbody rb = newCube.AddComponent<Rigidbody>();
        rb.isKinematic = true;

        // Optionally, set a material for the cube
        Renderer renderer = newCube.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Standard"));
        renderer.material.color = Color.red; // Set the color of the material  

        cube = newCube;
    }

    private void Update()
    {
        cube.transform.position = transform.position;
    }
}