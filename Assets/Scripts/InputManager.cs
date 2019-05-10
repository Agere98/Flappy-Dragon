using UnityEngine;

[RequireComponent (typeof (GameController))]
public class InputManager : MonoBehaviour {

    [SerializeField] Flapper player = null;

    GameController controller;

    private void Awake()
    {
        controller = GetComponent<GameController> ();
    }

    private void Update()
    {
        if (Input.GetButton ("Jump")) {
            if (!player) {
                Debug.LogWarning ("Input Manager: Player is not assigned");
            } else {
                switch (controller.state) {
                    case GameController.State.Waiting:
                        controller.StartGame ();
                        player.Flap ();
                        break;
                    case GameController.State.Playing:
                        player.Flap ();
                        break;
                    case GameController.State.GameOver:
                        controller.RestartGame ();
                        break;
                }
            }
        }
    }
}
