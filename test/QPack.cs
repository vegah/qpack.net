using System;
using System.Collections.Generic;
using Fantasista;
using Xunit;

namespace test
{
    public class UnitTest1
    {
        [Fact]
        public void MinusOneShouldBe()
        {
            var pack = new QPack();
            Assert.Equal(new byte[]{64},pack.Pack(-1));
        }

        [Fact]
        public void Minus60ShouldBe123()
        {
            var pack = new QPack();
            Assert.Equal(new byte[]{123},pack.Pack(-60));
        }

        [Fact]
        public void Minus0xfeShouldBeAGivenArray()
        {
            var pack = new QPack();
            Assert.Equal(new byte[]{233,2,255},pack.Pack(-0xfe));
        }

        [Fact]
        public void SeveralMoreNegativeNumbers()
        {
            var pack = new QPack();
            Assert.Equal(new byte[]{234, 70, 35, 1, 255},pack.Pack(-0xfedcba));
            Assert.Equal(new byte[]{234, 70, 35, 1, 255},pack.Pack(-0xfedcba));
            Assert.Equal(new byte[]{235, 138, 103, 69, 35, 1, 255, 255, 255},pack.Pack(-0xfedcba9876));
        }

        [Fact]
        public void SeveralPositiveNumbers()
        {
            var pack = new QPack();
            Assert.Equal(new byte[]{232, 120},pack.Pack(120));
            Assert.Equal(new byte[]{233, 254, 0},pack.Pack(0xfe));
            Assert.Equal(new byte[]{234, 186, 220, 254, 0},pack.Pack(0xfedcba));
            Assert.Equal(new byte[]{235, 118, 152, 186, 220, 254, 0, 0, 0},pack.Pack(0xfedcba9876));
        }

        [Fact]
        public void SeveralFloats()
        {
            var pack = new QPack();
            Assert.Equal(new byte[]{236, 135, 136, 155, 83, 201, 192, 243, 191},pack.Pack(-1.234567));
            Assert.Equal(new byte[]{236, 83, 5, 163, 146, 58, 221, 94, 64},pack.Pack(123.4567));
            
        }

        [Fact]
        public void SeveralStrings()
        {
            var pack = new QPack();
            Assert.Equal(new byte[]{140, 239, 163, 159, 32, 72, 105, 32, 81, 112, 97, 99, 107},pack.Pack(" Hi Qpack"));
        }

        [Fact]
        public void SeveralArrays()
        {
            var pack = new QPack();
            Assert.Equal(new byte[]{240, 126, 236, 154, 153, 153, 153, 153, 153, 241, 63,
			236, 154, 153, 153, 153, 153, 153, 1, 64},pack.Pack(new double[]{0.0, 1.1, 2.2}));
            Assert.Equal(new byte[]{242, 10, 20, 30, 40, 50},pack.Pack(new int[]{10, 20, 30, 40, 50}));
            Assert.Equal(new byte[]{252, 10, 20, 30, 40, 50, 60, 254},pack.Pack(new int[]{10, 20, 30, 40, 50, 60}));
            
        }

        [Fact]
        public void ComplexMaps()
        {
            var pack = new QPack();
            var map = new Dictionary<String, string[]>();
            map.Add("Names", new []{"Iris","Sasha"});
            Assert.Equal(new byte[]{239, 0, 244, 133, 78, 97, 109, 101, 115, 239, 132, 73, 114, 105,
			115, 133, 83, 97, 115, 104, 97},pack.Pack(new object[]{0,map}));
            
            
        }
        
    }
}
