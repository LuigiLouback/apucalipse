using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static Grid grid;

    private void Awake()
    {
        // Verifica se já temos uma referência ao Grid e evita duplicação
        if (grid == null)
        {
            grid = FindObjectOfType<Grid>();
            if (grid == null)
            {
                Debug.LogError("Grid não encontrado! Certifique-se de que o objeto Grid está presente na cena.");
            }
        }
    }
}
