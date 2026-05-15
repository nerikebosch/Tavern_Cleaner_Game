using UnityEngine;
using UnityEngine.AI;

public class PatronAI : MonoBehaviour
{
    public float wanderRadius = 6f; // How far they can pick a point to walk to
    public float waitTimer = 3f;    // How long they stand still before walking again

    private NavMeshAgent agent;

    // 1. We changed this to an Array [] so it can hold multiple Animators!
    private Animator[] animators;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // 2. Notice the "s" in GetComponents! This searches the entire Prefab 
        // and collects the Animator from the Head, Arms, Torso, etc., into a list.
        animators = GetComponentsInChildren<Animator>();

        timer = waitTimer;
    }

    void Update()
    {
        // 3. We check how fast the NavMesh Agent is moving...
        float currentSpeed = agent.velocity.magnitude;

        // 4. ...and we loop through EVERY body part, telling them all to move at that speed!
        if (animators != null && animators.Length > 0)
        {
            foreach (Animator anim in animators)
            {
                anim.SetFloat("Speed", currentSpeed);
            }
        }

        timer += Time.deltaTime;

        // If it's time to move, and we have actually reached our destination
        if (timer >= waitTimer && !agent.pathPending && agent.remainingDistance <= 0.1f)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
            waitTimer = Random.Range(2f, 6f); // Pick a random wait time for next stop
        }
    }

    // This math perfectly finds a random, valid point on your blue NavMesh
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}