using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Signals
{
    /// <summary>
    /// Represents the application. The application must have exactly one instance of the App class that 
    /// is created by calling the Initialize method. This is the rool object of the application and is 
    /// globally accessably through the App.Instance static property.
    /// </summary>
    public class App
    {
        string filename;
        /// <summary>
        /// The active view (determined by the selected tab page)
        /// </summary>
        private IView activeView;
        /// <summary>
        /// The list of documents in the application
        /// </summary>
        private List<Document> documents = new List<Document>();
        /// <summary>
        /// A reference for the main form
        /// </summary>
        private MainForm mainForm;
        /// <summary>
        /// Stores the single instace of the App class
        /// </summary>
        private static App theApp;
        /// <summary>
        /// Provides global access to the single instance of the App class
        /// </summary>
        public static App Instance
        {
            get { return theApp; }
        }
        /// <summary>
        /// Creates the single instance of the App class
        /// </summary>
        public static void Initialize(MainForm form)
        {
            theApp = new App();
            theApp.mainForm = form;
        }
        /// <summary>
        /// The main window
        /// </summary>
        public MainForm MainForm
        {
            get { return mainForm; }
        }

        /// <summary>
        /// Gets the active document
        /// </summary>
        /// <returns></returns>
        public Document ActiveDocument
        {
            get 
            {
                if (activeView == null)
                    return null;

                return activeView.GetDocument();
            }
        }
        
        /// <summary>
        /// Closes the active document
        /// </summary>
        public void CloseActiveView()
        {
            if (mainForm.TabControl.TabPages.Count == 0)
                return;

            Document docToClose = ActiveDocument;

            // Remove the view from the corresponding documetn's view list.
            docToClose.DetachView(activeView);
            // Close the tab page that contained the view
            mainForm.TabControl.TabPages.Remove(getTabPageForView(activeView));

            // Close the document as well if this was the last view of the document.
            // Remove the document from the document list.
            if (!docToClose.HasAnyView())
                documents.Remove(docToClose);
        }

        /// <summary>
        /// Creates a new document with a corresponding view
        /// </summary>
        public void NewDocument()
        {
            // Read the name of the document in a modal dialog
            NewDocForm form = new NewDocForm();
            if (form.ShowDialog() != DialogResult.OK)
                return;

            // Create a new document and add it to the list of documents
            // TODO: instantiate the concrete document, instead
            SignalDocument doc = new SignalDocument(form.DocName);  // changed it from Document to SignalDocument
            documents.Add(doc); // add it to the list of documents
            createView(doc, true); // make the view 
        }

        /// <summary>
        /// Refreshes the active view to point to the view of the active tab page.
        /// </summary>
        public void UpdateActiveView()
        {
            if (mainForm.TabControl.TabPages.Count == 0)
                activeView = null;
            else
                activeView = (IView)mainForm.TabControl.SelectedTab.Tag;
        }

        /// <summary>
        /// Creates a new view to the active document
        /// </summary>
        public void CreateViewForActiveDocument()
        {
            Document doc = ActiveDocument;
            if (doc == null)
                return;
            createView(doc, true);
        }

        /// <summary>
        /// Opens a document. Not implemented yet.
        /// </summary>
        public void OpenDocument()
        {
            // 1. Get the file path from the user (OpenFileDialog)
            // http://msdn.microsoft.com/en-us/library/system.windows.forms.openfiledialog.aspx
            // As opposed to the example in the MSDN dont open the file with OpenFile.
            // Instead, just get the file path selected by the user with the FileName property
            // (only if the user pressed the OK button and not something else).
           
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;
           
            if (openFileDialog.ShowDialog() == DialogResult.OK) // after clicking OK in the Open Dialog we receive the file path(filename)
                 filename=openFileDialog.FileName; ;

            // 2. Crate a new document, register it, create its first view, etc.
            // ... just like in the NewDocument method. 
            // Use the filename as the name of the document (use the GetFileNam static method of System.IO.Path).

          //  NewDocForm form = new NewDocForm();
          
            SignalDocument doc = new SignalDocument(filename); //we create a new signal doc with the filename received 
            documents.Add(doc); // add it to documents
            createView(doc, true); //make the new view 

            // 3. Load the data to the document (call the LoadDocument method of the document).

            doc.LoadDocument(filename); //we load the document

            // 4. Notify the views to refresh themselves.
            // Note: the UpdateAllViews method of the document class is responsible for it.
            // Though, it is protected and you cannot access that from here. Dont make it public.
            // Follow the principle of encapsulation. In the previous step you called the 
            // SignalDocument.LoadDocument method of the document. Call UpdateAllViews at the end of
            // the LoadDocument method. This is one of the crucial points of the Document-View
            // architecture that all the operations that modify the document should call the UpdateAllViews
            // methods to notify the views about the change (so that they can refresh themselves).

        }

        /// <summary>
        /// Saves the current document. Not implemented yet.
        /// </summary>
        public void SaveActiveDocument()
        {
            if (ActiveDocument == null)
                return;

            // Get path from the user. Use the SaveFileDialog common dialog.
            // http://msdn.microsoft.com/en-us/library/system.windows.forms.savefiledialog.aspx
            // ...
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            // Set the properties before showing the save file dialog
            saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;

            // Show the modal save file dialog.
            // Don’t do anything if the user has closed the dialog with a button 
            // other than the OK (return).
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            // Save the data of the document.
            // saveFileDialog.FileName contains the full path the user has selected
            ActiveDocument.SaveDocument(saveFileDialog.FileName);
        }

    

    /// <summary>
    /// Creates a new view and registeres it to the document (so that the doc can notify the view when
    /// the document has changes). It also creates a new tab page for the new view
    /// </summary>
    IView createView(Document doc, bool activateView)
        {
            // Create a new tab page
            //mainForm.TabControl.TabPages.Add(form.DocName, form.DocName);
            TabPage tp = new TabPage(doc.Name);
            mainForm.TabControl.TabPages.Add(tp);
           // DemoView view = new DemoView(doc);
            //TabPage tp = mainForm.TabControl.TabPages[form.DocName];
            GraphicsSignalView view = new GraphicsSignalView((SignalDocument)doc);
            tp.Controls.Add(view);
            tp.Tag = view;
            view.Dock = DockStyle.Fill;

            // Register the view to the document to be notified when the document is changed.
            doc.AttachView(view);

            // If this is not the first view of the document also add an ordinal number
            if (view.ViewNumber > 1)
                tp.Text = tp.Text + ":" + view.ViewNumber;

            // Select the new tab page
            if (activateView)
            {
                mainForm.TabControl.SelectTab(tp); // It triggers the SelectedIndexChanged event. Its handler will set the activeView member.
                activeView = view;
            }
            return view;
        }

        /// <summary>
        /// Returns the TabPage that contains the parameter view.
        /// It throws an exception if the view cannot be found on any of the tab pages of the TabControl.
        /// </summary>
        TabPage getTabPageForView(IView view)
        {
            foreach (TabPage page in mainForm.TabControl.TabPages)
               if (page.Tag == activeView)
                   return page;
            throw new Exception("Page for view not found.");
        }

    }
}
