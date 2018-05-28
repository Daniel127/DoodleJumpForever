using System.Collections.Generic;
using Patrones;
using UnityEngine;

namespace Managers
{
	public class PoolManager : Singleton<PoolManager>
	{
		/// <summary>
		/// Colección de piscinas de diferentes tipos de objeto
		/// </summary>
		private Dictionary<GameObject, ObjectPool> _pools;

		/// <summary>
		/// Objeto en la scena donde se almacenan los objetos, para evitar ensuciar la jerarquia
		/// </summary>
		private GameObject _poolStore;

		private void Awake()
		{
			_pools = new Dictionary<GameObject, ObjectPool>();
		}

		public void CreatePool(GameObject prefab, int quantity = 0)
		{
			ObjectPool pool = new ObjectPool(prefab, quantity);
			_pools.Add(prefab, pool);
		}

		/// <summary>
		/// Obtiene un objeto de la piscina, usar en vez de Instantiate.
		/// NOTA: Recordar que Awake() o Start() solo se ejecutan la primera vez
		/// que se instancia el objeto y que las variables no se reestablecerán.
		/// OnEnable se ejecuta despues de Spawn, tambien al activar IsActive
		/// </summary>
		public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
		{
			if (_pools.ContainsKey(prefab))
			{
				GameObject instance = _pools[prefab].Spawn(position, rotation);
				instance.transform.parent = null;
				return instance;
			}

			Debug.Log("#PoolManager# No existia una piscina para este prefab, se ha creado");
			CreatePool(prefab);
			return _pools[prefab].Spawn(position, rotation);
		}

		/// <summary>
		/// Retorna una instancia a la piscina, usar en vez de Destroy.
		/// </summary>
		public void Despawn(GameObject instance)
		{
			PoolMember poolMember = instance.GetComponent<PoolMember>();
			if (poolMember == null)
			{
				Debug.Log($"El objeto '{instance.name}' no fue instanciado desde una piscina. Se ha destruido.");
				Destroy(instance);
			}
			else
			{
				poolMember.MyPool.Despawn(instance);
				if(_poolStore == null)
					_poolStore = new GameObject("(Pool) Store");
				instance.transform.parent = _poolStore.transform;
			}
		}
	}
}