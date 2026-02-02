[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/k890snLZ)
[![Open in Visual Studio Code](https://classroom.github.com/assets/open-in-vscode-2e0aaae1b6195c2367325f4f02e2d04e9abb55f0b24a779b69b11b9e10269abc.svg)](https://classroom.github.com/online_ide?assignment_repo_id=22471486&assignment_repo_type=AssignmentRepo)
# 2D Game LAB SHEET 01
## Player Controller + Health System + Score UI (Unity 2D)

---

## Objective

Create a simple 2D scene where:

- The player can move using the keyboard  
- Health is shown on the UI and decreases when hitting an obstacle  
- Score is shown on the UI (displayed and reset-ready)

---

## Learning Outcomes

By the end of this lab, you will be able to:

- Build a Unity 2D project from scratch  
- Create a controllable 2D player  
- Create UI text (Health + Score)  
- Reduce health using collision detection  
- Stop the game when health becomes 0  
- Prepare health & score systems for restart  

---

## Requirements

- Unity Hub  
- Unity Editor (2D Core Template)  
- Visual Studio / VS Code  

---

## Part A — Create the Project 

1. Open **Unity Hub**
2. Click **New Project**
3. Select **2D (Core)**
4. Project Name: `SE4031_2D_Game`
5. Click **Create**

### Save Scene

6. `File → Save As` → name: `MainGame`

---

## Part B — Create Player Object 

### 1. Create Player

Hierarchy → Right Click → Create Empty
→ Rename to: Player


---

### 2. Add Sprite

Select Player → Inspector → Add Component → Sprite Renderer
→ Choose any sprite (Square recommended)


---

### 3. Add Physics

Add Component → BoxCollider2D
Add Component → Rigidbody2D


Set:

- Gravity Scale = `0`
- Constraints → Freeze Rotation Z 

---

## Part C — Player Movement Script 

### Create Scripts Folder

Project Window → Right click → Create Folder → name: Scripts


---

### Create Script: `PlayerMovement.cs`


---

### Create Script: `PlayerMovement.cs`

Scripts → Right click → Create → C# Script → PlayerMovement


```csharp
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(x, y);
        transform.Translate(move * speed * Time.deltaTime);
    }
}
```

Attach Script

Drag PlayerMovement.cs → onto Player

---

## Part D — Create UI (Health & Score) 

### Create Canvas

Hierarchy → Right Click → UI → Canvas


---

### Create Health Text

Canvas → Right Click → UI → Text - TextMeshPro
→ Click "Import TMP Essentials" if asked


Rename: `HealthText`  
Set text: `Health: 100`  
Anchor: **Top-Left**

---

### Create Score Text

Duplicate HealthText → Rename: ScoreText
Change text: Score: 0
Anchor: Top-Right


---

## Part E — Health System Script 

### Create Script: `PlayerHealth.cs`


Scripts → Create → C# Script → PlayerHealth


Paste:

```csharp
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public TMP_Text healthText;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        UpdateUI();

        if (currentHealth == 0)
        {
            Time.timeScale = 0f;
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        Time.timeScale = 1f;
        UpdateUI();
    }

    void UpdateUI()
    {
        healthText.text = "Health: " + currentHealth;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            TakeDamage(10);
        }
    }
}
```

### Attach Script

Drag PlayerHealth.cs → onto Player

---

### Link UI

Select Player → PlayerHealth component  
Drag HealthText → Health Text field

---

## Part F — Score System Scrips

### Create GameManager Object

Hierarchy → Right click → Create Empty  
Rename: GameManager

---

### Create Script: ScoreManager.cs

Scripts → Create → C# Script → ScoreManager


```csharp
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public TMP_Text scoreText;

    void Start()
    {
        UpdateUI();
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateUI();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
    }
}

```
### Attach Script

Drag ScoreManager.cs → onto GameManager

---

### Link UI

Select GameManager → ScoreManager component  
→ Drag ScoreText → Score Text field

---

## Part G — Obstacle Setup

### Create Obstacle

Hierarchy → Right Click → 2D Object → Sprite → Square  
→ Rename: Obstacle

---

### Add

BoxCollider2D → Is Trigger

---

### Create Tag

Inspector → Tag → Add Tag → create Obstacle  
→ Assign tag to Obstacle

Move obstacle in front of the player.

---

## Test Your Game

Press Play

You should see:

✔ Player moves  
✔ Health UI updates  
✔ Health decreases on hit  
✔ Game stops at 0 health  
✔ Score displays correctly
