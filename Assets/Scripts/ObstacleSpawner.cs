using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {

    [SerializeField] GameController controller = null;
    [SerializeField] GameObject prefab = null;
    [SerializeField] int poolSize = 8;
    [SerializeField] float[] spawnHeights = new float[] { 0f };
    [SerializeField] Vector2 velocity = Vector2.left;
    [SerializeField] float minTimeOffset = 1f;
    [SerializeField] float maxTimeOffset = 2f;

    Transform tr;
    GameObject[] pool;
    int poolIndex;

    private void Awake()
    {
        tr = transform;
    }

    public void StartSpawning()
    {
        if (!controller) {
            Debug.LogWarning ("Obstacle Spawner: Controller is not assigned");
            return;
        }
        if (!prefab) {
            Debug.LogWarning ("Obstacle Spawner: Prefab is not assigned");
            return;
        }
        if (poolSize <= 0) {
            throw new System.InvalidOperationException ("Pool size must be a positive integer");
        }
        pool = new GameObject[poolSize];
        poolIndex = 0;
        for (int i = 0; i < poolSize; i++) {
            pool[i] = Instantiate (prefab, tr.position, Quaternion.identity, tr);
            pool[i].SetActive (false);
        }
        StartCoroutine (Spawn ());
    }

    IEnumerator Spawn()
    {
        while (controller && controller.state == GameController.State.Playing) {
            GameObject obstacle = pool[poolIndex];
            poolIndex = (poolIndex + 1) % poolSize;
            obstacle.SetActive (false);
            obstacle.SetActive (true);
            obstacle.transform.localPosition = new Vector2 (0f, spawnHeights[Random.Range (0, spawnHeights.Length)]);
            obstacle.GetComponent<Rigidbody2D> ().velocity = velocity;
            yield return new WaitForSeconds (Random.Range (minTimeOffset, maxTimeOffset));
        }
    }

    public void ResetSpawner()
    {
        StopAllCoroutines ();
        foreach (GameObject obstacle in pool) {
            Destroy (obstacle);
        }
    }

    public void FreezeObstacles()
    {
        foreach (GameObject obstacle in pool) {
            obstacle.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
        }
    }
}
