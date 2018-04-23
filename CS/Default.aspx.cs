using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using DevExpress.Web;
using DevExpress.Web.ASPxRichEdit;
using DevExpress.Web.Data;
using DevExpress.Web.Office;
using DevExpress.XtraRichEdit;

//for online demos and examples
public class CustomCallbackException: Exception {
    public CustomCallbackException(string message)
        : base(message) {
    }
}

public partial class _Default: System.Web.UI.Page {
    private string OpenedCanceledDocumentIDsSessionKey = "OpenedCanceledDocumentIDsSessionKey";
    private List<string> OpenedCanceledDocumentIDs {
        get {
            if (Session[OpenedCanceledDocumentIDsSessionKey] == null) {
                Session[OpenedCanceledDocumentIDsSessionKey] = new List<string>();
            }
            return (List<string>)Session[OpenedCanceledDocumentIDsSessionKey];
        }
        set { Session[OpenedCanceledDocumentIDsSessionKey] = value; }
    }
    protected void re_Init(object sender, EventArgs e) {
        ASPxRichEdit richEdit = sender as ASPxRichEdit;
        GridViewEditItemTemplateContainer container = richEdit.NamingContainer as GridViewEditItemTemplateContainer;

        string documentID = GetDocumentID(container.Grid);
        if (!OpenedCanceledDocumentIDs.Contains(documentID)) {
            OpenedCanceledDocumentIDs.Add(documentID);
        }

        if (container.Grid.IsNewRowEditing) {
            richEdit.DocumentId = documentID;
            return;
        }

        //for text in db
        string rtfText = container.Grid.GetRowValues(container.VisibleIndex, "RtfContent").ToString();

        //for binary in db
        //byte[] rtfBinary = (byte[])container.Grid.GetRowValues(container.VisibleIndex, "RtfContent");

        richEdit.Open(documentID, DocumentFormat.Rtf, () => {
            //for text in db
            return Encoding.UTF8.GetBytes(rtfText);

            //for binary in db
            //return rtfBinary;
        });
    }
    protected void gv_RowInserting(object sender, ASPxDataInsertingEventArgs e) {
        ProcessDocument(sender as ASPxGridView, e.NewValues);
    }
    protected void gv_RowUpdating(object sender, ASPxDataUpdatingEventArgs e) {
        ProcessDocument(sender as ASPxGridView, e.NewValues);
    }
    protected void gv_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e) {
        if (e.CallbackName == "ADDNEWROW" || e.CallbackName == "STARTEDIT" || e.CallbackName == "CANCELEDIT") {
            foreach (string id in OpenedCanceledDocumentIDs) {
                DocumentManager.CloseDocument(id);
            }
            OpenedCanceledDocumentIDs.Clear();
        }
    }
    protected void gv_CustomErrorText(object sender, ASPxGridViewCustomErrorTextEventArgs e) {
        if (e.Exception is CustomCallbackException) {
            e.ErrorText = e.Exception.Message;
        }
    }
    private string GetDocumentID(ASPxGridView grid) {
        //TODO: For per-user editing, construct the DocumentID using the row's key plus user info, for example, System.Web.UI.User.Identity.Name. Then, close the document for editing by this DocumentID for this user only.
        if (grid.IsNewRowEditing)
            return Guid.NewGuid().ToString();
        else
            return grid.GetRowValues(grid.EditingRowVisibleIndex, grid.KeyFieldName).ToString();
    }
    private ASPxRichEdit GetRichEdit(ASPxGridView grid) {
        GridViewDataColumn columnRftContent = grid.Columns["RtfContent"] as GridViewDataColumn;
        return grid.FindEditRowCellTemplateControl(columnRftContent, "re") as ASPxRichEdit;
    }
    private void ProcessDocument(ASPxGridView grid, OrderedDictionary newValues) {
        ASPxRichEdit richEdit = GetRichEdit(grid);

        //for text in db
        newValues["RtfContent"] = Encoding.UTF8.GetString(richEdit.SaveCopy(DocumentFormat.Rtf));

        //for binary in db
        //newValues["RtfContent"] = richEdit.SaveCopy(DocumentFormat.Rtf);
        

        throw new CustomCallbackException("Data modifications are not allowed in online demos");
        //Note that data modifications are not allowed in online demos. To allow editing in local/offline mode, download the example and comment out the "throw..." operation in the ASPxGridView.RowUpdating event handler.

		OpenedCanceledDocumentIDs.Remove(richEdit.DocumentId);
		DocumentManager.CloseDocument(richEdit.DocumentId);
    }
}