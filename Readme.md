<img src="https://images4public4ccess.s3.amazonaws.com/DalilaDB/DalilaDB-Logo.png" width="100">

# StormDB(Beta) for Unity

A document based NoSql database for use in local applications.

- Compatible with PC, Mac, Linux, Windows Universal, iOS, tvOS, Android, WebGL
- Serialize to .xml documentd using DataContractSerializer 
- Save any serializable class or struct
- Sync and Async functions to store in background thread and dont frezee app
- Relations between collections(Tables)
- Stores from simple values to complex classes
- Easy query system to Save, Find, Update and Delete

## Instalation

1. Create or open a Unity proyect
2. Download and unzip the repository
3. Inside Unity proyect in Assets Folder, create a new Folder named StormDB and copy
4. From unziped folder copy only
    - "U.Storm.asmdef" file
    - "LICENSE" file
    - "/Editor" folder
    - "/Runtime" folder

![StormInstallation](https://images4public4ccess.s3.amazonaws.com/DalilaDB/DalilaDBInstallation1.JPG)

5. If this log appear in Unity console just delete it "Assembly for Assembly Definition File 'Assets/StormDB/U.Storm.asmdef' will not be compiled, because it has no scripts associated with it."
6. If this log appear continue anyway "Assets\StormDB\Editor\SavedDataEditorWindow.cs(8,7): error CS0246: The type or namespace name 'U' could not be found (are you missing a using directive or an assembly reference?)"

![StormInstallation2](https://images4public4ccess.s3.amazonaws.com/DalilaDB/DalilaDBInstallation2.JPG)

7. Inside Assets/StormDB/Editor delete the file "U.Storm.Editor.asmdef"

![StormInstallation3](https://images4public4ccess.s3.amazonaws.com/DalilaDB/DalilaDBInstallation3.JPG)

8. Wait while Unity compiles the files and delete all logs in console
9. When finish "U" button appear in Menu Bar
10. If button "U" is there and any logs are in console the installation was successful

![StormInstallation4](https://images4public4ccess.s3.amazonaws.com/DalilaDB/DalilaDBInstallation4.JPG)


## Basic Usage

### Elements

### Document

### Collection