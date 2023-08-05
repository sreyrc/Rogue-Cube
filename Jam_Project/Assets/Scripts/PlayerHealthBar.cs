using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    CharacterStats playerStats;
    RectTransform rectTransformFront, rectTransformBack;
    
    Image healthBarFront, healthBarBack;

    float maxBarWidth;
    float playerOriginalMaxHp, playerPrevHp, playerPrevMaxHp;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").
            GetComponent<CharacterStats>();

        playerPrevHp = playerStats.Hp;
        playerOriginalMaxHp = playerPrevMaxHp = playerStats.maxHp;

        healthBarFront = transform.GetChild(1).GetComponent<Image>();
        healthBarBack = transform.GetChild(0).GetComponent<Image>();

        healthBarFront.color = Color.green;
        healthBarBack.color = Color.grey;

        rectTransformFront = transform.GetChild(1).GetComponent<RectTransform>();
        rectTransformBack = transform.GetChild(0).GetComponent<RectTransform>();
        
        maxBarWidth = rectTransformBack.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        bool playerHpAltered = playerStats.Hp != playerPrevHp;
        bool playerMaxHpAltered = playerStats.maxHp != playerPrevMaxHp;

        // If player health has been altered
        if (playerHpAltered || playerMaxHpAltered)
        {
            if (playerMaxHpAltered) 
            { 
                maxBarWidth *= (playerStats.maxHp / playerPrevMaxHp);
                playerPrevMaxHp = playerStats.maxHp;

                Rect rectBack = rectTransformBack.rect;
                rectTransformBack.sizeDelta = new Vector2(maxBarWidth, rectBack.height);
            }
            else { playerPrevHp = playerStats.Hp; }

            // Change color
            // The less the health, the more redder and less greener
            float t = playerStats.Hp / playerStats.maxHp;
            healthBarFront.color = new Color(Mathf.Lerp(1.0f, 0.0f, t), Mathf.Lerp(0.0f, 1.0f, t), 0);

            // Change Health Bar Length
            Rect rectFront = rectTransformFront.rect;
            rectTransformFront.sizeDelta = new Vector2 (t * maxBarWidth, rectFront.height);
        }   
    }
}
