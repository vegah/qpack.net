using System.Collections.Generic;

namespace Fantasista
{
    internal class UnpackInformation
    {
        public UnpackInformation() 
        {
            Values = new List<object>();
        }
        public int Position {get;set;}
        public List<object> Values {get;set;} 
    }
}