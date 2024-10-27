using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.SearchService;
using UnityEngine;

public class SceneObj {
    public string imgPath;
    public string[] dialogues;

    public SceneObj(string imgPath, string[] dialogues) {
        this.imgPath = imgPath;
        this.dialogues = dialogues;
    }
}

public class SceneManager : MonoBehaviour {
    public GameObject backgroundGameObject;
    public SpriteRenderer backgroundSpriteRenderer;

    public SceneObj[] allScenes = new SceneObj[]{};
    void Start() {
        Debug.Log("-+ Starting scene manager +-");
        backgroundGameObject = GameObject.Find("Background");
        // TODO: Is there a better way to more immediately find this?
        foreach (Component comp in backgroundGameObject.GetComponents(typeof(SpriteRenderer))) {
            backgroundSpriteRenderer = (SpriteRenderer) comp;
            break;
        }

        Debug.Log("-+ Obtained sprite renderer. Initializing and loading scenes... -+");
        DirectoryInfo dir = new DirectoryInfo("Assets/Frames");
		FileInfo[] info = dir.GetFiles("*.jpg");
		foreach (FileInfo f in info) 
		{ 
            // Pull out the file path for SpriteRenderer
            string fileNameFull = f.FullName;

            // Pull out the file name base to get the associated text file of dialogues
            string fileNameBase = f.Name.Split(".")[1];
            Debug.Log(fileNameBase);

            // TODO: Do the same thing for the voice lines

            // SceneObj sceneObj = new SceneObj(f.FullName, );
            // allScenes.Append(new SceneObj);
		}


        Debug.Log("-+ Scene manager setup complete +-");
    }

    // Update is called once per frame
    void Update() {
        
    }
}
