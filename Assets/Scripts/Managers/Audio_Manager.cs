using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    [SerializeField] AudioSource src;    

    Dictionary<Sounds.Sound, SoundObject> soundClips = new Dictionary<Sounds.Sound, SoundObject>();

    public AudioClip click_Clip;

    float pitchValue = 1.0f;


    void Start()
    {
        SoundObject obj1 = new SoundObject();
        obj1.clip = click_Clip;
        obj1.defaultPitch = 2.0f;
        obj1.defaultVolume = 1.0f;
        soundClips.Add(Sounds.Sound.Click, obj1);
    }

    public void PlaySound(Sounds.Sound sound)
    {
        SoundObject soundObj = soundClips[Sounds.Sound.Click];
        src.clip = soundObj.clip;
        src.Play();
        src.pitch = soundObj.defaultPitch;
        src.volume = soundObj.defaultVolume;
        //pitchValue += 0.01f;
    }
}

public class SoundObject
{
    public AudioClip clip;
    public float defaultPitch = 1.0f;
    public float defaultVolume = 1.0f;
}

public class Sounds
{
    public enum Sound
    {
        Click
    };
}
