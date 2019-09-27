using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public AudioSource destroySound;
    public float offsetY;
    public float cubeSize = 0.2f;
    public int cubesInRow = 5;

    float cubesPivotDistance;
    Vector3 cubesPivot;

    public float explosionForce = 50f;
    public float explosionRadius = 4f;
    public float explosionUpward = 0.4f;

    public float sideSpeed;
    public float fallSpeed;
    private Rigidbody rb;
    public bool move;
    private int startMoveDirection;
    //private GameManager GameManager;


    // Start is called before the first frame update
    void Start()
    {
        //GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        move = false;
        startMoveDirection = -1;
        rb = this.GetComponent<Rigidbody>();

        var randomColor = Colors.colors[Random.Range(0, Colors.colors.Count)];
        this.GetComponent<Renderer>().material.SetColor("_Color", randomColor.Color);
        this.GetComponent<Renderer>().material.SetColor("_EmissionColor", randomColor.EmissionColor);

        //calculate pivot distance
        cubesPivotDistance = cubeSize * cubesInRow / 2;
        //use this value to create pivot vector)
        cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);
    }

    private void Update()
    {
        if (GameManager.instance.gameActive && (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)))
        {
            move = true;
        }
    }

    void FixedUpdate()
    {
        
        if (move)
        {
            if (GameManager.instance.gameActive)
            {
                Vector3 movement = new Vector3(0.0f, -1 * fallSpeed * Time.deltaTime, 0.0f);
                rb.MovePosition(rb.transform.position + movement);
            }
        }
        else
        {
            if(GameManager.instance.gameActive)
            {
                Vector3 movement = new Vector3(startMoveDirection * sideSpeed * Time.deltaTime, 0.0f, 0.0f);
                rb.MovePosition(rb.transform.position + movement);
            }
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Platform"))
        {
            move = false;
            startMoveDirection = 1;
            destroySound.Play();
            explode();
        }

        if (collision.gameObject.CompareTag("Border"))
        {
            startMoveDirection = startMoveDirection * -1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("End"))
        {
            Destroy(gameObject);
            GameManager.instance.InstantiateSphereOverPlatform();
        }
    }

    public void explode()
    {
        //make object disappear
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<SphereCollider>().enabled = false;

        GameManager.instance.SpawnBalls(transform.position, cubesInRow, explosionRadius, explosionForce, explosionUpward, cubeSize, gameObject.GetComponent<Renderer>().material);

        Destroy(gameObject, 5f);

    }

}
