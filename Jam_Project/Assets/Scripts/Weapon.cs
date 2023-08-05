using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Base damamge stats of this weapon
    [SerializeField] float baseAttackDamage, baseSpecialDamage;

    // Percentage increase over base
    [SerializeField] public float attackBonus, specialBonus;

    [SerializeField] public float knockBack;

    [SerializeField] public float criticalChance;
    [SerializeField] public float criticalBonus;

    public bool inUse = false;

    [SerializeField] ValueDisplayUI valueDisplayUI;

    private float Critical()
    {
        if (Random.Range(0, 100) < criticalChance) { return criticalBonus; }
        return 0.0f;
    }

    public float AttackDamage 
    { 
        get {
            float damage = baseAttackDamage * (1 + (attackBonus + Critical()) * 0.01f);
            valueDisplayUI.DisplayValue(damage);
            return damage;
        } 
    }
    public float SpecialDamage 
    { 
        get {
            float damage = baseSpecialDamage * (1 + (specialBonus + Critical()) * 0.01f);
            valueDisplayUI.DisplayValue(damage);
            return damage;
        }
    }


}
