using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ReturnToPointWalker : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float arriveDistance = 0.15f;

    private NavMeshAgent agent;
    private bool isWalking;

    public bool IsWalking => isWalking;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!isWalking || agent == null)
            return;

        if (!agent.pathPending && agent.remainingDistance <= Mathf.Max(agent.stoppingDistance, arriveDistance))
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude < 0.01f)
            {
                isWalking = false;
            }
        }
    }

    public void WalkTo(Vector3 worldPosition)
    {
        if (agent == null)
            return;

        gameObject.SetActive(true);
        agent.isStopped = false;
        agent.ResetPath();
        agent.SetDestination(worldPosition);
        isWalking = true;
    }

    public void StopWalking()
    {
        if (agent == null)
            return;

        agent.isStopped = true;
        agent.ResetPath();
        isWalking = false;
    }
}