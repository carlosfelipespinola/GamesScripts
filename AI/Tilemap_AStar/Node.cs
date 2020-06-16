using UnityEngine;

[System.Serializable]
public class Node
{
    public Node parent;
    public Vector3Int position;
    public int distanceFromTarget;
    public int distanceFromParent;
    public int cost
    {
        get
        {
            return distanceFromTarget + distanceFromParent;
        }
    }

    public Node(Vector3Int position, int distanceFromTarget, int distanceFromParent, Node parent = null)
    {
        this.position = position;
        this.distanceFromTarget = distanceFromTarget;
        this.distanceFromParent = distanceFromParent;
        this.parent = parent;
    }

    public override bool Equals(object obj)
    {
        return obj is Node node &&
               position.Equals(node.position);
    }

    public override int GetHashCode()
    {
        return 1206833562 + position.GetHashCode();
    }
}
