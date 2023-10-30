using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class NickNameDisplay : MonoBehaviour
{
    [SerializeField] PhotonView PV;
    [SerializeField] TextMeshProUGUI nickNameText;

    void Start()
    {
        if(PV.IsMine)
        {
            nickNameText.gameObject.SetActive(false);
        }

        nickNameText.text = PV.Owner.NickName;
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform.position);
    }
}
