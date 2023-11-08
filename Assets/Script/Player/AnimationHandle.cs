using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class AnimationHandle : MonoBehaviourPun
{
    PhotonView PV;
    Animator anim;
    InputHandle inputHandle;
    PlayerController playerController;
    UIManager uiManager;
    SoundFx sfx;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
        inputHandle = GetComponent<InputHandle>();
        playerController = GetComponent<PlayerController>();
        uiManager = GetComponentInChildren<UIManager>();
        sfx = GetComponentInChildren<SoundFx>();
    }

    void Start()
    {
       
    }

    void Update()
    {
        if(!PV.IsMine)
            return;


        if(PV.IsMine)
            anim.SetBool("isBlock", playerController.isBlocking);
            anim.SetBool("Sitting", uiManager.isPaused);
  
    }   

    public void FallingAnimation()
    {
        if(!playerController.grounded)
        {
            anim.SetBool("Falling", true);
        }
        else
        {
            anim.SetBool("Falling", false);
        }
    }

    public void MoveAnimation()
    {
        anim.SetFloat("Horizontal", inputHandle.move.x, 0.1f, Time.deltaTime);
        anim.SetFloat("Vertical", inputHandle.move.y, 0.1f, Time.deltaTime);
    }

    public void SprintAnimation(bool sprinting)
    {
        anim.SetBool("isSprint", sprinting);
    }

    public void RollForwardAnimation()
    {
        anim.SetTrigger("isRolling"); 
    }

    public void DodgeBackAnimation()
    {
        anim.SetTrigger("isDodgeBack"); 
    }

    public void ParriedAnimation()
    {
        anim.SetTrigger("Parried");
    }

    public void AttackAnimation(int _currentAttack)
    {
        if(_currentAttack != 0) anim.SetTrigger("Attack" + _currentAttack);
    }

    public void ParryAnimation()
    {
        anim.SetTrigger("Parry");
    }

    public void HurtAnimation()
    {
        anim.SetTrigger("Hurt");
        inputHandle.move = Vector2.zero;
    }

    public void BlockReactAnimation(string animName)
    {
        anim.Play(animName);
    }

    public void StartAttack()
    {
        playerController.isAttacking = true;
    } 

    public void ResetAttack()
    {
        playerController.isAttacking = false;
    } 

    public void Sfx(string sfxName)
    {
        if(sfxName == "Slash") 
        {
            sfx.footStepSfx.Stop();
            sfx.slashSfx.Play();
        }
        else if(sfxName == "Roll") 
        {
            sfx.footStepSfx.Stop();
            sfx.rollingSfx.Play();
        }
        else if(sfxName == "FootStep") 
        {
            if(inputHandle.move != Vector2.zero)
            {
                sfx.footStepSfx.Play();
            }
            else
            {
                sfx.footStepSfx.Stop();
            }
        }
        else if(sfxName == "Land") 
        {
            sfx.footStepSfx.Stop();
            sfx.landSfx.Play();
        }
        else
        {
            //sfx.footStepSfx.Stop();
        }
    }
}
