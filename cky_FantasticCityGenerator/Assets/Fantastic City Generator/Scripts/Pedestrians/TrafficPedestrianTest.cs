using UnityEngine;
using UnityEngine.AI;

public class TrafficPedestrianTest : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Transform target;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        agent.destination = target.position;
    }
}
