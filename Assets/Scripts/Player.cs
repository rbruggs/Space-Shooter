﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private Spawn_Manager _spawnManager;
    [SerializeField]
    private bool isTripleShotActive = false;
    [SerializeField]
    private GameObject _tripleShotPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<Spawn_Manager>();
        transform.position = new Vector3(0, 0, 0);

        if(_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }


    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Player movement
        Vector3 playerDirection = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(playerDirection * _speed * Time.deltaTime);

        //Player restraints
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        //Wrap around
        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        if (isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.3f, 0), Quaternion.identity);
        }

    }
    public void Damage()
    {
        _lives--;

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }
    public void ActivateTripleShot()
    {
        isTripleShotActive = true;
        StartCoroutine(TripleShotDownRoutine());
        
    }

    IEnumerator TripleShotDownRoutine()
    {
        while(isTripleShotActive == true)
        {
            yield return new WaitForSeconds(5.0f);
            isTripleShotActive = false;
        }
    }
}
