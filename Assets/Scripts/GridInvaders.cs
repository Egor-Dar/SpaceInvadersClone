using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GridInvaders : MonoBehaviour
{
    [Header("Invaders")] public InvaderController[] prefabs = new InvaderController[5];
    public AnimationCurve speed = new AnimationCurve();
    private float wightInvader=0.16f;
    public Vector3 direction { get; private set; } = Vector3.right;
    public Vector3 initialPosition { get; private set; }
    public System.Action<InvaderController> killed;

    public int AmountKilled { get; private set; }
    public int AmountAlive => TotalAmount - AmountKilled;
    public int TotalAmount => rows * columns;
    public float PercentKilled => (float) AmountKilled / (float) TotalAmount;

    [Header("Grid")] public int rows = 5;
    public int columns = 11;

    [Header("Missiles")] public Projectile missilePrefab;
    public float missileSpawnRate = 1.0f;

    private void Awake()
    {
        initialPosition = transform.position;

        for (int i = 0; i < rows; i++)
        {
            float width = 0.5f * (columns - 1);
            float height = 0.5f * (rows - 1);
            Vector2 centerOffset = new Vector2(-width * 0.5f, -height * 0.5f);
            Vector3 rowPosition = new Vector3(centerOffset.x, (0.5f * i) + centerOffset.y, 0.0f);

            for (int j = 0; j < columns; j++)
            {
                InvaderController invader = Instantiate(prefabs[i], transform);
                invader.killed += OnInvaderKilled;

                Vector3 position = rowPosition;
                position.x += 0.5f * j;
                invader.transform.localPosition = position;
            }
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), missileSpawnRate, missileSpawnRate);
    }

    private void MissileAttack()
    {
        int amountAlive = this.AmountAlive;

        if (amountAlive == 0) {
            return;
        }

        foreach (Transform invader in transform)
        {
            Vector2 playerRay = new Vector2(invader.transform.position.x, invader.transform.position.y - wightInvader);
            RaycastHit2D hit = Physics2D.Raycast(playerRay, Vector2.down);
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Invader") || !invader.gameObject.activeInHierarchy)
            {
                continue;
            }


            if (UnityEngine.Random.value < (1.0f / (float)amountAlive))
            {
                Instantiate(this.missilePrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }

    private void Update()
    {
        float speed = this.speed.Evaluate(PercentKilled);
        transform.position += direction * speed * Time.deltaTime;


        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (direction == Vector3.right && invader.position.x >= (rightEdge.x - 1.0f))
            {
                AdvanceRow();
                break;
            }
            else if (direction == Vector3.left && invader.position.x <= (leftEdge.x + 1.0f))
            {
                AdvanceRow();
                break;
            }
        }
    }

    private void AdvanceRow()
    {
        direction = new Vector3(-direction.x, 0.0f, 0.0f);

        Vector3 position = transform.position;
        position.y -= 1.0f;
        transform.position = position;
    }

    private void OnInvaderKilled(InvaderController invader)
    {
        invader.gameObject.SetActive(false);

        AmountKilled++;
        killed(invader);
    }

    public void ResetInvaders()
    {
        AmountKilled = 0;
        direction = Vector3.right;
        transform.position = initialPosition;

        foreach (Transform invader in transform)
        {
            invader.gameObject.SetActive(true);
        }
    }
}