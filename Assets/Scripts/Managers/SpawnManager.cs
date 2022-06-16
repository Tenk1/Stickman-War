using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class SpawnManager : Manager<SpawnManager>
{

    [SerializeField]  public bool canSpawn = false;
    [SerializeField]  public bool isMaxWave = false;

    [SerializeField]  public int currentWave;
    [SerializeField] private int index;

    [SerializeField] private List<SpawnPoint> spawnPoints;
    [SerializeField] public List<Wave> waves;

    public SoundHandler SFXHandler;

    public WaveUI m_waveUI;

    #region Manager implementation
    protected override IEnumerator InitCoroutine()
    {
        SubscribeEvents();
        
        yield break;
    }

    private void Update()
    {
        if (GameManager.Instance.IsPlaying)
        {
            if (canSpawn && !isMaxWave) { Spawn(); }
        }
        if (GameManager.Instance.m_GameState == GameState.gameVictory)
        {
            m_waveUI.clear();
        }
        
    }


    #endregion
    private void Spawn()
    {
        if (spawnPoints.Count >= 1)
        {
            int indexRand = Random.Range(0, spawnPoints.Count);  
            Wave w = waves[currentWave];
            if (w.Ennemies.Count > 0 && w.Ennemies.Count > index)
            {
                spawnPoints[indexRand].Spawn(w.Ennemies[index]);;
                EventManager.Instance.Raise(new EnemySpawnEvent() { nbEnemie = 1 });
                
                IncrementEnnemy();
            }
            
        }     
    }


    private void IncrementEnnemy()
    {         
        index++;   
        if (index >= waves[currentWave].Ennemies.Count)
        {
            canSpawn = false;
            IncrementWave();
        }
    }
    private void IncrementWave()
    {
        (SFXHandler.GetComponent(typeof(SoundHandler)) as SoundHandler).PlaySound(AudioSound.Wave, transform.position);
        currentWave++;
        index = 0;
        m_waveUI.affiche(currentWave, waves.Count);
        if (currentWave >= waves.Count)
        {
            isMaxWave = true; 
            canSpawn = false;
            EventManager.Instance.Raise(new NoNextWaveEvent() { });
        }
        
    }

    public void ResetWaves(){
        currentWave = 0;
        isMaxWave = false;
        canSpawn = false;
        m_waveUI.affiche(currentWave, waves.Count);
    }

    #region Events' subscription
    public override void SubscribeEvents()
    {
        //base.SubscribeEvents();
        EventManager.Instance.AddListener<SendNextWaveEvent>((SendNextWaveEvent e) => { canSpawn = true; });


    }

    public override void UnsubscribeEvents()
    {
        //  base.UnsubscribeEvents();
        EventManager.Instance.RemoveListener<SendNextWaveEvent>((SendNextWaveEvent e) => { });


    }
    #endregion
}
