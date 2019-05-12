﻿using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour {

    public enum State {
        Waiting,
        Playing,
        GameOver
    }

    public UnityEvent onGameStart;
    public UnityEvent onGameEnd;
    public UnityEvent onGameRestart;

    public State state { get; private set; }

    private void Start()
    {
        state = State.Waiting;
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
