using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;

    private bool _stopSpawing = false;
    // Start is called before the first frame update

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }
    
       
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawing == false)
        {   

            Vector3 spawnPos = new Vector3(Random.Range(-9f, 9f), 8, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);

        }


    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawing == false)
        {
            yield return new WaitForSeconds(Random.Range(5.0f, 8.0f));
            int randomPowerup = Random.Range(0, 3);
            Vector3 powerUpSpawnPos = new Vector3(Random.Range(-9f, 9f), 9, 0);
            GameObject newPowerUp = Instantiate(powerups[randomPowerup], powerUpSpawnPos, Quaternion.identity);
        }
        
    }

    public void OnPlayerDeath()
    {
        _stopSpawing = true;
    }

}
