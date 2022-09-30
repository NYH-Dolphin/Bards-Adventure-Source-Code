using DefaultNamespace;
using UnityEngine;
using Utils;

namespace Item
{
    public class RouteAnimationItem : ItemBehavior
    {
        public GameObject objRoute;
        public float fMoveTime = 2f;
        private Transform[] _tranRoutes;
        private Transform _tranOrigin;
        private TimeCountDown _countDown;
        private bool _bMove;

        private void Start()
        {
            base.Start();
            _tranOrigin = transform;
            _tranRoutes = objRoute.transform.GetComponentsInChildren<Transform>();
            _tranRoutes[0] = _tranRoutes[1];
            _countDown = new TimeCountDown(fMoveTime);
        }

        private void Update()
        {
            
            base.Update();
            
            if (_bMove)
            {
                _countDown.Tick(Time.deltaTime);
                if (_countDown.TimeOut)
                {
                    _bMove = false;
                    _countDown.FillTime();
                }

                iTween.PutOnPath(gameObject, _tranRoutes, _countDown.ValueRate);
            }
        }


        public override void OnTriggerEvent()
        {
            _bMove = true;
        }


        public override void Refresh()
        {
            _bMove = false;
            transform.position = _tranOrigin.position;
        }
    }
}