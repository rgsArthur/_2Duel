using UnityEngine;

public class Wizard : Character
{
    [Header("Настройки слабости")]
    [SerializeField][Range(0, 1)] private float _weakenChance = 0.25f;
    [SerializeField] private float _weakenDuration = 3f;
    [SerializeField] private float _damageMultiplier = 0.5f;

    private Character _weakenedTarget;
    private float _weakenTimer;
    private int _originalDamage;

    protected override void Start()
    {
        
        base.Start();
    }

    protected override void PerformAttack()
    {
        base.PerformAttack();

        if (!_target.IsAlive || Random.value > _weakenChance) return;
        _weakenedTarget = _target;
        _originalDamage = _weakenedTarget.CurrentDamage;
        _weakenedTarget.ModifyDamage(Mathf.RoundToInt(_originalDamage * _damageMultiplier));
        _weakenTimer = _weakenDuration;
        StatusChange("Ослаблен!");
    }

    protected override void Update()
    {
        base.Update();
        if (_weakenedTarget == null) return;
        _weakenTimer -= Time.deltaTime;
        if(_weakenTimer <= 0)
        {
            _weakenedTarget.ModifyDamage(_originalDamage);
            _weakenedTarget = null;
        }
    }
}