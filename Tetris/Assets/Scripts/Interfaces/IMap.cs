using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMap
{
    Vector3Int Size { get; }
    void SetCamera(Camera cameraArg);
}
