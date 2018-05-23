using System;
using System.Collections.Generic;
using Patrones;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
	public class LevelManager : Singleton<LevelManager>
	{
		public GameObject Player;

		[Header("Level Properties")]
		public float BoundUpper = 5.2f;
		public float LevelWidth = 2.27f;
		public float LevelHeight = 4.9f;

		[Header("Platforms")]
		public GameObject NormalPlatform;
		public GameObject[] SpecialPlatforms;
		[Header("Enemies")]
		public GameObject[] Enemies;

		[Header("Probabilities"), Range(0, 1)]
		public float EnemyProbability;
		[Range(0, 1)]
		public float SpecialPlatformProbability;

		[Header("Objects Distance")]
		public float MinDistance;
		public float MaxDistance = 2;



		private float _riseTime;
		private GameObject _finalObject;
		private List<GameObject> _levelObjects;
		private readonly object _lockObject = new object();

		private void Start()
		{
			Player = GameObject.FindGameObjectWithTag("Player");
			_levelObjects = new List<GameObject>();
			PoolManager.Instance.CreatePool(NormalPlatform, 20);
			foreach (GameObject platform in SpecialPlatforms)
			{
				PoolManager.Instance.CreatePool(platform, 5);
			}
			foreach (GameObject enemy in Enemies)
			{
				PoolManager.Instance.CreatePool(enemy, 1);
			}

			InitLevel();
		}

		public void InitLevel()
		{
			const float fraction = 0.06666667F;
			for (int i = 0; i < 15; i++)
			{
				GameObject platform = InstantiateLevelObject(NormalPlatform);
				Vector3 spawnPosition = platform.transform.position;
				spawnPosition.y = Mathf.Lerp(-LevelHeight, LevelHeight, fraction * i);
				platform.transform.position = spawnPosition;
				if (i == 14)
				{
					_finalObject = platform;
				}
			}
		}

		public void DestroyLevel()
		{
			_levelObjects.Clear();
			PoolMember[] poolMembers = FindObjectsOfType<PoolMember>();
			for (int i = 0; i < poolMembers.Length; i++)
			{
				PoolManager.Instance.Despawn(poolMembers[i].gameObject);
			}

		}

		private void FixedUpdate()
		{
			if (Player.transform.position.y > 0) 
			{
				Rigidbody2D playerRb = Player.GetComponent<Rigidbody2D>();
				if (playerRb.gravityScale > 0)
				{
					_riseTime = Time.time + Math.Abs(playerRb.velocity.y / (Physics2D.gravity.y * playerRb.gravityScale));
					playerRb.velocity = Vector2.zero;
					playerRb.gravityScale = 0;
				}
				Vector3 newPosition = Player.transform.position;
				newPosition.y = 0;
				Player.transform.position = newPosition;
			}
			
			Debug.Log($"#Tiempos# Time: {Time.time} - RiseTime {_riseTime}");

			if (Time.time < _riseTime)
			{
				DownLevel();
			}
			else
			{
				Player.GetComponent<Rigidbody2D>().gravityScale = 1.646f;
			}
		}

		public void DownLevel()
		{
			lock (_lockObject)
			{
				//Debug.Log($"LevelObjects: {_levelObjects.Count}");
				for (int i = 0; i < _levelObjects.Count; i++)
				{
					GameObject levelObject = _levelObjects[i];
					Vector3 newPosition = levelObject.transform.position;
					newPosition.y -= 0.1f;
					levelObject.transform.position = newPosition;
					if (newPosition.y <= -BoundUpper - 1)
					{
						RemoveLevelObject(levelObject);
					}

					if (levelObject.transform.position.y > _finalObject.transform.position.y)
					{
						_finalObject = levelObject;
					}
				}
				Debug.Log(_finalObject.transform.position.y - (BoundUpper));
				if (_finalObject.transform.position.y <= (BoundUpper - MaxDistance))
				{
					CreateLevelObject();
				}
				else if(Random.value <= 0.5 && _finalObject.transform.position.y <= (BoundUpper - MinDistance))
				{
					CreateLevelObject();
				}
				GameManager.Instance.Score += 1;
			}
		}

		private void CreateLevelObject()
		{
			GameObject newLevelObject;
			if (Random.value <= EnemyProbability)
			{
				newLevelObject = Enemies[Random.Range(0, Enemies.Length)];
			}
			else
			{
				newLevelObject = Random.value < SpecialPlatformProbability
					? SpecialPlatforms[Random.Range(0, SpecialPlatforms.Length)]
					: NormalPlatform;
			}

			InstantiateLevelObject(newLevelObject);
		}

		private GameObject InstantiateLevelObject(GameObject levelObject)
		{
			lock (_lockObject)
			{
				Vector3 spawnPosition = new Vector3
				{
					x = Random.Range(-LevelWidth, LevelWidth),
					y = 5
				};
				GameObject finalObject = PoolManager.Instance.Spawn(levelObject, spawnPosition, Quaternion.identity);
			    finalObject.GetComponent<Platform>()?.Init();
				_levelObjects.Add(finalObject);
				//_finalObject = finalObject;
				Debug.Log("#LevelManager# Objeto instanciado");
				return finalObject;
			}
			
		}

		public void RemoveLevelObject(GameObject levelObject)
		{
			lock (_lockObject)
			{
				_levelObjects.Remove(levelObject);
				PoolManager.Instance.Despawn(levelObject);
				Debug.Log("#LevelManager# Objeto Eliminado");
			}
		}
	}
}