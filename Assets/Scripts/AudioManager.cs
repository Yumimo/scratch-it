using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] public AudioSource bgmSource;
    [SerializeField] public AudioSource sfxSoruce;
    [SerializeField] public SfxData[] sfxTable;

    private int currentSfx;

    [Serializable]
    public class SfxData
    {
        [Range(0,1)]
        public float volume;
        public AudioClip clip;
    }
    void Awake()
    {
       Instance = this;
    }

    void Start()
    {
        
    }

    public void PlaySfx(int _index)
    {
        if(sfxSoruce == null) return;
        if(currentSfx == _index && sfxSoruce.isPlaying) return;
        sfxSoruce.clip = sfxTable[_index].clip;
        sfxSoruce.volume = sfxTable[_index].volume;
        sfxSoruce.Play();
    }
}
