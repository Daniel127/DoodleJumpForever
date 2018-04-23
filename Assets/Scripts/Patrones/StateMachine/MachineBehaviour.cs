using System;
using System.Collections.Generic;
using UnityEngine;

namespace Patrones.StateMachine
{
	public abstract class MachineBehaviour : MonoBehaviour, IMachine
	{
		/// <summary> 
		/// Añade estados a la maquina con llamadas a AddState(),
		/// Cuando todos los estados hayan sido añadidos, notifique a la 
		/// maquina con el estado a iniciar con SetInitialState()
		/// </summary>
		public abstract void AddStates();

		public virtual void Start()
		{
			Initialize();
		}

		public virtual void Initialize()
		{
			AddStates();

			CurrentState = InitialState;
			if (null == CurrentState)
			{
				throw new Exception($"\n {Name}.CurrentState es null en Initialize()!\tOlvidaste llamar a SetInitialState()?\n");
			}

			foreach (KeyValuePair<Type, State> pair in States)
			{
				pair.Value.Initialize();
			}

			OnEnter = true;
			OnExit = false;
		}

		public virtual void Update()
		{
			if (OnExit)
			{
				CurrentState.Exit();
				CurrentState = NextState;
				NextState = null;

				OnEnter = true;
				OnExit = false;
			}

			if (OnEnter)
			{
				CurrentState.Enter();

				OnEnter = false;
			}

			try
			{
				CurrentState.Execute();
			}
			catch (NullReferenceException e)
			{
				NullException(e);
			}
		}

		public virtual void FixedUpdate()
		{
			if (!(OnEnter && OnExit))
			{
				try
				{
					CurrentState.PhysicsExecute();
				}
				catch (NullReferenceException e)
				{
					NullException(e);
				}
			}
		}

		public virtual void LateUpdate()
		{
			if (!(OnEnter && OnExit))
			{
				try
				{
					CurrentState.PostExecute();
				}
				catch (NullReferenceException e)
				{
					NullException(e);
				}
			}
		}

		public virtual void OnCollisionEnter(Collision collision) { CurrentState.OnCollisionEnter(collision); }
		public virtual void OnCollisionStay(Collision collision) { CurrentState.OnCollisionStay(collision); }
		public virtual void OnCollisionExit(Collision collision) { CurrentState.OnCollisionExit(collision); }

		public virtual void OnTriggerEnter(Collider otherCollider) { CurrentState.OnTriggerEnter(otherCollider); }
		public virtual void OnTriggerStay(Collider otherCollider) { CurrentState.OnTriggerStay(otherCollider); }
		public virtual void OnTriggerExit(Collider otherCollider) { CurrentState.OnTriggerExit(otherCollider); }

		public void OnAnimatorIK(int layerIndex)
		{
			if (!(OnEnter && OnExit))
			{
				try
				{
					CurrentState.OnAnimatorIK(layerIndex);
				}
				catch (NullReferenceException e)
				{
					NullException(e);
				}
			}
		}

		public string Name
		{
			get { return name;}
			set { name = value; }
		}
		public void SetInitialState<T>() where T : State { InitialState = States[typeof(T)]; }
		public void SetInitialState(Type T) { InitialState = States[T]; }

		public void ChangeState<T>() where T : State { ChangeState(typeof(T)); }
		public void ChangeState(Type T)
		{
			if (null != NextState)
			{
				throw new Exception($"{Name} ya esta cambiando estados, deberias esperar antes de llamar ChangeState()!\n");
			}

			try
			{
				NextState = States[T];
			}
			catch (KeyNotFoundException e)
			{
				throw new Exception($"\n{Name} ChangeState() no puede encontrar el estado en la maquina!\tAñadiste el estado al que intentas cambiar?\n{e.Message}");
			}

			OnExit = true;
		}

		public bool IsCurrentState<T>() where T : State { return (CurrentState.GetType() == typeof(T)); }
		public bool IsCurrentState(Type T) { return (CurrentState.GetType() == T); }

		public void AddState<T>() where T : State, new()
		{
			if (!ContainsState<T>())
			{
				State item = new T();
				item.Machine = this;

				States.Add(typeof(T), item);
			}
		}
		public void AddState(Type T)
		{
			if (!ContainsState(T))
			{
				State item = (State)Activator.CreateInstance(T);
				item.Machine = this;

				States.Add(T, item);
			}
		}

		public void RemoveState<T>() where T : State { States.Remove(typeof(T)); }
		public void RemoveState(Type T) { States.Remove(T); }

		public bool ContainsState<T>() where T : State { return States.ContainsKey(typeof(T)); }
		public bool ContainsState(Type T) { return States.ContainsKey(T); }

		public void RemoveAllStates() { States.Clear(); }

		public T GetState<T>() where T : State { return (T)States[typeof(T)]; }


		protected State CurrentState { get; set; }
		protected State NextState { get; set; }
		protected State InitialState { get; set; }

		protected bool OnEnter { get; set; }
		protected bool OnExit { get; set; }

		protected Dictionary<Type, State> States = new Dictionary<Type, State>();

		private void NullException(NullReferenceException nullReferenceException)
		{
			if (InitialState == null)
			{
				throw new Exception($"\n {Name}.InitialState es null en la llamada a Execute()!\tAsignaste el estado inicial?\n - {nullReferenceException.Message}");
			}
			throw new Exception($"\n {Name}.CurrentState es null en la llamada a Execute()!\tHas cambiado el estado a un estado valido?\n - {nullReferenceException.Message}");
		}
	}
}