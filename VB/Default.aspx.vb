Imports System
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Text
Imports DevExpress.Web
Imports DevExpress.Web.ASPxRichEdit
Imports DevExpress.Web.Data
Imports DevExpress.Web.Office
Imports DevExpress.XtraRichEdit

'for online demos and examples
Public Class CustomCallbackException
    Inherits Exception

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub
End Class

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Private OpenedCanceledDocumentIDsSessionKey As String = "OpenedCanceledDocumentIDsSessionKey"
    Private Property OpenedCanceledDocumentIDs() As List(Of String)
        Get
            If Session(OpenedCanceledDocumentIDsSessionKey) Is Nothing Then
                Session(OpenedCanceledDocumentIDsSessionKey) = New List(Of String)()
            End If
            Return DirectCast(Session(OpenedCanceledDocumentIDsSessionKey), List(Of String))
        End Get
        Set(ByVal value As List(Of String))
            Session(OpenedCanceledDocumentIDsSessionKey) = value
        End Set
    End Property
    Protected Sub re_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim richEdit As ASPxRichEdit = TryCast(sender, ASPxRichEdit)
        Dim container As GridViewEditItemTemplateContainer = TryCast(richEdit.NamingContainer, GridViewEditItemTemplateContainer)

        Dim documentID As String = GetDocumentID(container.Grid)
        If Not OpenedCanceledDocumentIDs.Contains(documentID) Then
            OpenedCanceledDocumentIDs.Add(documentID)
        End If

        If container.Grid.IsNewRowEditing Then
            richEdit.DocumentId = documentID
            Return
        End If

        'for text in db
        Dim rtfText As String = container.Grid.GetRowValues(container.VisibleIndex, "RtfContent").ToString()

        'for binary in db
        'byte[] rtfBinary = (byte[])container.Grid.GetRowValues(container.VisibleIndex, "RtfContent");

        richEdit.Open(documentID, DocumentFormat.Rtf, Function() Encoding.UTF8.GetBytes(rtfText))
            'for text in db
            'for binary in db
            'return rtfBinary;
    End Sub
    Protected Sub gv_RowInserting(ByVal sender As Object, ByVal e As ASPxDataInsertingEventArgs)
        ProcessDocument(TryCast(sender, ASPxGridView), e.NewValues)
    End Sub
    Protected Sub gv_RowUpdating(ByVal sender As Object, ByVal e As ASPxDataUpdatingEventArgs)
        ProcessDocument(TryCast(sender, ASPxGridView), e.NewValues)
    End Sub
    Protected Sub gv_AfterPerformCallback(ByVal sender As Object, ByVal e As ASPxGridViewAfterPerformCallbackEventArgs)
        If e.CallbackName = "ADDNEWROW" OrElse e.CallbackName = "STARTEDIT" OrElse e.CallbackName = "CANCELEDIT" Then

            For Each id_Renamed As String In OpenedCanceledDocumentIDs
                DocumentManager.CloseDocument(id_Renamed)
            Next id_Renamed
            OpenedCanceledDocumentIDs.Clear()
        End If
    End Sub
    Protected Sub gv_CustomErrorText(ByVal sender As Object, ByVal e As ASPxGridViewCustomErrorTextEventArgs)
        If TypeOf e.Exception Is CustomCallbackException Then
            e.ErrorText = e.Exception.Message
        End If
    End Sub
    Private Function GetDocumentID(ByVal grid As ASPxGridView) As String
        'TODO: For per-user editing, construct the DocumentID using the row's key plus user info, for example, System.Web.UI.User.Identity.Name. Then, close the document for editing by this DocumentID for this user only.
        If grid.IsNewRowEditing Then
            Return Guid.NewGuid().ToString()
        Else
            Return grid.GetRowValues(grid.EditingRowVisibleIndex, grid.KeyFieldName).ToString()
        End If
    End Function
    Private Function GetRichEdit(ByVal grid As ASPxGridView) As ASPxRichEdit
        Dim columnRftContent As GridViewDataColumn = TryCast(grid.Columns("RtfContent"), GridViewDataColumn)
        Return TryCast(grid.FindEditRowCellTemplateControl(columnRftContent, "re"), ASPxRichEdit)
    End Function
    Private Sub ProcessDocument(ByVal grid As ASPxGridView, ByVal newValues As OrderedDictionary)
        Dim richEdit As ASPxRichEdit = GetRichEdit(grid)

        'for text in db
        newValues("RtfContent") = Encoding.UTF8.GetString(richEdit.SaveCopy(DocumentFormat.Rtf))

        'for binary in db
        'newValues["RtfContent"] = richEdit.SaveCopy(DocumentFormat.Rtf);


        Throw New CustomCallbackException("Data modifications are not allowed in online demos")
        'Note that data modifications are not allowed in online demos. To allow editing in local/offline mode, download the example and comment out the "throw..." operation in the ASPxGridView.RowUpdating event handler.

        OpenedCanceledDocumentIDs.Remove(richEdit.DocumentId)
        DocumentManager.CloseDocument(richEdit.DocumentId)
    End Sub
End Class