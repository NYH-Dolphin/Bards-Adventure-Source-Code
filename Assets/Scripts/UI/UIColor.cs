using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    class UIColor : MonoBehaviour
    {
        public List<GameObject> colorObj; // 透明的会变换的物体
        private TimeCountDown _count = new TimeCountDown(1.5f);
        private bool _bTrigger;


        private void Start()
        {
        }

        private void Update()
        {
            _count.Tick(Time.deltaTime);
            foreach (var obj in colorObj)
            {
                if (obj.GetComponent<Text>() != null)
                {
                    Color color = obj.GetComponent<Text>().color;
                    if (!_bTrigger)
                    {
                        color.a = _count.ValueRate;
                    }
                    else
                    {
                        color.a = 1 - _count.ValueRate;
                    }

                    obj.GetComponent<Text>().color = color;
                }
                if (obj.GetComponent<Image>() != null)
                {
                    Color color = obj.GetComponent<Image>().color;
                    if (!_bTrigger)
                    {
                        color.a = _count.ValueRate;
                    }
                    else
                    {
                        color.a = 1 - _count.ValueRate;
                    }
                    obj.GetComponent<Image>().color = color;
                }
            }

            if (_count.TimeOut)
            {
                _bTrigger = !_bTrigger;
                _count.FillTime();
            }
        }
    }
}