using UnityEngine;
using UnityEngine.SceneManagement;

public class Coin : MonoBehaviour
{
    public static int score { get; private set; }

    SpriteRenderer rn;
    Collider2D cl;

    private void Awake()
    {
        rn = GetComponent<SpriteRenderer> ();
        cl = GetComponent<Collider2D> ();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        score = 0;
    }

    private void OnEnable()
    {
        rn.enabled = true;
        cl.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        score++;
        rn.enabled = false;
        cl.enabled = false;
    }
}
