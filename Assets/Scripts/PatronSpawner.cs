using UnityEngine;
using System.Collections.Generic;

public class PatronSpawner : MonoBehaviour
{
    [Tooltip("Drag all 5 of your Patron Prefabs here")]
    public GameObject[] patronPrefabs;

    [Tooltip("Drag your 2 Spawn Point objects here")]
    public Transform[] spawnPoints;

    void Start()
    {
        SpawnRandomPatrons();
    }

    void SpawnRandomPatrons()
    {
        // Create a temporary list of the prefabs so we can pick from them
        List<GameObject> availablePatrons = new List<GameObject>(patronPrefabs);

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (availablePatrons.Count == 0) break;

            // Pick a random patron from the list
            int randomIndex = Random.Range(0, availablePatrons.Count);
            GameObject chosenPatron = availablePatrons[randomIndex];

            // Spawn them at the spawn point
            Instantiate(chosenPatron, spawnPoints[i].position, spawnPoints[i].rotation);

            // Remove them from the available list so we don't spawn twins
            availablePatrons.RemoveAt(randomIndex);
        }
    }
}