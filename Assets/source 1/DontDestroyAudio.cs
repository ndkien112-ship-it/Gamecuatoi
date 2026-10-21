using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DontDestroyAudio : MonoBehaviour
{
    private static DontDestroyAudio instance;
    private AudioSource audioSource;

    [Header("Volume Settings")]
    public float menuVolume = 0.4f;   // Âm lượng ở Main Menu
    public float gameVolume = 0.2f;   // Âm lượng khi vào Game
    public float fadeSpeed = 0.5f;    // Tốc độ giảm âm lượng (càng nhỏ càng mượt)

    [Header("Scene Name")]
    public string mainMenuSceneName = "Main menu"; // Tên chính xác của Scene Menu

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        // Đăng ký lắng nghe sự kiện khi chuyển Scene
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Hủy đăng ký khi object bị hủy
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (audioSource == null) return;

        // Nếu là scene Main Menu -> Đặt volume 0.4, ngược lại (vào game) -> Giảm xuống 0.2
        if (scene.name == mainMenuSceneName)
        {
            StopAllCoroutines();
            StartCoroutine(FadeVolume(menuVolume));
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FadeVolume(gameVolume));
        }
    }

    IEnumerator FadeVolume(float targetVolume)
    {
        while (!Mathf.Approximately(audioSource.volume, targetVolume))
        {
            audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        audioSource.volume = targetVolume;
    }
}