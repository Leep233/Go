namespace Go.Core
{
    public abstract class Player
    {
        public string Name { get; set; }


        public virtual bool Move(object arg) { return false; }
        public virtual void Lose() { }
    }
}