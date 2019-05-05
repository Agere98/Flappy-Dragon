using UnityEngine;
using UnityEngine.Events;

public class Dragon : MonoBehaviour
{
    [SerializeField] float groundHeight = -4f;

    Transform tr;
    Rigidbody2D rb;
    SpriteRenderer rn;
    bool crashed = false;

    public UnityEvent onCrash;

    private void Awake()
    {
        tr = transform;
        rb = GetComponent<Rigidbody2D> ();
        rn = GetComponent<SpriteRenderer> ();
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
        rn.enabled = false;
        onCrash.Invoke ();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals ("Obstacle"))
            Crash ();
    }
}
