using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { GAME, GAME_OVER }

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public AudioSource backgroundAudio;
    public GameObject gameOverUI;
    public GameObject activeGameUI;

    public GameObject sphere;
    public GameObject platform;
    public float offsetY;

    public bool gameActive;
    private bool canContinue;
    private GameState _activeGameState;
    private Vector3 cubesPivot;
    private List<GameObject> SpawnedBalls;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnedBalls = new List<GameObject>();
        ResetValues();
    }

    // Update is called once per frame
    void Update()
    {
        if(_activeGameState == GameState.GAME)
        {
            if(!gameActive && (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)))
            {
                StartCoroutine(SetGameActive(0.1f));
            }
        }
    }

    private IEnumerator SetGameActive(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ScoreManager.instance.SetScoreMessageToActive();
        gameActive = true;
        backgroundAudio.Play();
    }

    public void InstantiateSphere(Vector3 position)
    {
        var random = new Random();
        var createdSphere = Instantiate(sphere, new Vector3(Random.Range(-2.2f, 2.2f), position.y + offsetY, position.z), sphere.transform.rotation);
    }

    public void InstantiateSphereOverPlatform()
    {
        var random = new Random();
        var createdSphere = Instantiate(sphere, new Vector3(Random.Range(-2.2f, 2.2f), platform.transform.position.y + offsetY, platform.transform.position.z), sphere.transform.rotation);
    }

    public void GameOver()
    {
        backgroundAudio.Stop();
        gameOverUI.SetActive(true);
        activeGameUI.SetActive(false);
        _activeGameState = GameState.GAME_OVER;
    }

    public void Retry()
    {
        ResetValues();
        Vector3 startPosition = platform.GetComponent<PlatformMovement>().ResetPosition();
        Instantiate(sphere, new Vector3(startPosition.x, startPosition.y + offsetY, startPosition.z), sphere.transform.rotation);
        platform.GetComponent<PlatformMovement>().UnPause();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Continue()
    {
        gameActive = false;
        _activeGameState = GameState.GAME;

        gameOverUI.SetActive(false);
        activeGameUI.SetActive(true);
        canContinue = false;

        foreach (var el in SpawnedBalls)
        {
            Destroy(el);
        }

        Vector3 startPosition = platform.transform.position;
        Instantiate(sphere, new Vector3(startPosition.x, startPosition.y + offsetY, startPosition.z), sphere.transform.rotation);
        platform.GetComponent<PlatformMovement>().UnPause();
    }

    public bool GetContinueValue()
    {
        return canContinue;
    }

    public void SpawnBalls(Vector3 postion, float cubesInRow, float explosionRadius, float explosionForce, float explosionUpward, float cubeSize, Material material)
    {
        for (int x = 0; x < cubesInRow; x++)
        {
            for (int y = 0; y < cubesInRow; y++)
            {
                for (int z = 0; z < cubesInRow; z++)
                {
                    createPiece(postion, x, y, z, cubeSize, cubesInRow, material);
                }
            }
        }

        //get explosion position
        Vector3 explosionPos = postion;
        //get colliders in that position and radius
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
        //add explosion force to all colliders in that overlap sphere
        foreach (Collider hit in colliders)
        {
            //get rigidbody from collider object
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //add explosion force to this body with given parameters
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpward);
            }
        }
    }

    private void createPiece(Vector3 position,int x, int y, int z, float cubeSize, float cubesInRow, Material material)
    {
        //create piece
        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        SpawnedBalls.Add(piece);

        //set piece position and scale
        piece.transform.position = position + new Vector3(cubeSize * x, cubeSize * y + 0.5f, cubeSize * z) - new Vector3(cubeSize * cubesInRow / 2, cubeSize* cubesInRow / 2, cubeSize* cubesInRow / 2);
        piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
        piece.GetComponent<Renderer>().material = material;

        //add rigidbody and set mass
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;

        StartCoroutine(DestroyBall(piece));
    }

    IEnumerator DestroyBall(GameObject ball)
    {
        yield return new WaitForSeconds(2);
        SpawnedBalls.Remove(ball);
        Destroy(ball);
    }

    private void ResetValues()
    {
        gameActive = false;
        _activeGameState = GameState.GAME;
        canContinue = true;

        ScoreManager.instance.ResetCurrentScore();

        gameOverUI.SetActive(false);
        activeGameUI.SetActive(true);

        foreach (var el in SpawnedBalls)
        {
            Destroy(el);
        }        
    }
}
