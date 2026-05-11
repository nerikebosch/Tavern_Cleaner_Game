using UnityEngine;
using UnityEngine.AI;

public class PatronAI : MonoBehaviour
{
    public float wanderRadius = 6f; // How far they can pick a point to walk to
    public float waitTimer = 3f;    // How long they stand still before walking again

    private NavMeshAgent agent;
    private Animator anim;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        timer = waitTimer;
    }

    void Update()
    {
        // Tell the Animator how fast we are moving so the Walk animation plays!
        // (Assuming your Animator uses a "Speed" parameter just like your Waiter)
        if (anim != null)
        {
            anim.SetFloat("Speed", agent.velocity.magnitude);
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