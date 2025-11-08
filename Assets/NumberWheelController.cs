using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberWheelController : MonoBehaviour
{
    [Header("Configuración")]
    public int maxNumber = 10;              // Números del 1 al X
    public float spinDuration = 4f;
    public float spinSpeed = 500f;
    public Button spinButton;
    public Text resultText;                 // Texto donde mostrar el número ganador

    private bool isSpinning = false;
    private List<int> remainingNumbers = new List<int>();
    private float currentRotation = 0f;

    void Start()
    {
        // Inicializar lista de números
        ResetWheel();

        if (spinButton != null)
            spinButton.onClick.AddListener(SpinWheel);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            SpinWheel();
        }
    }

    public void ResetWheel()
    {
        remainingNumbers.Clear();
        for (int i = 1; i <= maxNumber; i++)
            remainingNumbers.Add(i);
    }

    public void SpinWheel()
    {
        if (!isSpinning && remainingNumbers.Count > 0)
            StartCoroutine(Spin());
    }

    private IEnumerator Spin()
    {
        isSpinning = true;

        // Elegir número aleatorio disponible
        int randomIndex = Random.Range(0, remainingNumbers.Count);
        int selectedNumber = remainingNumbers[randomIndex];

        // Calcular ángulo del número elegido
        float anglePerNumber = 360f / remainingNumbers.Count;
        float targetAngle = 360f * Random.Range(3, 6) + (randomIndex * anglePerNumber);

        float elapsed = 0f;
        float totalAngle = currentRotation;

        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / spinDuration;
            float speed = Mathf.Lerp(spinSpeed, 0, t);
            float deltaAngle = speed * Time.deltaTime;
            totalAngle += deltaAngle;
            transform.rotation = Quaternion.Euler(0, 0, -totalAngle);
            yield return null;
        }

        // Ajustar rotación exacta
        transform.rotation = Quaternion.Euler(0, 0, -targetAngle);
        currentRotation = targetAngle;

        // Mostrar resultado
        resultText.text = "Número: " + selectedNumber;
        Debug.Log($"Número elegido: {selectedNumber}");

        // Remover número elegido
        remainingNumbers.RemoveAt(randomIndex);

        // Actualizar ruleta visual
        GenerateWheelVisual();

        isSpinning = false;
    }

    public GameObject segmentPrefab;  // Prefab de un sector (una porción circular)
    public Transform wheelParent;     // Donde se crean los segmentos

    private void GenerateWheelVisual()
    {
        foreach (Transform child in wheelParent)
            Destroy(child.gameObject);

        int count = remainingNumbers.Count;
        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            GameObject segment = Instantiate(segmentPrefab, wheelParent);
            segment.transform.localRotation = Quaternion.Euler(0, 0, -i * angleStep);
            segment.GetComponentInChildren<Text>().text = remainingNumbers[i].ToString();
        }
    }

}
