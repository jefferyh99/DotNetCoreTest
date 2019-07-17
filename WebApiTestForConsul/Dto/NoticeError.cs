using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTestForConsul.Dto
{
    public class NoticeError
    {
        public string Node { get; set; }
        public string CheckID { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public string Output { get; set; }
        public string ServiceID { get; set; }
        public string ServiceName { get; set; }
        public string[] ServiceTags { get; set; }
        public Definition Definition { get; set; }
        public int CreateIndex { get; set; }
        public int ModifyIndex { get; set; }
    }

    public class Definition
    {
        public string Interval { get; set; }
        public string Timeout { get; set; }
        public string DeregisterCriticalServiceAfter { get; set; }
        public string HTTP { get; set; }
        public object Header { get; set; }
        public string Method { get; set; }
        public bool TLSSkipVerify { get; set; }
        public string TCP { get; set; }
    }

}
