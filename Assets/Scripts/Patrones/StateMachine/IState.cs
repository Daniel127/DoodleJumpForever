namespace Patrones.StateMachine
{
	public interface IState
	{
		bool IsActive { get; }
		IMachine Machine { get; }

		void Initialize();
		void Enter();
		void Execute();
		void PhysicsExecute();
		void PostExecute();
		void Exit();

		void OnCollisionEnter(UnityEngine.Collision collision);
		void OnCollisionStay(UnityEngine.Collision collision);
		void OnCollisionExit(UnityEngine.Collision collision);

		void OnTriggerEnter(UnityEngine.Collider collider);
		void OnTriggerStay(UnityEngine.Collider collider);
		void OnTriggerExit(UnityEngine.Collider collider);

		// ReSharper disable once InconsistentNaming
		void OnAnimatorIK(int layerIndex);

		T GetMachine<T>() where T : IMachine;
	}
}