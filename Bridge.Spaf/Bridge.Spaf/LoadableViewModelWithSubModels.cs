using System.Collections.Generic;

namespace Bridge.Spaf
{
    public abstract class LoadableViewModelWithSubModels : LoadableViewModel
    {
        protected List<IViewModelLifeCycle> Partials { get; } = new List<IViewModelLifeCycle>();

        public override void OnLoad(Dictionary<string, object> parameters)
        {
            base.OnLoad(parameters);
            this.Partials?.ForEach(f=> f.Init());
        }

        public override void OnLeave()
        {
            base.OnLeave();
            this.Partials?.ForEach(f=>f.DeInit());
        }
    }
}