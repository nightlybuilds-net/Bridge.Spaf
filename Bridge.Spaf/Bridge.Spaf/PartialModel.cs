using System;
using Bridge.jQuery2;

namespace Bridge.Spaf
{
    public abstract class PartialModel : ViewModelBase, IViewModelLifeCycle
    {
        protected abstract string HtmlUrl { get; }

        protected virtual Action OnPostInit { get; } = null;
        protected virtual Action OnPreDeInit { get; } = null;


        public void Init()
        {
            jQuery.Select($"#{this.ElementId()}").Load(HtmlUrl, null, (o, s, arg3) =>
            {
                base.ApplyBindings();
            });
            
            OnPostInit?.Invoke();
        }

        public void DeInit()
        {
            OnPreDeInit?.Invoke();
            base.RemoveBindings();
        }
    }

    public interface IViewModelLifeCycle
    {
        void Init();
        void DeInit();
    }
}



