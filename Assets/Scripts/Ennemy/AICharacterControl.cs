using System;
using UnityEngine;
using RootMotion.Dynamics;
using System.Collections;
using System.Collections.Generic;
using SDD.Events;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour, IEventHandler
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        Transform target;                                    // target to aim for
        public Animator animator;
        public BehaviourPuppet puppet { get; private set; }
        public PuppetMaster puppet2 { get; private set; }
        public EnemyLife enemyLife;

        public static int damage = 1;
        

        private void Start()
        {
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();
            animator.SetBool("Attack", false);
            puppet2 = transform.parent.GetComponentInChildren<PuppetMaster>();
            target = GameObject.FindWithTag("Player").transform;
            enemyLife = GetComponent<EnemyLife>();
            SubscribeEvents();
        }

        private void Update()
        {
            //Debug.Log("ETAT = "+etat);

            //Debug.Log(target);
            //agent.isStopped = false;

            puppet = transform.parent.GetComponentInChildren<BehaviourPuppet>();

            character.Move(agent.desiredVelocity, false, false);

            if (GameManager.Instance.m_GameState == GameState.gamePlay)
            { 
                if (target != null) { agent.SetDestination(target.position); } 
            }
            else
            {
               animator.SetBool("Attack", false);
               agent.SetDestination(transform.position);
                //target = null;
            }

            if (puppet.state == BehaviourPuppet.State.Puppet)
            {
                animator.SetBool("Puppet", true);
                if (agent.remainingDistance > agent.stoppingDistance) 
                { 
                    Run(); 
                }
                else 
                { 
                    if (GameManager.Instance.m_GameState == GameState.gamePlay) 
                    {
                        Attack(); 
                    } 
                }
            }
            else if (puppet.state == BehaviourPuppet.State.Unpinned){ animator.SetBool("Puppet", false); }
        }

        public void Attack()
        {
                character.Move(Vector3.zero, false, false);
                animator.SetBool("Attack", true);
        }

        public void Run()
        {
                animator.SetBool("Attack", false);
        }

        public void Unpin()
        {
            puppet.SetState(BehaviourPuppet.State.Unpinned);
        }

        public void SetTarget(Transform target)
        {
                this.target = target;
        }

        private void setDead(EnemyDeath e)
        {
            if (this == null)
                return;
            if (e.obj != transform.root.gameObject) { return; }
            
            UnsubscribeEvents();
            puppet2.Kill();
            StartCoroutine(destroyEnemy());
        }

        private IEnumerator destroyEnemy()
        {
            yield return new WaitForSeconds(3);
            Destroy(transform.root.root.gameObject);
        }


        public void Resurrect()
        {
            //Debug.Log("resurrect");
             if (enemyLife.m_currentLife > 0)
              {
               Debug.Log("resurrect2");
            puppet2.state = PuppetMaster.State.Alive;
             }
        }

        public void SubscribeEvents()
        {
            EventManager.Instance.AddListener<EnemyDeath>(setDead);
        }

        public void UnsubscribeEvents()
        {
            EventManager.Instance.RemoveListener<EnemyDeath>((EnemyDeath e) => { });
        }
    }


}
