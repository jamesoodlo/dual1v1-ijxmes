using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerStat : MonoBehaviourPun
{
    
    AnimationHandle animHandle;
    PlayerController playerController;
    
    public PhotonView PV;

    [SerializeField] Slider healthBar;

    const float maxHealth = 100f;

	float currentHealth = maxHealth;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        animHandle = GetComponent<AnimationHandle>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;   
    }

    void OnTriggerEnter(Collider other)
    {
        if(PV.IsMine)
        {
            if(other.gameObject.tag == "Damage" && !playerController.isBlocking && !playerController.isParry)
            {
                if(other.GetComponent<Weapons>().isAttacking)
                {
                    PV.RPC("RPC_TakeDamage", PV.Owner, other.GetComponent<Weapons>().damage); 
                }
                else if(other.GetComponent<Weapons>().isAttacking && playerController.isBlocking && !playerController.isParry)
                {
                    //PV.RPC("RPC_TakeDamage", PV.Owner, other.GetComponent<Weapons>().damage * 70 / 100); 
                    animHandle.BlockReactAnimation();
                }
                else if(other.GetComponent<Weapons>().isAttacking && playerController.isBlocking && playerController.isParry)
                {
                    //PV.RPC("RPC_TakeDamage", PV.Owner, other.GetComponent<Weapons>().damage * 70 / 100); 
                    //animHandle.BlockReactAnimation();
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
        animHandle.HurtAnimation();
        Debug.Log("Get Damage" + damage);
	}
}
