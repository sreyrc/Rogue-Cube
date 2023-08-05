using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LerpToggle : MonoBehaviour
{
    [SerializeField] private float maxAlpha;

    PlayerInteractionLogic playerInteractionLogic;
    Image redOverlay;
    float alpha = 0.0f;
    bool alphaUp = false;
    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        playerInteractionLogic = FindAnyObjectByType<PlayerInteractionLogic>();
        redOverlay = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInteractionLogic.isHurt && !alphaUp) 
        { 
            alphaUp = true;
            startTime = Time.time;
        }
        if (alphaUp)
        {
            alpha = Mathf.Lerp(0.0f, maxAlpha, (Time.time - startTime) / 0.1f);

        }
        else if (alpha > 0.0f)
        {
            alpha = Mathf.Lerp(maxAlpha, 0.0f, (Time.time - startTime) / 0.1f);
        }

        if (alpha >= maxAlpha)
        {
            playerInteractionLogic.isHurt = false;
            startTime = Time.time;
            alphaUp = false;
        }

        redOverlay.color = new Color(redOverlay.color.r, 0, 0, alpha);
    }
}
