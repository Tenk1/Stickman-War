using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioSound : ushort
{
    Pillard = 0,
    EnnemyHit = 1,
    PlayerHit = 2,
    Slash = 3,
    Bazooka = 4,
    MissileHit = 5,
    Victory = 6,
    Defeat = 7,
    EnnemyWalk = 8,
    CanonBall = 9,
    SwordSpawn = 10,
    Gun = 11,
    Wave = 12,
    Load = 13
}

public class SoundHandler : MonoBehaviour
{
    [SerializeField] public List<AudioClip> AudioList;

    private Dictionary<int, float> soundVolume = new Dictionary<int, float>()
    {
        { 0 , 1f },
        { 1 , 1f },
        { 2 , 1f },
        { 3 , 0.1f },
        { 4 , 0.3f },
        { 5 , 1f },
        { 6 , 1f },
        { 7 , 1f },
        { 8 , 1f },
        { 9 , 1f },
        { 10 , 1f },
        { 11 , 0.4f },
        { 12 , 1f },
        { 13 , 1f }
    };

    public void PlaySound(AudioSound pIndex, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(AudioList[(int)pIndex], position, soundVolume[(int)pIndex] );
    }
}
