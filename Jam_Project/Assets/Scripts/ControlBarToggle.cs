using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBarToggle : MonoBehaviour
{
    [SerializeField] GameObject controlsBarUI;
    bool controlBarToggleBool = true;

    private void Start()
    {
        controlsBarUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            controlBarToggleBool = !controlBarToggleBool;
            controlsBarUI.SetActive(controlBarToggleBool);
        }
    }
}
