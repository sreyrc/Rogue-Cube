using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueUIRiseUpAndDie : MonoBehaviour
{
    float startTime = 0.0f;
    float lifeTime = 2.0f;
    float riseDuration = 0.5f;
    float startSpeed = 10.0f;
    float speed;

    private void Start()
    {
        startTime = Time.time;
        speed = startSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        speed = Mathf.Lerp(speed, 0.0f, (Time.time - startTime) / riseDuration);
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (Time.time - startTime > lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
