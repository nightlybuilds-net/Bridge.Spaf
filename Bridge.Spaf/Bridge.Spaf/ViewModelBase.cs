using Retyped;

namespace Bridge.Spaf
{
    public abstract class ViewModelBase
    {
        private dom.HTMLElement _pageNode;

        /// <summary>
        /// Element id of the page 
        /// </summary>
        /// <returns></returns>
        protected abstract string ElementId();

        protected dom.HTMLElement PageNode => _pageNode ?? (this._pageNode = dom.document.getElementById(ElementId()));

        public void ApplyBindings()
        {
            knockout.ko.applyBindings(this, this.PageNode);
        }

        public void RemoveBindings()
        {
            knockout.ko.removeNode(this.PageNode);
        }
    }
}
