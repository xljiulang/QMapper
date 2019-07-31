using System;
using Xunit;

namespace MiniMapper.Test
{
    public class MapTest
    {
        [Fact]
        public void Map()
        {
            var a = new A();
            var b = a.AsMap().To<B>();

            Assert.Equal(a.Age, b.Age);
            Assert.Equal(a.Email, b.Email);
            Assert.Equal(a.Name, b.Name);

            Assert.Equal(a.Version, b.Version.ToString());
            Assert.Equal(a.Guid.ToString(), b.Guid.ToString());
            Assert.Equal(a.Uri.ToString(), b.Uri.ToString());

            Assert.Equal(a.Gender, (int)b.Gender);
            Assert.Equal(a.Time.ToString(), b.Time.ToLocalTime().DateTime.ToString());
        }


        [Fact]
        public void MapIgnore()
        {
            var a = new A();
            var b = a.AsMap().Ignore(i => i.Name).To<B>();

            Assert.Equal(a.Age, b.Age);
            Assert.Equal(a.Email, b.Email);
            Assert.NotEqual(a.Name, b.Name);
        }

        class A
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


        public enum Gender
        {
            男 = 0,
            女 = 1
        }


        class B
        {
            public string Name { get; set; }

            public int Age { get; set; }

            public string Email { get; set; }

            public Version Version { get; set; }

            public string Guid { get; set; }

            public Uri Uri { get; set; }

            public Gender Gender { get; set; }

            public DateTimeOffset Time { get; set; }
        }
    }
}
