using UnityEngine;

namespace DefaultNamespace
{
    public class AppearItem : ItemBehavior
    {
        public GameObject appearObject;
        private void Start()
        {
            appearObject.SetActive(false);
        }
        public override void OnTriggerEvent()
        {
            appearObject.SetActive(true);
        }
    }
}