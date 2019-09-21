using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PointerController : MonoBehaviour
{
    public GameManager GameManager;
    public GameObject platform;

    public AudioSource pointAudio;

    //ivate int pointCounter;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Sphere"))
        {
            var color = other.gameObject.GetComponent<Renderer>().material.GetColor("_Color");
            var emissionColor = other.gameObject.GetComponent<Renderer>().material.GetColor("_EmissionColor");
            pointAudio.Play();
            platform.GetComponent<PlatformMovement>().SetNewColor(color, emissionColor);
            GameManager.InstantiateSphere(transform.position);
            GameManager.AddPoint();
            //Debug.Log("Points : " + pointCounter);
            Debug.Log("Color : " + color);
            Debug.Log("Emission color : " + emissionColor);
            Destroy(other.gameObject);
        }
    }
}
