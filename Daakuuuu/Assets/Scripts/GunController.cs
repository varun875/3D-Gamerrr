using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour
{
    public Camera cam;                  // Your main camera
    public float range = 100f;
    public float damage = 25f;
    public ParticleSystem muzzleFlash;  // Optional
    public GameObject impactEffect;     // Optional

    private PlayerInput playerInput;
    private InputAction fireAction;

    void Awake()
    {
        // Try to get PlayerInput from this object or its parent
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            playerInput = GetComponentInParent<PlayerInput>();
        }

        if (playerInput != null)
        {
            // Assumes you have an action named "Fire" in your Input Actions map
            fireAction = playerInput.actions["Fire"];
        }
        else
        {
            Debug.LogError("PlayerInput component not found on GunController or its parents!");
        }
    }

    void Update()
    {
        // Check if the fire action was triggered this frame
        if (fireAction != null && fireAction.WasPressedThisFrame())
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();

        // Get mouse position using the New Input System
        Vector2 mousePos = Mouse.current != null ? Mouse.current.position.ReadValue() : new Vector2(Screen.width / 2f, Screen.height / 2f);
        
        // Note: If this is an FPS game and the cursor is locked to the center, 
        // you might want to shoot from the center of the screen instead:
        // Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        // Ray ray = cam.ScreenPointToRay(screenCenter);

        Ray ray = cam.ScreenPointToRay(mousePos);
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
                GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 2f); // auto-destroy after 2 seconds
            }
        }
    }
}
