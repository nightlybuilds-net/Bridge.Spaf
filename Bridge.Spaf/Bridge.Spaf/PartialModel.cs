using System;
using Bridge.jQuery2;

namespace Bridge.Spaf
{
    public abstract class PartialModel : ViewModelBase, IViewModelLifeCycle
    {
        protected abstract string HtmlUrl { get; }

        public void Init()
        {
            jQuery.Select($"#{this.ElementId()}").Load(HtmlUrl, null, (o, s, arg3) =>
            {
                base.ApplyBindings();
            });
        }

        public void DeInit()
        {
            base.RemoveBindings();
        }
    }

    public interface IViewModelLifeCycle
    {
        void Init();
        void DeInit();
    }
}



