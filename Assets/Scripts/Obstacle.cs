﻿using UnityEngine;
using UnityEngine.Events;

public class Obstacle : MonoBehaviour
{
    [SerializeField] bool stopOnGameOver = true;

    public UnityEvent onCrash;

    public GameController controller { get; set; }
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D> ();
    }

    private void Update()
    {
        if (stopOnGameOver && controller.state == GameController.State.GameOver) rb.velocity = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onCrash.Invoke ();
    }
}
