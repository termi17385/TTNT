using System.Collections;
using System.Collections.Generic;

using TTnT.Player.Weapons;

using UnityEngine;

public class WeaponManger : MonoBehaviour
{
	[SerializeField] private WeaponType weaponType = WeaponType.Primary;
	[SerializeField] private List<BaseWeapon> weapons;

	
	
	public BaseWeapon EquipedWeapon()
	{
		return weapons[(int)weaponType];
	}

	public void SwapWeapon(int _weapon) => weaponType = (WeaponType)_weapon;
}
