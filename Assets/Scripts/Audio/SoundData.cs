using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class SoundData
{
    public enum AudioType
    {
        sfx,
        bgm,
        ambient
    }

    public AudioType audioType;

    public string name;
    public AudioClip[] clips;

    [Range(0f, 1f)]
    public float volume = 1;

    [Range(0.1f, 3f)]
    public float pitch = 1;

    public bool loop;
    public bool replayWhenCalled;

    [HideInInspector]
    public AudioSource source;

    public void Play()
    {
        if (!source.isPlaying || replayWhenCalled)
        {
            source.clip = clips[Random.Range(0, clips.Length)];

            if (audioType == AudioType.sfx)
            {
                source.volume = GameData.sfxValue * volume;
            }

            if (audioType == AudioType.bgm)
            {
                source.volume = GameData.musicValue * volume;
            }
            if (audioType == AudioType.ambient)
            {
                source.volume = GameData.ambientValue * volume;
            }

            source.Play();
        }
    }
}
