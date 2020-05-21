using System.Collections.Generic;

namespace Bridge.Navigation
{
    public interface IAmLoadable
    {
        /// <summary>
        /// Called when navigate to this controller
        /// </summary>
        /// <param name="parameters"></param>
        void OnLoad(Dictionary<string, object> parameters);

        /// <summary>
        /// Called when controller is leaved for another 
        /// </summary>
        void OnLeave();

        /// <summary>
        /// Called when html is loaded but ko is not binded
        /// </summary>
        /// <param name="parameters"></param>
        void OnBeforeBinding(Dictionary<string, object> parameters);

        /// <summary>
        /// Called when html is loaded and ko is binded 
        /// </summary>
        /// <param name="parameters"></param>
        void OnBindingDone(Dictionary<string, object> parameters);
    }
}