namespace Clusternoid
{
	public struct ShooterInfo
	{
		public int damage;
	}

	public interface IShooter
	{
		void Shoot();

		ShooterInfo Info { get; }
	}
}
