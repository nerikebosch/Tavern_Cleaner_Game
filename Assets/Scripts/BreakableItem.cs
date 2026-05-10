using UnityEngine;

public class BreakableItem : MonoBehaviour
{
    [Header("Breaking Physics")]
    [Tooltip("How hard it needs to hit something to break. 3 is a good starting point!")]
    public float breakForceThreshold = 5f;

    [Header("Effects")]
    public AudioSource breakSound;
    public ParticleSystem shatterParticles;

    private bool isBroken = false;
    private MeshRenderer meshRenderer;
    private Collider itemCollider;

    void Start()
    {
        // Grab the components on the bottle
        meshRenderer = GetComponent<MeshRenderer>();
        itemCollider = GetComponent<Collider>();
    }

    // Unity automatically calls this when the Rigidbody hits anything
    void OnCollisionEnter(Collision collision)
    {
        if (isBroken) return; // Stop if it's already broken

        // Measure how hard the impact was
        float impactForce = collision.relativeVelocity.magnitude;

        // If the impact is harder than our threshold...
        if (impactForce >= breakForceThreshold)
        {
            Shatter();
        }
    }

    void Shatter()
    {
        isBroken = true;

        // 1. Play the sound!
        if (breakSound != null) breakSound.Play();

        // 2. Explode the glass chunks!
        if (shatterParticles != null) shatterParticles.Play();

        // 3. Make the intact bottle invisible and turn off its physics
        meshRenderer.enabled = false;
        itemCollider.enabled = false;

        // TODO: Later, we will tell the GameManager to deduct salary here!
        Debug.Log("Bottle Broke! Deducting Salary...");

        // 4. If the player is currently holding it with the SpringJoint, break the joint!
        SpringJoint joint = GetComponent<SpringJoint>();
        if (joint != null)
        {
            Destroy(joint);
        }

        // 5. Finally, permanently delete the object 2 seconds later (gives time for the sound to finish)
        Destroy(gameObject, 2f);
    }
}