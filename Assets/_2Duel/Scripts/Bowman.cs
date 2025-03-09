using UnityEngine;

public class Bowman : Character
{
    [Header("Настройки яда")]
    [SerializeField][Range(0, 1)] private float _poisonChance = 0.4f;
    [SerializeField] private int _poisonDamage = 3;
    [SerializeField] private float _poisonDuration = 4f;
    [SerializeField] private float _tickInterval = 1f;

    private Character _poisonedTarget;
    private float _poisonTimer;
    private float _nextTickTime;

    protected override void PerformAttack()
    {
        base.PerformAttack();

        if (!_target.IsAlive || Random.value > _poisonChance) return;

        _poisonedTarget = _target;
        _poisonTimer = _poisonDuration;
        _nextTickTime = 0;
        StatusChange("Отравлен!");
    }

    protected override void Update()
    {
        base.Update();
        if (_poisonedTarget == null) return;
        _poisonTimer -= Time.deltaTime;
        _nextTickTime -= Time.deltaTime;

        if (_nextTickTime <= 0)
        {
            _poisonedTarget.TakeDamage(_poisonDamage);
            _nextTickTime = _tickInterval;
        }

        if (_poisonTimer <= 0)
            _poisonedTarget = null;
    }
}