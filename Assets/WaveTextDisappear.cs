using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveTextDisappear : MonoBehaviour
{

    [SerializeField] private float baseDisappearTime = 2f;
    [SerializeField] private float baseFadeOutTime = 5f;

    private TMP_Text _text;

    private void OnEnable()
    {
        if (_text == null)
            _text = GetComponent<TMP_Text>();
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 1);

        StartCoroutine(Disappear());
    }

    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(baseDisappearTime);
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        var fadeOutTime = baseFadeOutTime;
        
        while (fadeOutTime >= 0)
        {
            fadeOutTime -= Time.deltaTime;
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, Math.Clamp(fadeOutTime/baseFadeOutTime, 0, 1));

            yield return null;
        }
        
        gameObject.SetActive(false);
    }
}
