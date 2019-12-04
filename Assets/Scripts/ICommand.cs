//Code refrenced from Youtube: Board to Bits
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    void Execute();
    void Undo();
}
