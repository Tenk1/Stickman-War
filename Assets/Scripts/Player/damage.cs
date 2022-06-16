using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using RootMotion.Dynamics;
using UnityStandardAssets.Characters.ThirdPerson;

public class damage : MonoBehaviour
{

    int enemy = 100;
    float speed;
    
    Vector3 oldPosition;
     //AICharacterControl enemy;
    // Start is called before the first frame update
    void Start()
    {
        oldPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        speed = Vector3.Distance(oldPosition, transform.position) * 100f;
        oldPosition = transform.position;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (GameManager.Instance.m_GameState == GameState.gamePlay)
        {
            if (collider.gameObject.layer == enemy)
            {
                if (gameObject.GetComponent<Autohand.Hand>() != null)
                {
                    if (gameObject.GetComponent<Autohand.Demo.XRHandControllerLink>().grabbing)
                    {
                        //  Debug.Log("Dommages sur ennemi :" + (int)((GetComponent<Rigidbody>().mass * speed * speed) / 50));
                        EventManager.Instance.Raise(new EnemyLifeEvent() { 
                            eLife = /*damagevalue * (int)speed * 2*/ (int)((GetComponent<Rigidbody>().mass * speed * speed * 2) / 50), obj = collider.transform.root.gameObject 
                        });

                    }
                }
                else if (gameObject.GetComponent<cannonBallScript>() != null)
                {
                    //   Debug.Log("Dommages sur ennemi :" + (int)((GetComponent<Rigidbody>().mass * speed * speed) / 100));
                    EventManager.Instance.Raise(new EnemyLifeEvent() { eLife = /*damagevalue * (int)speed*/ (int)((GetComponent<Rigidbody>().mass * speed * speed) / 50), obj = collider.transform.root.gameObject });

                }


            }
            else if (collider.gameObject.tag == "Player")
            {
                if (gameObject.GetComponent<Autohand.Hand>() == null && gameObject.GetComponent<cannonBallScript>() == null)
                {
                    // Debug.Log("Dommages sur joueur :" + (int)((GetComponent<Rigidbody>().mass * speed * speed) / 50));
                    EventManager.Instance.Raise(new LifeEvent() { eLife =/* AICharacterControl.damage * (int)speed*/ (int)((GetComponent<Rigidbody>().mass * speed * speed) / 50) });

                }
            }
        }
    }
}
