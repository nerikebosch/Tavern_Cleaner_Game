using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Where to Spawn")]
    [Tooltip("Drag all your SpawnPoint objects into this list")]
    public Transform[] spawnPoints;

    [Header("What to Spawn")]
    [Tooltip("Drag your Mug, Bottle, and Egg PREFABS into this list")]
    public GameObject[] itemPrefabs;

    [Header("Spawn Settings")]
    [Range(0f, 1f)]
    [Tooltip("0.5 means 50% chance a spot gets an item. 1.0 means every spot gets an item.")]
    public float spawnChance = 0.5f;

    void Start()
    {
        SpawnItems();
    }

    void SpawnItems()
    {
        // Look at every single spawn point we created
        foreach (Transform point in spawnPoints)
        {
            // Flip a coin based on our spawnChance
            if (Random.value <= spawnChance)
            {
                // Pick a random item from our Prefabs list
                int randomIndex = Random.Range(0, itemPrefabs.Length);
                GameObject itemToSpawn = itemPrefabs[randomIndex];

                // Spawn the chosen item at this exact point
                Instantiate(itemToSpawn, point.position, point.rotation);
            }
        }
    }
}