using System.Collections;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float duration = .5f; // Duration of the movement
    public float downDuration = .5f;
    private bool isMoving = false;

    public void MoveToHeight(float targetY)
    {
        var thisDuration = targetY > this.transform.position.y ? duration : downDuration;
        StartCoroutine(MoveToY(targetY, thisDuration));
    }

    IEnumerator MoveToY(float y, float time)
    {
        isMoving = true;
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(startPosition.x, y, startPosition.z);

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / time);
            t = Mathf.SmoothStep(0f, 1f, t); // Apply smoothstep easing
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
    }
}