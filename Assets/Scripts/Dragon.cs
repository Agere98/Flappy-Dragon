using UnityEngine;
using UnityEngine.Events;

public class Dragon : MonoBehaviour
{
    public int score { get; set; }

    [SerializeField] float groundHeight = -4f;

    Transform tr;
    Rigidbody2D rb;
    bool crashed = false;
    Vector3 spawnPoint;

    public UnityEvent onCrash;
    public UnityEvent onRespawn;

    private void Awake()
    {
        tr = transform;
        rb = GetComponent<Rigidbody2D> ();
        spawnPoint = tr.position;
    }

    private void FixedUpdate()
    {
        if (crashed) return;
        Vector2 face = rb.velocity + Vector2.right * 25f;
        tr.rotation = Quaternion.LookRotation (Vector3.forward, Vector3.Cross (Vector3.forward, face));
        if (tr.position.y < groundHeight) Crash ();
    }

    void Crash()
    {
        crashed = true;
        onCrash.Invoke ();
    }

    public void Respawn()
    {
        tr.position = spawnPoint;
        tr.rotation = Quaternion.identity;
        rb.velocity = Vector2.zero;
        crashed = false;
        score = 0;
        onRespawn.Invoke ();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals ("Obstacle"))
            Crash ();
    }
}
