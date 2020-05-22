using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Signals
{
    /// <summary>
    /// A demo view implementation. It is a UserControl and also implements the IView interface.
    /// </summary>
    public partial class DemoView : UserControl,IView
    {
        /// <summary>
        /// Backing field for the ViewNumber property.
        /// </summary>
        private int viewNumber;

        /// <summary>
        /// The ordinal number of the view.
        /// </summary>
        public int ViewNumber
        {
            get { return viewNumber; }
            set { viewNumber = value; }
        }

        public DemoView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The document the view is attached to.
        /// TODO: modify its type to the type of the concrete document.
        /// </summary>
        private Document document;

        public DemoView(Document document)
        {
            InitializeComponent();
            this.document = document;
        }

        /// <summary>
        /// Implements the Update method of the IView interface.
        /// </summary>
        public void Update()
        {
            Invalidate();
        }

        public Document GetDocument()
        {
            return document;
        }

        /// <summary>
        /// Overrides the UserControl.OnPaint. Implement the drawing here.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
    }
}
