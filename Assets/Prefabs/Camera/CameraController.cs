using UnityEngine;

namespace Prefabs.Camera
{
    public class CameraController : MonoBehaviour
    {
        public float speed = 0.1f;

        void Update()
        {
            Vector3 direction = new Vector3();
            if (Input.GetKey(KeyCode.W))
            {
                direction += Vector3.forward;
            }

            if (Input.GetKey(KeyCode.A))
            {
                direction += Vector3.left;
            }

            if (Input.GetKey(KeyCode.S))
            {
                direction += Vector3.back;
            }

            if (Input.GetKey(KeyCode.D))
            {
                direction += Vector3.right;
            }

            transform.position += direction * speed;
        }
    }
}