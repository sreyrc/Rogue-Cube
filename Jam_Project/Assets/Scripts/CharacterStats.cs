using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Class that stores all sorts of player stats
public class CharacterStats : MonoBehaviour
{
    [SerializeField]
    private float hp;
    public float maxHp;
    
    public float moveSpeed, rotationSpeed;

    public float Hp { 
        get { return hp; } 
        set 
        {
            if (value <= maxHp) hp = value;
            else hp = maxHp;
        } 
    }
}
