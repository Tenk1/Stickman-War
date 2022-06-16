using UnityEngine;

public class ScrollingUVs : MonoBehaviour
{
    public int materialIndex = 0;
    public Vector2 uvAnimationRate = new Vector2(1.0f, 0.0f);
    public string textureName = "_MainTex";
    Renderer r;

    private void Start()
    {
        r = GetComponent<Renderer>();
    }

    Vector2 uvOffset = Vector2.zero;

    void LateUpdate()
    {
        uvOffset += (uvAnimationRate * Time.deltaTime);
        if (r.enabled)
        {
            r.materials[materialIndex].SetTextureOffset(textureName, uvOffset);
        }
    }
}