
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace QvPen.UdonScript.UI
{
    public class UpdateLineWidth : UdonSharpBehaviour
    {
        [SerializeField]
        private QvPen_PenManager manager;

        [SerializeField]
        private Slider slider;

        public void ChangeLineWidth() 
        {
            manager._SetWidth(slider.value);
        }
    }

}

