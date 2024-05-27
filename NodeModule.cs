using UnityEngine;
using System.Collections;

public enum EasingStyle
{
    Linear,
    EaseIn,
    EaseOut,
    EaseInOut,
    ExpoIn,
    ExpoOut,
    ExpoInOut,
    CircIn,
    CircOut,
    CircInOut,
    SineIn,
    SineOut,
    SineInOut,
    CubicIn,
    CubicOut,
    CubicInOut
}

public class NodeModule : MonoBehaviour
{
    public bool is2D = true; // Toggle for 2D or 3D functionality

    public Transform[] nodes; // An array to store the reference to the nodes

    public bool loop = false; // Option to loop the animation
    public bool autoplayOnStart = false; // Option to autoplay the animation on start

    public EasingStyle easingStyle = EasingStyle.Linear; // Easing style for movement

    private int currentNodeIndex = 0; // The index of the current node being targeted
    private bool isMoving = false; // Flag to check if the object is currently moving

    private bool hasStarted = false; // Flag to check if the animation has started

    private void Start()
    {
        if (autoplayOnStart && !hasStarted)
        {
            PlayTween();
        }
    }

    public void PlayTween()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveToNextNode());
            hasStarted = true;
        }
    }

    private IEnumerator MoveToNextNode()
    {
        isMoving = true;

        // Get the position of the current node and the next node
        Vector3 startPos = transform.position;
        Vector3 targetPos = nodes[currentNodeIndex].position;

        float elapsedTime = 0f;
        float duration = 1f; // The duration of the tween in seconds

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the interpolation value between the start and target positions
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Apply the easing style to the interpolation value
            t = ApplyEasingStyle(t);

            // Apply the interpolation to move the object smoothly
            if (is2D)
            {
                transform.position = Vector2.Lerp(startPos, targetPos, t);
            }
            else
            {
                transform.position = Vector3.Lerp(startPos, targetPos, t);
            }

            yield return null;
        }

        // Update the current node index for the next move
        currentNodeIndex = (currentNodeIndex + 1) % nodes.Length;

        isMoving = false;

        if (loop || currentNodeIndex != 0)
        {
            StartCoroutine(MoveToNextNode());
        }
    }

    private float ApplyEasingStyle(float t)
    {
        switch (easingStyle)
        {
            case EasingStyle.Linear:
                return t;
            case EasingStyle.EaseIn:
                return Mathf.Pow(t, 2);
            case EasingStyle.EaseOut:
                return 1f - Mathf.Pow(1f - t, 2);
            case EasingStyle.EaseInOut:
                return t < 0.5f ? 2f * Mathf.Pow(t, 2) : 1f - Mathf.Pow(-2f * t + 2f, 2) * 0.5f;
            case EasingStyle.ExpoIn:
                return t == 0f ? 0f : Mathf.Pow(2f, 10f * (t - 1f));
            case EasingStyle.ExpoOut:
                return t == 1f ? 1f : 1f - Mathf.Pow(2f, -10f * t);
            case EasingStyle.ExpoInOut:
                if (t == 0f || t == 1f) return t;
                if (t < 0.5f) return 0.5f * Mathf.Pow(2f, 10f * (2f * t - 1f));
                return 0.5f * (2f - Mathf.Pow(2f, -10f * (2f * t - 1f)));
            case EasingStyle.CircIn:
                return 1f - Mathf.Sqrt(1f - Mathf.Pow(t, 2));
            case EasingStyle.CircOut:
                return Mathf.Sqrt(1f - Mathf.Pow(t - 1f, 2));
            case EasingStyle.CircInOut:
                return t < 0.5f ? 0.5f * (1f - Mathf.Sqrt(1f - 4f * Mathf.Pow(t, 2))) : 0.5f * (Mathf.Sqrt(1f - Mathf.Pow(-2f * t + 2f, 2)) + 1f);
            case EasingStyle.SineIn:
                return 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
            case EasingStyle.SineOut:
                return Mathf.Sin(t * Mathf.PI * 0.5f);
            case EasingStyle.SineInOut:
                return 0.5f * (1f - Mathf.Cos(t * Mathf.PI));
            case EasingStyle.CubicIn:
                return Mathf.Pow(t, 3);
            case EasingStyle.CubicOut:
                return 1f - Mathf.Pow(1f - t, 3);
            case EasingStyle.CubicInOut:
                return t < 0.5f ? 4f * Mathf.Pow(t, 3) : 1f - Mathf.Pow(-2f * t + 2f, 3) * 0.5f;
            default:
                return t;
        }
    }
}
