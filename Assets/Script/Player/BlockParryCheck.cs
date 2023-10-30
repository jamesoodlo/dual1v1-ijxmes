using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BlockParryCheck : MonoBehaviourPun, IPunObservable
{
    PlayerController playerController;
    public Weapons weapon;
    public bool isParry;

    void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        isParry = playerController.isParry;
    }

    [PunRPC]
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isParry);

        }
        else
        {
            isParry = (bool)stream.ReceiveNext();
        }
    }
}
