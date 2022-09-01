using System;
using UI;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class ItemBehavior : MonoBehaviour
    {
        public float triggerTime; // 触发时间
        

        private void Start()
        {
            DancingLineGameManager.Instance.ResisterItem(this);
        }

        /// <summary>
        /// 事件触发
        /// </summary>
        public virtual void OnTriggerEvent()
        {
        }

        protected void Update()
        {
            if (DancingLineGameManager.Instance.music.time - triggerTime <= 5 * Time.deltaTime &&
                DancingLineGameManager.Instance.music.time > triggerTime)
            {
                OnTriggerEvent();
            }
        }
        
    }
}