using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Weapons : MonoBehaviourPun, IPunObservable
{
    PhotonView PV;
    PlayerController playerController;
    public BlockParryCheck blockAndparry;
    public float damage = 10;
    public bool isAttacking;
    public bool isParried;

    void Start()
    {
        PV = GetComponentInParent<PhotonView>();
        playerController = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        isAttacking = playerController.isAttacking;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Parry")
        {
            if(other.GetComponent<BlockParryCheck>().isParry)
            {
                isParried = true;
            }
        }
    }

    [PunRPC]
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isParried);

        }
        else
        {
            isParried = (bool)stream.ReceiveNext();
        }
    }

    
}
