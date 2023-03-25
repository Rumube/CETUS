using UnityEngine;
using UnityEditor;

public class Randomiser : EditorWindow
{

    bool randomXrot, randomYrot, randomZrot;
    bool randomScale;
    float minScale, maxScale;

    [MenuItem("CETOOLS/Randomiser")]
    static void Init()
    {
        Randomiser window = (Randomiser)GetWindow(typeof(Randomiser));
    }

    private void OnGUI()
    {
        GUILayout.Label("Randomise selected objects", EditorStyles.boldLabel);
        GUILayout.Space(10);

        GUILayout.Label("Rotations");
        randomXrot = EditorGUILayout.Toggle("Randomise X Rotation", randomXrot);
        randomYrot = EditorGUILayout.Toggle("Randomise Y Rotation", randomYrot);
        randomZrot = EditorGUILayout.Toggle("Randomise Z Rotation", randomZrot);

        GUILayout.Label("Scaling");
        randomScale = EditorGUILayout.Toggle("Randomise Scale", randomScale);
        minScale = EditorGUILayout.FloatField("Min Scale", minScale);
        maxScale = EditorGUILayout.FloatField("Max Scale", maxScale);

        if (GUILayout.Button("Randomise"))
        {
            foreach (GameObject go in Selection.gameObjects)
            {
                go.transform.rotation = Quaternion.Euler(GetRandomRotation(go.transform.rotation.eulerAngles));

                if(randomScale)
                {
                    float scaleVal = Random.Range(minScale, maxScale);
                    go.transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
                }
            }
        }
    }

    private Vector3 GetRandomRotation(Vector3 currentRotation)
    {
        float x = randomXrot ? Random.Range(0f, 360f) : currentRotation.x;
        float y = randomYrot ? Random.Range(0f, 360f) : currentRotation.y;
        float z = randomZrot ? Random.Range(0f, 360f) : currentRotation.z;

        return new Vector3(x, y, z);
    }
}
