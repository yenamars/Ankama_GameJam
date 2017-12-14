using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGroundTrigger : MonoBehaviour
{


	public BaseWeapon WeaponToEquip;
	public void OnTriggerEnter2D(Collider2D collider)
	{

		PlayerMoveControler player = collider.GetComponent<PlayerMoveControler>();
		player.SetWeapon(WeaponToEquip);
		
		GameObject.Destroy(gameObject,0.1f);
	}
}
