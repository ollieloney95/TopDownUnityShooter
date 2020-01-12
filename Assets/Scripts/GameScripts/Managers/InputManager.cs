using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    // Use this for initialization
    
    public void Startup()
    {
        Debug.Log("Stst manager starting...");
        status = ManagerStatus.Started;
        InitializeDictionary();
    }

    static Dictionary<string, float> inputs;
    static string[] names = new string[4]
    {
        "horizontal_movement",
        "vertical_movement",
        "horizontal_shoot",
        "vertical_shoot"
    };
    static float[] values = new float[4]
    {
        0.2f,
        0f,
        0f,
        0f
    };
    private static void InitializeDictionary()
    {
        inputs = new Dictionary<string, float>();
        for (int i = 0; i < names.Length; ++i)
        {
            inputs.Add(names[i], values[i]);
        }
    }

    public float GetInput(string input_string)
    {
        return inputs[input_string];
    }
    public void SetInput(string input_string, float input_value)
    {
        inputs[input_string] = input_value;
        return;
    }

}
