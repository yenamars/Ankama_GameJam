using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrap : MonoBehaviour
{

    [Header("Trap")] public int power = 8;
    public float CoolDown;

    public void Update()
    {
        m_activeTimer -= Time.deltaTime;
    }
    
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(m_activeTimer > 0)
            return;

        m_activeTimer = CoolDown;
        Actor trapped = collider.GetComponent<Actor>();

        if (trapped != null)
        {
            trapped.Hit(power,HitType.Shot);
        }
    }

    private float m_activeTimer;
}
