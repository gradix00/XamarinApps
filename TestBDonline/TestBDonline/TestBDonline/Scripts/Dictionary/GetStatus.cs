using System;
using System.Collections.Generic;
using TestBDonline.Scripts.Structs;

namespace TestBDonline.Scripts.Dictionary
{
    internal class GetStatus
    {
        public Dictionary<string, Status> status = new Dictionary<string, Status>
        {
            {"Admin", Status.admin },
            {"User", Status.user },
            {"Banned", Status.banned }
        };
    }
}
