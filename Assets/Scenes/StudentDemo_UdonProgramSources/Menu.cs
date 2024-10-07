
using System.Linq;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
//using VRC.Udon.Editor.ProgramSources.UdonGraphProgram.UI.GraphView;

public class Menu : UdonSharpBehaviour
{
    [SerializeField] private Dropdown colorDropDown;
    [SerializeField] private Dropdown sizeDropDown;

    [SerializeField] private UdonBehaviour spawner;

    [SerializeField] private Material[] materials;
    [SerializeField] private Vector3[] sizes;


    public void ColorItemSelected() 
    {
        int index = colorDropDown.value;
        Debug.Log("Color changed to: " + colorDropDown.value);

        spawner.SetProgramVariable("color", materials[index]);
        
    }

    public void SizeItemSelected()
    {
        int index = sizeDropDown.value;
        Debug.Log("Size changed to: " + sizeDropDown.value);

        spawner.SetProgramVariable("size", sizes[index]);
    }
}
