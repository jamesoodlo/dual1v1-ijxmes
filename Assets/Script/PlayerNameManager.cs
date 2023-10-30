using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerNameManager : MonoBehaviour
{
    [SerializeField] TMP_InputField inputNickName;

    void Start()
    {
        
    }

    public void OnNickNameInputValueChange()
    {
        if(string.IsNullOrEmpty(inputNickName.text))
        {
            PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");    
        }
        else
        {
            PhotonNetwork.NickName = inputNickName.text;
        }
        
        
    }
}
