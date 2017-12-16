using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour, IDamageable
{
    private Rigidbody2D rgdBody2D;

	void Awake () 
    {
        rgdBody2D = GetComponent<Rigidbody2D>();
	}

    public void Hit(int damages, Vector2 pushForce)
    {
        rgdBody2D.AddForce(pushForce, ForceMode2D.Impulse);
    }
}
