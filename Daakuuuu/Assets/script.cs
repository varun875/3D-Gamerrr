using UnityEngine;

public class script : MonoBehaviour
{
    public Camera cam;                  // Your main camera
    public float range = 100f;
    public float damage = 25f;
    public ParticleSystem muzzleFlash;  // Optional
    public GameObject impactEffect;     // Optional

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Left click
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log("Hit: " + hit.collider.name);

            // Apply damage if the object has a Health component
            Health targetHealth = hit.transform.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage);
            }

            // Optional impact effect
            if (impactEffect != null)
            {
                Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }
}

