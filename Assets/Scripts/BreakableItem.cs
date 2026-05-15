using UnityEngine;

public class BreakableItem : MonoBehaviour
{
    [Header("Breaking Physics")]
    [Tooltip("How hard it needs to hit something to break. Try 8 or 10!")]
    public float breakForceThreshold = 8f;

    [Header("Effects")]
    public AudioSource breakSound;
    public ParticleSystem shatterParticles;

    private bool isBroken = false;
    private MeshRenderer meshRenderer;
    private Collider itemCollider;

    // NEW: A timer to stop it from breaking the moment it spawns
    private float spawnTime;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        itemCollider = GetComponent<Collider>();

        // Record the exact time this bottle was created
        spawnTime = Time.time;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isBroken) return;

        // 1. THE SPAWN FIX: If the bottle has existed for less than 1 second, ignore all collisions!
        if (Time.time - spawnTime < 1.0f) return;

        // 2. THE PICK-UP FIX: If the bottle bumps into the Waiter, ignore it!
        if (collision.gameObject.CompareTag("Player")) return;

        // Measure how hard the impact was
        float impactForce = collision.relativeVelocity.magnitude;

        // PRO-TIP: This will print the force in your console so you can see exactly how hard it hit!
        Debug.Log(gameObject.name + " hit something with a force of: " + impactForce);

        if (impactForce >= breakForceThreshold)
        {
            Shatter();
        }
    }

    void Shatter()
    {
        isBroken = true;

        if (breakSound != null) breakSound.Play();
        if (shatterParticles != null) shatterParticles.Play();

        meshRenderer.enabled = false;
        itemCollider.enabled = false;

        // If you have your TavernManager setup, this deducts the money!
        if (TavernManager.instance != null)
        {
            TavernManager.instance.AddPenalty(10.00f);
        }

        SpringJoint joint = GetComponent<SpringJoint>();
        if (joint != null) Destroy(joint);

        Destroy(gameObject, 2f);
    }
}