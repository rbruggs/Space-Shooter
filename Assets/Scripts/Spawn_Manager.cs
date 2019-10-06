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
    private GameObject _tripleShotPrefab;

    private bool _stopSpawing = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
       
    IEnumerator SpawnEnemyRoutine()
    {
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
        while (_stopSpawing == false)
        {
            yield return new WaitForSeconds(Random.Range(5.0f, 8.0f));
            Vector3 powerUpSpawnPos = new Vector3(Random.Range(-9f, 9f), 9, 0);
            GameObject newPowerUp = Instantiate(_tripleShotPrefab, powerUpSpawnPos, Quaternion.identity);
        }
        
    }

    public void OnPlayerDeath()
    {
        _stopSpawing = true;
    }

}
