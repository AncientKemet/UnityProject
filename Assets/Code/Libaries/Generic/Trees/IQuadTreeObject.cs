using OldBlood.Code.Libaries.Generic.Trees;
using UnityEngine;
using System.Collections;
using System;

public interface IQuadTreeObject
{
    QuadTree CurrentBranch { get; set; }

    Vector2 GetPosition();
    Vector2 PositionChange();
}