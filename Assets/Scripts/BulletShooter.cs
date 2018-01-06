using System;
using System.Collections.Generic;
using UnityEngine;

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
		private float speed = 5f;
		private int damage = 1;
		private float lifeTime =  10f;
		private int tagHash;

		void Awake()
		{
			// 좌표를 원점으로 설정
			transform.position = Vector3.zero;

			tagHash = gameObject.tag.GetHashCode();

			ps = GetComponent<ParticleSystem>();
			cEventList = new List<ParticleCollisionEvent>();

			// 기본 Emission 비활성화
			var em = ps.emission;
			em.enabled = false;

			int bulletMask = 1 << gameObject.layer;
			bulletMask = ~bulletMask;

			// 2D 충돌 설정
			var col = ps.collision;
			col.type = ParticleSystemCollisionType.World;
			col.mode = ParticleSystemCollisionMode.Collision2D;
			col.sendCollisionMessages = true;
			col.collidesWith = bulletMask;
			col.enabled = true;

			// TODO: 충돌 레이어 설정. 쏘는 쪽이 player면 player layer 무시,
			// 	enemy이면 enemy layer 무시하도록

			// shape 비활성화
			var shape = ps.shape;
			shape.enabled = false;
		}

		#region IShooter
		public ShooterInfo Info { get; }

		/// <summary>
		/// 파티클로 구성된 총알을 발사한다.
		/// </summary>
		public void Shoot(Vector3 shootingPos, Vector3 direction)
		{
			ps.Emit(new ParticleSystem.EmitParams{
				position = shootingPos,
				startSize = 1f,
				velocity = speed * direction,
				startLifetime = lifeTime}, 1);
		}
		#endregion

		/// <summary>
		/// 총알이 Collider에 적중했을 때 호출되는 콜백.
		/// </summary>
		/// <param name="other">파티클과 충돌한 GameObject</param>
		void OnParticleCollision(GameObject other)
		{
			var hl = other.GetComponent<HitListener>();
			ParticlePhysicsExtensions.GetCollisionEvents(ps, other, cEventList);

			var id = Guid.NewGuid();

			foreach (var ce in cEventList)
			{
				hl?.TriggerListener(new Attack(tagHash,
					damage,
					ce.normal,
					0f,
					0f));

				// TODO: 총알 파괴 이펙트 구현.
			}	
		}
	}
}
