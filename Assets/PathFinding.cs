using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PathFinding : MonoBehaviour
{
	public Transform seeker, target;
	Grid grid;

	void Awake()
	{
		grid = GetComponent<Grid>();
		
	}
    private void Start()
    {
		
	}

    void FixedUpdate()
	{
		grid.CreateGrid();
		FindPath(seeker.position, target.position);

	}
	public int maxSize
    {
        get
        {
			return grid.gridSizeX * grid.gridSizeY;
        }
    }

	void FindPath(Vector3 startPos, Vector3 targetPos)
	{
		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		HEAP<Node> openSet = new HEAP<Node>(maxSize);
		HashSet<Node> closedSet = new HashSet<Node>();
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
				RetracePath(startNode, targetNode);
				return;
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

	void RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();

		grid.path = path;

	}

	int GetDistance(Node nodeA, Node nodeB)
	{
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}
}