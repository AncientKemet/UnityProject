using System;
using Code.Libaries.Generic;
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Weather : MonoSingleton<Weather>
{
    private float _time = 0;
    [SerializeField]
    private Light _topLight, _bottomLight;

    public float Time = 0;
    public float ratio = 0;

    public Color DayColor;
    public Color NightColor;

    private void Update()
    {
        if (Time > 24)
        {
            Time -= 24;
        }
        if (Time < 0)
        {
            Time += 24;
        }
        if (_topLight != null && _bottomLight != null)
        if (Math.Abs(Time - _time) > 0.1f)
        {
            _time = Time;

            float dayNightRatio = Mathf.Abs((_time - 12f)/24f) * 2f;
            ratio = dayNightRatio;
            
            _topLight.color = DayColor * dayNightRatio + (NightColor * (1f - dayNightRatio));
            _bottomLight.color = DayColor * dayNightRatio + (NightColor * (1f - dayNightRatio));

            RenderSettings.fogColor = DayColor * dayNightRatio + (NightColor * (1f - dayNightRatio));
            RenderSettings.ambientLight = DayColor * dayNightRatio + (NightColor * (1f - dayNightRatio));
            RenderSettings.fogEndDistance = 30+dayNightRatio * 100;
        }
    }
}
