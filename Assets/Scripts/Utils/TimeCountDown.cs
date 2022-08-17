using UnityEngine;

namespace Utils
{
    public class TimeCountDown
    {
        protected float fValue;
        protected float fCurValue;

        //值发生变化的委托
        public System.Action<float> pActionValue;


        public TimeCountDown(float value)
        {
            fValue = value;
            FillTime();
        }

        public TimeCountDown()
        {
        }


        // 当前计时器进行了百分之多少
        public float ValueRate
        {
            get
            {
                if (fCurValue > 0)
                {
                    return fCurValue / Value;
                }

                return 0;
            }
        }

        public virtual float Value
        {
            get => fValue;
            set
            {
                fValue = value;
                if (pActionValue != null)
                {
                    pActionValue(fValue);
                }
            }
        }

        public virtual float CurValue
        {
            get { return Mathf.Max(fCurValue, 0); }
        }


        public virtual float TimePassed
        {
            get
            {
                if (fCurValue > 0)
                {
                    return fCurValue;
                }

                return 0;
            }
        }


        public int TimeSecond
        {
            get
            {
                int iDiff = (int) (fCurValue * 1000);
                if (iDiff > 0)
                {
                    return iDiff;
                }

                return 0;
            }
        }


        public void ClearTime()
        {
            fCurValue = 0F;
        }

        public void Tick(float delta)
        {
            fCurValue += delta;
        }

        public bool TimeOut
        {
            get => fCurValue >= fValue;
        }

        public void FillTime(bool addLast = false)
        {
            if (addLast)
            {
                float fRes = fCurValue;
                fCurValue = 0;
                fCurValue -= fRes;
            }
            else
            {
                fCurValue = 0;
            }
        }
    }
}
