using UnityEngine;

namespace Clusternoid
{
	public struct DamageInfo
	{
		public int damage;
		public Vector3 direction;

		public override string ToString()
		{
			return $"Damage: {damage}, Direction: {direction}";
		}
	}

	public interface IDamaged
	{
		void GetDamage(DamageInfo damageInfo);
	}
}
