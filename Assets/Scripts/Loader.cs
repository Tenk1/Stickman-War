using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public static class Loader
{
    public enum Scene
    {
        Menu, Egypte, Medieval, Moderne, Loading,
    }

    private static Action onLoaderCallBack; 

    public static void Load(Scene scene)
    {
       
        onLoaderCallBack = () =>
        {
            SceneManager.LoadScene(scene.ToString());
        };
        SceneManager.LoadScene(Scene.Loading.ToString());

    }

    public static void LoaderCallBack()
    {
        if (onLoaderCallBack != null)
        {
            onLoaderCallBack();
            onLoaderCallBack = null;
        }

    }
}
