using UnityEngine;
using U.DalilaDB;

public class DalilaDBTest : MonoBehaviour
{
    public class DalilaElementsTest : DalilaDBElements<DalilaElementsTest> { protected override string rootPath_ => Application.persistentDataPath; }
    public class DalilaVolatileElementsTest : DalilaDBVolatileElements<DalilaVolatileElementsTest> { }

    private void Awake()
    {
        DalilaElementsTest.DeleteAll();
    }

    void Start()
    {

        Debug.Log("DalilaDB: Starting Test . Remove in final version");

        var opp = DalilaElementsTest.Save("Number", 77);
        Debug.Log("DalilaDBElements: Saved Int: " + opp);
        if (!opp) Debug.LogError("DalilaDBElements: Error saving: " + opp.Error);

        var fopp = DalilaElementsTest.Find<int>("Number");
        Debug.Log("DalilaDBElements: Found Int: " + fopp);
        if (!fopp) Debug.LogError("DalilaDBElements: Error saving: " + fopp.Error);

        var opp2 = DalilaElementsTest.Save("Number", 77);
        Debug.Log("DalilaDBVolatileElements: Saved Int: " + opp2);
        if (!opp2) Debug.LogError("DalilaDBElements: Error saving: " + opp2.Error);

        var fopp2 = DalilaElementsTest.Find<int>("Number");
        Debug.Log("DalilaDBVolatileElements: Found Int: " + fopp2);
        if (!fopp2) Debug.LogError("DalilaDBElements: Error saving: " + fopp2.Error);

        if(opp && fopp && opp2 && fopp2)
            Debug.Log("DalilaDB: Successful test");
        else
            Debug.Log("DalilaDB: Failed test");

    }

    private void OnDestroy()
    {
        DalilaElementsTest.DeleteAll();
    }

}
