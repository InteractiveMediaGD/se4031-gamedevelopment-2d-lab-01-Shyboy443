using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    public float spawnInterval = 2f;
    public float minX = -7f;
    public float maxX = 7f;
    public float minY = -3.5f;
    public float maxY = 3.5f;
    
    private float nextSpawnTime;

    void Start()
    {
        // Create Collectible tag if needed
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnCollectible();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnCollectible()
    {
        // Limit max collectibles on screen
        GameObject[] existing = GameObject.FindGameObjectsWithTag("Collectible");
        if (existing.Length >= 5) return;
        
        Vector3 spawnPos = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            0f
        );
        
        GameObject collectible = new GameObject("Collectible");
        collectible.transform.position = spawnPos;
        collectible.tag = "Collectible";
        
        // Add sprite
        SpriteRenderer sr = collectible.AddComponent<SpriteRenderer>();
        sr.color = Color.yellow;
        collectible.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        
        // Create simple sprite
        Texture2D tex = new Texture2D(32, 32);
        Color[] colors = new Color[32 * 32];
        for (int i = 0; i < colors.Length; i++)
            colors[i] = Color.white;
        tex.SetPixels(colors);
        tex.Apply();
        sr.sprite = Sprite.Create(tex, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
        
        // Add trigger collider
        CircleCollider2D collider = collectible.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        
        // Destroy after 5 seconds if not collected
        Destroy(collectible, 5f);
    }
}
