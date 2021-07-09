using UnityEngine;
using U.DalilaDB;
using System.Runtime.Serialization;

[KnownType(typeof(UserDdbCollection))]
[DataContract()]
public class UserDdbCollection : DalilaDBCollection<UserDdbCollection>
{
    protected override string rootPath_ => throw new System.NotImplementedException();

    [DataMember()]
    public string username;

    [DataMember()]
    public int maxScore;

    [DataMember()]
    public float minTime;

    [DataMember()]
    public Vector3 lastPosition;

}
