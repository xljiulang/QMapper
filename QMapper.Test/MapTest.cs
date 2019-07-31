using QMapper.Model;
using Xunit;

namespace QMapper.Test
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
    }
}
