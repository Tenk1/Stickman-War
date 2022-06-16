using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SDD.Events;
using UnityEngine.SceneManagement;


public class LevelManager : Manager<LevelManager>
{

    #region Manager implementation
    protected override IEnumerator InitCoroutine()
    {
        yield break;
    }
    #endregion

    public void LoadMenu(string Menu)
    {
        //Loader.Load(Loader.Scene.Menu);
        SceneManager.LoadScene(Menu);
    }

    public void LoadEgypte(string Egypte)
    {
        //Loader.Load(Loader.Scene.Egypte);
        SceneManager.LoadScene(Egypte);
    }

    public void LoadMedieval(string Medieval)
    {
        //Loader.Load(Loader.Scene.Medieval);
        SceneManager.LoadScene(Medieval);
    }

    public void LoadModerne(string Moderne)
    {
        //Loader.Load(Loader.Scene.Moderne);
        SceneManager.LoadScene(Moderne);
    }

    public override void SubscribeEvents()
    {
        base.SubscribeEvents();
    }

    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
    }


}