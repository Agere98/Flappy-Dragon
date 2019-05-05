using UnityEngine;
using UnityEngine.SceneManagement;
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

    [SerializeField] TextMeshProUGUI scoreText;

    public UnityEvent onGameStart;
    public UnityEvent onGameEnd;

    private void Start()
    {
        state = State.Waiting;
    }

    private void Update()
    {
        scoreText.SetText ("Score: " + Coin.score);
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
        SceneManager.LoadSceneAsync (SceneManager.GetActiveScene ().buildIndex);
    }
}
