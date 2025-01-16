using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private GameManager gameManager;  // REF to the game manager
    private ScoreManager scoreManager; // REF to the score manager

    // Public variables to adjust in the Unity Inspector
    public float jumpForce = 10f; // Force of the jump
    public Rigidbody2D rb;      // Reference to the Rigidbody2D component
    public float gravityScale = 2f;

    public Transform birdVisual; // Reference to the visual part of the bird (child object)
    public float rotationSpeed = 5f; // Speed of rotation
    private float maxRotation = 45f;   // Maximum upward rotation angle
    private float minRotation = -45f; // Maximum downward rotation angle

    // Start is called before the first frame update
    void Start()
    {

        // Find the GameManager in the scene
        gameManager = FindObjectOfType<GameManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene.");
        }

        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager not found in the scene.");
        }

        // Ensure we have a Rigidbody2D component attached
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (rb == null)
        {
            Debug.LogError("No Rigidbody2D attached to the player object. Please attach one.");
        }

        rb.gravityScale = gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Check for input (Spacebar or screen tap on mobile)
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Jump();
        }

        // Check for input (Escape)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        // Adjust rotation based on vertical velocity
        AdjustRotation();
    }

    void Pause()
    {
        gameManager.ShowInGamePauseUI();
    }

    void Jump()
    {
        // Reset vertical velocity to zero before applying jump force
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

        // Apply upward force to simulate a jump
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void AdjustRotation()
    {
        if (birdVisual == null) return;

        float targetRotation;

        // Determine the target rotation based on the vertical velocity
        if (rb.linearVelocity.y > 0)
        {
            targetRotation = maxRotation;
        }
        else
        {
            targetRotation = minRotation;
        }

        // Interpolate towards the target rotation
        float zRotation = Mathf.LerpAngle(birdVisual.eulerAngles.z, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        birdVisual.rotation = Quaternion.Euler(0, 0, zRotation);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Trigger Game Over when the player collides with an obstacle
        Debug.Log("Obstacle !");
        gameManager.GameOver();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Updates the score when the player collides with a score trigger
        Debug.Log("Scored !");
        scoreManager.AddScore(1);
        gameManager.UpdateCurrentScore(scoreManager.CurrentScore);
    }

}