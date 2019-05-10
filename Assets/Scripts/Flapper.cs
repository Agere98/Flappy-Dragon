using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof (Rigidbody2D))]
public class Flapper : MonoBehaviour {

    [SerializeField] float flapForce = 8f;
    [SerializeField] float flapDelay = .1f;
    [SerializeField] float maxHeight = 3.5f;
    [SerializeField] float borderHeight = 2.5f;

    Transform tr;
    Rigidbody2D rb;

    public UnityEvent onFlap;

    private void Awake()
    {
        tr = transform;
        rb = GetComponent<Rigidbody2D> ();
    }

    public void Flap()
    {
        StartCoroutine (AddForceWithDelay ());
        onFlap.Invoke ();
    }

    IEnumerator AddForceWithDelay()
    {
        yield return new WaitForSeconds (flapDelay);
        float force = (1f - Mathf.Clamp01 ((tr.localPosition.y - borderHeight) / (maxHeight - borderHeight))) * flapForce;
        rb.velocity = new Vector2 (0f, force);
    }
}
