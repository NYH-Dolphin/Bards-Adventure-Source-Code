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
        public GameObject objParticleEffect;
        public GameObject objRoute;
        private Transform[] _tranRoutes;

        private Vector3 _vCrownOriginalPos;
        private Vector3 _vParticlePos;
        private TimeCountDown _colorTimer = new TimeCountDown(1f);
        private bool _bColor;
        private TimeCountDown _particleRouteTimer = new TimeCountDown(1f);
        private bool _bRoute;


        private void Start()
        {
            bGetCrown = false;
            DancingLineGameManager.Instance.RegisterCrown(this);
            _vCrownOriginalPos = objCrown.transform.position;
            _vParticlePos = objParticleEffect.transform.position;
            _tranRoutes = objRoute.transform.GetComponentsInChildren<Transform>();
            _tranRoutes[0] = _tranRoutes[1];
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

            if (_bRoute)
            {
                _particleRouteTimer.Tick(Time.deltaTime);
                if (_particleRouteTimer.TimeOut)
                {
                    _bRoute = false;
                    _particleRouteTimer.FillTime();
                }
                iTween.PutOnPath(objParticleEffect, _tranRoutes, _particleRouteTimer.ValueRate);
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
                    objParticleEffect.SetActive(true);
                }
                else
                {
                    objParticleEffect.SetActive(false);
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
            _bRoute = true;
        }


        public void Refresh()
        {
            objParticleEffect.SetActive(true);
            objParticleEffect.transform.position = _vParticlePos;
            objCrown.SetActive(true);
            objCrown.transform.position = _vCrownOriginalPos;
            bGetCrown = false;
            _bRoute = false;
        }
    }
}