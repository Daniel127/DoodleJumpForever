//#define STATEMACHINE_VERBOSE
//using System.Reflection;

using System;
using UnityEngine;

namespace Patrones.StateMachine
{
	[Serializable]
	public abstract class State : IState
	{
		public virtual void Execute() { }
		public virtual void PhysicsExecute() { }
		public virtual void PostExecute() { }

		public virtual void OnCollisionEnter(Collision collision)
		{
#if (STATEMACHINE_VERBOSE)
			Debug.Log($"#StateMachine# {Machine.name}.{GetType().Name}::{MethodBase.GetCurrentMethod().Name}()");
#endif
		}
		public virtual void OnCollisionStay(Collision collision) { }
		public virtual void OnCollisionExit(Collision collision)
		{
#if (STATEMACHINE_VERBOSE)
			Debug.Log($"#StateMachine# {Machine.name}.{GetType().Name}::{MethodBase.GetCurrentMethod().Name}()");
#endif
		}

		public virtual void OnTriggerEnter(Collider collider)
		{
#if (STATEMACHINE_VERBOSE)
			Debug.Log($"#StateMachine# {Machine.name}.{GetType().Name}::{MethodBase.GetCurrentMethod().Name}()");
#endif
		}
		public virtual void OnTriggerStay(Collider collider) { }
		public virtual void OnTriggerExit(Collider collider)
		{
#if (STATEMACHINE_VERBOSE)
			Debug.Log($"#StateMachine# {Machine.name}.{GetType().Name}::{MethodBase.GetCurrentMethod().Name}()");
#endif
		}

		public virtual void OnAnimatorIK(int layerIndex) { }

		public virtual void Initialize()
		{
#if (STATEMACHINE_VERBOSE)
			Debug.Log($"#StateMachine# {Machine.name}.{GetType().Name}::{MethodBase.GetCurrentMethod().Name}()");
#endif
		}

		public virtual void Enter()
		{
#if (STATEMACHINE_VERBOSE)
			Debug.Log($"#StateMachine# {Machine.name}.{GetType().Name}::{MethodBase.GetCurrentMethod().Name}()");
#endif
		}

		public virtual void Exit()
		{
#if (STATEMACHINE_VERBOSE)
			Debug.Log($"#StateMachine# {Machine.name}.{GetType().Name}::{MethodBase.GetCurrentMethod().Name}()");
#endif
		}

		public T GetMachine<T>() where T : IMachine
		{
			try
			{
				return (T)Machine;
			}
			catch (InvalidCastException e)
			{
				if (typeof(T) == typeof(MachineState) || typeof(T).IsSubclassOf(typeof(MachineState)))
				{
					throw new Exception($"#StateMachine# {Machine.Name}.GetMachine() no puede retornar el tipo solicitado!\tLa maquina hereda de MachineBehaviour no de MachineState! - {e.Message}");
				}

				if (typeof(T) == typeof(MachineBehaviour) || typeof(T).IsSubclassOf(typeof(MachineBehaviour)))
				{
					throw new Exception($"#StateMachine# {Machine.Name}.GetMachine() no puede retornar el tipo solicitado!\tLa maquina hereda de MachineState no de MachineBehaviour! - {e.Message}");
				}

				throw new Exception($"#StateMachine# {Machine.Name}.GetMachine() no puede retornar el tipo solicitado!\n - {e.Message}");
			}
		}

		public IMachine Machine { get; internal set; }

		public bool IsActive { get { return Machine.IsCurrentState(GetType()); } }
	}
}