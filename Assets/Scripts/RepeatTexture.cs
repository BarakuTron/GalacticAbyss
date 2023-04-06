using UnityEngine;

public class RepeatTexture : MonoBehaviour
{
    public float textureRepeatDistance = 1.0f;
    public Vector2 tilingFactor = new Vector2(1.0f, 1.0f);

    private Renderer floorRenderer;

    private void Awake()
    {
        floorRenderer = GetComponent<Renderer>();
        float planeWidth = transform.localScale.x;
        float planeLength = transform.localScale.z;
        Vector2 textureTiling = new Vector2(planeWidth / textureRepeatDistance, planeLength / textureRepeatDistance);
        tilingFactor = textureTiling;
        floorRenderer.material.mainTextureScale = tilingFactor;
    }
}