namespace Editor.Compilador
{
    public class Token
    {
        public TipoToken Type { get; }
        public string Value { get; }

        public Token(TipoToken type, string value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Type}: {Value}";
        }
    }
}
