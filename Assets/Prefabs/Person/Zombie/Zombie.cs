using System;
using UnityEngine;
using Utilities;

namespace Prefabs.Person.Zombie
{
    public class Zombie : Person
    {
        public float damagePerSecond = 0.05f;

        private float _damageTimer = 0;

        private Person _personToAttack;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            MoveToClosestPerson();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            if (_damageTimer > 0)
            {
                _damageTimer -= Time.deltaTime;
            }
            if(_damageTimer <= 0 && _personToAttack != null)
            {
                _damageTimer = 1;
                Debug.Log("Damaging soldier");
                _personToAttack.Health.Decrease(damagePerSecond); 
            }
        }

        private void MoveToClosestPerson()
        {
            GameObject[] personObjects = GameObject.FindGameObjectsWithTag(TagManager.Soldier);

            GameObject closest = null;
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;
            foreach (GameObject go in personObjects)
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = go;
                    distance = curDistance;
                }
            }

            if (closest != null)
            {
                SetDestination(closest.transform.position);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.gameObject.name);
            if (other.gameObject.transform.parent.CompareTag(TagManager.Soldier))
            {
                _personToAttack = other.gameObject.GetComponentInParent<Soldier.Soldier>();
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(TagManager.Soldier))
            {
                _personToAttack = null;
            }
        }
    }
}