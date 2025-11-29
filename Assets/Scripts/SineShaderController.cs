using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

[ExecuteAlways]
public class SineShaderController : MonoBehaviour {
    [SerializeField] Material sineMaterial;
    [SerializeField] UIDocument uiDocument;

    [Header("Value Ranges")]
    [SerializeField] float pixelFreqMin = 0f;
    [SerializeField] float pixelFreqMax = 100f;
    [SerializeField] float timeScaleMin = 0f;
    [SerializeField] float timeScaleMax = 10f;
    [SerializeField] float simTimeOffsetMin = -100000f;
    [SerializeField] float simTimeOffsetMax = 100000f;

    static readonly int P_PixelFreq = Shader.PropertyToID("_PixelFreq");
    static readonly int P_TimeScale = Shader.PropertyToID("_TimeScale");
    static readonly int P_SimTimeOffset = Shader.PropertyToID("_SimTimeOffset");

    Slider pixelFreqSlider;
    TextField pixelFreqInput;
    Slider timeScaleSlider;
    TextField timeScaleInput;
    Slider simTimeOffsetSlider;
    TextField simTimeOffsetInput;

    bool isInitialized = false;

    private void OnEnable() {
        if (uiDocument == null) uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null || sineMaterial == null) return;

        isInitialized = false;
        if (Application.isPlaying) {
            StartCoroutine(WaitForUIDocument());
        } else {
            if (uiDocument.rootVisualElement != null) {
                InitializeUI();
            }
        }
    }

    private void Start() {
        if (uiDocument == null) uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null || sineMaterial == null) return;

        if (!isInitialized && uiDocument.rootVisualElement != null) {
            InitializeUI();
        }
    }

    private IEnumerator WaitForUIDocument() {
        while (uiDocument.rootVisualElement == null) {
            yield return null;
        }
        InitializeUI();
    }

    private void InitializeUI() {
        if (isInitialized) return;
        var root = uiDocument.rootVisualElement;
        if (root == null) return;

        pixelFreqSlider = root.Q<Slider>("pixel-freq-slider");
        pixelFreqInput = root.Q<TextField>("pixel-freq-input");
        timeScaleSlider = root.Q<Slider>("time-scale-slider");
        timeScaleInput = root.Q<TextField>("time-scale-input");
        simTimeOffsetSlider = root.Q<Slider>("sim-time-offset-slider");
        simTimeOffsetInput = root.Q<TextField>("sim-time-offset-input");

        if (pixelFreqSlider != null) {
            pixelFreqSlider.lowValue = pixelFreqMin;
            pixelFreqSlider.highValue = pixelFreqMax;
        }

        if (timeScaleSlider != null) {
            timeScaleSlider.lowValue = timeScaleMin;
            timeScaleSlider.highValue = timeScaleMax;
        }

        if (simTimeOffsetSlider != null) {
            simTimeOffsetSlider.lowValue = simTimeOffsetMin;
            simTimeOffsetSlider.highValue = simTimeOffsetMax;
        }

        UpdateUIFromMaterial();

        if (pixelFreqSlider != null) {
            pixelFreqSlider.RegisterValueChangedCallback(OnPixelFreqSliderChanged);
        }

        if (pixelFreqInput != null) {
            pixelFreqInput.RegisterValueChangedCallback(OnPixelFreqInputChanged);
        }

        if (timeScaleSlider != null) {
            timeScaleSlider.RegisterValueChangedCallback(OnTimeScaleSliderChanged);
        }

        if (timeScaleInput != null) {
            timeScaleInput.RegisterValueChangedCallback(OnTimeScaleInputChanged);
        }

        if (simTimeOffsetSlider != null) {
            simTimeOffsetSlider.RegisterValueChangedCallback(OnSimTimeOffsetSliderChanged);
        }

        if (simTimeOffsetInput != null) {
            simTimeOffsetInput.RegisterValueChangedCallback(OnSimTimeOffsetInputChanged);
        }

        isInitialized = true;
    }

    private void UpdateUIFromMaterial() {
        if (sineMaterial == null) return;

        float pixelFreq = sineMaterial.GetFloat(P_PixelFreq);
        float timeScale = sineMaterial.GetFloat(P_TimeScale);
        float simTimeOffset = sineMaterial.GetFloat(P_SimTimeOffset);

        if (pixelFreqSlider != null) pixelFreqSlider.SetValueWithoutNotify(pixelFreq);
        if (pixelFreqInput != null) {
            pixelFreqInput.SetValueWithoutNotify(pixelFreq.ToString("F3"));
        }

        if (timeScaleSlider != null) timeScaleSlider.SetValueWithoutNotify(timeScale);
        if (timeScaleInput != null) {
            timeScaleInput.SetValueWithoutNotify(timeScale.ToString("F3"));
        }

        if (simTimeOffsetSlider != null) simTimeOffsetSlider.SetValueWithoutNotify(simTimeOffset);
        if (simTimeOffsetInput != null) {
            simTimeOffsetInput.SetValueWithoutNotify(simTimeOffset.ToString("F3"));
        }
    }

    private void OnPixelFreqSliderChanged(ChangeEvent<float> evt) {
        SetPixelFreq(evt.newValue);
        if (pixelFreqInput != null) pixelFreqInput.value = evt.newValue.ToString("F3");
    }

    private void OnPixelFreqInputChanged(ChangeEvent<string> evt) {
        if (float.TryParse(evt.newValue, out float result)) {
            result = Mathf.Clamp(result, pixelFreqMin, pixelFreqMax);
            SetPixelFreq(result);
            if (pixelFreqSlider != null) pixelFreqSlider.value = result;
        }
    }

    private void OnTimeScaleSliderChanged(ChangeEvent<float> evt) {
        SetTimeScale(evt.newValue);
        if (timeScaleInput != null) timeScaleInput.value = evt.newValue.ToString("F3");
    }

    private void OnTimeScaleInputChanged(ChangeEvent<string> evt) {
        if (float.TryParse(evt.newValue, out float result)) {
            result = Mathf.Clamp(result, timeScaleMin, timeScaleMax);
            SetTimeScale(result);
            if (timeScaleSlider != null) timeScaleSlider.value = result;
        }
    }

    private void OnSimTimeOffsetSliderChanged(ChangeEvent<float> evt) {
        SetSimTimeOffset(evt.newValue);
        if (simTimeOffsetInput != null) simTimeOffsetInput.value = evt.newValue.ToString("F3");
    }

    private void OnSimTimeOffsetInputChanged(ChangeEvent<string> evt) {
        if (float.TryParse(evt.newValue, out float result)) {
            result = Mathf.Clamp(result, simTimeOffsetMin, simTimeOffsetMax);
            SetSimTimeOffset(result);
            if (simTimeOffsetSlider != null) simTimeOffsetSlider.value = result;
        }
    }

    public void SetPixelFreq(float value) {
        if (sineMaterial != null) {
            sineMaterial.SetFloat(P_PixelFreq, value);
        }
    }

    public float GetPixelFreq() {
        return sineMaterial != null ? sineMaterial.GetFloat(P_PixelFreq) : 0f;
    }

    public void SetTimeScale(float value) {
        if (sineMaterial != null) {
            sineMaterial.SetFloat(P_TimeScale, value);
        }
    }

    public float GetTimeScale() {
        return sineMaterial != null ? sineMaterial.GetFloat(P_TimeScale) : 0f;
    }

    public void SetSimTimeOffset(float value) {
        if (sineMaterial != null) {
            sineMaterial.SetFloat(P_SimTimeOffset, value);
        }
    }

    public float GetSimTimeOffset() {
        return sineMaterial != null ? sineMaterial.GetFloat(P_SimTimeOffset) : 0f;
    }
}

