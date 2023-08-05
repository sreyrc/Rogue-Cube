using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    CharacterStats playerStats;
    RectTransform rectTransform;
    Image image;

    float originalBarWidth;
    float playerPrevHp;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").
            GetComponent<CharacterStats>();

        playerPrevHp = playerStats.Hp;

        image = GetComponent<Image>();
        image.color = Color.green;

        rectTransform = GetComponent<RectTransform>();
        originalBarWidth = rectTransform.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        // If player health has been altered
        if (playerStats.Hp != playerPrevHp)
        {
            playerPrevHp = playerStats.Hp;

            // Change color
            // The less the health, the more redder and less greener
            float t = playerStats.Hp / playerStats.maxHp;
            image.color = new Color(Mathf.Lerp(1.0f, 0.0f, t), Mathf.Lerp(0.0f, 1.0f, t), 0);

            // Change Health Bar Length
            Rect rect = rectTransform.rect;
            rectTransform.sizeDelta = new Vector2 (t * originalBarWidth, rect.height);
        }   
    }
}
