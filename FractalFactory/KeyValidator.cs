namespace FractalFactory
{
    internal class KeyValidator
    {
        public static bool IsIntChar(char c)
        {
            if (IsEditingKey(c))
                return true;

            return char.IsDigit(c);
        }

        public static bool IsFloatingPointInput(char c)
        {
            if (IsEditingKey(c))
                return true;

            return (char.IsDigit(c) || (c == '.') || (c == '-')) && !char.IsControl(c);
        }

        public static bool IsEditingKey(char c)
        {
            return ((c == '\u0001')  // <ctrl> a
                || (c == '\u0003')   // <ctrl> c
                || (c == '\u0016')   // <ctrl> v
                || (c == '\u001a')   // <ctrl> z
                || (c == '\b'));     // backspace
        }
    }
}
