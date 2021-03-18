using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonBase<SoundManager>
{
    //public List<AudioClip> BGM;

    public int maxSFXSource = 10;
    //AudioSource BGMsource;
    AudioSource[] SFXSources;
    private void OnEnable()
    {/*
        BGMsource = gameObject.AddComponent<AudioSource>();
        BGMsource.playOnAwake = false;
        BGMsource.volume =0.4f;
        BGMsource.loop = true;*/
        SFXSources = new AudioSource[maxSFXSource];
        for (int i = 0; i < SFXSources.Length; i++)
        {
            SFXSources[i] = gameObject.AddComponent<AudioSource>();
            SFXSources[i].playOnAwake = false;
            SFXSources[i].volume = 1;
            SFXSources[i].loop = false;
        }
    }/*
    public void PlayBGM(int n, bool isLoop = true, float volume = 0.4f)
    {
        BGMsource.clip = BGM[n];

        BGMsource.volume = volume;
        BGMsource.loop = isLoop;
        BGMsource.Play();
        return;

    }
    public void PlayBGM(string name, bool isLoop = true, float volume = 0.4f)
    {
        for (int i = 0; i < BGM.Count; i++)
        {
            if (BGM[i].name.CompareTo(name) == 0)
            {
                BGMsource.clip = BGM[i];

                BGMsource.volume = volume;
                BGMsource.loop = isLoop;
                BGMsource.Play();
                return;
            }
        }
        Debug.LogError(name + " is not exist!");
    }
    public void StopBGM()
    {
        if (BGMsource)
        {
            if (BGMsource.isPlaying)
            {
                BGMsource.Stop();
            }
        }
    }*/
    public void PlaySFX(AudioClip _clip, bool isLoop = false, float volume = 0.7f)
    {
        if (_clip == null) return;
        AudioSource source = GetEmptyAudio();
        source.clip = _clip;

        source.volume = volume;
        source.loop = isLoop;
        source.Play();
        return;

    }
    public void StopSFX(string name)
    {
        for (int i = 0; i < SFXSources.Length; i++)
        {
            if (SFXSources[i].isPlaying)
            {
                if (SFXSources[i].clip.name.CompareTo(name) == 0)
                {
                    SFXSources[i].Stop();
                }
            }
        }

    }
    private AudioSource GetEmptyAudio()
    {
        float maxP = 0;
        int maxIdx = 0;
        for (int i = 0; i < SFXSources.Length; i++)
        {
            if (!SFXSources[i].isPlaying)
            {
                return SFXSources[i];
            }
            float p = SFXSources[i].time / SFXSources[i].clip.length;
            if (p > maxP && !SFXSources[i].loop)
            {
                maxP = p;
                maxIdx = i;
            }
        }
        return SFXSources[maxIdx];

    }
}
