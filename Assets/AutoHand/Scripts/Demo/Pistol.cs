using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

namespace Autohand.Demo{
    public class Pistol : MonoBehaviour
    {
        public Rigidbody body;
        public SoundHandler SFXHandler;
        public Transform barrelTip;
        int hitPower = 300;
        public float recoilPower = 1;
        public float range = 100;
        public LayerMask layer;

        private void Start() {
            if(body == null && GetComponent<Rigidbody>() != null)
                body = GetComponent<Rigidbody>();
        }
        

        public void Shoot() {
            (SFXHandler.GetComponent(typeof(SoundHandler)) as SoundHandler).PlaySound(AudioSound.Gun, transform.position);
            if (GameManager.Instance.m_GameState == GameState.gamePlay)
            {
                
                RaycastHit hit;
                if (Physics.Raycast(barrelTip.position, barrelTip.forward, out hit, range, layer))
                {
                    var hitBody = hit.transform.GetComponent<Rigidbody>();
                    if (hitBody != null)
                    {
                        Debug.DrawRay(barrelTip.position, (hit.point - barrelTip.position), Color.green, 5);
                        hitBody.GetComponent<Smash>()?.DoSmash();
                        hitBody.AddForceAtPosition((hit.point - barrelTip.position).normalized * 300, hit.point, ForceMode.Impulse);
                        EventManager.Instance.Raise(new EnemyLifeEvent() { eLife = hitPower, obj = hitBody.transform.root.gameObject });
                    }
                }
                else
                    Debug.DrawRay(barrelTip.position, barrelTip.forward * range, Color.red, 1);

                body.AddForce(barrelTip.transform.up * recoilPower * 5, ForceMode.Impulse);
            }
        }
    }
}
