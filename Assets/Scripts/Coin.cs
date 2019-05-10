using UnityEngine;

[RequireComponent (typeof (SpriteRenderer))]
[RequireComponent (typeof (Collider2D))]
public class Coin : MonoBehaviour {

    SpriteRenderer rn;
    Collider2D cl;

    private void Awake()
    {
        rn = GetComponent<SpriteRenderer> ();
        cl = GetComponent<Collider2D> ();
    }

    private void OnEnable()
    {
        rn.enabled = true;
        cl.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Dragon dragon = collision.GetComponent<Dragon> ();
        if (dragon) {
            dragon.score++;
            rn.enabled = false;
            cl.enabled = false;
        }
    }
}
