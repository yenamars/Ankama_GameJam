using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoveControler : Actor
{

	public Transform GunRoot;
	public BaseWeapon Defaultweapon;
	[HideInInspector]
    public BaseWeapon weapon;
    public SoundToPlay fallSound;
    public ParticleSystem[] lifeGainFX;
    public ParticleSystem smokeFallFX;
    public ParticleSystem smokeRunFX;

	[HideInInspector] public Vector3 Orientation;
    [HideInInspector] public bool isAlive;

	
	public LayerMask Mask;

	[Header("ui")] public Image LifeGauge;

	public StackableShakeData ShakeData;
	public override void Awake()
	{
		SetWeapon(Defaultweapon);
        //isAlive = true;
		m_colliders = GetComponentsInChildren<Collider2D>().ToList();
		base.Awake();
	}

	public void SetAlive(bool alive)
	{
		if(m_colliders == null)
			m_colliders = GetComponentsInChildren<Collider2D>().ToList();
		foreach (Collider2D mCollider in m_colliders)
		{
			mCollider.enabled = alive;
			
		}
		if (alive == false )
		{
			if(m_rigidbody != null)
				m_rigidbody.velocity = Vector2.zero;
			if(weapon !=null)
				weapon.StopShoot();
		}
		isAlive = alive;
	}

	public override void Update()
	{
        if (isAlive == false)
            return;
        
        if(m_mainCamera == null)
	        m_mainCamera = Camera.main;
		base.Update();
		Direction = new Vector3( Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0);

		SetVelocity();

        if (Direction.x != 0.0f || Direction.y != 0.0f)
        {
            animator.SetBool("Running", true); 
            smokeRunFX.Emit(1);
        }
        else
        {
            animator.SetBool("Running", false); 
        }

        Vector3 LookTarget = new Vector3(0.0f, 0.0f, 0.0f);

        if(m_mainCamera != null)
		    LookTarget = m_mainCamera.ScreenToWorldPoint(Input.mousePosition);
        
		LookTarget.z = transform.position.z;
		Vector3 difPosition = LookTarget - transform.position;

		float angle = Mathf.Atan(difPosition.y / difPosition.x);

        if (difPosition.x > 0.0f)
        {
            Arm.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Rad2Deg * angle - 90.0f);  
            Arm.transform.localScale = new Vector3(-1.0f * transform.localScale.x, 1.0f, 1.0f);
        }
        else
        {
            Arm.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Rad2Deg * angle + 90.0f); 
            Arm.transform.localScale = new Vector3(1.0f * transform.localScale.x, 1.0f, 1.0f);
        }
		//Arm.transform.rotation = Quaternion.Euler(0,0,Mathf.Rad2Deg*angle -(difPosition.x > 0?90:-90));

        if (Input.GetMouseButton(0)/* || Input.GetAxis("Fire1") > 0.5f*/)
        {
            weapon.Shoot();
        }
        else
        {
            weapon.StopShoot();
        }
	}

	public void HitGround()
	{
		StackableShake.instance.Shake(ShakeData);
        SoundManager.instance.PlaySound(fallSound);
        smokeFallFX.Play(false);
	}

    public bool IsAlive()
    {
        return isAlive;
    }

	public void SetWeapon(BaseWeapon weaponToEquip)
	{
		if(GunRoot.transform.childCount > 0)
			GameObject.Destroy(GunRoot.transform.GetChild(0).gameObject);
		weapon = GameObject.Instantiate<BaseWeapon>(weaponToEquip, GunRoot);
		weapon.transform.localPosition = Vector3.zero;
		weapon.transform.localScale = Vector3.one;
		weapon.transform.localRotation = Quaternion.identity;
	}

	public override void Hit(int damages, Vector2 pushForce)
	{
        if (isAlive == false)
            return;
        
        if (damages < 0)
        {
            for (int i = 0; i < lifeGainFX.Length; i++)
            {
                lifeGainFX[i].Play(false);
            }
        }

        base.Hit(damages, pushForce);

		if(LifeGauge != null)
        	LifeGauge.fillAmount = (float)(m_lifePoint) / LifePoint;

        animator.SetTrigger("HitTrigger");

		Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, 2, Mask, -1.0f, 1.0f);
		for (int i = 0; i < colls.Length; i++)
		{
			IDamageable d = colls[i].GetComponent<IDamageable>();
			if (d != null)
			{
				Vector3 p = colls[i].transform.position;
				d.Hit(0, -(transform.position - colls[i].transform.position )* 10);
			}
		}

        if(damages > 0)
            PostProcess.instance.Hit();
	}

//	private IEnumerator InvulnerableCoroutine()
//	{
//		yield return new WaitForSeconds(10.0f);
//		animator.SetBool("Hit",false);
//	}

	public override void OnDeath()
    {
        if (destroyFX != null)
        {
            Instantiate(destroyFX, transform.position, new Quaternion(0.0f, 0.0f, 0.0f, 1.0f));
        }

        SetAlive(false);

        weapon.StopShoot();
        Direction = Vector2.zero;
        SetVelocity();
        Arm.SetActive(false);

        animator.SetTrigger("Death");
        StartCoroutine(ReloadCoroutine());
    }

    IEnumerator ReloadCoroutine()
    {
	    if(SceneControler.Instance != null)
		    SceneControler.Instance.OnGameOver();
        yield return new WaitForSeconds(3.0f);
        if(SceneControler.Instance != null)
            SceneControler.Instance.Reload();
    }

	private List<Collider2D> m_colliders;
}

