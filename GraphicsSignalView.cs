using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Signals
{
    public partial class GraphicsSignalView : UserControl,IView
    {

        private  double zoomFactor = 1; // Zoom factor variable
        private int PixelPerSec = 150000; // Pixel per Sec variable to adjust the X axis parameters so they fit in the View 
        private int PixelPerValue=5;    // Pixel per value variable to adjust the Y axis parameters so they fit in the View 
        private int viewNumber;
        public int ViewNumber
        {
            get { return viewNumber; }
            set { viewNumber = value; }
        }
        public GraphicsSignalView()
        {
            InitializeComponent();
        }
        

        private SignalDocument document;

  
        public GraphicsSignalView(SignalDocument document)
        {
            InitializeComponent();
            this.document = document;
        }

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
        protected override void OnPaint(PaintEventArgs e) // after each invalidate usage the onPaint is called again
        {
            base.OnPaint(e);
            Pen bluePen = new Pen(Color.Blue, 3); // blue Pen to draw the Axes
            Pen redPen = new Pen(Color.Red, 1); // Red Pen to Link the points 
            e.Graphics.DrawLine(bluePen, 0, 0,0, ClientSize.Height ); // drawing the Y axis 
            e.Graphics.DrawLine(bluePen, 0, ClientSize.Height/2, ClientSize.Width, ClientSize.Height /2); // drawing the X axis 
            Brush redBrush = new SolidBrush(Color.Red); // red Brush to draw the Points ( the idea is to fill in small rectangles )
            
            float yRealprev =(float) document.SignalValues[0].Value; // store the previous Original Y value received from document  
            float yprev = (float)((ClientSize.Height / 2) - (yRealprev*PixelPerValue)*zoomFactor); // the previous (Y Axis) value to be implemented after applying the zoom factor and the pixelPerValue variables
           
            float tprev = 0;  // store the previous Original (X Axis) time received from document  

            float tReal;   // the original(X Axis) value received from document 
            float t;       // the X time to be implemented 
            float yReal; // the original (Y Axis) value received from document 
            float y;    // the (Y Axis) value to be implemented 
            e.Graphics.FillRectangle(redBrush,0, yprev , 3, 3);  // draw the first point 
            
            for (int i=1;i<document.SignalValues.Count();i++)   // loop to draw the rest of the points after taking their values from the document List
            {

                 tReal = (int)document.SignalValues[i].TimeStamp.Subtract(document.SignalValues[0].TimeStamp).Ticks;
                 t = (float)((tReal / PixelPerSec)*zoomFactor);
                
                 yReal = (int)document.SignalValues[i].Value;
                 y = (float)((ClientSize.Height / 2) -  (yReal*PixelPerValue)*zoomFactor);
                
                e.Graphics.FillRectangle(redBrush, t, y, 3, 3);  // draw the rest of the points
                e.Graphics.DrawLine(redPen, tprev, yprev, t, y);  // draw the link between the points to have a full graph
                yprev = y;
                tprev = t;

            }
            
            
        }

        private void plusButton_Click(object sender, EventArgs e) 
        {
            zoomFactor = zoomFactor * 1.2; // increase the zoom factor 
            Invalidate();  // invalidate to redraw again (call the OnPaint)
        }

        private void minusButton_Click(object sender, EventArgs e)
        {
            zoomFactor = zoomFactor / 1.2; //decrease the zoom factor 
            Invalidate();   // invalidate to redraw again (call the OnPaint)
        }
    }
}
