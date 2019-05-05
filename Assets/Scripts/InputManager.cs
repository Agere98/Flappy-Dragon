using UnityEngine;

public class InputManager : MonoBehaviour
{

    [SerializeField] Flapper player;
    [SerializeField] GameController controller;

    private void Update()
    {
        if (Input.GetButtonDown ("Jump")) {
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
