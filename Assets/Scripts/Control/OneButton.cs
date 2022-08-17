using System;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Serialization;
using UnityEngine;

namespace Control
{
    /// <summary>
    /// 自己重写的 Button 按钮
    /// 1、单击
    /// 2、双击
    /// 3、长按
    /// </summary>    
    public class OneButton : Selectable, IPointerClickHandler, ISubmitHandler
    {
        [Serializable]
        public class ButtonClickedEvent : UnityEvent
        {
        }

        // Event delegates triggered on click.
        [FormerlySerializedAs("m_OnClick")] [FormerlySerializedAs("onClick")] [SerializeField]
        private ButtonClickedEvent mOnClick = new ButtonClickedEvent();

        protected OneButton()
        {
        }


        public ButtonClickedEvent ONClick
        {
            get => mOnClick;
            set => mOnClick = value;
        }

        private void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;

            UISystemProfilerApi.AddMarker("Button.onClick", this);
            mOnClick.Invoke();
        }


        [Serializable]
        public class ButtonLongPressEvent : UnityEvent
        {
        }

        [FormerlySerializedAs("m_onLongPress")] [FormerlySerializedAs("onLongPress")] [SerializeField]
        private ButtonLongPressEvent mONLongPress = new ButtonLongPressEvent();

        public ButtonLongPressEvent ONLongPress
        {
            get => mONLongPress;
            set => mONLongPress = value;
        }

        [FormerlySerializedAs("m_onLongPressCancel")] [FormerlySerializedAs("onLongPressCancel")] [SerializeField]
        private ButtonLongPressEvent mONLongPressCancel = new ButtonLongPressEvent();

        public ButtonLongPressEvent ONLongPressCancel => mONLongPressCancel;


        private bool _myIsStartPress;

        private float _myCurPointDownTime;

        private float my_longPressTime = 0.15f;

        private bool _myLongPressTrigger;


        void Update()
        {
            CheckIsLongPress();
        }

        void CheckIsLongPress()
        {
            if (_myIsStartPress && !_myLongPressTrigger)
            {
                if (Time.time > _myCurPointDownTime + my_longPressTime)
                {
                    _myLongPressTrigger = true;
                    _myIsStartPress = false;
                    if (mONLongPress != null)
                    {
                        ONLongPress.Invoke();
                        bCancel = true;
                    }
                }
            }
        }


        public virtual void OnPointerClick(PointerEventData eventData)
        {
            //(避免已經點擊進入長按后，擡起的情況)
            if (!_myLongPressTrigger)
            {
                ONClick.Invoke();
            }
        }


        public virtual void OnSubmit(BaseEventData eventData)
        {
            Press();

            // if we get set disabled during the press
            // don't run the coroutine.
            if (!IsActive() || !IsInteractable())
                return;

            DoStateTransition(SelectionState.Pressed, false);
            StartCoroutine(OnFinishSubmit());
        }

        private IEnumerator OnFinishSubmit()
        {
            var fadeTime = colors.fadeDuration;
            var elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            DoStateTransition(currentSelectionState, false);
        }


        private bool bCancel;

        public override void OnPointerDown(PointerEventData eventData)
        {
            // 按下刷新當前時間
            base.OnPointerDown(eventData);
            _myCurPointDownTime = Time.time;
            _myIsStartPress = true;
            _myLongPressTrigger = false;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            // 指針擡起，結束開始長按
            base.OnPointerUp(eventData);
            _myIsStartPress = false;
            if (bCancel)
            {
                ONLongPressCancel?.Invoke();
                bCancel = false;
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            // 指針移出，結束開始長按，計時長按標志
            base.OnPointerExit(eventData);
            _myIsStartPress = false;
        }
    }
}