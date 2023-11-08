using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerStat : MonoBehaviourPunCallbacks
{
    
    AnimationHandle animHandle;
    PlayerController playerController;
    PlayerManager playerManager;
    SoundFx sfx;

    public PhotonView PV;

    [SerializeField] Slider healthBar;
    [SerializeField] Slider staminaBar;

    const float maxHealth = 100f;
    public float currentHealth = maxHealth;

    const float maxStamina = 100f;
    public float currentStamina = maxStamina;
    float regenStaminaRate = 15; 
    float timeSinceLastRegen = 0f;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        animHandle = GetComponent<AnimationHandle>();
        playerController = GetComponent<PlayerController>();
        sfx = GetComponentInChildren<SoundFx>();
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;   

        staminaBar.maxValue = maxStamina;
        staminaBar.value = currentStamina;

        RegenrateStamina();

        //TakeDamage(0.0f);
        
        if(transform.position.y < -10f) // Die if you fall out of the world
		{
			Die();
		}
        
    }

    public void RegenrateStamina()
    { 
        timeSinceLastRegen += Time.deltaTime;

        if(currentStamina < 0) currentStamina = 0;

        if (currentStamina < maxStamina)
        { 
            if (timeSinceLastRegen >= 1.0f)
            {
                currentStamina += regenStaminaRate * Time.deltaTime;
            }
        }
    }

    public void TakeStamina(float drainStamina)
    {
        currentStamina -= drainStamina;
    }

    public void TakeDamage(float damage)
	{
		PV.RPC(nameof(RPC_TakeDamage), PV.Owner, damage);
	}

    void OnTriggerEnter(Collider other)
    {
        if(PV.IsMine)
        {
            if(other.gameObject.tag == "Damage")
            {
                if(other.GetComponent<Weapons>().isAttacking && !playerController.isBlocking && !playerController.isParry)
                {
                    TakeDamage(other.GetComponent<Weapons>().damage);
                }
                else if(other.GetComponent<Weapons>().isAttacking && playerController.isBlocking && !playerController.isParry)
                {
                    TakeDamage(other.GetComponent<Weapons>().damage * 60 / 100);
                    animHandle.BlockReactAnimation("Blocked");
                    
                }
                else if(other.GetComponent<Weapons>().isAttacking && playerController.isBlocking && playerController.isParry)
                {
                    //animHandle.ParryAnimation();
                }
                else
                {

                }
            }
        }  
    }

    [PunRPC]
    void RPC_TakeDamage(float damage, PhotonMessageInfo info)
	{
	    currentHealth -= damage;
        sfx.hurtSfx.Play();
        animHandle.HurtAnimation();
        
        if(currentHealth <= 0)
		{
            Die();
            PlayerManager.Find(info.Sender).GetKill();
		}
	}

    void Die()
    {
        playerManager.Die();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
        }
        else
        {
            currentHealth = (float)stream.ReceiveNext();
        }
    }
   
}
