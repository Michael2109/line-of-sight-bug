using Game_Objects.Person.Scripts;
using UnityEngine;
using UnityEngine.AI;

namespace Prefabs.Person
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Person : MonoBehaviour
    {
        public Statistic Health { get; } = new Statistic(1);

        private bool _selected;

        private NavMeshAgent _agent;

        protected virtual void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        protected virtual void Update()
        {
            if (!_agent.velocity.Equals(Vector3.zero))
            {
                transform.rotation = Quaternion.LookRotation(_agent.velocity.normalized);
            }

            CheckHealth();
        }

        public bool IsSelected()
        {
            return _selected;
        }

        public void SetSelected(bool selected)
        {
            _selected = selected;
        }

        public void SetDestination(Vector3 destination)
        {
            _agent.SetDestination(destination);
        }

        protected virtual void CheckHealth()
        {
            if (Health.IsMin())
            {
                Destroy(gameObject);
            }
        }
    }
}