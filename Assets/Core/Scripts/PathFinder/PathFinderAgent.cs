using System.Collections.Generic;
using UnityEngine;

public class PathFinderAgent : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float rotateSpeed = 45f;

    private Stack<PathNode> currentPath;
    private PathNode targetNode;
    private Vector3? destination;

    public bool HasReachedDestination { get; private set; } = false;

    public Vector3? Destination
    {
        get => destination;
        set
        {
            destination = value;

            if (destination != null)
            {
                currentPath = PathFinderManager.Instance.CreatePath(transform.position, destination.Value);

                if (currentPath == null || currentPath.Count == 0)
                {
                    Debug.LogWarning("PathFinderAgent: No path found to destination.");
                    targetNode = null;
                    HasReachedDestination = true; // No hay a d�nde ir
                }
                else
                {
                    targetNode = currentPath.Pop();
                    HasReachedDestination = false;
                }
            }
        }
    }
    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }

    void Update()
    {
        if (Destination == null || HasReachedDestination || targetNode == null)
            return;
        Debug.Log(targetNode.Position);
        Vector3 targetPosition = targetNode.Position;
        Vector3 diff = targetPosition - transform.position;

        
        if (diff.sqrMagnitude < 0.0001f)
        {
            if (currentPath != null && currentPath.Count > 0)
                targetNode = currentPath.Pop();
            else
                HasReachedDestination = true;
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(diff.normalized, transform.up);
        float maxDistanceDelta = movementSpeed * Time.deltaTime;
        float maxDegreesDelta = rotateSpeed * Time.deltaTime;

        Vector3 updatedPosition = Vector3.MoveTowards(transform.position, targetPosition, maxDistanceDelta);
        Quaternion updatedRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDegreesDelta);

        transform.SetPositionAndRotation(updatedPosition, updatedRotation);

        if (diff.sqrMagnitude <= Mathf.Epsilon * Mathf.Epsilon)
        {
            if (currentPath.Count > 0)
                targetNode = currentPath.Pop();
            else
                HasReachedDestination = true;
        }
    }
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (targetNode == null)
            return;

        float lineThickness = 5f;
        UnityEditor.Handles.color = Color.red;

        UnityEditor.Handles.DrawLine(transform.position, targetNode.Position, lineThickness);

        if (currentPath != null && currentPath.Count > 0)
        {
            PathNode[] nodes = currentPath.ToArray();

            UnityEditor.Handles.DrawLine(targetNode.Position, nodes[0].Position, lineThickness);

            for (int i = 0; i < nodes.Length - 1; i++)
                UnityEditor.Handles.DrawLine(nodes[i].Position, nodes[i + 1].Position, lineThickness);
        }
    }
#endif
}
