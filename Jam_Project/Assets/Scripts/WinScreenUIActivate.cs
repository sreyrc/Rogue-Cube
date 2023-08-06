using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreenUIActivate : MonoBehaviour
{
    [SerializeField] GameObject winScreenUI;

    void DelayedActivate()
    {
        winScreenUI.SetActive(true);
    }
    public void ActivateWinScreen()
    {
        Invoke("DelayedActivate", 1.5f);
    }
}
