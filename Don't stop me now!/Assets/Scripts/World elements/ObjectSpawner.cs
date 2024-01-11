using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;

    [Tooltip("Delay before the first spawn")]
    public float initialDelay = 0f;

    [Tooltip("Delay between each spawn")] public float spawnTimer = 1f;

    [Tooltip("Random range added or subtracted to the spawn timer")]
    public float spawnDelayRandomRange = 0f;

    [Tooltip("Maximum number of spawned object. Used for object pooling.")]
    public int maxSpawnCount = 5;

    private GameObject[] _spawnedObjects;
    private int _currentSpawnIndex = 0;
    private Collider2D _boxCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        _spawnedObjects = new GameObject[maxSpawnCount];

        // Spawn all objects at start and deactivate them
        // object pooling -> reduces computation time during gameplay
        for (int i = 0; i < maxSpawnCount; i++)
        {
            _spawnedObjects[i] = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            _spawnedObjects[i].SetActive(false);
        }

        _boxCollider2D = GetComponent<BoxCollider2D>();
        if (_boxCollider2D == null)
            StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            _spawnedObjects[_currentSpawnIndex].SetActive(false);
            _spawnedObjects[_currentSpawnIndex].transform.position = transform.position;
            _spawnedObjects[_currentSpawnIndex].SetActive(true);
            _currentSpawnIndex = (_currentSpawnIndex + 1) % maxSpawnCount;

            yield return new WaitForSeconds(spawnTimer + Random.Range(-spawnDelayRandomRange, spawnDelayRandomRange));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            StartCoroutine(SpawnCoroutine());
    }
}