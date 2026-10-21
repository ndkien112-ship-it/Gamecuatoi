using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudioController : MonoBehaviour
{
    [Header("Audio SFX Clips")]
    public AudioClip jumpSound;       
    public AudioClip landSound;       
    public AudioClip lavaDeathSound;  

    [Header("Vapor Effect")]
    public GameObject vaporEffectPrefab;

    private AudioSource audioSource;
    private CharacterController controller;
    private bool wasInAir = false;
    private bool isDead = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();

        if (audioSource != null)
        {
            audioSource.spatialBlend = 0f;
        }
    }

    void Update()
    {
        if (isDead) return;

        if (Input.GetButtonDown("Jump"))
        {
            PlayJumpSound();
        }

        if (controller != null)
        {
            if (!controller.isGrounded)
            {
                wasInAir = true;
            }
            else if (controller.isGrounded && wasInAir)
            {
                PlayLandSound();
                wasInAir = false;
            }
        }
    }

    public void PlayJumpSound()
    {
        if (jumpSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(jumpSound);
            wasInAir = true;
        }
    }

    public void PlayLandSound()
    {
        if (landSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(landSound);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lava") && !isDead)
        {
            StartCoroutine(HandleLavaDeath());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Lava") && !isDead)
        {
            StartCoroutine(HandleLavaDeath());
        }
        else if (collision.gameObject.CompareTag("Ground") && wasInAir)
        {
            PlayLandSound();
            wasInAir = false;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Lava") && !isDead)
        {
            StartCoroutine(HandleLavaDeath());
        }
    }

    IEnumerator HandleLavaDeath()
    {
        isDead = true;

        if (GameManagerUI.instance != null)
        {
            GameManagerUI.instance.AddDeath();
        }

        if (controller != null)
        {
            controller.enabled = false;
        }

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.enabled = false;
        }

        if (vaporEffectPrefab != null)
        {
            GameObject vapor = Instantiate(vaporEffectPrefab, transform.position, Quaternion.identity);
            Destroy(vapor, 2.0f);
        }

        if (lavaDeathSound != null)
        {
            AudioSource.PlayClipAtPoint(lavaDeathSound, transform.position, 1.0f);
        }

        float delayTime = (lavaDeathSound != null) ? lavaDeathSound.length : 0.8f;
        yield return new WaitForSeconds(delayTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}