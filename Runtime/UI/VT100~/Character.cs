namespace HamerSoft.PuniTY.UI.VT100
{
    public class Character
    {
        internal char Char { get; set; }
        internal GraphicAttributes Attributes { get; set; }

        public Character()
            : this(' ')
        {
        }

        public Character(char _char)
        {
            Char = _char;
            Attributes = new GraphicAttributes();
        }
    }
}