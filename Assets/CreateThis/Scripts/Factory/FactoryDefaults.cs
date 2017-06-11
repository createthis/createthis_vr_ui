using CreateThis.VR.UI;

namespace CreateThis.Factory {
    public static class FactoryDefaults {
        public static PanelProfile panel;

        private static T OverrideDefault<T>(T defaultProfile, T overrideProfile) {
            T result = defaultProfile;
            if (overrideProfile != null) result = overrideProfile;
            return result;
        }

        public static PanelProfile GetProfile(PanelProfile overrideProfile) {
            return OverrideDefault(panel, overrideProfile);
        }
    }
}