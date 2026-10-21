using UnityEngine;

public class LavaRiser : MonoBehaviour
{
    public float riseSpeed = 0.8f;

    void Update()
    {
        transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
    }
}