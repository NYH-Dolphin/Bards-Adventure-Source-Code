using Control;
using UnityEngine;

namespace DefaultNamespace
{
    public class CameraItem : ItemBehavior
    {
        public GameObject objToFollow;
        private CameraMovement _myCamera;


        private void Start()
        {
            _myCamera = transform.GetComponent<CameraMovement>();
        }

        public override void OnTriggerEvent()
        {
            _myCamera.objectToFollow = objToFollow;
        }
        
    }
}