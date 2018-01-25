using System.Collections.Generic;
using Bridge.Navigation;

namespace Bridge.Spaf
{
    public abstract class LoadableViewModel : ViewModelBase, IAmLoadable
    {
        public virtual void OnLoad(Dictionary<string, object> parameters)
        {
            base.ApplyBindings();
        }

        public virtual void OnLeave()
        {
            base.RemoveBindings();
        }
    }
}