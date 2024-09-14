using TMPro;
using UnityEngine;

public class InteractionPromtUI : MonoBehaviour
{
    private Camera _mainCam;
    [SerializeField] private GameObject __uiPanel;
    [SerializeField] private TextMeshProUGUI _promtText;

    private void Start()
    {
        _mainCam = Camera.main;
        __uiPanel.SetActive(false);
    }

    private void LateUpdate()
    {
        var rotation = _mainCam.transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
    }

    public bool IsDisplayed = false;

    public void Setup(string promtText)
    {
        _promtText.text = promtText;
        __uiPanel.SetActive(true);
        IsDisplayed = true;
    }

    public void Close()
    {
        IsDisplayed = false;
        __uiPanel.SetActive(false);
    }
}
