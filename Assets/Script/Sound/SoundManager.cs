using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //public SettingData settingData;

    public SoundFx[] soundFx;

    public AudioSource effectSound;

    void Start()
    {

    }

    void Update()
    {
        soundFx = FindObjectsOfType<SoundFx>();

        for (int i = 0; i < soundFx.Length; i++)
        {
            if(soundFx != null)
                soundFx[i].volumeSfx = effectSound.volume;
        }
    }
}
