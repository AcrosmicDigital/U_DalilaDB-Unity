using UnityEngine;
using U.DalilaDB;
using System.Runtime.Serialization;

[KnownType(typeof(UserDdbVolatileCollection))]
[DataContract()]
public class UserDdbVolatileCollection : DalilaDBVolatileCollection<UserDdbVolatileCollection>
{
    protected override int cacheSize_ => 100;

    [DataMember()]
    public string username;

    [DataMember()]
    public int maxScore;

    [DataMember()]
    public float minTime;

    [DataMember()]
    public Vector3 lastPosition;

}
