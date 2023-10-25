// Patrol.cs
using UnityEngine;
using UnityEngine.AI;

public class navigation_patrol : MonoBehaviour
{

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        GotoNextPoint();
    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // This the sample code from the original Unity Manual. 
        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        // destPoint = (destPoint + 1) % points.Length;

        // I added the code below.
        // Randomly pick the next destination
        int newDestPoint = 0;

        // Randomly pick the next destination from the list of destinations.
        // If the next destination happens to be the current location, try again. 
        do
        {
            newDestPoint = Random.Range(0, points.Length);
        } while (destPoint == newDestPoint);

        destPoint = newDestPoint;
    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }
}
