using UnityEngine;

public class PathObstacle : MonoBehaviour
{
    [SerializeField] private float speedReductionPerc = 0.5f;
    [SerializeField] private LayerMask affectedLayers;

    void OnTriggerEnter(Collider other)
    {
        PathFinderAgent agent = other.GetComponentInParent<PathFinderAgent>();

        if (agent && ((1 << agent.gameObject.layer) & affectedLayers.value) != 0)
        {
            agent.MovementSpeed *= speedReductionPerc;
        }
    }

    void OnTriggerExit(Collider other)
    {
        PathFinderAgent agent = other.GetComponentInParent<PathFinderAgent>();

        if (agent && ((1 << agent.gameObject.layer) & affectedLayers.value) != 0)
        {
            agent.MovementSpeed /= speedReductionPerc;
        }
    }
}
