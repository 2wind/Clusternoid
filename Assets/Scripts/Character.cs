using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clusternoid
{
	public class Character : MonoBehaviour, ICharacter
	{
		public IDamaged DamagedComponent { get; private set; }

		void Awake()
		{
			DamagedComponent = new CharacterDamaged();
		}
	}
}
