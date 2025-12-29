using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Popieyes.AI
{
    public class StateMachine : MonoBehaviour
    {
        private Dictionary<System.Type, IState> _stateCache = new Dictionary<System.Type, IState>();
        private IState _currentState;
        public IState CurrentState => _currentState;
       
        #region UNITY CALLBACKS
        void Awake()
        {
            var states = GetComponents<IState>();
            foreach (var state in states)
            {
                _stateCache.Add(state.GetType(), state);
                state.OnAwake();
            }
            _currentState = _stateCache.First().Value;
        }
        void Start()
        {
            _currentState?.OnEnter();
        }
        void Update()
        {
            _currentState?.OnStep();
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
        public void SwitchState<T>() where T : IState
        {
            if (_stateCache.TryGetValue(typeof(T), out IState newState))
            {
                _currentState?.OnExit();
                _currentState = newState;
                _currentState.OnEnter();
            }
        }
        #endregion
    }
}
