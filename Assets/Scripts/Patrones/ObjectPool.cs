using System.Collections.Generic;
using UnityEngine;

namespace Patrones
{
	/// <summary>
	/// La clase Pool representa la piscina para un prefab particular.
	/// </summary>
	public class ObjectPool
	{
		/// <summary>
		/// Id utilizado para el nombre de las instancias, solo es cosmetico
		/// </summary>
		private int _nextId = 1;
		/// <summary>
		/// Objetos inactivos que se pueden reutilizar
		/// </summary>
		private readonly Queue<GameObject> _inactive;
		/// <summary>
		/// Prefab que se utilizara para las instancias de la piscina
		/// </summary>
		private readonly GameObject _prefab;
		/// <summary>
		/// Objeto usado para el Lock
		/// </summary>
		private readonly object _lockObject;

		/// <summary>
		/// Construye la piscina con una cantidad de objetos inicial.
		/// </summary>
		/// <param name="prefab">Prefab al que esta dirigida la piscina</param>
		/// <param name="initialQuantity">Cantidad inicial de instancias</param>
		public ObjectPool(GameObject prefab, int initialQuantity = 2)
		{
			_lockObject = new object();
			_prefab = prefab;
			_inactive = new Queue<GameObject>(initialQuantity);
			for (int i = 0; i < initialQuantity; i++)
			{
				GameObject instance = Spawn(new Vector3(0, -100, 0), Quaternion.identity);
				Despawn(instance);
			}
		}

		/// <summary>
		/// Obtiene un objeto de la piscina, usar en vez de Instantiate.
		/// </summary>
		/// <param name="position">Nueva posición del objeto</param>
		/// <param name="rotation">Nueva rotación del objeto</param>
		/// <returns>Instancia del prefab requerido</returns>
		public GameObject Spawn(Vector3 position, Quaternion rotation)
		{
			GameObject instance;
			lock (_lockObject)
			{
				if (_inactive.Count == 0)
				{
					instance = Object.Instantiate(_prefab, position, rotation);
					instance.name = $"{_prefab.name} ({_nextId++})";
					instance.AddComponent<PoolMember>().MyPool = this;
				}
				else
				{
					instance = _inactive.Dequeue();
					if (instance == null)
					{
						Debug.LogError("#ObjectPool# El objeto a dejado de existir, puede ser causa de una llamada a Destroy");
						return Spawn(position, rotation);
					}
				}
			}
			instance.transform.position = position;
			instance.transform.rotation = rotation;
			instance.SetActive(true);
			return instance;

		}

		/// <summary>
		/// Retorna una instancia a la piscina, usar en vez de Destroy.
		/// </summary>
		/// <param name="instance">Objeto a retornar</param>
		public void Despawn(GameObject instance)
		{
			instance.SetActive(false);
			_inactive.Enqueue(instance);
		}
	}

	/// <inheritdoc />
	/// <summary>
	/// Se añade a los objetos instanciados, nos permite conocer la piscina a la que pertenece.
	/// </summary>
	public class PoolMember : MonoBehaviour
	{
		public ObjectPool MyPool { get; set; }
	}
}