using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;

public class SceneSetup : EditorWindow
{
    [MenuItem("Tools/Setup Game Scene")]
    public static void SetupScene()
    {
        // Create Player
        GameObject player = CreatePlayer();
        
        // Create multiple moving Obstacles
        CreateObstacles();
        
        // Create Canvas and UI
        GameObject canvas = CreateCanvas();
        GameObject healthText = CreateHealthText(canvas);
        GameObject scoreText = CreateScoreText(canvas);
        GameObject gameOverText = CreateGameOverText(canvas);
        
        // Create GameManager with collectible spawner
        GameObject gameManager = CreateGameManager();
        
        // Link references
        LinkReferences(player, healthText, gameOverText, gameManager, scoreText);
        
        Debug.Log("✅ Scene setup complete! Press Play to test.");
        Debug.Log("🎮 Controls: WASD or Arrow keys to move");
        Debug.Log("⭐ Collect yellow items for score (+10)");
        Debug.Log("💔 Avoid red obstacles (-10 health)");
        Debug.Log("🔄 Press R to restart when game over");
    }
    
    static GameObject CreatePlayer()
    {
        // Check if Player already exists
        GameObject existing = GameObject.Find("Player");
        if (existing != null)
        {
            DestroyImmediate(existing);
        }
        
        GameObject player = new GameObject("Player");
        player.transform.position = new Vector3(-3f, 0f, 0f);
        
        // Add SpriteRenderer with default white square
        SpriteRenderer sr = player.AddComponent<SpriteRenderer>();
        sr.sprite = CreateSquareSprite();
        sr.color = new Color(0f, 0.5f, 1f); // Blue color
        
        // Add BoxCollider2D
        BoxCollider2D collider = player.AddComponent<BoxCollider2D>();
        
        // Add Rigidbody2D
        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        // Add scripts
        player.AddComponent<PlayerMovement>();
        player.AddComponent<PlayerHealth>();
        
        return player;
    }
    
    static void CreateObstacles()
    {
        // Remove existing obstacles
        GameObject[] existingObstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obj in existingObstacles)
        {
            DestroyImmediate(obj);
        }
        
        // Also find by name in case tag isn't set
        for (int i = 1; i <= 5; i++)
        {
            GameObject existing = GameObject.Find("Obstacle" + i);
            if (existing != null) DestroyImmediate(existing);
            existing = GameObject.Find("Obstacle");
            if (existing != null) DestroyImmediate(existing);
        }
        
        // Ensure Obstacle tag exists
        CreateTagIfNotExists("Obstacle");
        CreateTagIfNotExists("Collectible");
        
        // Create 3 moving obstacles at different positions
        Vector3[] positions = new Vector3[]
        {
            new Vector3(3f, 0f, 0f),
            new Vector3(0f, 2f, 0f),
            new Vector3(-1f, -2f, 0f)
        };
        
        float[] speeds = new float[] { 2f, 2.5f, 3f };
        
        for (int i = 0; i < 3; i++)
        {
            CreateSingleObstacle("Obstacle" + (i + 1), positions[i], speeds[i]);
        }
    }
    
    static GameObject CreateSingleObstacle(string name, Vector3 position, float speed)
    {
        GameObject obstacle = new GameObject(name);
        obstacle.transform.position = position;
        obstacle.tag = "Obstacle";
        
        // Add SpriteRenderer
        SpriteRenderer sr = obstacle.AddComponent<SpriteRenderer>();
        sr.sprite = CreateSquareSprite();
        sr.color = Color.red;
        
        // Add BoxCollider2D as trigger
        BoxCollider2D collider = obstacle.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        
        // Add movement script
        ObstacleMovement movement = obstacle.AddComponent<ObstacleMovement>();
        movement.speed = speed;
        
        return obstacle;
    }
    
    static GameObject CreateCanvas()
    {
        // Check if Canvas already exists
        GameObject existing = GameObject.Find("Canvas");
        if (existing != null)
        {
            DestroyImmediate(existing);
        }
        
        // Create Canvas
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        canvasObj.AddComponent<GraphicRaycaster>();
        
        return canvasObj;
    }
    
    static GameObject CreateHealthText(GameObject canvas)
    {
        GameObject textObj = new GameObject("HealthText");
        textObj.transform.SetParent(canvas.transform, false);
        
        RectTransform rect = textObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);
        rect.anchoredPosition = new Vector2(20, -20);
        rect.sizeDelta = new Vector2(300, 50);
        
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = "Health: 100";
        tmp.fontSize = 36;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Left;
        
        return textObj;
    }
    
    static GameObject CreateScoreText(GameObject canvas)
    {
        GameObject textObj = new GameObject("ScoreText");
        textObj.transform.SetParent(canvas.transform, false);
        
        RectTransform rect = textObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(1, 1);
        rect.anchoredPosition = new Vector2(-20, -20);
        rect.sizeDelta = new Vector2(300, 50);
        
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = "Score: 0";
        tmp.fontSize = 36;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Right;
        
        return textObj;
    }
    
    static GameObject CreateGameOverText(GameObject canvas)
    {
        GameObject textObj = new GameObject("GameOverText");
        textObj.transform.SetParent(canvas.transform, false);
        
        RectTransform rect = textObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(0, 0);
        rect.sizeDelta = new Vector2(600, 200);
        
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = "GAME OVER!\nPress R to Restart";
        tmp.fontSize = 48;
        tmp.color = Color.red;
        tmp.alignment = TextAlignmentOptions.Center;
        
        textObj.SetActive(false); // Hidden by default
        
        return textObj;
    }
    
    static GameObject CreateGameManager()
    {
        // Check if GameManager already exists
        GameObject existing = GameObject.Find("GameManager");
        if (existing != null)
        {
            DestroyImmediate(existing);
        }
        
        GameObject gameManager = new GameObject("GameManager");
        gameManager.AddComponent<ScoreManager>();
        gameManager.AddComponent<CollectibleSpawner>();
        
        return gameManager;
    }
    
    static void LinkReferences(GameObject player, GameObject healthText, GameObject gameOverText, GameObject gameManager, GameObject scoreText)
    {
        // Link HealthText and GameOverText to PlayerHealth
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.healthText = healthText.GetComponent<TMP_Text>();
            playerHealth.gameOverText = gameOverText.GetComponent<TMP_Text>();
        }
        
        // Link ScoreText to ScoreManager
        ScoreManager scoreManager = gameManager.GetComponent<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.scoreText = scoreText.GetComponent<TMP_Text>();
        }
    }
    
    static Sprite CreateSquareSprite()
    {
        // Try to find the built-in square sprite
        Sprite[] sprites = Resources.FindObjectsOfTypeAll<Sprite>();
        foreach (Sprite s in sprites)
        {
            if (s.name == "Square" || s.name == "UISprite")
            {
                return s;
            }
        }
        
        // Create a simple white texture as fallback
        Texture2D tex = new Texture2D(32, 32);
        Color[] colors = new Color[32 * 32];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.white;
        }
        tex.SetPixels(colors);
        tex.Apply();
        
        return Sprite.Create(tex, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
    }
    
    static void CreateTagIfNotExists(string tagName)
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");
        
        bool found = false;
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(tagName))
            {
                found = true;
                break;
            }
        }
        
        if (!found)
        {
            tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);
            SerializedProperty n = tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1);
            n.stringValue = tagName;
            tagManager.ApplyModifiedProperties();
        }
    }
}
