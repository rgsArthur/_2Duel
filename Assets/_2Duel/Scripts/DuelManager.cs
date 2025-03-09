using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class DuelManager : MonoBehaviour
{
    [SerializeField] private CharacterFactory _factory;
    [SerializeField] private UIManager _uiManager;
    private Character[] _fighters = new Character[2];
    private void Start()
    {
        InitDuel();
        _uiManager.SetRestart(RestartDuel);
    }

    private void InitDuel()
    {
        _uiManager.HideVictory();

        _fighters[0] = _factory.CreateRandom(0);
        _fighters[1] = _factory.CreateRandom(1);

        _fighters[0].SetTarget(_fighters[1]);
        _fighters[1].SetTarget(_fighters[0]);

        StartCoroutine(DelayedUIInit());
    }
    private IEnumerator DelayedUIInit()
    {
        yield return null;
        _uiManager.InitiUI(_fighters);
    }
    private void RestartDuel()
    {
         foreach(var fighter in _fighters)
         {
             if (fighter != null)
                Destroy(fighter.gameObject);   
         }
         InitDuel();
     }
}