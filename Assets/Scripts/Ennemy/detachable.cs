using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detachable : MonoBehaviour
{
    public GameObject part;
    public GameObject cut;
    private GameObject bodyPartCopy;
    private Rigidbody rigidbodyCopy;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        skinnedMeshRenderer = part.GetComponent<SkinnedMeshRenderer>();
        bodyPartCopy = cut.gameObject;
        rigidbodyCopy = bodyPartCopy.GetComponent<Rigidbody>();
        bodyPartCopy.SetActive(false);
    }

    public void Cut()
    {
        part.SetActive(false);
        skinnedMeshRenderer.enabled = false;
        bodyPartCopy.SetActive(true);
        var random = UnityEngine.Random.Range(-10, 10);
        rigidbodyCopy.AddTorque(new Vector3(random, random, random), ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Sword"))
        {
            part.SetActive(false);
          //  skinnedMeshRenderer.enabled = false;
            bodyPartCopy.SetActive(true);
            var random = UnityEngine.Random.Range(-10, 10);
            rigidbodyCopy.AddTorque(new Vector3(random, random, random), ForceMode.Impulse);
        }
    }
}
