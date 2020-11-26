using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
	PathHandler pathHandler;
 Grid grid;

	void Awake()
	{
		grid = GetComponent<Grid>();
		pathHandler = GetComponent<PathHandler>();
		
	}

	public int maxSize
    {
        get
        {
			return grid.gridSizeX * grid.gridSizeY;
        }
    }
	public void StartFindPath(Vector3 startPos , Vector3 targetPos)
    {
		StartCoroutine(FindPath(startPos, targetPos));
    }

	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
	{
		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);
        
		HEAP<Node> openSet = new HEAP<Node>(maxSize);
		HashSet<Node> closedSet = new HashSet<Node>();
		Vector3[] wayponts = new Vector3[0];
		bool pathSuccess = false;
		if (startNode.walkable)
		{
			openSet.add(startNode);

		//parent : (n-1)/2
		// child left : 2n+1
		//child right : 2n+2

		while (openSet.Count > 0)
		{
			Node node = openSet.removeFirst();
			closedSet.Add(node);

			if (node == targetNode)
			{

				pathSuccess = true;
				break;
			}

			foreach (Node neighbour in grid.GetNeighbours(node))
			{
				if (!neighbour.walkable || closedSet.Contains(neighbour))
				{
					continue;
				}

				int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
				if (newCostToNeighbour < neighbour.gCost || !openSet.contains(neighbour))
				{
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = node;

					if (!openSet.contains(neighbour))
						openSet.add(neighbour);
				}
			}
		}
		}
		yield return null;
		if (pathSuccess)
			wayponts = RetracePath(startNode, targetNode);
		pathHandler.FinishedProcessingPath(wayponts, pathSuccess);

		
	}

	Vector3[] RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		Vector3[] waypoints = simplifyPath(path);
		Array.Reverse(waypoints);
		return waypoints;

	}
	Vector3[] simplifyPath(List<Node> path)
    {
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;
		for (int i = 1; i < path.Count; i++)
        {
			Vector2 directionNew = new Vector2(path[1 - i].gridX - path[i].gridX, path[1 - i].gridY - path[i].gridY);
			if(directionNew != directionOld)
            {
				waypoints.Add(path[i].worldPosition);
            }
			directionOld = directionNew;

		}
		return waypoints.ToArray();
	}//

	int GetDistance(Node nodeA, Node nodeB)
	{
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}
}