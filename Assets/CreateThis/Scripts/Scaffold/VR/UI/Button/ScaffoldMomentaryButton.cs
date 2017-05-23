using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CreateThis.Scaffold.VR.UI.Button {
    public class ScaffoldMomentaryButton : ScaffoldBase {
        public GameObject target;
        public string buttonText;
        public GameObject buttonBody;
        public Material material;
        public Material highlight;
        public Material outline;
        public AudioClip buttonClickDown;
        public AudioClip buttonClickUp;
        public TextAlignment alignment;
        public int fontSize;
        public Color fontColor;

        public override void Generate() {
            base.Generate();
        }
    }
}