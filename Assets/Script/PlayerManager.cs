using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{

    PhotonView PV;
    
    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if(PV.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
        Transform spawnPoints = SpawnManager.Instance.GetSpawnpoint();
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","Player"), spawnPoints.position, spawnPoints.rotation, 0, new object[] { PV.ViewID });
        Debug.Log("Instantiated Player Controller");
    }
}
