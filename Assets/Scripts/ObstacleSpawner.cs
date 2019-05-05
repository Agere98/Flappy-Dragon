using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{

    [SerializeField] GameController controller;
    [SerializeField] GameObject prefab;
    [SerializeField] int poolSize = 8;
    [SerializeField] float minHeight;
    [SerializeField] float maxHeight;
    [SerializeField] Vector3 velocity;
    [SerializeField] float minTimeOffset;
    [SerializeField] float maxTimeOffset;

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
        }
        StartCoroutine (Spawn ());
    }

    IEnumerator Spawn()
    {
        while (controller.state == GameController.State.Playing) {
            GameObject obstacle = pool[poolIndex];
            poolIndex = (poolIndex + 1) % poolSize;
            obstacle.SetActive (false);
            obstacle.SetActive (true);
            obstacle.transform.position = new Vector2 (tr.position.x, Random.Range (minHeight, maxHeight));
            obstacle.GetComponent<Rigidbody2D> ().velocity = velocity;
            yield return new WaitForSeconds (Random.Range (minTimeOffset, maxTimeOffset));
        }
    }
}
