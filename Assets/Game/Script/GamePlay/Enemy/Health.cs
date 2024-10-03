using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;
    private int _currentHealth;

    public event Action<int> OnTakeDamage;
    public event Action OnDeath;
    public bool IsDead => _currentHealth <= 0;

    public void Initialize()
    {
        _currentHealth = MaxHealth;
    }

    public void DealDamage(int damage)
    {
        if (_currentHealth <= 0) return;
        _currentHealth = Mathf.Max(_currentHealth - damage, 0);
        OnTakeDamage?.Invoke(_currentHealth);
        if (_currentHealth <= 0) OnDeath?.Invoke();
    }
}