using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize; // Tamanho do grid no mundo
    public float nodeRadius; // Raio de cada nó
    Node[,] grid; // Matriz de nós

    float nodeDiameter; // Diâmetro do nó
    int gridSizeX, gridSizeY; // Tamanho do grid em X e Y

    public List<Node> path; // Caminho atual

    void Awake()
    {
        // Calcula o diâmetro de cada nó
        nodeDiameter = nodeRadius * 2;
        // Calcula quantos nós teremos na direção X e Y
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        // Cria o grid
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        // Calcula o canto inferior esquerdo do grid no mundo
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        // Criação dos nós no grid
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // Calcula a posição do ponto no mundo para cada nó
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                // Verifica se o nó é "walkable" (se pode ser atravessado)
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                // Cria o nó no grid
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    void OnDrawGizmos()
    {
        // Desenha o contorno do grid no mundo
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

        // Se o grid foi criado, desenhe cada nó do grid
        if (grid != null)
        {
            foreach (Node n in grid)
            {
                // Define a cor dos nós baseados se são caminháveis ou não
                Gizmos.color = (n.walkable) ? Color.white : Color.red;

                // Se o caminho atual contém esse nó, mude a cor para preto
                if (path != null && path.Contains(n))
                {
                    Gizmos.color = Color.black;
                }

                // Desenha cada nó como um cubo, para que seja visualizado no editor
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
