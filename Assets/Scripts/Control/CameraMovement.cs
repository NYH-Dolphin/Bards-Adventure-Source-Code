using System;
using UnityEngine;

namespace Control
{
    public class CameraMovement : MonoBehaviour
    {
        public GameObject objectToFollow;

        public float speed = 2.0f;

        private void Start()
        {
            transform.position = objectToFollow.transform.position;
            transform.rotation = objectToFollow.transform.rotation;
        }

        private void Update()
        {
            float interpolation = speed * Time.deltaTime;
            Vector3 position = transform.position;
            position.y = Mathf.Lerp(transform.position.y, objectToFollow.transform.position.y, interpolation);
            position.z = Mathf.Lerp(transform.position.z, objectToFollow.transform.position.z, interpolation);
            position.x = Mathf.Lerp(transform.position.x, objectToFollow.transform.position.x, interpolation);
            transform.position = position;
        }
    }
}