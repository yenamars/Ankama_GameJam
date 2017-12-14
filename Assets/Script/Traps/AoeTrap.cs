using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeTrap : BaseTrap
{
	[Header("AOE")] public float range;
    public LayerMask damagesLayer;
    private Vector2 position;

    public override void Awake()
    {
        base.Awake();
        position = new Vector2(transform.position.x, transform.position.y);
    }

	protected override void ApplyEffect(TrapTarget trapped)
	{
        Instantiate(trapFX, transform);
        StackableShake.instance.Shake(trapShake);

        Collider2D[] colls = Physics2D.OverlapCircleAll(position, range, damagesLayer, -1.0f, 1.0f);

        for (int i = 0; i < colls.Length; i++)
        {
            IDamageable d = colls[i].GetComponent<IDamageable>();
            if (d != null)
            {
                Vector3 p = colls[i].transform.position;
                d.Hit(power, new Vector2(0.0f, 0.0f));
            }
        }

//		foreach (Actor actor in m_mobRoot.GetComponentsInChildren<Actor>())
//		{
//			Vector3 distanceToTrap = actor.transform.position - transform.position;
//			distanceToTrap.z = 0;
//            if(distanceToTrap.sqrMagnitude < range*range)
//				actor.Hit(power,Vector2.zero);
//		}
	}
        
    public override void Hit(int damages, Vector2 pushForce)
    {
        ApplyEffect(null);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
