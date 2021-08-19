using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameManager : MonoBehaviour
{
    private GridInvaders invaders;
    private PlayerController player;
    public Text scoreText;
    private int Score;

    public int score { get; set; }


    private void Awake()
    {
        this.player = FindObjectOfType<PlayerController>();
        this.invaders = FindObjectOfType<GridInvaders>();
    }

    private void Start()
    {
        this.player.killed += OnPlayerKilled;
        this.invaders.killed += OnInvaderKilled;
    }

    private void SetScore(int score)
    {
        this.score = score;
        this.scoreText.text = this.score.ToString();
    }
    private void OnInvaderKilled(InvaderController invader)
    {
        SetScore(this.score + invader.score);

    }
    private void OnPlayerKilled()
    {
        this.player.gameObject.SetActive(false);
    }

}
