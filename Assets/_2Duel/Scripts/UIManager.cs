using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _nameTexts;
    [SerializeField] private TextMeshProUGUI _damagePopupPrefab;
    [SerializeField] private TextMeshProUGUI _statusPopupPrefab;
    [SerializeField] private float _popupYOffset = 4f;
    [SerializeField] private Slider[] _healthSldiers;
    [SerializeField] private Vector2 _healthBarOffset = new Vector2(0, 2f);
    [SerializeField] private GameObject _victoryPanel;
    [SerializeField] private TextMeshProUGUI _victoryText;
    [SerializeField] private Button _restartButton;
    private Character[] _currentCharacters;

    public event Action OnRestartRequested;

    private void Awake()
    {
        _victoryPanel.SetActive(false);
    }

    public void InitiUI(Character[] characters)
    {
        _currentCharacters = characters;
        for (int i = 0; i < characters.Length; i++)
        {
            int index = i;
            if (characters[index] == null || _nameTexts[i] == null)
                Debug.Log("ÍÅÒÓ ÈÌ¨Í");
            _nameTexts[index].text = characters[index].Name;
            _healthSldiers[index].maxValue = characters[index].MaxHealth;
            _healthSldiers[index].transform.position = Camera.main.WorldToScreenPoint((Vector2) characters[index].transform.position + _healthBarOffset);
            _healthSldiers[index].value = characters[index].CurrentHealth;
            characters[index].OnHealthChanged += (health) =>
            {
                if (_healthSldiers[index] != null)
                    _healthSldiers[index].value = health;
                ShowDamage(characters[index].transform.position, characters[index].MaxHealth - health);
            };
            characters[index].OnStatusChanged += (status) =>
                ShowStatus(characters[index].transform.position, status);
            characters[index].OnDeath += () =>
                ShowVictory(characters[1 - index].Name);
        }
    }
    public void UpdateHealthUI(int index, int health)
    {
        _healthSldiers[index].value = health;
    }

    public void ShowDamage(Vector2 worldPos, int damage)
    {
        CreatePopup(worldPos, damage.ToString(), Color.red, _damagePopupPrefab);
    }

    public void ShowStatus(Vector2 worldPos, string text)
    {
        if (!string.IsNullOrEmpty(text))
            CreatePopup(worldPos, text, Color.white, _statusPopupPrefab);
    }

    private void CreatePopup(Vector2 worldPos, string text, Color color, TextMeshProUGUI prefab)
    {
        var popup = Instantiate(prefab, transform);
        popup.rectTransform.position = Camera.main.WorldToScreenPoint(new Vector2(worldPos.x, worldPos.y));
        popup.text = text;
        popup.color = color;
        Destroy(popup.gameObject, 2.5f);
    }
    public void ShowVictory(string winnerName)
    {
        if (string.IsNullOrEmpty(winnerName))
        {
            _victoryPanel.SetActive(false);
            return;
        }
        _victoryPanel.SetActive(true);
        _victoryText.text = winnerName + " ïîáåäèë!";
    }

    public void HideVictory()
    {
        if (_victoryPanel != null)
            _victoryPanel.SetActive(false);
    }

    public void SetRestart(Action action)
    {
        _restartButton.onClick.RemoveAllListeners();
        _restartButton.onClick.AddListener(() => action?.Invoke());
        _victoryPanel.SetActive(false);
    }
}
