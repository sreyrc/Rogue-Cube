using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    [SerializeField]
    private float timer;

    private void Update()
    {
        timer -= Time.deltaTime;
        
        if(timer < 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
