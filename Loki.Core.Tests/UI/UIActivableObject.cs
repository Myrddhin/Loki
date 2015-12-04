using System;

using Loki.UI;

namespace Loki.Core.Tests.UI
{
    public class UIActivableObject : UIObject, IActivable
    {
        public event EventHandler<ActivationEventArgs> Activated;

        public bool IsActive { get; private set; }

        public void Activate()
        {
            IsActive = true;
        }
    }
}