using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // O personagem que a câmera segue
    public float minX, maxX, minY, maxY; // Limites do mapa

    void Update()
    {
        // Pega a posição desejada da câmera
        float targetX = Mathf.Clamp(target.transform.position.x, minX, maxX);
        float targetY = Mathf.Clamp(target.transform.position.y, minY, maxY);

        // Ajusta a posição da câmera, sem ultrapassar os limites
        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
}
