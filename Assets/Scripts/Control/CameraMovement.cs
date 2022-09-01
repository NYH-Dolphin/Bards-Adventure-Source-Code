using UnityEngine;

namespace Control
{
    public class CameraMovement : MonoBehaviour
    {
        public GameObject objectToFollow;
        public GameObject originalObject;
        private GameObject recordObjToFollow; // 记录的Object
        public float speed = 2.0f;

        private void Start()
        {
            transform.position = objectToFollow.transform.position;
            transform.rotation = objectToFollow.transform.rotation;
            recordObjToFollow = objectToFollow;
        }

        private void Update()
        {
            float interpolation = speed * Time.deltaTime;
            Vector3 position = transform.position;
            position.y = Mathf.Lerp(transform.position.y, objectToFollow.transform.position.y, interpolation);
            position.z = Mathf.Lerp(transform.position.z, objectToFollow.transform.position.z, interpolation);
            position.x = Mathf.Lerp(transform.position.x, objectToFollow.transform.position.x, interpolation);
            transform.position = position;
            transform.rotation = Quaternion.Lerp(transform.rotation, objectToFollow.transform.rotation,
                Time.deltaTime * speed * 0.5f);
        }


        public void RecordObjectToFollow()
        {
            recordObjToFollow = objectToFollow;
        }

        public void Refresh()
        {
            transform.position = recordObjToFollow.transform.position;
            transform.rotation = recordObjToFollow.transform.rotation;
            objectToFollow = recordObjToFollow;
        }

        public void Restart()
        {
            transform.position = originalObject.transform.position;
            transform.rotation = originalObject.transform.rotation;
            objectToFollow = originalObject;
            recordObjToFollow = originalObject;
        }
    }
}