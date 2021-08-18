using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderController : MonoBehaviour
{
    [SerializeField] private int lifes;

    public int score;
    public System.Action<InvaderController> killed;

    private void Start()
    {
        score = 10 * lifes;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            lifes--;
            if (lifes == 0)
            {
                this.killed?.Invoke(this);
                this.gameObject.SetActive(false);
            }
        }
    }
}