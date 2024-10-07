
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class BinDestroyObjs : UdonSharpBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var pickup = other.GetComponent<VRC_Pickup>();
        if (pickup != null) 
        {
            Debug.Log("!!!!!!!!!!!!! VRC PICKUP !!!!!!!!!!!!!!!!!!!");
            
            pickup.Drop();
            Destroy(other.gameObject);
            //Networking.Destroy(other.gameObject);
        }
    }
}
