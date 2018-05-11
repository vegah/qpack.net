# QPack for .net
A library for using qpack serializing/deserialzing data.  Based on:  
https://github.com/transceptor-technology/go-qpack

## Install
Nuget package coming soon  
  
## How to use
var bytes = QPack.Pack(2); // QPacks an number  
var bytes = QPack.Pack("string"); // QPacks a string  
var bytes = QPack.Pack(new int[]{10,20,30}); // QPacks an ienumerable  
  
var obj = QPack.Unpack(bytes); // Returns an object with the correct type.  

## Note
When unpacking:  
Dictionaries are always IDictionary<string,string>, even if the data "really" is integers.  This may change.  
IEnumerables are always IEnumerable<string>.  This may change.  
  
## Status
Not even beta.  Probably some stuff that isn't working.  