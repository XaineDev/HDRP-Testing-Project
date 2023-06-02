using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Controller for players health and health bar
/// </summary>
public class HealthController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;
    
    [SerializeField]
    private float currentHealth;
    
    private bool _takenDamage = false;
    private bool _isDead = false;
    
    [SerializeField]
    private bool isInvincible = false;

    private TMP_Text _healthText;
    private RectTransform _healthBar;
    private RectTransform _healthBarBackground;

    private RectTransform _healthDisplay;

    [SerializeField]
    private float maxTimeSinceHealthUpdate = 3f;
    private float _timeSinceUpdate = 0f;

    private TMP_Text _waveText; // using wave text as a death message because why not
    private WaveTextDisappear _waveTextDisappear;
    
    void Start()
    {
        _healthDisplay = GameObject.Find("HealthDisplay").GetComponent<RectTransform>();
        _healthText = GameObject.Find("HealthText").GetComponent<TMP_Text>();
        _healthBar = GameObject.Find("HealthForeground").GetComponent<RectTransform>();
        _healthBarBackground = GameObject.Find("HealthBackground").GetComponent<RectTransform>();
        _waveText = GameObject.Find("WaveText").GetComponent<TMP_Text>();
        _waveTextDisappear = _waveText.GetComponent<WaveTextDisappear>();
        
        // gameobject.find is bad but its fine because there will only be one of these in the scene
        
        currentHealth = maxHealth;
    }

    
    void Update()
    {
        _timeSinceUpdate += Time.deltaTime;
        if (!_takenDamage && (_timeSinceUpdate < maxTimeSinceHealthUpdate)) return;
        _timeSinceUpdate = 0f;
        
        const int healthBarOffset = 2;
        
        var healthPercent = Math.Max(0,Math.Min(1, currentHealth / maxHealth));
        var healthDisplayWidth = _healthDisplay.rect.width;
        var healthBarWidth = (healthDisplayWidth-healthBarOffset*2) * healthPercent;

        // update health bar
        _healthBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthBarWidth);

        // update text
        _healthText.SetText(currentHealth.ToString(CultureInfo.CurrentCulture));
        var pos = _healthText.rectTransform.anchoredPosition ;
        pos.x = (255 * healthPercent) - 255;
        _healthText.rectTransform.anchoredPosition  = pos;
        _takenDamage = false;
    }

    /// <summary>
    /// Set player invincibility
    /// </summary>
    /// <param name="state">invincibility state</param>
    public void SetInvincible(bool state = true)
    {
        isInvincible = state;
    }
    
    /// <summary>
    /// Toggles Invincibility on or off
    /// </summary>
    public void ToggleInvincibility()
    {
        isInvincible = !isInvincible;
    }
    
    /// <summary>
    /// Apply x amount of damage to player
    /// </summary>
    /// <param name="damage"> amount of damage to take </param>
    public void TakeDamage(int damage = 1)
    {
        if (isInvincible)
        {
            return;
        }

        _takenDamage = true;
        currentHealth = Math.Max(0,currentHealth - damage);
        
        if (currentHealth != 0) return;
        
        _isDead = true;
        _waveText.SetText("You Died!");
        _waveText.fontSize = 72f;
        _waveText.fontStyle = FontStyles.Bold;
        _waveText.color = Color.red;
        _waveTextDisappear.enabled = false;
        _waveText.gameObject.SetActive(true);
    }
    
    /// <summary>
    /// Get if player is dead
    /// </summary>
    /// <returns>if player is dead ( hp &lt;= 0 )</returns>
    public bool IsDead()
    {
        return _isDead;
    }
}
