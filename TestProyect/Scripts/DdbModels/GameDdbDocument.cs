using UnityEngine;
using U.DalilaDB;
using System.Runtime.Serialization;

[KnownType(typeof(GameDdbDocument))]
[DataContract()]
public class GameDdbDocument : DalilaDBDocument<GameDdbDocument>
{
    protected override string rootPath_ => Application.persistentDataPath;

    [DataMember()]
    public int lives;

    [DataMember()]
    public Vector3 position;

    [DataMember()]
    public float Time;
}
