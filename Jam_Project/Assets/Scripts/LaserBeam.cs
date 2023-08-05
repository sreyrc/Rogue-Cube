using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    GameObject player;
    GameObject laserBeamCylinder;

    [SerializeField] ParticleSystem beamHitParticles;
    [SerializeField] Animator animator;

    float beamOnTimer;
    [SerializeField] float beamOnDuration = 5.0f;
    
    float beamPauseTimer;
    [SerializeField] float beamPauseDuration = 2.0f;

    [SerializeField] float maxBeamRotationSpeed = 1.0f;

    ParticleSystem beamParticlesActiveNow = null;

    float maxBeamLength = 15.0f;

    bool beamOn = false;

    void ActivateBeamCylinder()
    {
        laserBeamCylinder.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        laserBeamCylinder = transform.GetChild(0).gameObject;

        beamOnTimer = beamOnDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (beamOnTimer > 0.0f)
        {
            // If beam timer is activated and beam not on yet,
            // Switch on and activate child cylinder object
            if(!beamOn) { 
                beamOn = true;
                Invoke("ActivateBeamCylinder", 0.5f);
                animator.SetBool("emitBeam", true);
            }

            // Rotate laser towards the player
            Vector3 targetDirection = player.transform.position - transform.position;
            targetDirection.Normalize();
            Debug.DrawRay(transform.position, targetDirection, Color.red);

            // Scale rotation speed according to angle between forward and target dir
            // Closer to target - slower is the rotation
            float angle = Mathf.Acos(Vector3.Dot(transform.forward, targetDirection));
            float rotationSpeed = maxBeamRotationSpeed * (angle/Mathf.PI);
            
            // New direction after rotation increment
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, 
                targetDirection, Time.deltaTime * rotationSpeed, 0.0f);

            newDirection.Normalize();

            // Rotate towards this new dir
            transform.rotation = Quaternion.LookRotation(newDirection);

            beamOnTimer -= Time.deltaTime;

            // Adjust laser size if obtacle hit

            // Cast a ray from this object to new dir
            RaycastHit hitInfo;

            if (Physics.Raycast(transform.position, newDirection, out hitInfo, LayerMask.GetMask("No Beam Effect")))
            {
                transform.localScale = new Vector3(transform.localScale.x,
                    transform.localScale.y, hitInfo.distance * 0.5f);

                // Emit particles
                if (beamParticlesActiveNow == null)
                {
                    beamParticlesActiveNow = Instantiate(beamHitParticles, hitInfo.point, Quaternion.identity);
                }

                beamParticlesActiveNow.transform.position = hitInfo.point;
            }
            else
            {
                // Restore to original length if nothing hit
                transform.localScale = new Vector3(transform.localScale.x,
                    transform.localScale.y, maxBeamLength);

                if (beamParticlesActiveNow != null)
                {
                    // Tranport is somewhere far off
                    beamParticlesActiveNow.transform.position = new Vector3(100, 100, 100);
                }
            }
        }
        else
        {
            // If on - switch it off. Disable the laser and start pause timer
            if (beamOn)
            {
                beamOn = false;
                //if any beam particle(s) still active - destroy
                if (beamParticlesActiveNow != null)
                {
                    Destroy(beamParticlesActiveNow);
                }
                beamPauseTimer = beamPauseDuration;
                laserBeamCylinder.SetActive(false);
                animator.SetBool("emitBeam", false);
            }
            beamPauseTimer -= Time.deltaTime;

            // If pause time up, activate beam timer
            if(beamPauseTimer <= 0.0f)
            {
                beamOnTimer = beamOnDuration;
            }
        }
    }
}
