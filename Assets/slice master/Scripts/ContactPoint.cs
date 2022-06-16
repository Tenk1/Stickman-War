using UnityEngine;

public class ContactPoint : MonoBehaviour
{
    [SerializeField] public KatanaCut katana;

    public Vector3 lastPosition;
    public static float velocity;

    public void FixedUpdate()
    {
        if (GameManager.Instance.m_GameState == GameState.gamePlay)
        {
            velocity = Vector3.Distance(transform.position, lastPosition) / Time.fixedDeltaTime;
            lastPosition = transform.position;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.m_GameState == GameState.gamePlay)
        {
            var sliceable = other.gameObject.GetComponent<Sliceable>();
            if (sliceable)
            {
                katana.Cut(transform.position, sliceable, velocity);
            }
        }
    }
}