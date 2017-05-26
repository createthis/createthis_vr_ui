namespace CreateThis.Factory.VR.UI {
    public class KeyboardKey : BaseFactory {
        protected static Key K(string value) {
            return new Key(KeyType.Key, value);
        }

        protected static Key W(string value) {
            return Key.Wide(value);
        }

        protected struct Key {
            public KeyType type;
            public string value;
            public bool on;
            public float width;

            public Key(KeyType type, string value, bool on) {
                this.type = type;
                this.value = value;
                this.on = on;
                width = -1;
            }

            public Key(KeyType type, string value) {
                this.type = type;
                this.value = value;
                on = false;
                width = -1;
            }

            public Key(KeyType type, float width) {
                this.type = type;
                this.value = null;
                on = false;
                this.width = width;
            }

            public Key(KeyType type, string value, float width) {
                this.type = type;
                this.value = value;
                on = false;
                this.width = width;
            }

            public Key(string value) {
                type = KeyType.Key;
                this.value = value;
                on = false;
                width = -1;
            }

            public static Key Wide(string value) {
                return new Key(KeyType.Wide, value);
            }

            public static Key ShiftLock(string value, bool on) {
                return new Key(KeyType.ShiftLock, value, on);
            }

            public static Key NumLock(string value) {
                return new Key(KeyType.NumLock, value);
            }

            public static Key ABC(string value) {
                return new Key(KeyType.ABC, value);
            }

            public static Key Return(string value) {
                return new Key(KeyType.Return, value);
            }

            public static Key Symbol(string value) {
                return new Key(KeyType.Symbol, value);
            }

            public static Key Spacer(float width) {
                return new Key(KeyType.Spacer, width);
            }

            public static Key Space(string value) {
                return new Key(KeyType.Space, value);
            }

            public static Key Backspace(string value) {
                return new Key(KeyType.Backspace, value);
            }

            public static Key Done(string value) {
                return new Key(KeyType.Done, value);
            }
        }

        protected enum KeyType {
            Key,
            Wide,
            ShiftLock,
            NumLock,
            Return,
            Symbol,
            Spacer,
            Space,
            ABC,
            Done,
            Backspace
        }
    }
}