using System;
using System.Collections.Generic;
using System.Text;

namespace Go.Core
{
    public interface ILeelaMessageHandler
    {
        void Handle(string message);
       
    }
}
