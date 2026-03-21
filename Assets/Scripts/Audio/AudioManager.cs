using UnityEngine.Audio;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    public SoundData[] sounds;
    Dictionary<string, SoundData> soundsDict;
    public static AudioManager instance;
    public UnityEvent OnChangeAudioValue;

    // Start is called before the first frame update
    void Start()
    {
        soundsDict = sounds.ToDictionary(sound => sound.name);
        Play("BGM");
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (SoundData sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;

            sound.source.loop = sound.loop;
        }
    }

    public void Play(string name)
    {
        //SoundData s = Array.Find(sounds, sound => sound.name == name);
        if (!soundsDict.ContainsKey(name)) return;
        SoundData s = soundsDict[name];

        if (!GameData.sfxOn && s.audioType == SoundData.AudioType.sfx)
        {
            return;
        }

        if (!GameData.musicOn && s.audioType == SoundData.AudioType.bgm)
        {
            return;
        }

        if (s == null)
        {
            return;
        }

        if (!s.source.isPlaying || s.replayWhenCalled)
        {
            s.Play();
        }
    }

    public void Stop(string name)
    {
        //SoundData s = Array.Find(sounds, sound => sound.name == name);
        if (!soundsDict.ContainsKey(name)) return;
        SoundData s = soundsDict[name];

        if (s == null)
        {
            return;
        }

        if (s.source.isPlaying)
        {
            s.source.Stop();
        }
    }

    public bool IsPlaying(string name)
    {
        //SoundData s = Array.Find(sounds, sound => sound.name == name);
        if (!soundsDict.ContainsKey(name)) return false;
        SoundData s = soundsDict[name];

        if (s == null) return false;

        return (s.source.isPlaying);
    }
    public void OnVolumeChange()
    {
        foreach (SoundData sound in sounds)
        {
            if (sound.audioType == SoundData.AudioType.sfx)
            {
                sound.source.volume = GameData.sfxValue * sound.volume;
            }

            if (sound.audioType == SoundData.AudioType.bgm)
            {
                sound.source.volume = GameData.musicValue * sound.volume;
            }

            if (sound.audioType == SoundData.AudioType.ambient)
            {
                sound.source.volume = GameData.ambientValue * sound.volume;
            }
        }

        OnChangeAudioValue.Invoke();
    }
}
