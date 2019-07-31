using System;

namespace QMapper.Model
{
    public class A
    {
        public string Name { get; set; } = "A";

        public int? Age { get; set; } = 9;

        public string Email { get; set; } = "@A";

        public string Version { get; set; } = "2.0";

        public Guid Guid { get; set; } = Guid.NewGuid();

        public string Uri { get; set; } = "http://www.mimimapper.com/";

        public int Gender { get; set; }

        public DateTime Time { get; set; } = DateTime.Now;
    }
}
