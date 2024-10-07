
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SpawnTargetScript : UdonSharpBehaviour
{
    [SerializeField] private GameObject geometry;
    public Material color;
    public Vector3 size;

    public void Spawn()
    {
        geometry.GetComponent<Renderer>().material = color;
        geometry.transform.localScale = size;
        geometry.transform.position = transform.position;
        geometry.transform.rotation = transform.rotation;

        Instantiate(geometry);
    }
}
