using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;
    [SerializeField] float dashCooldown;

    bool isDashing = false;
    float dashTimer = 0.0f;
    float timeUntilCanDash = 0.0f;

    TrailRenderer trailRenderer;

    public bool IsDashing { get { return isDashing; } }

    private void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && timeUntilCanDash <= 0.0f)
        {
            trailRenderer.enabled = true;
            dashTimer = dashDuration;
            timeUntilCanDash = dashCooldown;
            isDashing = true;
        }

        if (timeUntilCanDash >= 0.0f)
        {
            timeUntilCanDash -= Time.deltaTime;
        }

        if (dashTimer > 0.0f)
        {
            transform.Translate(transform.forward * dashSpeed * Time.deltaTime, Space.World);
            dashTimer -= Time.deltaTime;
        }
        else if (isDashing)
        {
            isDashing = false;
            trailRenderer.enabled = false;
        }
    }
}
