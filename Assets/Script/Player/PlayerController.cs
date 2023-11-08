using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    PhotonView PV;
    Animator anim;
    Weapons weapon;
    PlayerStat playerStat;
    InputHandle inputHandle;
    AnimationHandle animHandle;
    PlayerManager playerManager;
    UIManager uiManager;
    
    [SerializeField] GameObject ui;

    [Header("Movement System")]
    Vector3 moveAmount, smoothMoveVelocity;
    Rigidbody rb;
    [SerializeField] float timeSinceRollDodge;
    public bool grounded, dodgeBack, rolling, sprinting;
    [SerializeField] float mouseSensitivity, moveSpeed, walkSpeed, sprintSpeed, jumpForce, smoothTime;

    [Header("Attack System")]
    public bool isAttacking = false;
    [SerializeField] int currentAttack = 0;
    [SerializeField] float timeSinceAttack;

    [Header("Block & Parry System")]
    public bool isBlocking = false;
    public bool isParry = false;
    [SerializeField] bool isParried = false;
    [SerializeField] GameObject blockTrigger;
    [SerializeField] float timeSinceBlock;

    void Awake()
    {
        inputHandle = GetComponent<InputHandle>();
        animHandle = GetComponent<AnimationHandle>();
        playerStat = GetComponent<PlayerStat>();
        weapon = GetComponentInChildren<Weapons>();
        uiManager = GetComponentInChildren<UIManager>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if(!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(ui);
            Destroy(rb);
        }
    }

    void Update()
    {
        if(!PV.IsMine)
            return;

        
        animHandle.FallingAnimation();

        if(!uiManager.isPaused)
            Look();

        if(grounded && !uiManager.isPaused)
        {
            Move();
            Attack();
            Block();
            //Parried(); 
            Sprint();
        }
        

        timeSinceRollDodge += Time.deltaTime;
    }

    void FixedUpdate()
    {
        if(!PV.IsMine)
            return;

        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);

        if(timeSinceRollDodge > 0.75f && playerStat.currentStamina > 15f && grounded)
            RollForward();
            DodgeBackward(); 
    }

    public void Look()
    {
        transform.Rotate(Vector3.up * inputHandle.look.x * mouseSensitivity);
    }

    public void Move()
    {
        Vector3 moveDir = new Vector3(inputHandle.move.x, 0, inputHandle.move.y).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * moveSpeed, ref smoothMoveVelocity, smoothTime);

        animHandle.MoveAnimation();
    }

    public void Sprint()
    {
        if(inputHandle.sprint && inputHandle.move.y == 1 && playerStat.currentStamina > 1.0f && !rolling && !dodgeBack) 
        {
            playerStat.TakeStamina(0.1f);
            moveSpeed = sprintSpeed;
            sprinting = true;
            animHandle.SprintAnimation(true);
        }
        else
        {
            sprinting = false;
            animHandle.SprintAnimation(false);
            moveSpeed = walkSpeed;
        }
    }

    public void Jump()
    {
        if(inputHandle.jump) 
        {
              
        }
    }

    public void RollForward()
    {
        if(inputHandle.jump) 
        {
            if(inputHandle.move.y == 1)
            {
                rolling = true;
                //playerStat.TakeStamina(15f);
                animHandle.RollForwardAnimation();
                timeSinceRollDodge = 0f;
                StartCoroutine(stopRoll(0.25f));

                float rollSpeed = 20f;
                rb.AddForce(transform.forward * rollSpeed, ForceMode.Acceleration);
            }
        }
    }

    public void DodgeBackward()
    {
        if(inputHandle.jump) 
        {
            if(inputHandle.move.y == -1)
            {
                dodgeBack = true;
                //playerStat.TakeStamina(5.0f);
                animHandle.DodgeBackAnimation();
                timeSinceRollDodge = 0f;
                StartCoroutine(stopDogde(0.25f));

                float dodgeSpeed = 20f;
                rb.AddForce(-transform.forward * dodgeSpeed, ForceMode.Acceleration);
            }
        } 
    }

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    public void Attack()
    {
        timeSinceAttack += Time.deltaTime;

        if (inputHandle.attack && timeSinceAttack > 0.5f && !isParried && playerStat.currentStamina > 15)
        {
            currentAttack++;

            //playerStat.TakeStamina(15f);
                    
            inputHandle.move = Vector2.zero;
                    
            if (currentAttack > 4) currentAttack = 1;
                        
            //Reset Attack When Time out
            if (timeSinceAttack > 1.0f) currentAttack = 1;

            //Call Trigger Attack Animation
            PV.RPC("RPC_Attack", RpcTarget.AllBufferedViaServer, currentAttack);
                
            //Reset Timer
            timeSinceAttack = 0;
        }
    }

    private void Block()
    {
        blockTrigger.SetActive(isBlocking);

        if(inputHandle.block)
        {
            timeSinceBlock += Time.deltaTime;
            inputHandle.move = Vector2.zero;

            isBlocking = true;
            isParry = true;

            if(timeSinceBlock > 0.5f)
            {
                isParry = false;
            }
        }
        else
        {
            isParry = false;
            isBlocking = false;
            timeSinceBlock = 0;
        }
    } 

    public void Parried()
    {
        isParried = weapon.isParried;

        if(isParried)
        {
            inputHandle.move = Vector2.zero;

            animHandle.ParriedAnimation();
            
            //weapon.isParried = false;

            StartCoroutine(resetBool(weapon.isParried, 1.5f));
        }
    }

    IEnumerator stopRoll(float stopTime)
    {
        rolling = true;
        yield return new WaitForSeconds(stopTime);
        rolling = false;
        rb.velocity = Vector3.zero;
    }

    IEnumerator stopDogde(float stopTime)
    {
        dodgeBack = true;
        yield return new WaitForSeconds(stopTime);
        dodgeBack = false;
        rb.velocity = Vector3.zero;
    }

    IEnumerator resetBool(bool BoolValue, float stopTime)
    {
        BoolValue = true;
        yield return new WaitForSeconds(stopTime);
        BoolValue = false;
    }

    [PunRPC]
    void RPC_Attack(int _currentAttack, PhotonMessageInfo info)
    {
        if(_currentAttack != 0) animHandle.AttackAnimation(_currentAttack);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isAttacking);
            stream.SendNext(isParry);
            stream.SendNext(isParried);
            stream.SendNext(isBlocking);
            stream.SendNext(timeSinceRollDodge);
        }
        else
        {
            isAttacking = (bool)stream.ReceiveNext();
            isParried = (bool)stream.ReceiveNext();
            isParry = (bool)stream.ReceiveNext();
            isBlocking = (bool)stream.ReceiveNext();
            timeSinceRollDodge = (float)stream.ReceiveNext();
        }
    }
}
