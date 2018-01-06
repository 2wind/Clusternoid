namespace Clusternoid
{
	public class CharacterDamaged : IDamaged
	{
		public void GetDamage(DamageInfo info)
		{
			UnityEngine.Debug.Log(info);
		}
	}
}