using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArmySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemiesPrefabs;

    [Header("Spawning parameters")]
    [SerializeField] private Vector3 startPos;
    [SerializeField] private float XAxisStep;
    [SerializeField] private float ZAxisStep;
    [SerializeField] private int maxEnemiesInRow;
    [SerializeField] private int overallEnemiesNumber;

    private Vector3 currentPos;
    private int currEnemiesNumberInRow;

    private void Awake() {
        currentPos = startPos;

        SpawnEnemies();
    }

    private void SpawnEnemies() {
        for (int i = 0; i < overallEnemiesNumber; i++) {
            if (currEnemiesNumberInRow == maxEnemiesInRow) {
                currentPos.z -= ZAxisStep;
                currentPos.x = startPos.x;
                currEnemiesNumberInRow = 0;
            }
            int rand = Random.Range(0, enemiesPrefabs.Count);
            Instantiate(enemiesPrefabs[rand], currentPos, Quaternion.identity);
            currentPos.x += XAxisStep;
            currEnemiesNumberInRow++;
        }
    }
}
