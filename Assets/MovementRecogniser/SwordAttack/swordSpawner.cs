using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordSpawner : MonoBehaviour
{
    public Transform sourceSword;
    public GameObject SwordToInvoke;

    public SoundHandler SFXHandler;

    private List<swordInvoked> swordList;
    // Start is called before the first frame update
    void Start()
    {
        swordList = new List<swordInvoked>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void createSword()
    {
        (SFXHandler.GetComponent(typeof(SoundHandler)) as SoundHandler).PlaySound(AudioSound.SwordSpawn, transform.position);
        GameObject swordInvoked = (GameObject)Instantiate( SwordToInvoke, transform.position, transform.rotation);
        (swordInvoked.GetComponent(typeof(KatanaCut)) as KatanaCut).setSFXHandler(SFXHandler);
        swordInvoked swordInvokedTemp = swordInvoked.GetComponent<swordInvoked>();
        swordInvokedTemp.initSource(sourceSword);
        swordList.Add(swordInvokedTemp);
    }

    public void setOrientationSword()
    {
        foreach(swordInvoked swordy in swordList){
            swordy.animOn = true;
        }
    }
}
