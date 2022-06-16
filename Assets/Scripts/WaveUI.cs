using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    public Text UIText;

    public void affiche(int currentWave, int maxWave) {
        UIText.text = "Wave " + currentWave + "/" + maxWave;
    }
    public void clear()
    {
        UIText.text = "";
    }

}
