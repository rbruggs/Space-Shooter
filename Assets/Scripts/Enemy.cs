﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4f;

    private Player _player;
    private Animator _animator;



    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y <= -6f)
        {
            float randomX = Random.Range(-9f, 9f);
            transform.position = new Vector3(randomX, 8, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit " + other.transform.name);
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if(_player != null)
            {
                _player.AddScore(10);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Player")
        {
            if (_player != null)
            {
                _player.Damage();
            }
            _animator.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            Destroy(this.gameObject, 2.8f);
        }
    }
}
