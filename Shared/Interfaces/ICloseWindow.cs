using System;

namespace Shared.Interfaces
{
    public interface IWindowOperations
    {
        Action<bool> WindowMaximized { get; set; }

        event Action Minimize;
        event Action Maximize;
        event Action Restore;
        event Action Close;
    }
}