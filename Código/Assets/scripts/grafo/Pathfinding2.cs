using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding2 : MonoBehaviour
{
    public Transform seeker;
    public Transform target;

    void Awake()
    {
        // Certifique-se de que o Grid está configurado através do GridManager
        if (GridManager.grid == null)
        {
            Debug.LogError("Grid não encontrado! Certifique-se de que o GridManager está inicializado.");
            return;
        }

        // Define o target como o jogador, caso não tenha sido atribuído no Editor
        if (target == null && Jogador.PlayerTransform != null)
        {
            target = Jogador.PlayerTransform;
        }
    }

    void Update()
    {
        // Atualiza o caminho apenas se seeker e target estiverem definidos
        if (seeker != null && target != null)
        {
            FindPath(seeker.position, target.position);
        }
    }

    public void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        if (GridManager.grid == null)
        {
            Debug.LogError("Grid está nulo no método FindPath!");
            return;
        }

        Node startNode = GridManager.grid.NodeFromWorldPoint(startPos);
        Node targetNode = GridManager.grid.NodeFromWorldPoint(targetPos);

        if (startNode == null || targetNode == null)
        {
            Debug.LogError("Nós inicializados como nulos no FindPath. Certifique-se de que os pontos estão dentro da área do Grid.");
            return;
        }

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].hCost < node.hCost)
                {
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            if (node == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in GridManager.grid.GetNeighbours(node))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                    neighbour.gCost = 0;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = node;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                
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

        if (GridManager.grid != null)
        {
            GridManager.grid.path = path;
        }
        else
        {
            Debug.LogError("Grid está nulo ao retracear o caminho.");
        }
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

