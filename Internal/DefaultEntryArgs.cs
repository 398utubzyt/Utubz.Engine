using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utubz.Internal
{
    internal class DefaultEntryArgs : IEntryArgs
    {
        public string[] Args { get; }

        internal DefaultEntryArgs(string[] args)
        {
            Args = args;
        }
    }
}
