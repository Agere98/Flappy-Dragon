using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameController controller;
    [SerializeField] GameObject prefab;
    [SerializeField] int poolSize = 8;
    [SerializeField] float[] spawnHeights;
    [SerializeField] Vector3 velocity;
    [SerializeField] float minTimeOffset;
    [SerializeField] float maxTimeOffset;
    [SerializeField] bool spawnOnStart = false;

    Transform tr;
    GameObject[] pool;
    int poolIndex;

    private void Awake()
    {
        tr = transform;
    }

    public void StartSpawning()
    {
        pool = new GameObject[poolSize];
        poolIndex = 0;
        for (int i = 0; i < poolSize; i++) {
            pool[i] = Instantiate (prefab, tr.position, Quaternion.identity, tr);
            pool[i].SetActive (false);
            Obstacle obstacle = pool[i].GetComponent<Obstacle> ();
            if (obstacle) obstacle.controller = controller;
        }
        StartCoroutine (Spawn ());
    }

    IEnumerator Spawn()
    {
        if (!spawnOnStart) yield return new WaitForSeconds (Random.Range (minTimeOffset, maxTimeOffset));
        while (controller.state == GameController.State.Playing) {
            GameObject obstacle = pool[poolIndex];
            poolIndex = (poolIndex + 1) % poolSize;
            obstacle.SetActive (false);
            obstacle.SetActive (true);
            obstacle.transform.position = new Vector2 (tr.position.x, spawnHeights[Random.Range (0, spawnHeights.Length)]);
            obstacle.GetComponent<Rigidbody2D> ().velocity = velocity;
            yield return new WaitForSeconds (Random.Range (minTimeOffset, maxTimeOffset));
        }
    }

    public void ResetSpawner()
    {
        StopAllCoroutines ();
        foreach (GameObject gameObject in pool) {
            Destroy (gameObject);
        }
    }
}
