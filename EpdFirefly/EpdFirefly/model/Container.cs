/*
 *Application name  : Elektronische patientendossier
 *Author            : Team firefly
*/
namespace ArrayContainer
{
    using System.Collections.Generic;
    
    /// <summary>
    /// Container
    /// </summary>
    public class Container
    {
        private static Container container;
        private Dictionary<string, dynamic> array = new Dictionary<string, dynamic>();

        /// <summary>
        /// sends instance if not already exists
        /// </summary>
        public static Container GetInstance
        {
            get
            {
                if (container == null) container = new Container();
                return container;
                
            }
        }

        /// <summary>
        /// Sends all items in existing array
        /// </summary>
        public dynamic GetAll()
        {
            return array;
        }


        /// <summary>
        /// Sets key value to array
        /// </summary>
        public void Set(string key, dynamic value)
        {
            array[key] = value;
        }


        /// <summary>
        /// gives back value by key
        /// </summary>
        public dynamic Get(string key, string optionalkey = null)
        {
            if (string.IsNullOrEmpty(optionalkey))
                return array[key];
            return array[key][optionalkey];
        }

        /// <summary>
        /// unsets key and value  
        /// </summary>
        public void Unset(string key)
        {
            array.Remove(key);
        }

        /// <summary>
        /// Destroys all keys and values
        /// </summary>
        public void Destroy()
        {
            array.Clear();
        }
    }
}