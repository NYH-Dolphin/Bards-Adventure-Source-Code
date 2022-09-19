using DefaultNamespace;
using UI;
using UnityEngine;

public class AnimationItem : ItemBehavior
{
    public float yOffset = 3f;
    public float fAppearTime = 0.3f;
    public iTween.EaseType easetype = iTween.EaseType.easeOutBack;

    private Vector3 originalPos;

    private void Start()
    {
        base.Start();
        DancingLineGameManager.Instance.ResisterItem(this);
        Vector3 pos = transform.position;
        pos.y -= yOffset;
        transform.position = pos;
        originalPos = pos;
    }


    public override void OnTriggerEvent()
    {
        iTween.MoveAdd(gameObject,
            iTween.Hash("time", fAppearTime, "amount", new Vector3(0, yOffset, 0), "easetype", easetype));
    }


    public override void Refresh()
    {
        transform.position = originalPos;
    }
}