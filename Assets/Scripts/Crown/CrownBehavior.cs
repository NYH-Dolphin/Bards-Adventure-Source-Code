using System;
using System.Collections;
using Control;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace DefaultNamespace
{
    public class CrownBehavior : MonoBehaviour
    {
        public int iIndex;
        public Image imgYellowCrown;
        public GameObject objCrown;

        private TimeCountDown _colorTimer = new TimeCountDown(0.5f);
        private bool _bColor;


        private void Start()
        {
            bGetCrown = false;
            DancingLineGameManager.Instance.RegisterCrown(this);
        }

        private void Update()
        {
            Color color = imgYellowCrown.color;
            if (_bColor)
            {
                _colorTimer.Tick(Time.deltaTime);
                color.a = _colorTimer.ValueRate;
                imgYellowCrown.color = color;
                if (_colorTimer.TimeOut)
                {
                    _colorTimer.FillTime();
                    _bColor = false;
                }
            }
        }

        private bool _bGetCrown; // 是否获取了皇冠

        public bool bGetCrown
        {
            get => _bGetCrown;
            set
            {
                _bGetCrown = value;
                if (_bGetCrown)
                {
                    _bColor = true;
                }
                else
                {
                    GameObject.Find("Line").GetComponent<LineController>().crownGet[iIndex] = false;
                    Color color = imgYellowCrown.color;
                    color.a = 0;
                    imgYellowCrown.color = color;
                }
            }
        }

        /// <summary>
        /// 取得了皇冠触发
        /// </summary>
        /// <param name="line"></param>
        public void OnGetCrown(LineController line)
        {
            bGetCrown = true;
            line.RecordCheckPoint(iIndex);
            GameObject.Find("Main Camera").GetComponent<CameraMovement>().RecordObjectToFollow(); // 摄像机记录位置
            objCrown.SetActive(false);
        }


        public void Refresh()
        {
            objCrown.SetActive(true);
            bGetCrown = false;
        }
    }
}