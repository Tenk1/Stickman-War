using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooterScript : MonoBehaviour
{
    public GameObject cannonBall;
    public float shootForce = 0f;
    public SoundHandler SFXHandler;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void shootBall()
    {
        (SFXHandler.GetComponent(typeof(SoundHandler)) as SoundHandler).PlaySound(AudioSound.Bazooka, transform.position);
        GameObject projectile = (GameObject)Instantiate(
            cannonBall, transform.position, transform.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * shootForce);
    }
}
