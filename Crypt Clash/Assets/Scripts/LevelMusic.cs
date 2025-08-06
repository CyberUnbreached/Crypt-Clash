using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LevelMusic : MonoBehaviour
{
    public AudioClip musicClip;

    void Start()
    {
        AudioSource musicSource = GetComponent<AudioSource>();
        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.spatialBlend = 0f; 
        musicSource.Play();
    }
}
