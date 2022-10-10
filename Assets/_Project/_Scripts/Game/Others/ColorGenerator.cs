using UnityEngine;

[DefaultExecutionOrder(-2)]
internal class ColorGenerator : MonoBehaviour
{
    private float _hue;

    [SerializeField, Tooltip("Randomizes the Hue value for every call on the function [GetColor]")]
    private bool randomizeHue;

    private void Awake()
    {
        _hue = Random.Range(0, 11) / 10.0f;
    }


    public Color GetColor(float s = .5f, float v = .7f)
    {
        if (randomizeHue)
            _hue = Random.Range(0, 11) / 10.0f;

        return Color.HSVToRGB(_hue, s, v);
    }
}
