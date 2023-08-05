using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField] protected string info;
    public string Info { get { return info; } }
    public abstract void ApplyEffect(GameObject go);
}
