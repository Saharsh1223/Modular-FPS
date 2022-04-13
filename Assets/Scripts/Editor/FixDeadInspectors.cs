/// <summary>
/// This toggles the inspector mode whenever a new scene object is selected.
/// This can be used to partially restore inspector windows to a working state after
/// random Unity editor glitching has occured and all inspector windows are unresponsive.
/// It's important that FixDeadInspector window be visible somewhere in the Editor for this to work!
/// </summary>
#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
public class FixDeadInspectors : EditorWindow
{
    [MenuItem("Glitches/Fix Dead Inspectors")]
    static void Init()
    {
        FixDeadInspectors window = (FixDeadInspectors)GetWindow(typeof(FixDeadInspectors));
        window.minSize = Vector2.zero;
        window.Show();
    }
 
    private int lastComponentCount;
    private GameObject lastActiveGameObject;
    void OnInspectorUpdate() //whenever the user adds or removes a component to the active object
    {
        if (lastActiveGameObject == Selection.activeGameObject)
            if (Selection.activeGameObject != null)
            {
                var componentCount = Selection.activeGameObject.GetComponents<Component>().Length;
                if (componentCount != lastComponentCount)
                {
                    lastComponentCount = componentCount;
                    ToggleInspectorMode();
                }
            }
    }
 
    void OnSelectionChange() //whenever a scene object is select refresh the inspector
    {
        lastActiveGameObject = Selection.activeGameObject;
        if (lastActiveGameObject == Selection.activeGameObject)
            if (Selection.activeGameObject != null)
                lastComponentCount = Selection.activeGameObject.GetComponents<Component>().Length;
            else
                lastComponentCount = -1;
        ToggleInspectorMode();
    }
   
    void ToggleInspectorMode()
    {
        EditorWindow[] allWindows = Resources.FindObjectsOfTypeAll<EditorWindow>();
        foreach (var targetInspector in allWindows) //for all windows in the editor
            if (targetInspector != null  && targetInspector.GetType().Name == "InspectorWindow") //only for inspector windows
            {
                Type type = Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.InspectorWindow"); //Get the type of the inspector window to find out the variable/method from
                FieldInfo field = type.GetField("m_InspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);    //get the field we want to read, for the type (not our instance)
 
                for (int i = 0; i < 2; i++) //toggle the InspectorMode
                {
                    InspectorMode mode = (InspectorMode)field.GetValue(targetInspector); //read the value for our target inspector
                    mode = (mode == InspectorMode.Normal ? InspectorMode.Debug : InspectorMode.Normal); //toggle the value
                    MethodInfo method = type.GetMethod("SetMode", BindingFlags.NonPublic | BindingFlags.Instance); //Find the method to change the mode for the type
                    method.Invoke(targetInspector, new object[] {mode}); //Call the function on our targetInspector, with the new mode as an object[]
                }
                targetInspector.Repaint(); //refresh inspector
            }
    }
}
#endif