﻿namespace GasyTek.Lakana.WPF.Services
{
    /// <summary>
    /// Represents objects that can be queried for possiblity of closing.
    /// </summary>
    public interface ICloseable
    {
        bool CanClose();
    }
}
