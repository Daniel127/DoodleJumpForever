using Managers;
using Patrones.StateMachine;

namespace GameStates
{
	public class MainState : State
	{
		private GameMachine _machine;
		public override void Initialize()
		{
			_machine = (GameMachine) Machine;
			_machine.PlayButton.onClick.AddListener(Play);
		}

		public override void Enter()
		{
			_machine.MainMenu.SetActive(true);
			GameManager.Instance.CurrentState = GameManager.GameState.MainMenu;
		}

		public override void Exit()
		{
			_machine.MainMenu.SetActive(false);
		}

		private void Play()
		{
			_machine.ChangeState<GameState>();
		}
	}
}
