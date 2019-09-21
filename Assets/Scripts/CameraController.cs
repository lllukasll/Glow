using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject platform;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - platform.transform.position;
        transform.position = platform.transform.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, platform.transform.position.y + offset.y, transform.position.z);
    }
}
