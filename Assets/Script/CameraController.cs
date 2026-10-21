using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 2f, -4f);
    public float smoothSpeed = 10f;

    void LateUpdate()
    {
        if (target == null) return;

        // Giữ vị trí camera theo player + offset
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Ép camera luôn nhìn thẳng vào ngực/đầu nhân vật
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}