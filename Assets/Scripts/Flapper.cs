using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Flapper : MonoBehaviour
{
    [SerializeField] float flapForce = 3f;
    [SerializeField] float flapDelay = .1f;
    [SerializeField] float maxHeight = 3.5f;
    [SerializeField] float borderHeight = 2.5f;

    public UnityEvent onFlap;

    Transform tr;
    Rigidbody2D rb;

    private void Awake()
    {
        tr = transform;
        rb = GetComponent<Rigidbody2D> ();
    }

    public void Flap()
    {
        StartCoroutine (AddForceWithDelay());
        onFlap.Invoke ();
    }

    IEnumerator AddForceWithDelay()
    {
        yield return new WaitForSeconds (flapDelay);
        rb.velocity = Vector2.zero;
        float force = (1f - Mathf.Clamp01 ((tr.position.y - borderHeight) / (maxHeight - borderHeight))) * flapForce;
        rb.AddForce (Vector3.up * force, ForceMode2D.Impulse);
    }
}
