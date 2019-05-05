using UnityEngine;

public class OffsetScroller : MonoBehaviour
{
    [SerializeField] GameController controller;
    [SerializeField] bool stopOnGameOver = true;
    [SerializeField] float scrollSpeed;

    Vector2 savedOffset;
    new MeshRenderer renderer;

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer> ();
    }

    void Start()
    {
        savedOffset = renderer.sharedMaterial.GetTextureOffset ("_MainTex");
    }

    void Update()
    {
        if (stopOnGameOver && controller.state == GameController.State.GameOver) return;
        float x = Mathf.Repeat (Time.time * scrollSpeed, 1);
        Vector2 offset = new Vector2 (x, savedOffset.y);
        renderer.sharedMaterial.SetTextureOffset ("_MainTex", offset);
    }

    void OnDisable()
    {
        renderer.sharedMaterial.SetTextureOffset ("_MainTex", savedOffset);
    }
}