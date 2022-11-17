using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Audio Manager", menuName="ScriptableObject/Audio Manager")]
public class AudioManager : SingletonScriptableObject<AudioManager>
{
    public float volume = 1;
    public float pitch = 1;

    public void PlayAudio(AudioSource source, AudioClip clip) {
        source.volume = volume;
        source.pitch = pitch;
        source.clip = clip;
        //if(!source.isPlaying)
        source.Play(); 
    }
}
