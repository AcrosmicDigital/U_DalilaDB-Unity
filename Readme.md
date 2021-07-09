<img src="https://images4public4ccess.s3.amazonaws.com/DalilaDB/DalilaDB-Logo.png" width="100">

# DalilaDB(Beta) for Unity

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
3. Inside Unity proyect in Assets Folder, create a new Folder named DalilaDB and copy
4. From unziped folder copy only
    - "U.DalilaDB.asmdef" file
    - "LICENSE" file
    - "/Editor" folder
    - "/Runtime" folder

![DalilaDBInstallation1](https://images4public4ccess.s3.amazonaws.com/DalilaDB/DalilaDBInstallation6.JPG)

5. If ok if this log appear in Unity console "Assembly for Assembly Definition File 'Assets/DalilaDB/U.DalilaDB.asmdef' will not be compiled, because it has no scripts associated with it."

![DalilaDBInstallation2](https://images4public4ccess.s3.amazonaws.com/DalilaDB/DalilaDBInstallation6.JPG)

6. Wait while Unity compiles the files 
7. Delete all logs in console
8. When finish "U" button must appear in Menu Bar

![DalilaDBInstallation3](https://images4public4ccess.s3.amazonaws.com/DalilaDB/DalilaDBInstallation6.JPG)

9. Create and open a new scene just for test purpouse
10. Inside this scene create a empty GameObject

![DalilaDBInstallation4](https://images4public4ccess.s3.amazonaws.com/DalilaDB/DalilaDBInstallation6.JPG)

11. To this GameObject add the component named "DalilaDBTest" located in Assets/DalilaDB/Editor

![DalilaDBInstallation5](https://images4public4ccess.s3.amazonaws.com/DalilaDB/DalilaDBInstallation5.JPG)

12. Play the scene and in console must appear a log saying "DalilaDB: Successful test", if you can see this log the installation was successfully

![DalilaDBInstallation6](https://images4public4ccess.s3.amazonaws.com/DalilaDB/DalilaDBInstallation6.JPG)

13. Now you can delete the Test GameObject and/or Test Scene
14. If something went wrong, please create a new issue with logs and Unity version

## Basic Usage

### Elements

### Document

### Collection
