using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clusternoid
{
	public class SimpleObjectPool<T> where T : new()
	{
		private Stack<T> pool = new Stack<T>();

		public T Get()
		{
			if (pool.Count > 0)
				return pool.Pop();
			else
				return new T();
		}

		public void Return(T obj)
		{
			pool.Push(obj);
		}

		public void SimpleObject(int initialSize = 10)
		{
			for (int i = 0; i < initialSize; i++)
			{
				var obj = new T();
				pool.Push(obj);
			}
		}
	}
}
