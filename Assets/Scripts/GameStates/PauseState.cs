using Managers;
using Patrones.StateMachine;
using UnityEngine;

namespace GameStates
{
	public class PauseState : State
	{
		private GameMachine _machine;

		public override void Initialize()
		{
			_machine = (GameMachine)Machine;
			_machine.ResumeButton.onClick.AddListener(Resume);
		}

		public override void Enter()
		{
			_machine.PauseMenu.SetActive(true);
			GameManager.Instance.CurrentState = GameManager.GameState.Pause;
		}

		public override void Execute()
		{
			base.Execute();
		}

		public override void Exit()
		{
			_machine.PauseMenu.SetActive(false);
		}

		private void Resume()
		{
			_machine.ChangeState<GameState>();
		}
	}
}
