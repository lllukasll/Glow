using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    //public GameObject diePanel;
    public GameObject Light;
    public float sideSpeed;
    public float topSpeed;
    public int startMoveDirection = 1;

    private Color nextColor;
    private Color nextEmissionColor;
    private bool changeCoolor;
    public float transitionTime = 40f;
    private float currentTransitionTime = 0f;

    private Rigidbody rb;
    private Vector3 StartPosition;
    private bool paused;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        changeCoolor = true;
        StartPosition = transform.position;
        paused = false;
    }

    private void Update()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ColorChange();
        if(!paused && GameManager.instance.gameActive)
        {
            Vector3 movement = new Vector3(sideSpeed * Time.deltaTime * startMoveDirection, topSpeed * Time.deltaTime, 0.0f);
            rb.transform.position += movement;
        }else
        {
            rb.Sleep();
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            startMoveDirection = startMoveDirection * -1;
        }

        if (collision.gameObject.CompareTag("Sphere"))
        {
            GameManager.instance.GameOver();
            paused = true;
        }
    }

    void ColorChange()
    {
        if (changeCoolor)
        {
            if (currentTransitionTime <= 0)
            {
                changeCoolor = false;
            }
            else
            {
                currentTransitionTime -= Time.deltaTime;
                this.GetComponent<Renderer>().material.SetColor("_Color", Color.Lerp(this.GetComponent<Renderer>().material.GetColor("_Color"), nextColor, transitionTime / currentTransitionTime - 1));
                this.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.Lerp(this.GetComponent<Renderer>().material.GetColor("_EmissionColor"), nextEmissionColor, transitionTime / currentTransitionTime - 1));
                Light.GetComponent<Light>().color = Color.Lerp(Light.GetComponent<Light>().color, nextColor, transitionTime / currentTransitionTime - 1);
            }
        }
    }

    public void SetNewColor(Color newColor, Color newEmissionColor)
    {
        nextColor = newColor;
        nextEmissionColor = newEmissionColor;
        currentTransitionTime = transitionTime;
        changeCoolor = true;
        
    }

    public void UnPause()
    {
        paused = false;
    }

    public Vector3 ResetPosition()
    {
        rb.transform.position = StartPosition;
        return transform.position;
    }
}
