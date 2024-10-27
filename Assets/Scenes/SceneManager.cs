using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;

public class SceneObj {
    public string imgPath;
    public List<string> dialogues = new List<string>();
    private int currDialogueIdx;

    public SceneObj(string imgPath) {
        this.imgPath = imgPath;
        currDialogueIdx = 0;
    }

    public string NextDialogue() {
        currDialogueIdx += 1;
        if (currDialogueIdx >= dialogues.Count) {
            return null; // Indicates switch to next scene
        } else {
            return dialogues[currDialogueIdx];
        }
    }
}

public class SceneManager : MonoBehaviour {
    public GameObject backgroundGameObject;
    public SpriteRenderer backgroundSpriteRenderer;
    public TextMeshProUGUI dialogueText;

    public List<SceneObj> allScenes = new List<SceneObj>();
    public SceneObj currScene = null;
    public int currSceneIdx = 0;

    // Set up a scene to be the current scene by updating background image
    // Such as startup or when passed in on the last dialogue of a scene
    public void SetCurrentScene(SceneObj scene) {
        byte[] fileData = File.ReadAllBytes(scene.imgPath);
        Texture2D textureLoad = new Texture2D(2, 2);
        textureLoad.LoadImage(fileData);
        backgroundSpriteRenderer.sprite = Sprite.Create(textureLoad, new Rect(0, 0, textureLoad.width, textureLoad.height), new UnityEngine.Vector2(0.5f, 0.5f));

        // TODO: If this is scene 3, let's load the fishing minigame
        //       Always hold on to the last scene or whatever though

        currScene = scene;
        SetCurrentDialogue(currScene.dialogues[0]);
    }

    // Update the content of our text field with a particular dialogue
    public void SetCurrentDialogue(string currDialogue) {
        dialogueText.SetText(currDialogue);
    }

    void Start() {
        Debug.Log("-+ Starting scene manager +-");

        // Get the background sprite renderer
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
            // Pull out the file path for SpriteRenderer and initialize object
            string fileNameFull = f.FullName;
            SceneObj newScene = new SceneObj(fileNameFull);

            // Pull out the file name base to get the associated text file of dialogues
            string fileNameBase = f.Name.Split(".")[0];
            // TODO: Better way from directory object above? Or put these elsewhere for organization anyway?
            FileInfo dialogueText = new FileInfo("./Assets/Frames/" + fileNameBase + ".txt");
            StreamReader reader = dialogueText.OpenText();
            string text = null;
            while ((text = reader.ReadLine()) != null) {
                newScene.dialogues.Add(text);
            }

            // TODO: Do the same thing for the voice lines
            
            allScenes.Add(newScene);
		}
        Debug.Log("-+ Scene manager setup complete. First scene loading. +-");

        SceneObj firstScene = allScenes[0];
        SetCurrentScene(firstScene);

        Debug.Log("-+ Setup complete! Game should have started. +-");
    }

    // Update is called once per frame
    void Update() {
        // On space keypress, proceed to next scene element
        if (Input.GetKeyDown("space")) {
            string nextText = currScene.NextDialogue();
            if (nextText == null) {
                currSceneIdx += 1;
                if (currSceneIdx >= allScenes.Count) {
                    SetCurrentDialogue("GAME OVER.");
                } else {
                    SceneObj nextScene = allScenes[currSceneIdx];
                    SetCurrentScene(nextScene);
                }
            } else {
                SetCurrentDialogue(nextText);
            }
        }
    }
}
