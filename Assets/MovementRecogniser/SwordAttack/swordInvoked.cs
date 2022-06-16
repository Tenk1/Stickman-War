using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordInvoked : MonoBehaviour
{
    private Transform source;

    public bool animOn = false;
    private bool rotationOn = false;

    public float movementSpeed = 2000f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    public void initSource(Transform sourceParam)
    {
        this.source = sourceParam; 
    }

    // Update is called once per frame
    void Update()
    {
        if (animOn)
        {
            transform.position += transform.forward * Time.deltaTime * movementSpeed;
        }
    }
}
