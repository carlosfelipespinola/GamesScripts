using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStar
{
    private Transform agent;
    private Transform target;
    private Tilemap walkableTileMap;
    

    public AStar(Transform agent, Transform target, Tilemap walkableArea)
    {
        this.agent = agent;
        this.target = target;
        this.walkableTileMap = walkableArea;
    }

    public void UpdateTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public List<Node> FindPath()
    {
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        Vector3Int targetInt = walkableTileMap.WorldToCell(target.position);
        Vector3Int currentPosition = walkableTileMap.WorldToCell(agent.position);
        Node currentNode = new Node(currentPosition, distanceFromParent: 0, distanceFromTarget: Distance(targetInt, currentPosition));

        openList.Add(currentNode);

        do
        {
            openList.Sort((a, b) => a.cost - b.cost);
            currentNode = openList[0];
            openList.RemoveAt(0);
            closedList.Add(currentNode);
            if (currentNode.distanceFromTarget == 0)
            {
                break;
            }
            var neighbors = NeighborsOf(currentNode);
            foreach (var neighbor in neighbors)
            {
                if (closedList.Contains(neighbor))
                {
                    continue;
                }

                if (openList.Contains(neighbor))
                {
                    int neighborIndex = openList.IndexOf(neighbor);
                    var previousNeighbor = openList.ElementAt(neighborIndex);
                    if (neighbor.distanceFromParent < previousNeighbor.distanceFromParent)
                    {
                        openList.RemoveAt(neighborIndex);
                        openList.Add(neighbor);
                    }

                }
                else
                {
                    openList.Add(neighbor);
                }

            }
        } while (openList.Count > 0);
        List<Node> path = new List<Node>();
        while (currentNode.parent != null)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        openList.Clear();
        closedList.Clear();
        path.Reverse();
        return path;
    }

    int Distance(Vector3Int target, Vector3Int position)
    {
        var xDistance = Mathf.Abs(target.x - position.x);
        var yDistance = Mathf.Abs(target.y - position.y);
        return xDistance + yDistance;
    }

    List<Node> NeighborsOf(Node parent)
    {
        Vector3Int targetInt = walkableTileMap.WorldToCell(target.position);
        List<Vector3Int> neighborsTiles = FindNeighborsTilesPositionOf(parent.position);
        return neighborsTiles.Select(position => new Node(position, distanceFromParent: Distance(parent.position, position), distanceFromTarget: Distance(targetInt, position), parent: parent)).ToList();
    }

    List<Vector3Int> FindNeighborsTilesPositionOf(Vector3Int position)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();
        Vector3Int[] positions = new Vector3Int[] { position + Vector3Int.up, position + Vector3Int.right, position + Vector3Int.down, position + Vector3Int.left };
        foreach (var testPosition in positions)
        {
            if (walkableTileMap.GetTile(testPosition) != null)
            {
                neighbors.Add(testPosition);
            }
        }
        return neighbors;
    }
}
