using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTeleporter : MonoBehaviour
{

    public GameObject destinationWorld;
    public GameObject menuWorld;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void teleportToWorld()
    {
        menuWorld.SetActive(false);
        destinationWorld.SetActive(true);
    }

}
