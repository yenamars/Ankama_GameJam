using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : Actor
{
	[Header("BaseAI")] 
    public float maxSpeed;
    public AudioSource deathSound;
    public Vector2 deathSoundRandomPitch;
    protected bool isAlive;
    protected bool isActive;
    private bool finishDeath;
    private Collider2D coll;
    private Spawner spawner;

	[HideInInspector] public bool IsDead = false;
	
	public virtual void Awake()
	{
		m_target = GameObject.FindGameObjectWithTag("Player");
        coll = GetComponent<Collider2D>();
		base.Awake();
	}
	
    public void SetSpawner(Spawner s)
    {
        spawner = s;
    }

    void OnEnable()
    {
        isAlive = true;
        isActive = false;
        finishDeath = false;
        coll.enabled = true;
    }

	public virtual void Update()
	{
        if (isActive == false || isAlive == false)
            return;

		base.Update();
		if(m_target == null)
			m_target = GameObject.FindGameObjectWithTag("Player");
		Direction = m_target.transform.position - transform.position;
		//m_rigidbody.velocity = Direction*Speed;
		if (Direction.magnitude > 1)
		{
			float magnitude = Direction.magnitude;
			Direction.x /= magnitude;
			Direction.y /= magnitude;
		}
        Vector3 force = Direction*Speed;
        if (force.magnitude < maxSpeed)
        {
            m_rigidbody.AddForce(force, ForceMode2D.Force);
        }
            
		Vector3 LookTarget = m_target.transform.position;
		LookTarget.z = transform.position.z;
		Vector3 difPosition = LookTarget - transform.position;

		float angle = Mathf.Atan(difPosition.y / difPosition.x);
		Arm.transform.rotation = Quaternion.Euler(0,0,Mathf.Rad2Deg*angle -(difPosition.x > 0?90:-90));
	}

    public override void Hit(int damages, Vector2 pushForce)
    {
        if (isAlive == false)
            return;
        
        m_lifePoint -= damages;
        if (m_lifePoint <= 0)
        {
            OnDeath();
        }

        m_rigidbody.AddForce(pushForce, ForceMode2D.Impulse);
    }

	public void OnCollisionEnter2D(Collision2D collision)
	{
        if (isAlive == false)
            return;
        
		PlayerMoveControler player = collision.collider.GetComponent<PlayerMoveControler>();
		if (player != null)
		{
			player.Hit(1,Vector2.zero);
		}
	}
	
    public override void Activate()
    {
        isActive = true;
    }

    public void FinishDeath()
    {
        finishDeath = true;
    }

    public override void OnDeath()
    {
        StartCoroutine(DeathCoroutine());
    }
	public void Stop()
	{
		isActive =false;
        m_rigidbody.velocity = Vector2.zero;
	}

    IEnumerator DeathCoroutine()
    {
	    IsDead = true;
        isActive = false;
        isAlive = false;
        coll.enabled = false;

        if (spawner != null)
        {
            spawner.mobIsDead(); 
        }

        Vector2 vector20 = new Vector2(0.0f, 0.0f);
        m_rigidbody.velocity = vector20;
        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        if (destroyFX != null)
        {
            Instantiate(destroyFX, transform.position, new Quaternion(0.0f, 0.0f, 0.0f, 1.0f));
        }

        if (deathSound != null)
        {
            deathSound.pitch = Random.Range(deathSoundRandomPitch.x, deathSoundRandomPitch.y);
            deathSound.Play();
        }

        if (score > 0)
        {
            MoneyManager.instance.SpawnMoney(score, transform.position);
        }

        if (animator != null)
        {
            animator.SetTrigger("Death");
        }
            
        while (finishDeath == false)
        {
            yield return wait;
            m_rigidbody.velocity = vector20;
        }

        if (disableOnDeath == true)
        {
            gameObject.SetActive(false);
        }
    }

	protected GameObject m_target;

	
}
