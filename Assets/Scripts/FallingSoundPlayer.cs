using UnityEngine;

public class FallingSoundPlayer : MonoBehaviour
{
    public AudioClip[] fallSounds;
    
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayRandomSound()
    {
        if (fallSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, fallSounds.Length);
            audioSource.PlayOneShot(fallSounds[randomIndex]);
        }
    }
}

