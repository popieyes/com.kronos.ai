using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Popieyes.AI.Transitions;
using UnityEditor;

namespace Popieyes.AI
{
    public abstract class StateMachine<T> : MonoBehaviour
    {
        [Header("Configuration")]
        public bool debug = false;
        private Dictionary<Type, State<T>> _stateCache = new Dictionary<Type, State<T>>();
        private State<T> _currentState;
        public State<T> CurrentState => _currentState;
        private List<Transition> _transitions = new List<Transition>(); 
        
       
        #region UNITY CALLBACKS
        void Awake()
        {
            T context = GetComponent<T>();
            StateBase[] objs = GetComponents<StateBase>();
            foreach (var obj in objs)
            {
                if(obj is State<T> state)
                {
                    _stateCache.Add(state.GetType(), state);
                    state.SetContext(context);
                    state.SetupTransitions();
                    state.OnAwake();
                }
            }
            if(_stateCache.Count > 0)
                _currentState = _stateCache.First().Value;
        }
        void Start()
        {
            _currentState?.OnEnter();
        }
        void Update()
        {
            _currentState?.OnStep();
            CheckTransitions();
            CheckAnyStateTransitions();
        }

        void FixedUpdate()
        {
            _currentState?.OnFixedStep();
         
        }

        void OnTriggerEnter(Collider other)
        {
            _currentState?.TriggerEnter(other);
        }
        void OnTriggerExit(Collider other)
        {
            _currentState?.TriggerExit(other);
        }
        void OnTriggerStay(Collider other)
        {
            _currentState?.TriggerStay(other);
        }
        void OnCollisionEnter(Collision collision)
        {
            _currentState?.CollisionEnter(collision);
        }
        void OnCollisionExit(Collision collision)
        {
            _currentState?.CollisionExit(collision);
        }
        void OnCollisionStay(Collision collision)
        {
            _currentState?.CollisionStay(collision);
        }
       
        #endregion

        #region STATE MACHINE
        /// <summary>
        /// Switches to the specified state type.
        /// </summary>
        /// <typeparam name="T">The type of the state to switch to.</typeparam>
        private void SwitchState<U>() where U : State<T>
        {
            SwitchState(typeof(U));
        }
        /// <summary>
        /// Switches to the specified state type.
        /// </summary>
        /// <param name="type">The type of the state to switch to.</param>
        private void SwitchState(Type type)
        {
            if(_currentState != null && _currentState.GetType() == type)
                return;
            if (_stateCache.TryGetValue(type, out State<T> newState))
            {
                _currentState?.OnExit();
                _currentState = newState;
                _currentState.OnEnter();
            }
            else Debug.LogError($"{name}'s State Machine is trying to switch to {type} state but the state is not added to the GameObject");
        }
        /// <summary>
        /// Checks and executes transitions for the current state.
        /// </summary>
        private void CheckTransitions()
        {
            foreach(var transition in _currentState.Transitions)
            {
                if(transition.Condition())
                {
                    SwitchState(transition.TargetState);
                    break;
                }
            }
        }
        /// <summary>
        /// Checks and executes any-state transitions.
        /// </summary>
        private void CheckAnyStateTransitions()
        {
            foreach(var transition in _transitions)
            {
                if(transition.Condition())
                {
                    SwitchState(transition.TargetState);
                    break;
                }
            }
        }
        /// <summary>
        /// Adds a transition that can be triggered from any state.
        /// </summary>
        /// <param name="transition">The transition to add.</param>
        public void AddAnyStateTransition(Transition transition)
        {
            _transitions.Add(transition);
        }
        #endregion
    }
}
