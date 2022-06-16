using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using SDD.Events;
using System.Linq;
using Player;

public enum GameState { gameMenu, gamePlay, gameNextLevel, gamePause, gameOver, gameVictory, gamePreVictory }

public class GameManager : Manager<GameManager>
{
    #region Game State
    [SerializeField] public GameState m_GameState;
    public bool IsPlaying { get { return m_GameState == GameState.gamePlay; } }
    public GameObject playerLife;

    public GameObject Stands;
    public GameObject Pillars;
    bool started = false;
    bool replay = false;

    public SoundHandler SFXHandler;
    bool gameOverDone = false;

    public GameObject YouWinText;
    public GameObject YouLoseText;

    public Vector3 targetPillars, pillarsOrigin, targetStands, standsOrigin;
    public EnnemyUI m_ennemyUI;
    #endregion

    #region Manager implementation
    protected override IEnumerator InitCoroutine()
    {
        SubscribeEvents();
       // InitNewGame(); // essentiellement pour que les statistiques du jeu soient mise à jour en HUD
        yield break;
    }
    #endregion

    #region Game flow & Gameplay
    //Game initialization
    void InitNewGame()
    {
        started = true;
        EventManager.Instance.Raise(new SendNextWaveEvent() { });
    }
    #endregion

    #region Ennemy
    [SerializeField] private int ennemyCount = 0;
    [SerializeField] private bool isNoMoreWave;

    public void IncrementEnnemy(EnemySpawnEvent e)
    {
       // Debug.Log("Ennemy Spawn");
        ennemyCount += e.nbEnemie;
        try
        {
            m_ennemyUI.affiche(ennemyCount);
        }
        catch { }
    }

    public void DecrementEnnemy(EnemyDeath e)
    {
        ennemyCount -= 1;
        try
        {
            m_ennemyUI.affiche(ennemyCount);
            //Debug.Log("decrementation enemie , test : "+(ennemyCount <= 0 && !isNoMoreWave));
            if (ennemyCount <= 0 && !isNoMoreWave)
            {
                // Debug.Log("Envoit event new waves par GM");
                EventManager.Instance.Raise(new SendNextWaveEvent() { });
            }
            if (ennemyCount <= 0 && isNoMoreWave)
            {
                m_ennemyUI.clear();
                Victory();
            }
        }
        catch { }
    }

    #endregion

    private void Update()
    {
        // Debug.Log(m_GameState + " + " + replay);
       // Debug.Log(standsOrigin + "/" + targetStands);

        if (m_GameState == GameState.gameMenu && replay == true)
        {
            //ApparitionPilliers();
            ApparitionStands();
            
        }

        if (m_GameState == GameState.gamePlay)
        {
            DisparitionPilliers();
            DisparitionStands();
            
            
        }

        if(m_GameState == GameState.gameVictory)
        { 
            ApparitionPilliers();
        }

        if(m_GameState == GameState.gamePause)
        {
            ApparitionPilliers();
            
        }

        if (m_GameState == GameState.gameOver)
        {
            GameOver();
            ApparitionPilliers();
        }

    }

    #region Animations
    public void ApparitionPilliers()
    {
        if (Pillars.transform.localPosition.y <= pillarsOrigin.y)
        {
                Pillars.transform.localPosition += new Vector3(0, Time.deltaTime, 0);
        }
    }

    public void DisparitionPilliers()
    {
        if (Pillars.transform.localPosition.y > targetPillars.y)
        {
            Pillars.transform.localPosition += new Vector3(0, -Time.deltaTime, 0);
        }
    }

    public void ApparitionStands()
    {
        
        if (Stands.transform.localPosition.y <= standsOrigin.y)
        {
           // Debug.Log("lol");
            Stands.transform.localPosition += new Vector3(0, Time.deltaTime, 0);
        }
    }

    public void DisparitionStands()
    {
        if (Stands.transform.localPosition.y > targetStands.y )
        {
            Stands.transform.localPosition += new Vector3(0, -Time.deltaTime, 0);
        }
    }

    #endregion
    
    #region GameState methods

    public void Play()
    {
        gameOverDone = false;
        if (m_GameState == GameState.gameMenu)
        {
            InitNewGame();
            m_GameState = GameState.gamePlay;

            // EventManager.Instance.Raise(new GamePlayEvent());
        }
        else
        {
            ResetGame();
            SpawnManager.Instance.ResetWaves();
            InitNewGame();
            replay = true;
        }
        
    }

    private void Victory()
    {
        YouWinText.SetActive(true);
        gameOverDone = false;
        (SFXHandler.GetComponent(typeof(SoundHandler)) as SoundHandler).PlaySound(AudioSound.Victory, transform.position);
        m_GameState = GameState.gameVictory;
    }

    private void GameOver()
    {
        if (!gameOverDone)
        {
            (SFXHandler.GetComponent(typeof(SoundHandler)) as SoundHandler).PlaySound(AudioSound.Defeat, transform.position);
            gameOverDone = true;
        }
        YouLoseText.SetActive(true);
        m_GameState = GameState.gameOver;
    }

    public void Pause()
    {
        if (started)
        {
            m_GameState = GameState.gamePause;
        }
    }

    public void UnPause()
    {
        m_GameState = GameState.gamePlay;
    }

    public void ResetGame()
    {
        (playerLife.GetComponent(typeof(PlayerLife)) as PlayerLife).Start();
        isNoMoreWave = false;
        m_GameState = GameState.gameMenu;
        started = false;

        YouLoseText.SetActive(false);
        YouWinText.SetActive(false);

        foreach (GameObject t in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            EventManager.Instance.Raise(new EnemyLifeEvent() { eLife = 1000, obj = t });
        }
    }

    #endregion

    #region Events' subscription
    public override void SubscribeEvents()
    {
        base.SubscribeEvents();
        EventManager.Instance.AddListener<NoNextWaveEvent>((NoNextWaveEvent e )=> { isNoMoreWave = true; });
        EventManager.Instance.AddListener<EnemySpawnEvent>(IncrementEnnemy);
        EventManager.Instance.AddListener<EnemyDeath>(DecrementEnnemy);
    }

    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        EventManager.Instance.RemoveListener<NoNextWaveEvent>((NoNextWaveEvent e) => { });
        EventManager.Instance.RemoveListener<EnemySpawnEvent>((EnemySpawnEvent e) => { });
        EventManager.Instance.RemoveListener<EnemyDeath>((EnemyDeath e) => { });
    }
    #endregion
}
