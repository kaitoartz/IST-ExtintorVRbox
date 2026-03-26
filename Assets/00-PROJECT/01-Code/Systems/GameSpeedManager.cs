using UnityEngine;

public class GameSpeedManager : MonoBehaviour
{
    private bool isSpeedUp = false;
    private float currentSpeedFactor = 1f;

    public bool IsSpeedUp => isSpeedUp;

    public float CurrentSpeedFactor => currentSpeedFactor;
    
    public void ActivateSpeedUp(float speedFactor)
    {
        Time.timeScale = speedFactor;

        currentSpeedFactor = speedFactor;

        AudioSource[] audioSources = Resources.FindObjectsOfTypeAll<AudioSource>();
        
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.gameObject.scene.isLoaded)
            {
                audioSource.pitch = speedFactor > 1f ? speedFactor : 1f;
            }
        }

        Debug.Log("Game speed set to: " + speedFactor);

        isSpeedUp = speedFactor > 1f;
    }
}