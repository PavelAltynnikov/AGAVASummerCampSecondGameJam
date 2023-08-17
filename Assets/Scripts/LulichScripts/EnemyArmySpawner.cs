using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArmySpawner : MonoBehaviour
{
    [SerializeField] private List<Animal> enemiesPrefabs;

    [Header("Spawning parameters")]
    [SerializeField] private Vector3 startPos;
    [SerializeField] private float XAxisStep;
    [SerializeField] private float ZAxisStep;
    [SerializeField] private int maxEnemiesInRow;
    [SerializeField] private int overallEnemiesNumber;

    private Vector3 currentPos;
    private int currEnemiesNumberInRow;
    private List<Animal> _enemies = new List<Animal>();


    public void MoveEnemyArmy(Vector3 point)
    {
        StartCoroutine(nameof(MoveTo), point);
    }

    private void MoveTo(Vector3 endPoint)
    {
        // var offset = new Vector3(0, 0, 20);
        var offset = new Vector3(0, 0, 0);

        foreach (Animal enemy in _enemies)
        {
            Vector3 targetPoint = enemy.transform.position - endPoint - offset;
            enemy.Go(targetPoint, 2f);
        }
    }

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
            Animal enemy = Instantiate(enemiesPrefabs[rand], currentPos, Quaternion.identity);
            _enemies.Add(enemy);

            currentPos.x += XAxisStep;
            currEnemiesNumberInRow++;
        }
    }
}
