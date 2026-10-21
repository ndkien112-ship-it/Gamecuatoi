using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManagerUI : MonoBehaviour
{
    public static GameManagerUI instance;

    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI deathText;

    private float elapsedTime = 0f;
    private int deathCount = 0;
    private bool isGameActive = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Giữ bộ đếm không bị mất khi load lại Scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Mỗi khi load lại Scene (sau khi chết hoặc qua màn)
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cho đồng hồ chạy lại bình thường
        isGameActive = true;

        // Nếu mất liên kết Text UI do vừa reload Scene, tự tìm lại
        if (timerText == null || deathText == null)
        {
            FindUITexts();
        }

        // Cập nhật lại số lần chết hiện tại lên màn hình
        UpdateDeathUI();
    }

    private void FindUITexts()
    {
        // Tự tìm Canvas và Text trong Scene mới nếu bị rỗng
        TextMeshProUGUI[] texts = FindObjectsOfType<TextMeshProUGUI>();
        foreach (var t in texts)
        {
            if (t.gameObject.name.Contains("Timer") || t.text.Contains("TIME"))
            {
                timerText = t;
            }
            else if (t.gameObject.name.Contains("Death") || t.text.Contains("DEATH"))
            {
                deathText = t;
            }
        }
    }

    private void Update()
    {
        if (isGameActive)
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            if (timerText != null) 
                timerText.text = string.Format("TIME: {0:00}:{1:00}", minutes, seconds);
        }
    }

    public void AddDeath()
    {
        deathCount++;
        UpdateDeathUI();
    }

    private void UpdateDeathUI()
    {
        if (deathText != null) 
            deathText.text = "DEATHS: " + deathCount;
    }

    public void StopTimer()
    {
        isGameActive = false;
    }
}