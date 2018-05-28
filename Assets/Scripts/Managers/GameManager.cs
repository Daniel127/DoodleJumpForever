using GameStates;
using Patrones;
using UnityEngine;

namespace Managers
{
	[RequireComponent(typeof(GameMachine))]
	public class GameManager : Singleton<GameManager>
	{
		[SerializeField]
		private int _score;

		public GameState CurrentState;
		public int Score
		{
			get { return _score; }
			set { _score = value; }
		}

		public bool EndGame;

		private void Start()
		{
			Score = 0;
			CurrentState = GameState.MainMenu;
		}

		public enum GameState
		{
			MainMenu,
			Game,
			Pause,
			EndMenu
		}

	}
}
