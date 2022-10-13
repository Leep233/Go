using System;
using System.Collections.Generic;
using System.Text;

namespace Go.Core
{
    public struct LeelaCommand
    {
        public string id;

        public string command;

        public object arg;

        public LeelaCommand(string id,string command, object arg)
        {
            this.id = id;
            this.command = command;
            this.arg = arg;
        }

        public override string ToString()
        {
            return $"{id} {command} {arg ?? ""}";
        }
    }
}
