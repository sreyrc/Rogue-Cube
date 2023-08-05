using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Fall : MonoBehaviour
{
    float fallSpeedMax = 10.0f;
    float timeTillMaxSpeed = 2.0f;
    float fallStartTime, speed;

    private void Start()
    {
        fallStartTime = Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        speed = Mathf.Lerp(0, fallSpeedMax, (Time.deltaTime - fallStartTime) / timeTillMaxSpeed);
        transform.Translate(Vector3.down * fallSpeedMax * Time.deltaTime);

        if (transform.position.y < -10.0f)
        {
            CharacterStats stats = GetComponent<CharacterStats>();
            if (stats != null) { stats.Hp = 0; }
        }
    }
}
