<%@ Page Language="vb" AutoEventWireup="true" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.ASPxRichEdit.v15.1, Version=15.1.12.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRichEdit" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.12.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <dx:ASPxGridView ID="gv" runat="server" AutoGenerateColumns="False" DataSourceID="ads"
        KeyFieldName="ID" OnRowUpdating="gv_RowUpdating" OnCustomErrorText="gv_CustomErrorText"
        OnRowInserting="gv_RowInserting" OnAfterPerformCallback="gv_AfterPerformCallback">
        <Columns>
            <dx:GridViewCommandColumn VisibleIndex="0" ShowNewButton="true" ShowEditButton="True"
                ShowDeleteButton="true" />
            <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" VisibleIndex="1">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Trademark" VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Model" VisibleIndex="3">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="RtfContent" VisibleIndex="4" Visible="false">
                <EditFormSettings Visible="True" />
                <EditItemTemplate>
                    <dx:ASPxRichEdit ID="re" runat="server" OnInit="re_Init">
                    </dx:ASPxRichEdit>
                </EditItemTemplate>
            </dx:GridViewDataTextColumn>
        </Columns>
        <SettingsEditing EditFormColumnCount="1">
        </SettingsEditing>
    </dx:ASPxGridView>
    <asp:AccessDataSource ID="ads" runat="server" DataFile="~/App_Data/CarsDB.mdb" SelectCommand="SELECT [ID], [Trademark], [Model], [RtfContent] FROM [Cars]"
        InsertCommand="INSERT INTO [Cars] ([ID], [Trademark], [Model], [RtfContent]) VALUES (?, ?, ?, ?)"
        UpdateCommand="UPDATE [Cars] SET [Trademark] = ?, [Model] = ?, [RtfContent] = ? WHERE [ID] = ?"
        DeleteCommand="DELETE FROM [Cars] WHERE [ID] = ?">
        <InsertParameters>
            <asp:Parameter Name="ID" Type="Int32" />
            <asp:Parameter Name="Trademark" Type="String" />
            <asp:Parameter Name="Model" Type="String" />
            <asp:Parameter Name="RtfContent" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="Trademark" Type="String" />
            <asp:Parameter Name="Model" Type="String" />
            <asp:Parameter Name="RtfContent" Type="String" />
            <asp:Parameter Name="ID" Type="Int32" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="ID" Type="Int32" />
        </DeleteParameters>
    </asp:AccessDataSource>
    </form>
</body>
</html>