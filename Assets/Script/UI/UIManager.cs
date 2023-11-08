using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class UIManager : MonoBehaviour
{
    UIInput uiInput;

    public bool isPaused = false;
    [SerializeField] GameObject pausePanel;
    

    void Awake()
    {
        uiInput = GetComponent<UIInput>();
    }

    void Start()
    {
        Cursor.visible = isPaused;
        pausePanel.SetActive(false);
    }

    void Update()
    {
        Cursor.visible = isPaused;

        if(uiInput.escape)
        {
            isPaused = true;
        }

        pausePanel.SetActive(isPaused);
    }

    public void Resume()
    {
        isPaused = false;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
