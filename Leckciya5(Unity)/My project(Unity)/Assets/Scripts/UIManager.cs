using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    public SpringMassController springMassController;
    public TMP_InputField massInput;
    public TMP_InputField stiffnessInput;
    public TMP_InputField dampingInput;
    public TMP_InputField displacementInput;
    public TMP_Text infoText;
    public TMP_Text displacementText;

    public void StartSimulation()
    {
        float mass = ParseInput(massInput, 1f);
        float stiffness = ParseInput(stiffnessInput, 10f);
        float damping = ParseInput(dampingInput, 0.5f);
        float displacement = ParseInput(displacementInput, 2f);

        springMassController.StartSimulation(mass, stiffness, damping, displacement);

        UpdateInfoText();
        UpdateDisplacementText();
    }

    public void ResetSimulation()
    {
        springMassController.ResetSystem();
        UpdateInfoText();
        UpdateDisplacementText();
    }

    private float ParseInput(TMP_InputField inputField, float defaultValue)
    {
        if (float.TryParse(inputField.text, out float result))
            return result;

        return defaultValue;
    }

    private void Update()
    {
        UpdateInfoText();
        UpdateDisplacementText();
    }

    private void UpdateInfoText()
    {
        string state = springMassController.isRunning ? "Идёт моделирование" : "Остановлено";

        infoText.text =
            $"Состояние: {state}\n" +
            $"Время: {springMassController.ElapsedTime:F2} с\n" +
            $"Макс. смещение: {springMassController.MaxDisplacement:F2} усл. ед.";
    }

    private void UpdateDisplacementText()
    {
        float currentDisplacement = springMassController.CurrentDisplacement;
        displacementText.text = $"Текущее смещение: {currentDisplacement:F2} усл. ед.";
    }
}