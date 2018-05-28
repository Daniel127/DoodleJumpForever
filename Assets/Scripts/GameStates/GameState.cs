using Managers;
using Patrones.StateMachine;
using UnityEngine;

namespace GameStates
{
	public class GameState : State
	{
		private GameMachine _machine;

		public override void Initialize()
		{
			_machine = (GameMachine)Machine;
			_machine.PauseButton.onClick.AddListener(Pause);
		}

		public override void Enter()
		{
			LevelManager.Instance.enabled = true;
			LevelManager.Instance.InitLevel();
			_machine.GameMenu.SetActive(true);
			_machine.Player.SetActive(true);
			_machine.Player.GetComponent<Player>().Init();
			GameManager.Instance.CurrentState = GameManager.GameState.Game;
		}

		public override void Execute()
		{
			if (GameManager.Instance.EndGame)
				_machine.ChangeState<EndGameState>();
			_machine.Score.text = $"{GameManager.Instance.Score}";
		}

		public override void Exit()
		{
			_machine.GameMenu.SetActive(false);
		}

		private void Pause()
		{
			_machine.ChangeState<PauseState>();
		}
	}
}
