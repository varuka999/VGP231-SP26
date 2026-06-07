using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TMPRenderQueueSetter : MonoBehaviour
{
    [SerializeField] private int renderQueue = 4000;
    [SerializeField] private bool modifySharedMaterial = false;

    private void Awake()
    {
        TMP_Text text = GetComponent<TMP_Text>();

        if (modifySharedMaterial)
        {
            text.fontSharedMaterial.renderQueue = renderQueue;
        }
        else
        {
            text.fontMaterial.renderQueue = renderQueue;
        }
    }
}