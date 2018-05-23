using Managers;
using Patrones.StateMachine;
using UnityEngine;

namespace GameStates
{
	public class EndGameState : State
	{
		private GameMachine _machine;

		public override void Initialize()
		{
			_machine = (GameMachine)Machine;
			_machine.TryAgainButton.onClick.AddListener(TryAgain);
			_machine.MainMenuButton.onClick.AddListener(GoMenu);
		}

		public override void Enter()
		{
			_machine.Player.SetActive(false);
			_machine.Player.transform.position = Vector3.zero;
			_machine.EndGameMenu.SetActive(true);

			int highScore = PlayerPrefs.GetInt("HighScore", 0);			
			if (GameManager.Instance.Score > highScore)
			{
				highScore = GameManager.Instance.Score;
				PlayerPrefs.SetInt("HighScore", highScore);
			}
			_machine.ScoreText.text = $"{GameManager.Instance.Score}";
			_machine.HighScoreText.text = $"{highScore}";
			LevelManager.Instance.DestroyLevel();
			LevelManager.Instance.enabled = false;
			GameManager.Instance.CurrentState = GameManager.GameState.EndMenu;
		}

		//public override void Execute()
		//{
		//	base.Execute();
		//}

		public override void Exit()
		{
			GameManager.Instance.EndGame = false;
			_machine.EndGameMenu.SetActive(false);
		}

		private void TryAgain()
		{
			GameManager.Instance.Score = 0;
			LevelManager.Instance.InitLevel();
			_machine.ChangeState<GameState>();
		}

		private void GoMenu()
		{
			_machine.ChangeState<MainState>();
		}
	}
}
