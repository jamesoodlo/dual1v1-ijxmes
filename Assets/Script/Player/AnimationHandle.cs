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
    PlayerGroundCheck groundCheck;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
        inputHandle = GetComponent<InputHandle>();
        groundCheck = GetComponentInChildren<PlayerGroundCheck>();
        playerController = GetComponent<PlayerController>();
    }

    void Start()
    {
       
    }

    void Update()
    {
        if(!PV.IsMine)
        {
            return;
        }


        if(PV.IsMine)
        {
            anim.SetBool("isBlock", playerController.isBlocking);
        } 
  
    }   

    void FixedUpdate()
    {
        if(PV.IsMine)
            MoveAnimation();
    }

    public void MoveAnimation()
    {
        if(!groundCheck.isJumping) 
            anim.SetBool("isDodgeBack", false);
            anim.SetBool("isRolling", false); 

        if(inputHandle.sprint && inputHandle.move.y == 1)
        {
            anim.SetBool("isSprint", true);
        }
        else
        {
            anim.SetBool("isSprint", false);
            anim.SetFloat("Horizontal", inputHandle.move.x, 0.1f, Time.deltaTime);
            anim.SetFloat("Vertical", inputHandle.move.y, 0.1f, Time.deltaTime);
        }
    }

    public void RollforwardAnimation()
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

    public void HurtAnimation()
    {
        anim.SetTrigger("Hurt");
        inputHandle.move = Vector2.zero;
    }

    public void BlockReactAnimation()
    {
        anim.SetTrigger("isBlocked");
    }

    public void StartAttack()
    {
        playerController.isAttacking = true;
    } 

    public void ResetAttack()
    {
        playerController.isAttacking = false;
    } 
}
