using UnityEngine;

namespace DefaultNamespace
{
    public class AudioItem : ItemBehavior
    {
        public AudioSource audio;
        
        public override void OnTriggerEvent()
        {
            if (PlayerPrefs.GetInt(audio.name, 0) == 0)
            {
                PlayerPrefs.SetInt(audio.name, 1);
                audio.Play();
            }

            
            
        }
    }
}