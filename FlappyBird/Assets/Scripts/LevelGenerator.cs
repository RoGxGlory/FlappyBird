using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // Public variables for pipe generation and movement
    public GameObject pipePrefab;     // Reference to the pipe prefab

    public float initialSpawnRate = 2f; // Initial time interval between spawns
    public float initialPipeSpeed = 2f; // Initial speed of pipes
    public float initialGapSize = 6f; // Initial gap size between pipes

    public float spawnRateIncreaseSpeed = 0.025f; // How fast spawn rate decreases
    public float pipeSpeedIncreaseSpeed = 0.0025f; // How fast pipe speed increases
    public float gapSizeDecreaseSpeed = 0.05f; // How fast gap size decreases
    public float minimumSpawnRate = 0.5f; // Minimum spawn rate
    public float maximumPipeSpeed = 6f; // Maximum pipe speed
    public float minimumGapSize = 1f; // Minimum gap size

    private float currentSpawnRate; // Current spawn rate
    private float currentPipeSpeed; // Current pipe speed
    private float currentGapSize; // Current gap size


    public float timer;              // Timer to track spawn intervals

    public bool bIsPlaying = false;   // Bool showing if the game is being played

    void Start()
    {
        currentSpawnRate = initialSpawnRate;
        currentPipeSpeed = initialPipeSpeed;
        currentGapSize = initialGapSize;
        timer = 0f;
    }

    void Update()
    {
        if (bIsPlaying)
        {
            // Update the timer
            timer += Time.deltaTime;

            // Spawn a new pipe if the timer exceeds the current spawn rate
            if (timer >= currentSpawnRate)
            {
                SpawnPipe();
                timer = 0f;
            }

            // Gradually increase difficulty over time
            IncreaseDifficulty();
        }
    }

    public void SpawnPipe()
    {
        // Randomize the Y position within a range based on the gap size
        float spawnY = Random.Range(-currentGapSize / 1.2f, currentGapSize / 1.2f);

        // Instantiate the pipe prefab
        Vector3 spawnPosition = new Vector3(transform.position.x, spawnY, 0f);
        GameObject pipe = Instantiate(pipePrefab, spawnPosition, Quaternion.identity);

        // Set pipe movement speed
        PipeMovement pipeMovement = pipe.AddComponent<PipeMovement>();
        pipeMovement.speed = currentPipeSpeed;

        // Destroy the pipe after it moves offscreen
        Destroy(pipe, 10f);
    }

    void IncreaseDifficulty()
    {
        // Decrease the spawn rate (increase spawn frequency)
        currentSpawnRate = Mathf.Max(minimumSpawnRate, currentSpawnRate - spawnRateIncreaseSpeed * Time.deltaTime);

        // Increase the pipe speed
        currentPipeSpeed = Mathf.Min(maximumPipeSpeed, currentPipeSpeed + pipeSpeedIncreaseSpeed * Time.deltaTime);

        // Decrease the pipe gap size
        currentGapSize = Mathf.Max(minimumGapSize, currentGapSize - gapSizeDecreaseSpeed * Time.deltaTime);
    }

    public void ResetDifficulty()
    {
        currentSpawnRate = initialSpawnRate;
        currentPipeSpeed = initialPipeSpeed;
        currentGapSize = initialGapSize;
        timer = 0f;
    }
}

public class PipeMovement : MonoBehaviour
{
    public float speed = 2f; // Speed at which the pipe moves left

    void Update()
    {
        // Move the pipe to the left
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}
