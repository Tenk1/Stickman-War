using SDD.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;

public class missile : MonoBehaviour
{
    // Start is called before the first frame update

    //public GameObject explosionEffect;
    public float explostionForce = 10f;
    public float radius = 10f;
    public int damage = 3;

    public GameObject explosionVFX;
    private SoundHandler SFXHandler;

    public void instantiateSFX(SoundHandler pSFX)
    {
        SFXHandler = pSFX;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }
    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        (SFXHandler.GetComponent(typeof(SoundHandler)) as SoundHandler).PlaySound(AudioSound.MissileHit, transform.position);

        GameObject explosion = Instantiate(explosionVFX, transform.position, Quaternion.identity);
        explosion.transform.localScale = new Vector3(1, 1, 1);
        Destroy(explosion, 2);
        foreach (Collider near in colliders)
        {
            Rigidbody rig = near.GetComponent<Rigidbody>();
            UnityStandardAssets.Characters.ThirdPerson.AICharacterControl ennemy = near.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>();

            if (rig!= null)
            {
                rig.AddExplosionForce(explostionForce, transform.position, radius, 1f, ForceMode.Impulse);
            }
            
            if (ennemy != null)
            {
                if(ennemy.puppet2.state == PuppetMaster.State.Alive)
                {
                    EventManager.Instance.Raise(new EnemyLifeEvent() { eLife = damage, obj = ennemy.transform.root.gameObject });
                    //ennemy.TakeDamage(damage);
                    ennemy.Unpin();
                }
            }
        }
        //Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
