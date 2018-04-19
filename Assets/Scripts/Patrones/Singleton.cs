using UnityEngine;

// ReSharper disable StaticMemberInGenericType
namespace Patrones
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;
		private static bool _applicationIsQuitting;
		private static readonly object Lock = new object();

		public static T Instance
		{
			get
			{
				if (_applicationIsQuitting)
				{
					Debug.LogWarning($"#Singleton# Instancia '{typeof(T)}' ya destruida en el cierre de la app. No se volvera a crear - retorna null.");
					return null;
				}

				lock (Lock)
				{
					if (_instance != null)
						return _instance;

					T[] instances = FindObjectsOfType<T>();
					_instance = instances.Length > 0 ? instances[0] : null;

					if (_instance == null)
					{
						GameObject singleton = new GameObject();
						_instance = singleton.AddComponent<T>();
						singleton.name = $"(Singleton) {typeof(T)}";
						DontDestroyOnLoad(singleton);
						Debug.LogError($"#Singleton# Una instancia de {typeof(T)} es necesaria en la escena, '{singleton.name}' ha sido creado con DontDestroyOnLoad.");
					}
					else
					{
						if (instances.Length > 1)
						{
							Debug.LogError("#Singleton# Algo ha ido muy mal, nunca deberia haber mas de 1 singleton! Reabrir la escena puede solucionarlo.");
						}
						Debug.Log($"#Singleton# Usando instancia ya creada: {_instance.gameObject.name}");
					}
					return _instance;
				}
			}
		}
		public void OnDestroy()
		{
			_applicationIsQuitting = true;
		}
	}
}