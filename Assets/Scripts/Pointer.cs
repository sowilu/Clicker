using UnityEngine;
using DG.Tweening;

public class Pointer : MonoBehaviour
{
    void Start()
    {
        InvokeRepeating("Click", 1, 1);
    }

    void Click()
    {
        transform.DOLocalMove(transform.localPosition / 3 * 2, 0.1f).SetLoops(2, LoopType.Yoyo);
        Clicker.instance.Clicks += Clicker.clickValue;
    }
}
