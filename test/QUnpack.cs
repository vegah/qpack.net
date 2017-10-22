using Fantasista;
using Xunit;

namespace test
{
    public class QUnpack
    {
        [Fact]
        public void Numbers()
        {
         Assert.Equal((int)-1,QPack.Unpack(new byte[]{64}));   
         Assert.Equal((sbyte)0,QPack.Unpack(new byte[]{0}));   
         Assert.Equal((sbyte)1,QPack.Unpack(new byte[]{1}));   
 
         Assert.Equal((int)-16702650,QPack.Unpack(new byte[]{234, 70, 35, 1, 255}));   
         Assert.Equal((long)-1094624909430,QPack.Unpack(new byte[]{235, 138, 103, 69, 35, 1, 255, 255, 255}));   

        }
    }
}