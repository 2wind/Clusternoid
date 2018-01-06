using UnityEngine;

namespace Clusternoid
{
	/// <summary>
	/// 파티클이나 collider에 충돌시 데미지를 받을 오브젝트에 부착하는 컴퍼넌트.
	/// </summary>
	public class DamageReciever : MonoBehaviour
	{
		/// <summary>
		/// 파티클 충돌시 호출되는 콜백.
		/// </summary>
		/// <param name="sender">총알을 발사한 파티클 시스템이 부착된 오브젝트</param>
		void OnParticleCollision(GameObject sender)
		{
			var shooter = sender.GetComponent<IShooter>();
			if (shooter != null)
			{
				var damage = shooter.Info.damage;
				Debug.Log("Damage: " + damage);

				// TODO: 데미지 전달하기
			}
		}
	}	
}
