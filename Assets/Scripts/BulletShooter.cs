using UnityEngine;
using System.Collections.Generic;

namespace Clusternoid
{
	/// <summary>
	/// 파티클로 구성된 총알을 발사하는 IShooter 구현.
	/// </summary>
	/// <remarks>
	/// 총알 발사 후 이 컴퍼넌트가 부착된 객체가 비활성화될 경우 파티클 시스템과 총알(파티클)이 비활성화되기 때문에
	/// 마지막 총알의 lifeTime까지는 비활성화하지 말 것.
	/// </remarks>
	[RequireComponent(typeof(ParticleSystem))]
	public class BulletShooter : MonoBehaviour, IShooter
	{
		/// <summary>
		/// 총알을 발사할 파티클 시스템.
		/// </summary>
		private ParticleSystem ps;
		private List<ParticleCollisionEvent> cEventList;

		void Awake()
		{
			ps = GetComponent<ParticleSystem>();
			cEventList = new List<ParticleCollisionEvent>();

			// 기본 Emission 비활성화
			var em = ps.emission;
			em.enabled = false;

			// 2D 충돌 설정
			var col = ps.collision;
			col.type = ParticleSystemCollisionType.World;
			col.mode = ParticleSystemCollisionMode.Collision2D;
			col.sendCollisionMessages = true;
			col.enabled = true;

			// TODO: 충돌 레이어 설정. 쏘는 쪽이 player면 player layer 무시,
			// 	enemy이면 enemy layer 무시하도록
		}

		#region IShooter
		public ShooterInfo Info { get; }

		/// <summary>
		/// 파티클로 구성된 총알을 발사한다.
		/// </summary>
		public void Shoot()
		{
			ps.Emit(new ParticleSystem.EmitParams{
				startColor = Color.yellow,
				startSize = 1f,
				velocity = 2f * Vector3.up,
				startLifetime = 10f}, 1);
		}
		#endregion

		/// <summary>
		/// 총알이 Collider에 적중했을 때 호출되는 콜백.
		/// </summary>
		/// <param name="other">파티클과 충돌한 GameObject</param>
		void OnParticleCollision(GameObject other)
		{
			var fo = other.GetComponent<ICharacter>();
			ParticlePhysicsExtensions.GetCollisionEvents(ps, other, cEventList);

			foreach (var ce in cEventList)
			{
				if (fo != null)
				{
					// 총알이 접촉한 캐릭터에게 데미지를 전달함.
					fo.DamagedComponent.GetDamage(new DamageInfo{
						damage = 1,
						direction = ce.normal});
				}

				// TODO: 총알 파괴 이펙트 구현.
			}	
		}
	}
}
