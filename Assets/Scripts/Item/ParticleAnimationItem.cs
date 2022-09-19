using DefaultNamespace;
using UI;
using UnityEngine;

namespace Item
{
    public class ParticleAnimationItem : ItemBehavior
    {
        public ParticleSystem particle;
        private void Start()
        {
            base.Start();
            DancingLineGameManager.Instance.ResisterItem(this);
        }


        public override void OnTriggerEvent()
        {
            particle.Play();
        }


        public override void Refresh()
        {
            particle.Stop();
        }
    }
}