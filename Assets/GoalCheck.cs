using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalCheck : MonoBehaviour
{
    public string nextSceneName = "Level 2";

    // 1. Dùng nếu bệ vàng TÍCH chọn Is Trigger
    private void OnTriggerEnter(Collider other)
    {
        CheckAndLoadScene(other.gameObject);
    }

    // 2. Dùng nếu bệ vàng KHÔNG tích Is Trigger (bệ rắn)
    private void OnCollisionEnter(Collision collision)
    {
        CheckAndLoadScene(collision.gameObject);
    }

    // 3. Dùng riêng cho CharacterController của Player
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        CheckAndLoadScene(hit.gameObject);
    }

    void CheckAndLoadScene(GameObject obj)
    {
        if (obj.CompareTag("Player") || obj.GetComponent<CharacterController>() != null)
        {
            Debug.Log("Đã chạm bệ đích! Đang chuyển sang: " + nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }
    }
}