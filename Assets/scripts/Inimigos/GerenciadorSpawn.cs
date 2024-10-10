using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform[] spawnPoints; // Pontos de spawn
    [SerializeField] private GameObject enemy1; // Primeiro tipo de inimigo
    [SerializeField] private GameObject enemy2; // Segundo tipo de inimigo

    void Start()
    {
        InvokeRepeating("SpawnEnemies", 0.5f, 0.5f);
    }

    private void SpawnEnemies()
    {
        int index = Random.Range(0, spawnPoints.Length); // Escolhe um ponto de spawn aleatório
        int enemyIndex = Random.Range(0, 2); // Gera 0 ou 1 para escolher o tipo de inimigo

        GameObject enemyToSpawn = (enemyIndex == 0) ? enemy1 : enemy2; // Escolhe o inimigo baseado no índice

        Instantiate(enemyToSpawn, spawnPoints[index].position, Quaternion.identity); // Spawn do inimigo escolhido
    }
}
