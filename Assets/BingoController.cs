using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BingoController : MonoBehaviour
{
    private TextMeshProUGUI _number;

    [SerializeField] private List<int> NumbersRepeated = new List<int>();

    NumberPanelController _numberPanelController;

    // Animaciones
    public Animator _animator;
    [SerializeField] private Motion cambioNumAnim;


    // Count Effect
    public int CountFPS = 30;
    public float Duration = 1f;
    public int _value;

    public int Value
    {
        get
        {
            return _value;
        }
        set
        {
            UpdateText(value);
            _value = value;
        }
    }

    private Coroutine CountingCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        _number = GetComponent<TextMeshProUGUI>();
        _number.text = "0";
        NumbersRepeated.Clear();
        _numberPanelController = GameObject.FindFirstObjectByType<NumberPanelController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            RollNumber();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            StartNewGame();
        }
    }

    private void RollNumber()
    {
        while(NumbersRepeated.Count < 90)
        {
            Value = Random.Range(1, 91);
            if (!NumbersRepeated.Contains(Value))
            {
                _animator.Play(cambioNumAnim.name);
                NumbersRepeated.Add(Value);
                _numberPanelController.UpdatePanel(Value);
                break;
            }
        }
    }

    private void StartNewGame()
    {
        Value = 00;
        NumbersRepeated.Clear();
        _numberPanelController.ClearPanel();
    }

    public void UpdateText(int newValue)
    {
        if(CountingCoroutine != null)
        {
            StopCoroutine(CountingCoroutine);
        }

        CountingCoroutine = StartCoroutine(CountText(newValue));
    }

    private IEnumerator CountText(int newValue)
    {
        WaitForSeconds Wait = new WaitForSeconds(1f / CountFPS);
        int previousValue = _value;
        int stepAmount;

        if(newValue - previousValue < 0)
        {
            stepAmount = Mathf.FloorToInt((newValue - previousValue) / (CountFPS * Duration));
        }
        else
        {
            stepAmount = Mathf.CeilToInt((newValue - previousValue) / (CountFPS * Duration));
        }

        if(previousValue < newValue)
        {
            while(previousValue < newValue)
            {
                previousValue += stepAmount;

                if(previousValue > newValue)
                {
                    previousValue = newValue;
                }
                _number.SetText(previousValue.ToString());

                yield return Wait;
            }
        }
        else
        {
            while (previousValue > newValue)
            {
                previousValue += stepAmount;

                if (previousValue < newValue)
                {
                    previousValue = newValue;
                }
                _number.SetText(previousValue.ToString());

                yield return Wait;
            }
        }
    }


}
