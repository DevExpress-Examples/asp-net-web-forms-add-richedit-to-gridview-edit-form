<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128543510/19.2.7%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T260978)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->
# Grid View for ASP.NET Web Forms - How to use the Rich Text Editor to edit formatted text in the Edit Form

This example demonstrates how to add the [Rich Text Editor](https://docs.devexpress.com/AspNet/17721/components/rich-text-editor) to the [Grid View](https://docs.devexpress.com/AspNet/5823/components/grid-view) control's edit form. The Rich Text Editor displays formatted text loaded from the data source bound to the Grid View and allows users to edit this text.

> **Note**  
> Data modification is not allowed in the online version of this example. To allow edit operations, download the example and comment out the [throw](./CS/Default.aspx.cs#L97) expression in the `RowUpdating` event handler.

## Overview

Follow the steps below to add the Rich Text Editor in the Grid View's edit form:

1. Create the [Grid View](https://docs.devexpress.com/AspNet/DevExpress.Web.ASPxGridView) control, populate it with columns, and bind the control to a data source.

2. Add the [Rich Text Editor](https://docs.devexpress.com/AspNet/DevExpress.Web.ASPxRichEdit.ASPxRichEdit) control to the [EditItemTemplate](https://docs.devexpress.com/AspNet/DevExpress.Web.GridViewDataColumn.EditItemTemplate?p=netframework) of the column that contains formatted text.

3. Handle the Rich Text Editor control's [Init](https://learn.microsoft.com/en-us/dotnet/api/system.web.ui.control.init?view=netframework-4.8.1) event that occurs after the edit form appears. Use the Grid View's [IsNewRowEditing](https://docs.devexpress.com/AspNet/DevExpress.Web.ASPxGridView.IsNewRowEditing?p=netframework) property to identify if the edit form appeared in response to a click on the **New** or **Edit** button. If a user clicked the **New** button, assign a new unique identifier to the Rich Text Editor's [DocumentId](https://docs.devexpress.com/AspNet/DevExpress.Web.ASPxRichEdit.ASPxRichEdit.DocumentId?p=netframework) property. Otherwise, get a formatted text string from the corresponding cell and pass the string and the row key to the control's [Open](https://docs.devexpress.com/AspNet/DevExpress.Web.ASPxRichEdit.ASPxRichEdit.Open(System.String-DevExpress.XtraRichEdit.DocumentFormat-System.Func-System.Byte---)) method.

4. Handle the Grid View's [RowInserting](https://docs.devexpress.com/AspNet/DevExpress.Web.ASPxGridView.RowInserting?p=netframework) and [RowUpdating](https://docs.devexpress.com/AspNet/DevExpress.Web.ASPxGridView.RowUpdating?p=netframework) events. In both event handlers, call the Rich Text Editor control's [SaveCopy](https://docs.devexpress.com/AspNet/DevExpress.Web.ASPxRichEdit.ASPxRichEdit.SaveCopy(DevExpress.XtraRichEdit.DocumentFormat)) method to get content of the document opened in the control as an array of bytes. Save the array back to the data source.

5. To prevent synchronization issues, handle the Rich Text Editor's client-side [Init](https://docs.devexpress.com/AspNet/js-ASPxClientControlBase.Init) event and the Grid View's client-side [BeginCallback](https://docs.devexpress.com/AspNet/js-ASPxClientGridView.BeginCallback) event as follows:

    ```js
    function OnRichEditInit(s, e) {
        s.requestSettings.pendingPeriod = 1;
    }
    function OnGridBeginCallback(s, e) {
        if (e.command == "UPDATEEDIT")
            re.updateWatcherHelper.HasChanges = function () { return false; }
    }
    ```

## Files to Review

* [Default.aspx](./CS/Default.aspx) (VB: [Default.aspx](./VB/Default.aspx))
* [Default.aspx.cs](./CS/Default.aspx.cs) (VB: [Default.aspx.vb](./VB/Default.aspx.vb))

## Documentation

* [Grid View - Edit Data](https://docs.devexpress.com/AspNet/3712/components/grid-view/concepts/edit-data)
* [Rich Text Editor - Document Management](https://docs.devexpress.com/AspNet/401562/components/rich-text-editor/document-management)

## More Examples

* [Grid View for ASP.NET Web Forms - How to use the HTML Editor to edit formatted text in the Edit Form](https://www.devexpress.com/Support/Center/p/E4257)
<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=asp-net-web-forms-add-richedit-to-gridview-edit-form&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=asp-net-web-forms-add-richedit-to-gridview-edit-form&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
