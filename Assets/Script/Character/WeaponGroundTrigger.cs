using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGroundTrigger : MonoBehaviour
{


	public BaseWeapon[] Weapons;
    public SoundToPlay soundToPlay;

	public void OnTriggerEnter2D(Collider2D collider)
	{
		PlayerMoveControler player = collider.GetComponent<PlayerMoveControler>();

        int i = Random.Range(0, Weapons.Length);
        BaseWeapon weaponToGrab = Weapons[i];
            
        if (weaponToGrab.id == player.weapon.id)
        {
            weaponToGrab = Weapons[(i + 1) % Weapons.Length];
        }

        player.SetWeapon(weaponToGrab);
        SoundManager.instance.PlaySound(soundToPlay);
		GameObject.Destroy(gameObject,0.1f);
	}
}
