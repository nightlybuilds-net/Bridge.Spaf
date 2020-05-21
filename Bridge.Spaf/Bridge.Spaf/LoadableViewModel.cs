using System.Collections.Generic;
using Bridge.Navigation;

namespace Bridge.Spaf
{
    public abstract class LoadableViewModel : ViewModelBase, IAmLoadable
    {
        protected List<IViewModelLifeCycle> Partials { get; } = new List<IViewModelLifeCycle>();

        public virtual void OnLoad(Dictionary<string, object> parameters)
        {
            base.ApplyBindings();
            this.Partials?.ForEach(f=> f.Init(parameters));
        }

        public virtual void OnLeave()
        {
            this.Partials?.ForEach(f=>f.DeInit());
            base.RemoveBindings();
        }

        public virtual void OnBeforeBinding(Dictionary<string, object> parameters)
        {
        }

        public virtual void OnBindingDone(Dictionary<string, object> parameters)
        {
        }
    }
}