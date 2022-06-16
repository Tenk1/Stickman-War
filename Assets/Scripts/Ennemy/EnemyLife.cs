using SDD.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : MonoBehaviour, IEventHandler
{
    
    [SerializeField] public int m_maxLife;
    public int m_currentLife;

    private void Start()
    {
        m_currentLife = m_maxLife;
        SubscribeEvents();
    }

    private void LifeHasBeenModified(EnemyLifeEvent e)
    {
        if (e.obj != transform.root.gameObject) { return; }
        if (e.eLife > 0 && m_currentLife>0) { applyDommage(e.eLife); }
        if (e.eLife < 0) { applyHealing(e.eLife); }
    }

    private void applyHealing(int heal)
    {
        m_currentLife += heal;
        if (m_currentLife > m_maxLife) m_currentLife = m_maxLife;
    }

    public void applyDommage(int dommage)
    {
        m_currentLife -= dommage;
        if (m_currentLife <= 0)
        {
            EventManager.Instance.Raise(new EnemyDeath() { obj = transform.root.gameObject });
            UnsubscribeEvents();
        }
    }

  
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<EnemyLifeEvent>(LifeHasBeenModified);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<EnemyLifeEvent>(LifeHasBeenModified);
    }

}
