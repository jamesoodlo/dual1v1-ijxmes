using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    PhotonView PV;
    InputHandle inputHandle;
    Animator anim;
    AnimationHandle animHandle;
    Weapons weapon;

    [SerializeField] Slider healthBar;
    [SerializeField] GameObject ui;

    [Header("Movement System")]
    Vector3 moveAmount, smoothMoveVelocity;
    Rigidbody rb;
    float timeSinceRollDodge;
    public bool grounded, dodgeBack, rolling;
    [SerializeField] float mouseSensitivity, walkSpeed, sprintSpeed, jumpForce, smoothTime;

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
        weapon = GetComponentInChildren<Weapons>();
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
        }

        
    }

    void Update()
    {
        if(!PV.IsMine)
            return;

        if(PV.IsMine)
        {
            Look();
            Move();
            Jump();
            Attack();
            Block();
            Parried(); 
        }
        
    }

    void FixedUpdate()
    {
        if(!PV.IsMine)
            return;

        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    public void Look()
    {
        transform.Rotate(Vector3.up * inputHandle.look.x * mouseSensitivity);
    }

    public void Move()
    {
        Vector3 moveDir = new Vector3(inputHandle.move.x, 0, inputHandle.move.y).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (inputHandle.sprint ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
    }

    public void Jump()
    {
        timeSinceRollDodge += Time.deltaTime;

        if(inputHandle.jump && inputHandle.move != Vector2.zero && !isAttacking && timeSinceRollDodge > 1.0f)
        {
            if(inputHandle.move.y == 1) 
            {
                rolling = true;
                animHandle.RollforwardAnimation();
                timeSinceRollDodge = 0f;
                Rollforward();
            }
            else if(inputHandle.move.y == -1) 
            {
                dodgeBack = true;
                animHandle.DodgeBackAnimation();
                timeSinceRollDodge = 0f;
                DodgeBackward();
            }  
        }

        if(rolling)
        {
            StartCoroutine(stopRoll(0.15f));
        }

        if(dodgeBack)
        {
            StartCoroutine(stopDogde(0.15f));
        }
    }

    public void Rollforward()
    {   
        float rollSpeed = 15f;

        rb.AddForce(transform.forward * rollSpeed, ForceMode.Acceleration);
        
        if(!grounded)
        {
            
        }
    }

    public void DodgeBackward()
    {   
        float dodgeSpeed = 20f;

        rb.AddForce(-transform.forward * dodgeSpeed, ForceMode.Acceleration);

        if(!grounded)
        {
            
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

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    public void Attack()
    {
        timeSinceAttack += Time.deltaTime;

        if (inputHandle.attack && timeSinceAttack > 0.5f)
        {
            currentAttack++;
                
            inputHandle.move = Vector2.zero;
                
            if (currentAttack > 3) currentAttack = 1;
                    
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
            weapon.isParried = false;
        }
    }

    [PunRPC]
    void RPC_Attack(int _currentAttack, PhotonMessageInfo info)
    {
        if(_currentAttack != 0) animHandle.AttackAnimation(_currentAttack);

        Debug.Log(_currentAttack);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isAttacking);
            stream.SendNext(isParry);
            stream.SendNext(isParried);
            stream.SendNext(isBlocking);
        }
        else
        {
            isAttacking = (bool)stream.ReceiveNext();
            isParried = (bool)stream.ReceiveNext();
            isParry = (bool)stream.ReceiveNext();
            isBlocking = (bool)stream.ReceiveNext();
        }
    }
}
