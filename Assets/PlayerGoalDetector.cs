using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGoalDetector : MonoBehaviour
{
    public string nextSceneName = "Level 2";
    public float delayTime = 3.0f;
    private bool isLevelCompleted = false;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (isLevelCompleted) return;

        // Kiểm tra đúng Bệ Đích
        if (hit.gameObject.CompareTag("Goal") || hit.gameObject.GetComponent<GoalCheck>() != null || hit.gameObject.name.Contains("Goal"))
        {
            // hit.normal.y > 0.7f đảm bảo nhân vật đang đứng/đáp từ TRÊN MẶT BỆ xuống
            if (hit.normal.y > 0.7f)
            {
                StartCoroutine(CompleteLevelRoutine(hit.gameObject));
            }
        }
    }

    IEnumerator CompleteLevelRoutine(GameObject goalObject)
    {
        isLevelCompleted = true;

        // Dừng đồng hồ đếm giờ ngay khi cán đích
        if (GameManagerUI.instance != null)
        {
            GameManagerUI.instance.StopTimer();
        }

        // Bật pháo hoa
        ParticleSystem fireworks = goalObject.GetComponentInChildren<ParticleSystem>();
        if (fireworks != null)
        {
            fireworks.Play();
        }

        Debug.Log("CHÚC MỪNG! BẠN ĐÃ ĐÁP CHÂN LÊN BỆ ĐÍCH!");

        yield return new WaitForSeconds(delayTime);

        SceneManager.LoadScene(nextSceneName);
    }
}