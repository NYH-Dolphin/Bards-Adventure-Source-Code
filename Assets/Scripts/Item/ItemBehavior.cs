using System;
using UI;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class ItemBehavior : MonoBehaviour
    {
        public float triggerTime; // 触发时间
        private bool _bTriggered;

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
            if (!_bTriggered && DancingLineGameManager.Instance.music.time > triggerTime &&
                DancingLineGameManager.Instance.music.time - triggerTime <= 10 * Time.deltaTime)
            {
                OnTriggerEvent();
                _bTriggered = true;
            }
            
            if (_bTriggered && DancingLineGameManager.Instance.music.time - triggerTime > 10 * Time.deltaTime)
            {
                _bTriggered = false;
            }
        }
    }
}