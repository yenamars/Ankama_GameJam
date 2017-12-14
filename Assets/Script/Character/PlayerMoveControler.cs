﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveControler : Actor
{

	public Transform GunRoot;
	public BaseWeapon Defaultweapon;
	[HideInInspector]
    public BaseWeapon weapon;

	[HideInInspector] public Vector3 Orientation;

	public override void Awake()
	{
		SetWeapon(Defaultweapon);
		base.Awake();
	}

	public override void Update()
	{
		base.Update();
		Direction = new Vector3( Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0);
		SetVelocity();

		Vector3 LookTarget = m_mainCamera.ScreenToWorldPoint(Input.mousePosition);
		LookTarget.z = transform.position.z;
		Vector3 difPosition = LookTarget - transform.position;

		float angle = Mathf.Atan(difPosition.y / difPosition.x);
		Arm.transform.rotation = Quaternion.Euler(0,0,Mathf.Rad2Deg*angle -(difPosition.x > 0?90:-90));

        if (Input.GetMouseButton(0)/* || Input.GetAxis("Fire1") > 0.5f*/)
        {
            weapon.Shoot();
        }
        else
        {
            weapon.StopShoot();
        }
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
}
