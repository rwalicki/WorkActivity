using System;

namespace Shared.Interfaces
{
    public interface ICloseWindow
    {
        event Action Close;
    }
}