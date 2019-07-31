using System;

namespace QMapper.Model
{
    public class UserInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int? Age { get; set; }

        public string Email { get; set; }

        public string Guid { get; set; }

        public string Uri { get; set; }

        public Gender Gender { get; set; }

        public DateTime Time { get; set; }
    }
}
