namespace Sokabon.CommandSystem
{
    public class PlayerNoOp : Command
    {
        public PlayerNoOp()
        {
            IsPlayerInput = true;
        }
    }
}