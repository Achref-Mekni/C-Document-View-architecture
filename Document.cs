using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Signals
{
    /// <summary>
    /// The base class for all the concrete document types. In this example we have only one document type, 
    /// though it is straightforward to make this separation to support extensibility.
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Backing field for the Name property.
        /// </summary>
        private string name;

        /// <summary>
        /// The list of views that display this document.
        /// </summary>
        List<IView> views = new List<IView>();

        /// <summary>
        /// Backing field for the ViewCount property.
        /// </summary>
        private int viewCount;

        /// <summary>
        /// The name of the document.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// The number of views attached to this document.
        /// </summary>
        public int ViewCount
        {
            get { return viewCount; }
        }


        public Document(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Registers a view to the document.
        /// </summary>
        /// <param name="v"></param>
        public void AttachView(IView v)
        {
            views.Add(v);
            viewCount++;
            v.ViewNumber = viewCount;
            v.Update();
        }

        /// <summary>
        /// Unregisters a view from the document.
        /// </summary>
        /// <param name="v"></param>
        public void DetachView(IView v)
        {
            views.Remove(v);
            viewCount--;
        }

        public bool HasAnyView()
        {
            return views.Count > 0;
        }
        
        /// <summary>
        /// Updates all the views of the document.
        /// </summary>
        protected void UpdateAllViews()
        {
            foreach (IView view in views)
                view.Update();
        }

        /// <summary>
        /// Loads the data of the document. Can be overwridden in the derived class
        /// </summary>
        public virtual void LoadDocument(string filePath)
        {
          
        }

        /// <summary>
        /// Saves the data of the document. Can be overwridden in the derived class
        /// </summary>
        public virtual void SaveDocument(string filePath)
        { }
    }
}
