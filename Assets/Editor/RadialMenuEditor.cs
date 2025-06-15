using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(RadialMenu))]
public class RadialMenuEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RadialMenu menu = (RadialMenu)target;
        if (DrawDefaultInspector())
        {
            menu.Refresh();
        }
    }
}