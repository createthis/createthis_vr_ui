namespace CreateThis.Factory.VR.UI.Button {
    public class MomentaryButtonFactory : ButtonBaseFactory {
        protected override ButtonBehavior buttonBehavior {
            get { return ButtonBehavior.Momentary; }
            set { }
        }
    }
}