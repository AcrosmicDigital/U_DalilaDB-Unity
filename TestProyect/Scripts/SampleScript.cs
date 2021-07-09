using UnityEngine;

public class SampleScript : MonoBehaviour
{

    public int lives;

    private void Awake()
    {
        // Find an int object with key "Lives"
        var readOpp = MainDdbElements.Find<int>("Lives");

        // If Lives key was find assign the Data to lives propertie 
        if (readOpp)
            lives = readOpp.Data;
        // If not found lives will be a default value
        else
            lives = 10;


        // Find a Vector 3 with the position, if cant find will return a Vector3.Zero
        var readOppPoss = MainDdbElements.FindOrDefault<Vector3>("Position", Vector3.zero);

        // Assign the position to GameObject transform
        transform.position = readOppPoss.Data;
    }

    private void OnDisable()
    {
        // Save the lives with "Lives" key
        var saveOpp = MainDdbElements.Save(lives, "Lives");

        // If save fail, print a log
        if (!saveOpp)
            Debug.LogError("Cant save data, Error: " + saveOpp.Error);


        // Save the possition
        MainDdbElements.Save(transform.position, "Position");
    }

    public void DeleteSavedData()
    {
        // Delete the key
        MainDdbElements.Delete("Position");
        MainDdbElements.Delete("Lives");
    }

    public void UpdateSavedData(int newLives, Vector3 newPosition)
    {
        // Update Lives
        var upp = MainDdbElements.Update<int>("Lives", (v) => { v = v + newLives; return v; });

        // Update position
        var uppPos = MainDdbElements.Update<Vector3>("Position", v => newPosition);
    }

}
