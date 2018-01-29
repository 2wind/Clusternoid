using UnityEngine;

namespace Clusternoid
{
	public struct ShooterInfo
	{
		public int damage;
	}

	public interface IShooter
	{
		void Shoot(Vector3 shootingPos, Vector3 direction);

		ShooterInfo Info { get; }
	}
}
