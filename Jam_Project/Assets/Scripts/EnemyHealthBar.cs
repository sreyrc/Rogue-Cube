using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    CharacterStats stats;
    float originalWidth;
    bool statsLoaded = false;
    MeshRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        originalWidth = transform.localScale.x;
        renderer = GetComponentInChildren<MeshRenderer>();
    }

    // Update is called once per frame  
    void Update()
    {
        // Loading paren's (enemy's) stats in update loop
        // Since enemy is assigned as parent later - and not in the beginning
        if(!statsLoaded)
        {
            stats = GetComponentInParent<CharacterStats>();
            statsLoaded = true;
        }
        float t = stats.Hp / stats.maxHp;
        if(t < 0) { t = 0; } // to prevent bar to go in the negative direction
        transform.localScale = new Vector3(t * originalWidth, transform.localScale.y, transform.localScale.z);
        renderer.material.color = new Color(Mathf.Lerp(1.0f, 0.0f, t), Mathf.Lerp(0.0f, 1.0f, t), 0);
    }
}
