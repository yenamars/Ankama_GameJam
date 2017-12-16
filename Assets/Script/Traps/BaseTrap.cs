﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseTrap : MonoBehaviour, IDamageable
{

    [Header("Trap")] public int power = 8;
    public float StopDuration = 2;
    public float CoolDown;

    [Header("Visual")] public Sprite ActiveSprite;
    public Sprite OffSprite;

    [Header("FXs")]
    public GameObject trapFX;
    public StackableShakeData trapShake;

    public virtual void Awake()
    {
        m_renderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Update()
    {
        m_activeTimer -= Time.deltaTime;
        m_renderer.sprite = m_activeTimer < 0 ? ActiveSprite : OffSprite;
    }
    
    public void OnTriggerStay2D(Collider2D collider)
    {
        if(m_activeTimer > 0)
            return;

        m_activeTimer = CoolDown;
        TrapTarget trapped = collider.GetComponent<TrapTarget>();
        ApplyEffect(trapped);
    }

    protected virtual void ApplyEffect(TrapTarget trapped)
    { 
        if (trapped != null)
        {
            trapped.Parent.Hit(power, Vector2.zero);
            trapped.Parent.StopFor(StopDuration);
        }
    }

    public virtual void Hit(int damages, Vector2 pushForce)
    {
    }

    private float m_activeTimer;
    private SpriteRenderer m_renderer;
}
