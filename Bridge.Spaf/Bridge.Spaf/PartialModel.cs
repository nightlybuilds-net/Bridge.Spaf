using System;
using System.Collections.Generic;
using Bridge.jQuery2;

namespace Bridge.Spaf
{
    public abstract class PartialModel : ViewModelBase, IViewModelLifeCycle
    {
        protected abstract string HtmlUrl { get; }

        /// <summary>
        /// Init partial
        /// </summary>
        /// <param name="parameters">data for init the partials</param>
        public virtual void Init(Dictionary<string,object> parameters)
        {
            jQuery.Select($"#{this.ElementId()}").Load(HtmlUrl, null, (o, s, arg3) =>
            {
                base.ApplyBindings();
            });
        }

        public virtual void DeInit()
        {
            base.RemoveBindings();
        }
    }

    public interface IViewModelLifeCycle
    {
        void Init(Dictionary<string, object> parameters);
        void DeInit();
    }
}



