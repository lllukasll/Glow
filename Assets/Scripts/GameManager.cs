using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { GAME, GAME_OVER }

public class GameManager : MonoBehaviour
{
    public AudioSource backgroundAudio;
    public GameObject gameOverUI;
    public GameObject activeGameUI;

    public Text countText;
    public Text scoreCountText;
    public Text currentScoreMessage;
    public Text gameOverScoreMessage;
    public Text currentBestScoreText;
    public Text bestScoreText;

    public GameObject sphere;
    public GameObject platform;
    public float offsetY;

    public int pointCounter;
    public int bestScore;

    public bool gameActive;
    private GameState _activeGameState;
    private Vector3 cubesPivot;
    private List<GameObject> SpawnedBalls;

    // Start is called before the first frame update
    void Start()
    {
        bestScore = PlayerPrefs.GetInt("highscore", bestScore);
        currentBestScoreText.text = bestScore.ToString();
        bestScoreText.text = bestScore.ToString();

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
                StartCoroutine(SetGameActive(0.5f));
            }
        }
    }

    private IEnumerator SetGameActive(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameOverScoreMessage.text = "THAT'S JUST BAD";
        currentScoreMessage.text = "STARTED";
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

    public void AddPoint()
    {
        pointCounter++;
            
        if(pointCounter > bestScore)
        {
            bestScore = pointCounter;
            currentBestScoreText.text = bestScore.ToString();
            bestScoreText.text = bestScore.ToString();
            PlayerPrefs.SetInt("highscore", bestScore);
        }

        UpdateScore();
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
        pointCounter = 0;

        countText.text = "";
        scoreCountText.text = "0";
        currentScoreMessage.text = "TOUCH TO START";

        gameOverUI.SetActive(false);
        activeGameUI.SetActive(true);

        foreach (var el in SpawnedBalls)
        {
            Destroy(el);
        }        
    }

    private void UpdateScore()
    {
        countText.text = pointCounter.ToString();
        scoreCountText.text = pointCounter.ToString();

        if (pointCounter == 1) { StartCoroutine(SetScoreMessage("EHM... THAT'S OK")); }
        if (pointCounter == 5) { StartCoroutine(SetScoreMessage("YOU'RE DOING FINE"));}
        if (pointCounter == 10) { StartCoroutine(SetScoreMessage("THAT'S PRETTY GOOD"));}
        if (pointCounter == 15) { StartCoroutine(SetScoreMessage("YOU'RE GREATE"));}
        if (pointCounter == 20) { StartCoroutine(SetScoreMessage("AWESOME SCORE"));}
        if (pointCounter == 25) { StartCoroutine(SetScoreMessage("THIS IS AMAZING"));}
        if (pointCounter == 30) { StartCoroutine(SetScoreMessage("PERFECT"));}
        if (pointCounter == 45) { StartCoroutine(SetScoreMessage("IMPOSIBLE !"));}
        if (pointCounter == 50) { StartCoroutine(SetScoreMessage("THAT'S EPIC !!!"));}
    }

    private IEnumerator SetScoreMessage(string message)
    {
        gameOverScoreMessage.text = message;
        currentScoreMessage.text = message;
        yield return new WaitForSeconds(1);
        currentScoreMessage.text = "";
    }
}
