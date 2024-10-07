
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MirrorSystem : UdonSharpBehaviour
{
    [SerializeField] private GameObject toggleObject;

    public void Toggle() 
    {
        if (toggleObject.activeSelf)
        {
            toggleObject.SetActive(false);
        } 
        else 
        { 
            toggleObject.SetActive(true);
        }  
    }
}
