using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BLINDED_AM_ME;
using UnityEditor;
using UnityEngine;
using Autohand;

public class KatanaCut : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rgd;
   // [SerializeField] private Transform hand;
    
    [Header("Parameters")]
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private float rotationOffset;
    [SerializeField] static public float cutVelocityThreshold = 10f;
    [SerializeField] private float cutCheckInterval = .05f;

    public SoundHandler SFXHandler;

    private UnityEngine.ContactPoint? lastContact;
    private float lastCutTime;
    private HashSet<Sliceable> previousCuts;
    detachable detachable;

    private void Update()
    {
        
        if (Time.time - lastCutTime > cutCheckInterval && GameManager.Instance.m_GameState == GameState.gamePlay)
        {
            previousCuts = new HashSet<Sliceable>(FindObjectsOfType<Sliceable>());
            lastCutTime = Time.time;
        }
    }

    public void setSFXHandler(SoundHandler pSFX)
    {
        SFXHandler = pSFX;
    }

    public void Cut(Vector3 contact , Sliceable victim, float velocity)
    {
        if (GameManager.Instance.m_GameState == GameState.gamePlay)
        {
            (SFXHandler.GetComponent(typeof(SoundHandler)) as SoundHandler).PlaySound(AudioSound.Slash, transform.position);
            if (velocity > cutVelocityThreshold && previousCuts.Contains(victim))
            {
                //detachable.Cut();
                previousCuts.Remove(victim);

                var cutNormal = transform.right;

                var cuts = MeshCut
                    .Cut(victim.gameObject, contact, cutNormal, victim.CutMaterial)
                    .OrderByDescending(c => c.GetComponent<MeshFilter>().mesh.Volume())
                    .ToList();

                FixChunk(cuts[0], victim, cutNormal);
                FixChunk(cuts[1], victim, -cutNormal);
            }
        }
    }

    private void FixChunk(GameObject chunk, Sliceable victim, Vector3 offsetDirection)
    {
        // Resets collider to adjust for current mesh
        Destroy(chunk.GetComponent<Collider>());
        var collider = chunk.AddComponent<MeshCollider>();
        collider.convex = true;
       
        var sliceable = chunk.GetOrAddComponent<Sliceable>();
        var rigidbody = chunk.GetOrAddComponent<Rigidbody>();
        var meshFilter = chunk.GetOrAddComponent<MeshFilter>();
        var grabbable = chunk.GetOrAddComponent<Grabbable>();

        sliceable.CutMaterial = victim.CutMaterial;
        sliceable.MaterialDensity = victim.MaterialDensity;
        rigidbody.mass = meshFilter.mesh.Volume() * sliceable.MaterialDensity;
        rigidbody.MovePosition(chunk.transform.position + offsetDirection * .01f);
    }
}
