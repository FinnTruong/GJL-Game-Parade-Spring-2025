using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
[CustomEditor(typeof(CustomSlider))]
public class CustomSliderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CustomSlider slider = target as CustomSlider;
        if (DrawDefaultInspector())
        {
            slider.Refresh();
        }
    }
}
#endif


public class CustomSlider : MonoBehaviour
{
    [Range(0, 1)]
    public float value;

    [SerializeField] RectTransform fill;
    [SerializeField] float minAnchorY, maxAnchorY;

    public float Value
    {
        get => value;
        set
        {
            this.value = value;
            Refresh();
        }
    }

    [ExecuteAlways]
    public void Refresh()
    {
        fill.anchorMax = new Vector2(1, Mathf.Lerp(minAnchorY, maxAnchorY, value));
    }
}
