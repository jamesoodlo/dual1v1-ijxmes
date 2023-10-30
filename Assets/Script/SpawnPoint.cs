using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] GameObject playerMockUp;

    void Awake()
    {
        playerMockUp.SetActive(false);
    }
}
