using System.Collections;
using Fantasista;
using Xunit;

namespace test
{
    public class QUnpack
    {
        [Fact]
        public void Integers()
        {
         Assert.Equal((int)-1,QPack.Unpack(new byte[]{64}));   
         Assert.Equal((sbyte)0,QPack.Unpack(new byte[]{0}));   
         Assert.Equal((sbyte)1,QPack.Unpack(new byte[]{1}));   
 
         Assert.Equal((int)-16702650,QPack.Unpack(new byte[]{234, 70, 35, 1, 255}));   
         Assert.Equal((long)-1094624909430,QPack.Unpack(new byte[]{235, 138, 103, 69, 35, 1, 255, 255, 255}));   
        }

        [Fact]
        public void Floats()
        {
            Assert.Equal((double)123.4567, QPack.Unpack(new byte[]{236, 83, 5, 163, 146, 58, 221, 94, 64}));
            Assert.Equal((double)-1.234567, QPack.Unpack(new byte[]{236, 135, 136, 155, 83, 201, 192, 243, 191}));
        }

        [Fact]
        public void Strings()
        {
            Assert.Equal(" Hi Qpack", QPack.Unpack(new byte[]{140, 239, 163, 159, 32, 72, 105, 32, 81, 112, 97, 99, 107}));
        }

        [Fact]
        public void Maps()
        {
            var map = QPack.Unpack(new byte[]{244, 134, 109, 121, 110, 97, 109, 101, 132, 73, 114, 105, 115}) as IDictionary;
            Assert.NotNull(map);
            Assert.Equal(1,map.Count);
            Assert.Equal(true,map.Contains("myname"));
            Assert.Equal("Iris",map["myname"]);
        }

        [Fact]
        public void LongArrays()
        {
            var array = QPack.Unpack(new byte[]{252, 10, 20, 30, 40, 50, 60, 254}) as IList;
            Assert.NotNull(array);
            Assert.Equal(6,array.Count);
            for (var i=0;i<6;i++)
            {
                Assert.Equal(((i+1)*10).ToString(),array[i]);
            }
        }

        [Fact]
        public void ShortArrays()
        {
            var array = QPack.Unpack(new byte[]{242, 10, 20, 30, 40, 50}) as IList;
            Assert.NotNull(array);
            Assert.Equal(5,array.Count);
            for (var i=0;i<5;i++)
            {
                Assert.Equal(((i+1)*10).ToString(),array[i]);
            }
        }


        [Fact]
        public void SpecialCase1()
        {
            Assert.Equal((object)null, QPack.Unpack(new byte[]{251}));
            
        }

    }
}