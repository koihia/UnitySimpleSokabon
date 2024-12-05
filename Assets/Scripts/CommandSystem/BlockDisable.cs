using System;

namespace Sokabon.CommandSystem
{
    public class BlockDisable : Command
    {
        private Block _block;
        
        public BlockDisable(Block block)
        {
            _block = block;
        }
        
        public override void Execute(Action onComplete)
        {
            _block.SetEnabled(false, false, onComplete);
            onComplete?.Invoke();
        }
        
        public override void Undo(Action onComplete)
        {
            _block.SetEnabled(true, true, onComplete);
            onComplete?.Invoke();
        }
    }
}