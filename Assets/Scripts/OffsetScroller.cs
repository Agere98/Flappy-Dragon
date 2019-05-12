using UnityEngine;

[RequireComponent (typeof (MeshRenderer))]
public class OffsetScroller : MonoBehaviour {

    [SerializeField] GameController controller = null;
    [SerializeField] bool stopOnGameOver = true;
    [SerializeField] float scrollSpeed = .05f;

    Vector2 savedOffset;
    MeshRenderer rn;

    private void Awake()
    {
        rn = GetComponent<MeshRenderer> ();
    }

    void Start()
    {
        savedOffset = rn.sharedMaterial.GetTextureOffset ("_MainTex");
    }

    void Update()
    {
        if (stopOnGameOver && controller && controller.state == GameController.State.GameOver) return;
        float x = Mathf.Repeat (Time.time * scrollSpeed, 1);
        Vector2 offset = new Vector2 (x, savedOffset.y);
        rn.sharedMaterial.SetTextureOffset ("_MainTex", offset);
    }

    void OnDisable()
    {
        rn.sharedMaterial.SetTextureOffset ("_MainTex", savedOffset);
    }
}