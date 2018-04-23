namespace Patrones.StateMachine
{
	public interface IMachine
	{
		string Name { get; set; }

		void SetInitialState<T>() where T : State;
		void SetInitialState(System.Type T);
		void ChangeState<T>() where T : State;
		void ChangeState(System.Type T);
		bool IsCurrentState<T>() where T : State;
		bool IsCurrentState(System.Type T);
		T GetState<T>() where T : State;
		void AddState<T>() where T : State, new();
		void AddState(System.Type T);
		void RemoveState<T>() where T : State;
		void RemoveState(System.Type T);
		bool ContainsState<T>() where T : State;
		bool ContainsState(System.Type T);
		void RemoveAllStates();

		void OnCollisionEnter(UnityEngine.Collision collision);
		void OnCollisionStay(UnityEngine.Collision collision);
		void OnCollisionExit(UnityEngine.Collision collision);

		void OnTriggerEnter(UnityEngine.Collider collider);
		void OnTriggerStay(UnityEngine.Collider collider);
		void OnTriggerExit(UnityEngine.Collider collider);
	}
}