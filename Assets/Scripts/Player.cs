using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedBoost = 8.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private Spawn_Manager _spawnManager;
    private bool isTripleShotActive = false;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    private bool _isShieldsActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private int _score = 0;

    [SerializeField]
    private UIManager _uiManager;

    [SerializeField]
    private GameObject _right_Engine_Fire;
    [SerializeField]
    private GameObject _left_Engine_Fire;
    private float _is_Right_Engine_Burning;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<Spawn_Manager>();
        transform.position = new Vector3(0, 0, 0);
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if(_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }        

        if(_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
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
        if (_isShieldsActive == true)
        {
            _isShieldsActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
            _lives--;
            _uiManager.updateLives(_lives);

        if(_lives == 2)
        {
            _is_Right_Engine_Burning = Random.Range(0, 2);
            if(_is_Right_Engine_Burning == 0)
            {
                _right_Engine_Fire.SetActive(true);
            }
            else
            {
                _left_Engine_Fire.SetActive(true);
            }
        }

        if(_lives == 1)
        {
            if(_is_Right_Engine_Burning == 0)
            {
                _left_Engine_Fire.SetActive(true);
            }
            else
            {
                _right_Engine_Fire.SetActive(true);
            }
            
        }
        
        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath(); 
            _uiManager.displayGaveOver();
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
         yield return new WaitForSeconds(5.0f);
         isTripleShotActive = false;
        
    }

    public void ActivateSpeedBoost()
    {
        _speed = _speedBoost;
        StartCoroutine(SpeedBoostDownRoutine());
    }

       IEnumerator SpeedBoostDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed = 5.0f;
        
    }

    public void ActivateShields()
    {
        _isShieldsActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.updateScore(_score);
    }
}
