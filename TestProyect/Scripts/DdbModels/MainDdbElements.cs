using UnityEngine;
using U.DalilaDB;

public class MainDdbElements : DalilaDBElements<MainDdbElements>
{
    protected override string rootPath_ => Application.persistentDataPath;
}
