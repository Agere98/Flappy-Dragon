using UnityEngine;
using UnityEngine.Events;

public class Sheep : MonoBehaviour {

    public UnityEvent onCrash;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onCrash.Invoke ();
    }
}
