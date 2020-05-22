using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Signals
{
    /// <summary>
    /// Common interface for the all the views (there can be different types for views). It an interface and not a base class 
    /// as views usually derive from the UserControl base class and a sub class can have only one super class in .NET.
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Update the view based on the current state of the document.
        /// </summary>
        void Update();
        /// <summary>
        /// Gets the document the view is attached to.
        /// </summary>
        Document GetDocument();
        /// <summary>
        /// The ordinal number of the view. A .NET interface may also cantains properties and event
        /// that the implementing class must contain
        /// </summary>
        int ViewNumber { get; set; }
    }
}
