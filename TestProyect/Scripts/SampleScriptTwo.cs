using UnityEngine;

public class SampleScriptTwo : MonoBehaviour
{

    // Properties
    private float time = 0;

    // DalilaDBDocument instance
    private GameDdbDocument match;

    private void Awake()
    {
        // Find a saved document or return a new one and assign to match
        var opp = GameDdbDocument.FindOrDefault(new GameDdbDocument());
        match = opp.Data;

        // If cant find a document just print a log
        if (!opp)
            Debug.Log("Cant find a saved match");

    }

    private void Update()
    {
        // Add the elapsed time
        time += Time.deltaTime;
    }

    public void ChangeLives(int v)
    {
        // Change the lives value, but dont save it
        match.lives -= v;
    }

    public void DeleteSavedData()
    {
        // Delete the document
        GameDdbDocument.Delete();
    }


    private void OnDisable()
    {
        // Assing  the seconds of the match to the document
        match.Time = time;

        // Save all data
        match?.Save();
    }

}
