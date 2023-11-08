using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFx : MonoBehaviour
{
    public float volumeSfx;
    public AudioSource hurtSfx;
    public AudioSource slashSfx;
    public AudioSource footStepSfx;
    public AudioSource rollingSfx;
    public AudioSource landSfx;

    void Update()
    {
        if(hurtSfx != null) hurtSfx.volume = volumeSfx;
        if(slashSfx != null) slashSfx.volume = volumeSfx;
        if(footStepSfx != null) footStepSfx.volume = volumeSfx;
        if(rollingSfx != null) rollingSfx.volume = volumeSfx;
        if(landSfx != null) landSfx.volume = volumeSfx;
    }
}
