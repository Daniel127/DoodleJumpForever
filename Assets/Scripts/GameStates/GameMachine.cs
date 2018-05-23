using Patrones.StateMachine;
using UnityEngine;
using UnityEngine.UI;

namespace GameStates
{
	public class GameMachine : MachineBehaviour
	{
		public GameObject Player;

		[Header("Main Menu Properties")]
		public GameObject MainMenu;
		public Button PlayButton;

		[Header("Game UI Properties")]
		public GameObject GameMenu;
		public Button PauseButton;
		public Text Score;

		[Header("Pause Menu Properties")]
		public GameObject PauseMenu;
		public Button ResumeButton;

		[Header("EndGame Menu Properties")]
		public GameObject EndGameMenu;
		public Button TryAgainButton;
		public Button MainMenuButton;

		public override void AddStates()
		{
			AddState<MainState>();
			AddState<GameState>();
			AddState<PauseState>();
			AddState<EndGameState>();
			SetInitialState<MainState>();
		}

		public override void Start()
		{
			if (Player == null)
				Player = GameObject.FindGameObjectWithTag("Player");
			base.Start();
		}
	}
}
