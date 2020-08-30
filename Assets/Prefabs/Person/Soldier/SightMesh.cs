using UnityEngine;

namespace Prefabs.Person.Soldier
{
    public class SightMesh : MonoBehaviour
    {
        public Quaternion iniRot;
 
        private void Start(){
            iniRot = transform.rotation;
        }
 
        private void LateUpdate(){
            transform.rotation = iniRot;
        }

    }
}
