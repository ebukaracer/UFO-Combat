using DG.Tweening;
using UnityEngine;

internal class TweenCore
{
    public float Duration;
    public Ease EaseType;
}

internal class TweenProperties : TweenCore
{
    public Vector2 EndValue;
    public Vector2 StartValue;
}

internal class TweenProperties2 : TweenCore
{
    public float EndValue;
    public float StartValue;
}