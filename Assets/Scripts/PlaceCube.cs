//Code refrenced from Youtube: Board to Bits
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceCubCommand : ICommand
{
    Vector3 position;
    GameObject gameObject;
    Transform cube;

    public LevelEditorScript levelEditor;

    public PlaceCubCommand(Vector3 position, GameObject gameObject, Transform cube)
    {
        this.position = position;
        this.gameObject = gameObject;
        this.cube = cube;
    }
    public void Execute()
    {
        levelEditor.objectSpawner(gameObject);
    }

    public void Undo()
    {
        levelEditor.deleteObject(gameObject);
    }
}
