using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameController : MonoBehaviour
{
    public enum State {
        Waiting,
        Playing,
        GameOver
    }

    public State state { get; private set; }

    [SerializeField] Dragon dragon;
    [SerializeField] TextMeshProUGUI scoreText;

    public UnityEvent onGameStart;
    public UnityEvent onGameEnd;
    public UnityEvent onGameRestart;

    private void Start()
    {
        state = State.Waiting;
    }

    private void Update()
    {
        scoreText?.SetText ("Score: " + dragon.score);
    }

    public void StartGame()
    {
        state = State.Playing;
        onGameStart.Invoke ();
    }

    public void EndGame()
    {
        state = State.GameOver;
        onGameEnd.Invoke ();
    }

    public void RestartGame()
    {
        state = State.Waiting;
        onGameRestart.Invoke ();
    }
}
