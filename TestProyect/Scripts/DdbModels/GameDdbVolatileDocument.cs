using UnityEngine;
using U.DalilaDB;
using System.Runtime.Serialization;

[KnownType(typeof(GameDdbVolatileDocument))]
[DataContract()]
public class GameDdbVolatileDocument : DalilaDBVolatileDocument<GameDdbVolatileDocument>
{
    [DataMember()]
    public int lives;

    [DataMember()]
    public Vector3 position;

    [DataMember()]
    public float Time;
}
