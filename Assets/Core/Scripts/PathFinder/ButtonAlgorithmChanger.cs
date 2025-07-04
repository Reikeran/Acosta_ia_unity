using UnityEngine;
using TMPro;

public class ButtonAlgorithmChanger : MonoBehaviour
{
    [SerializeField] private PathFinderManager pathFinderManager;

    public void SetToBreadthFirst()
    {
        SetAlgorithm(PathFinderManager.PathfindingStrategy.BreadthFirst);
    }

    public void SetToDepthFirst()
    {
        SetAlgorithm(PathFinderManager.PathfindingStrategy.DepthFirst);
    }

    public void SetToDijkstra()
    {
        SetAlgorithm(PathFinderManager.PathfindingStrategy.Dijkstra);
    }

    public void SetToAStar()
    {
        SetAlgorithm(PathFinderManager.PathfindingStrategy.AStar);
    }

    private void SetAlgorithm(PathFinderManager.PathfindingStrategy strategy)
    {
        if (pathFinderManager == null)
        {
            Debug.LogWarning("PathFinderManager is not assigned!");
            return;
        }

        pathFinderManager.SetPathfindingStrategy(strategy);

        
    }
}
