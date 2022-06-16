using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave 
{
    public List<GameObject> ennemies;

    public List<GameObject> Ennemies { get => ennemies; set => ennemies = value; }
}
