using System;
using System.Diagnostics;
using Fixie;
using Thing.Tests.Integration.Utils;

namespace Thing.Tests.Integration
{
    internal class RespawnDbData : CaseBehavior
    {
        public void Execute(Case context, Action next)
        {
            Debug.WriteLine($"      Start Case {nameof(RespawnDbData)} " + context.Name);
            next();
            Debug.WriteLine($"      End Case {nameof(RespawnDbData)} " + context.Name);
        }
    }
}