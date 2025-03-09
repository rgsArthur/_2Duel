using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour
{  
    [SerializeField] private string[] _possibleNames;
    [SerializeField] protected int _maxHealth = 100;
    public int MaxHealth => _maxHealth;
    [SerializeField] protected int _baseDamage = 15;
    [SerializeField] protected float _attackInterval = 4f;

    public event Action<int> OnHealthChanged;
    public event Action<string> OnStatusChanged;
    public event Action OnDeath;

    public string Name { get; private set; }
    public int CurrentHealth { get; protected set; }
    public int CurrentDamage { get; protected set; }
    public bool IsAlive { get; set; } = true;

    protected Character _target;
    protected float _attackTimer;

    private string GetRandomName()
    {
        return  _possibleNames.Length > 0 ? _possibleNames[UnityEngine.Random.Range(0, _possibleNames.Length)] : "Безымянный";
    }
    protected virtual void Start()
    {
        Name = GetRandomName();
        Debug.Log("Настроилось имя?" + Name);
        CurrentHealth = _maxHealth;  
        CurrentDamage = _baseDamage;
    }

    protected virtual void Update()
    {
        if (!IsAlive) return;
        _attackTimer -= Time.deltaTime;
        if (_attackTimer <= 0 && _target != null)
        {
            PerformAttack();
            _attackTimer = _attackInterval;
        }
    }

    protected virtual void PerformAttack()
    {
        _target.TakeDamage(CurrentDamage);
    }

    public virtual void ModifyDamage(int newDamage)
    {
        CurrentDamage = newDamage; 
    }

    public void TakeDamage(int damageAmount)
    {
        if (!IsAlive) return;
        CurrentHealth = Mathf.Max(0, CurrentHealth - damageAmount);
        OnHealthChanged?.Invoke(CurrentHealth);
        if (CurrentHealth <= 0)
        {
            IsAlive = false;
            OnDeath?.Invoke();
        }
    }

    public void SetTarget(Character target) => _target = target;

    protected void StatusChange(string status)
    {
        OnStatusChanged?.Invoke(status);
    }
}