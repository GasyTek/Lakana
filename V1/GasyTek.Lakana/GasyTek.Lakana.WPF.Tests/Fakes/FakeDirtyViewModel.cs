﻿using GasyTek.Lakana.WPF.Services;

namespace GasyTek.Lakana.WPF.Tests.Fakes
{
    public class FakeDirtyViewModel : ICloseable
    {
        public bool CanClose()
        {
            return false;
        }
    }
}
