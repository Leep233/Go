using Go.Interfaces;
using Go.Models;

namespace Go.Core
{
    public abstract class Player
    {

        public  IGOObserver<Stone> Judge { get; set; }
        public string Name { get; set; }

        public virtual bool Press(object arg) 
        { 
            return  Judge?.Press(arg as Stone)??false;
        }
        public virtual void Lose() { }

        public Player(IGOObserver<Stone> judge)
        {
            this.Judge = judge;
        }

        public Player()
        {

        }
    }
}