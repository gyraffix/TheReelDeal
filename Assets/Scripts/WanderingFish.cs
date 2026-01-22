using UnityEngine;
using UnityEngine.AI;

public class WanderingFish : MonoBehaviour
{
    public float radius;
    public float wanderTimer;

    private NavMeshAgent agent;
    private float timer;
    private float offset;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = Random.value;
        agent = GetComponent<NavMeshAgent>();
        wanderTimer = wanderTimer + offset;
        timer = wanderTimer;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > wanderTimer)
        {
            Vector3 newTarget = RandomSpherePos(transform.position, radius, -1);
            float distance = (transform.position - newTarget).magnitude;
            if (distance<0.01)
            {
                Debug.LogWarning("Problem with sampling navmesh position / lookat...?");
            }
            agent.SetDestination(newTarget);
            
            timer = 0;
            
        }
    }

    public static Vector3 RandomSpherePos(Vector3 origin, float dist, int layerMask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit hit;

        NavMesh.SamplePosition(randDirection, out hit, dist, layerMask);

        return hit.position;
    }

}
