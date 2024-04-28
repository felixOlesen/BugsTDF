using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] List<Sound> sounds;

    public static AudioManager audioManagerInstance;

    private void Awake() {

        // Checks for Singleton Object
        if(audioManagerInstance == null) {
            audioManagerInstance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sound sound in sounds) {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.clip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.loop = sound.loop;
        }
    }

    public void PlaySound(string name) {
        Sound currentSound = sounds.Find(sound => sound.name == name);
        if(currentSound == null){
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        
        currentSound.audioSource.Play();

    }
}
