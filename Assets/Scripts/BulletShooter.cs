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
		private ParticleSystem partSys;

		void Awake()
		{
			partSys = GetComponent<ParticleSystem>();
		}

		#region IShooter
		/// <summary>
		/// 파티클로 구성된 총알을 발사한다.
		/// </summary>
		public void Shoot()
		{
			partSys.Emit(new ParticleSystem.EmitParams{
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

		}
	}
}