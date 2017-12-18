using System.Collections;
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
    public SpriteRenderer spriteRenderer;

    [Header("FXs")]
    public GameObject trapFX;
    public StackableShakeData trapShake;

    public virtual void Awake()
    {
        m_renderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        if(m_reloading == true)
            return;

        m_activeTimer = CoolDown;
        TrapTarget trapped = collider.GetComponent<TrapTarget>();
        ApplyEffect(trapped);

        StartCoroutine(ReloadCoroutine());
    }

    protected virtual void ApplyEffect(TrapTarget trapped)
    { 
        if (trapped != null)
        {
            trapped.Parent.Hit(power, Vector2.zero);
            trapped.Parent.StopFor(StopDuration);
        }
    }

    IEnumerator ReloadCoroutine()
    {
        m_reloading = true;
        m_renderer.sprite = OffSprite;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.0f);
        yield return new WaitForSeconds(m_activeTimer);
        m_renderer.sprite = ActiveSprite;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1.0f);
        m_reloading = false;
    }

    public virtual void Hit(int damages, Vector2 pushForce)
    {
    }

    private float m_activeTimer;
    private SpriteRenderer m_renderer;

    private bool m_reloading;
}
