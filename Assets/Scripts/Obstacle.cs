using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private int lifes=3;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            lifes--;
            if (lifes == 0)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
