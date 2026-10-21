using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float dayDurationInSeconds = 120f;
    private static float timeElapsed = 0f;

    void Update()
    {
        timeElapsed += Time.deltaTime;
        float currentRotationX = (timeElapsed / dayDurationInSeconds) * 360f;
        transform.localRotation = Quaternion.Euler(currentRotationX, 170f, 0f);
    }
}