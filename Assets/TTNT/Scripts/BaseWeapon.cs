using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTnT.Player.Weapons
{
	public enum ShootMode
	{
		SemiAuto,
		FullAuto
		//BurstAuto,
	}

	public enum WeaponType
	{
		Primary = 0,
		Secondary = 1,
		Grenade = 2,
		Equipment = 3,
		SecondaryEquipment = 4
	}
	
	public class BaseWeapon : MonoBehaviour
	{
		[Header("Name")]
		public string weaponName = "m1911";
		public WeaponType weaponType = WeaponType.Secondary;
		
		[Header("Damage and FireRate")]
		public int damage = 10;
		public float fireRate = .25f;

		[Header("Range and Spread")]
		public float weaponRange = 50f;
		public float spread = .02f;

		[Header("Ammo and Reloading")] 
		public int bullets;
		public int clips;
		
		[SerializeField] private int clipSize = 8;
		[SerializeField] private int maxClips = 5;

		private ShootMode shootMode;
        private bool fireMode;
		private int count = 0;
		
		public virtual bool SwapFireMode()
		{
			if(Input.GetKeyDown(KeyCode.X))
			{
				count++;
				if(count > 1) count = 0;
				shootMode = (ShootMode)count;
			}
			
			var semi = Input.GetKeyDown(KeyCode.Mouse0);
			var auto = Input.GetKey(KeyCode.Mouse0);

			fireMode = shootMode switch
			{
				ShootMode.SemiAuto => semi,
				ShootMode.FullAuto => auto,
				_                  => fireMode
			};

			return fireMode;
		}
		public virtual void ReloadWeapon() => bullets = clipSize;
		public virtual void SetAmmo(int _clips)
		{
			clips += _clips;
			if(clips >= maxClips) clips = maxClips;
		}
	}
}