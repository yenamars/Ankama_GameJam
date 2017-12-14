using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseTrap : MonoBehaviour
{

    [Header("Trap")] public int power = 8;
    public float StopDuration = 2;
    public float CoolDown;

    [Header("Visual")] public Sprite ActiveSprite;
    public Sprite OffSprite;

    [Header("FXs")]
    public GameObject trapFX;
    public StackableShakeData trapShake;

    public void Awake()
    {
        m_renderer = GetComponentInChildren<SpriteRenderer>();
        m_mobRoot = GameObject.FindGameObjectWithTag("MobRoot");
    }

    public void Update()
    {
        m_activeTimer -= Time.deltaTime;
        m_renderer.sprite = m_activeTimer < 0 ? ActiveSprite : OffSprite;
    }
    
    public void OnTriggerEnter2D(Collider2D collider)
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


    private float m_activeTimer;
    private SpriteRenderer m_renderer;
    protected GameObject m_mobRoot;
}
