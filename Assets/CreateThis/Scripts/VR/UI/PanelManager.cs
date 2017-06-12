using UnityEngine;
using System.Collections.Generic;
using CreateThis.VR.UI.Panel;

namespace CreateThis.VR.UI {
    public static class PanelManager {
        private static List<PanelBase> panels;
        private static bool hasInitialized;

        public static void HideAllPanels(PanelBase except) {
            Initialize();

            foreach (PanelBase panel in panels) {
                if (panel && panel != except) panel.SetVisible(false);
            }
        }

        public static void AddPanel(PanelBase panel) {
            Initialize();

            panels.Add(panel);
        }

        private static void Initialize() {
            if (hasInitialized) return;
            panels = new List<PanelBase>();
            hasInitialized = true;
        }
    }
}