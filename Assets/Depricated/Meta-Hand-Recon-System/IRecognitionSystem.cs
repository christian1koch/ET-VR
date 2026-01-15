using System;

public interface IRecognitionSystem
{
    event Action<int> OnRecognized;
}

