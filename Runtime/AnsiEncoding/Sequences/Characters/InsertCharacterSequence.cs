namespace HamerSoft.PuniTY.AnsiEncoding.Characters
{
    public class InsertCharacterSequence : Sequence
    {
        public override char Command => '@';
        private const char EmptyCharacter = ' ';

        public override void Execute(IScreen screen, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
                parameters = "1";
            
            if (int.TryParse(parameters, out var emptyChars))
                for (int i = 0; i < emptyChars; i++)
                    screen.AddCharacter(EmptyCharacter);
        }
    }
}