using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using TMPro;
using UnityEngine;
public class SceneObj {
    public string imgPath;
    public List<string> dialogues = new List<string>();
    private int currDialogueIdx;
    public bool hasDialogueEnded = false;
    public bool hasEvent = false;

    public SceneObj(string imgPath) {
        this.imgPath = imgPath;
        currDialogueIdx = 0;
    }

    public string NextDialogue() {
        currDialogueIdx += 1;
        if (currDialogueIdx >= dialogues.Count) {
            hasDialogueEnded = true;
            return null; // Indicates switch to next scene
        } else {
            return dialogues[currDialogueIdx];
        }
    }
}

public class PageManager : MonoBehaviour {
    public GameObject minigameOverlay;
    public GameObject backgroundGameObject;
    public SpriteRenderer backgroundSpriteRenderer;
    public TextMeshProUGUI dialogueText;
    public SpriteRenderer dialogueBg;

    public List<SceneObj> allScenes = new List<SceneObj>();
    public SceneObj currScene = null;
    public int currSceneIdx = 0;


    // Set up a scene to be the current scene by updating background image
    // Such as startup or when passed in on the last dialogue of a scene
    public void SetCurrentScene(SceneObj scene) {
        Texture2D img = Resources.Load<Texture2D>("Frames/" + scene.imgPath);
        // byte[] fileData = File.ReadAllBytes(scene.imgPath);
        // Texture2D textureLoad = new Texture2D(2, 2);
        // textureLoad.LoadImage(fileData);
        backgroundSpriteRenderer.sprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new UnityEngine.Vector2(0.5f, 0.5f));

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
            backgroundSpriteRenderer = (SpriteRenderer)comp;
            break;
        }

        Debug.Log("-+ Obtained sprite renderer. Initializing and loading scenes... -+");

        string[] fileNames = { "ballad_1", "ballad_2", "ballad_3", "ballad_4" };
        foreach (string fName in fileNames) {
            // Pull out the file path for SpriteRenderer and initialize object
            SceneObj newScene = new SceneObj(fName);

            // Pull out the associated text file of dialogues
            // TODO: Better way from directory object above? Or put these elsewhere for organization anyway?

            string textFile = Resources.Load<TextAsset>("Frames/" + fName).text;
            foreach (string line in textFile.Split("\n")) {
                if (line != "") newScene.dialogues.Add(line); // Skip the last empty newline. Classic.
            }

            // TODO: Do the same thing for the voice lines

            // NOTE: This is just for the demo purposes, but we'd need some other flag for scenes that trigger actions
            // Many solutions to do that come to mind, but later (if ever)
            if (fName == "ballad_3") {
                newScene.hasEvent = true;
            }
            allScenes.Add(newScene);
        }
        Debug.Log("-+ Scene manager setup complete. First scene loading. +-");

        SceneObj firstScene = allScenes[0];
        SetCurrentScene(firstScene);

        Cursor.visible = false;
        Debug.Log("-+ Setup complete! Game should have started. +-");
    }

    // Update is called once per frame
    void Update() {
        // Check if the page we are on has an event ongoing. If not, we continue VN style
        // Kind of cheesing this because I'm just going to load in other elements now
        if (currScene.hasEvent && currScene.hasDialogueEnded) {
            // Do nothing :) Let the other loaded object/minigame handle things and prevent below continuations
        } else {
            // On space keypress, proceed to next page/dialogue
            if (Input.GetKeyDown("space") || Input.GetMouseButtonDown(0)) {
                string nextText = currScene.NextDialogue();
                if (nextText == null) {
                    currSceneIdx += 1;
                    if (currSceneIdx >= allScenes.Count) {
                        SetCurrentDialogue("GAME OVER.\nClick exit to exit.");
                        Cursor.visible = true;
                    } else {
                        // Check if we have an event to trigger
                        if (currScene.hasEvent) {
                            // TODO: Trigger scene switch
                            Debug.Log("Triggering scene switch!");
                            InitFishingMinigame();

                            /* And then once that is done, e.g. a soul is obtained, we trigger the next scene stuff as below
                            * Assumptions:
                            * - The next scene will not also have an event
                            * - There will be a next scene definitively
                            */
                            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                        } else {
                            SceneObj nextScene = allScenes[currSceneIdx];
                            SetCurrentScene(nextScene);
                        }

                    }
                } else {
                    SetCurrentDialogue(nextText);
                }
            }
        }
    }

    // Cheesy way of loading/unloading necessary elements for the fishing minigame
    // Things aren't set up to transfer across actual Scenes but that should be done in the future!
    public void InitFishingMinigame() {
        dialogueBg.enabled = false;
        dialogueText.enabled = false;
        minigameOverlay = Instantiate(minigameOverlay);
    }

    public void FinFishingMinigame() {
        Destroy(minigameOverlay);
        dialogueBg.enabled = true;
        dialogueText.enabled = true;
        SceneObj nextScene = allScenes[currSceneIdx];
        SetCurrentScene(nextScene);
    }
}
