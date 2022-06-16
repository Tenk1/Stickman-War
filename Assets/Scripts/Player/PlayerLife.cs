using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using System;

namespace Player
{
    public class PlayerLife : MonoBehaviour, IEventHandler
    {
        [Header("Vie")]
        [SerializeField] private int m_currentLife;
        [SerializeField] private int m_maxLife;

        [Header("Invulterabilité")]
        [SerializeField] private bool m_isInvincible;
        [SerializeField] private float invincibilityDurationSeconds;
        private bool IsInvincible { get => m_isInvincible; set => m_isInvincible = value; }

        public HealthBar playerHealth;

        public void Start()
        {
            m_isInvincible = false;
            m_currentLife = m_maxLife;
            playerHealth.SetHealth(m_currentLife, m_maxLife);
            SubscribeEvents();
        }

        private void LifeHasBeenModified(LifeEvent e)
        {
            if (e.eLife > 0 && m_currentLife > 0)
            {
                if (m_isInvincible) { return; }
                applyDommage(e.eLife);
            }
            if (e.eLife < 0){ applyHealing(e.eLife); }
            playerHealth.SetHealth(m_currentLife, m_maxLife);
          //  Debug.Log("Vie joueur : " + m_currentLife + ", Vie max : " + m_maxLife + ", expected value : " + (float)m_currentLife/m_maxLife);
        }

        private void applyHealing(int heal)
        {
            m_currentLife += heal;
            if (m_currentLife > m_maxLife) m_currentLife = m_maxLife;
            
        }

        private void applyDommage(int dommage)
        {
            m_currentLife -= dommage;
            if (m_currentLife <= 0) { EventManager.Instance.Raise(new GameOverEvent() {  });
                GameManager.Instance.m_GameState = GameState.gameOver;
                    }
            //   else { StartCoroutine("SetInvincible");}
        }

        private IEnumerator SetInvincible()
        {
            m_isInvincible = true;

            yield return new WaitForSeconds(invincibilityDurationSeconds);

            m_isInvincible = false;
        }

        public void SubscribeEvents()
        {
            EventManager.Instance.AddListener<LifeEvent>(LifeHasBeenModified);
        }

        public void UnsubscribeEvents()
        {
            EventManager.Instance.RemoveListener<LifeEvent>(LifeHasBeenModified);
        }
    }
}


