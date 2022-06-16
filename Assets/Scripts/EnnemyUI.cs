using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnnemyUI : MonoBehaviour
{
    public Text UIText;

    public void affiche(int nbEnnemies)
    {
        try
        {
            UIText.text = "Ennemies " + nbEnnemies;
        }
        catch { }
    }
    public void clear()
    {
        try { 
            UIText.text = "";
        }
        catch { }
    }
}
