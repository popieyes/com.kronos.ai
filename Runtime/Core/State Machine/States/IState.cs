using UnityEngine;

namespace Popieyes.AI
{
    public interface IState
    {
        public void OnAwake() {}
        public void OnEnter();
        public void OnExit();
        public void OnStep();
        public void OnFixedStep();
        public void TriggerEnter(Collider other) { }
        public void TriggerExit(Collider other) { }
        public void TriggerStay(Collider other) { }
        public void CollisionEnter(Collision collision) { }
        public void CollisionExit(Collision collision) { }
        public void CollisionStay(Collision collision) { }
    }
}
