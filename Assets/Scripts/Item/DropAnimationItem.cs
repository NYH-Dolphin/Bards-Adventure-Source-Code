using DefaultNamespace;
using UI;
using UnityEngine;

namespace Item
{
    public class DropAnimationItem : ItemBehavior
    {
        private Transform _originTransform;
        public Rigidbody objRigidbody;

        private void Start()
        {
            base.Start();
            DancingLineGameManager.Instance.ResisterItem(this);
            _originTransform = transform;
        }

        public override void OnTriggerEvent()
        {
            objRigidbody.useGravity = true;
        }


        public override void Refresh()
        {
            objRigidbody.useGravity = false;
            transform.position = _originTransform.position;
            transform.rotation = _originTransform.rotation;
        }
    }
}