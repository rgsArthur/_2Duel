using System.Collections;
using UnityEngine;

public class Warrior : Character
{
    [Header("Настройки оглушения")]
    [SerializeField][Range(0, 1)] private float _stunChance = 0.3f;
    [SerializeField] private float _stunDuration = 2f;
    private float _stunTimer;
    private Character _stunnedTarget;

    protected override void PerformAttack()
    {
        base.PerformAttack();
        if (!_target.IsAlive || Random.value > _stunChance) return;
        _stunnedTarget = _target;
        _stunnedTarget.enabled = false;
        _stunTimer = _stunDuration;
        StatusChange("Оглушён!");
    }

    protected override void Update()
    {
        base.Update();

        if (_stunnedTarget == null) return;
        _stunTimer -= Time.deltaTime;
        if (_stunTimer <= 0)
        {
            _stunnedTarget.enabled = true;
            _stunnedTarget = null;
        }
    }
}