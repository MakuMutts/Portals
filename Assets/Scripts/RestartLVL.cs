using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLVL : MonoBehaviour
{
    
    public AudioClip deathSound;

    private static bool playerIsDead = false;
    private static AudioClip staticDeathSound;

    private void Awake()
    {
        staticDeathSound = deathSound;
        DontDestroyOnLoad(gameObject); // Не уничтожаем объект при загрузке сцены
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerIsDead)
        {
            playerIsDead = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (playerIsDead)
        {
            // Создаем временный объект для воспроизведения звука
            GameObject soundPlayer = new GameObject("DeathSoundPlayer");
            AudioSource audioSource = soundPlayer.AddComponent<AudioSource>();
            audioSource.PlayOneShot(staticDeathSound);

            // Уничтожаем после проигрывания
            Destroy(soundPlayer, staticDeathSound.length);

            playerIsDead = false;
            Destroy(gameObject); // Уничтожаем основной объект после использования
        }
    }
}