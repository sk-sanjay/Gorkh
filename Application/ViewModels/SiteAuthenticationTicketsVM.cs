﻿using System;

namespace Application.ViewModels
{
    public class SiteAuthenticationTicketsVM
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public byte[] Value { get; set; }
        public DateTimeOffset? LastActivity { get; set; }
        public DateTimeOffset? Expires { get; set; }
        public string RemoteIpAddress { get; set; }
        public string OperatingSystem { get; set; }
        public string UserAgentFamily { get; set; }
        public string UserAgentVersion { get; set; }
    }
}
