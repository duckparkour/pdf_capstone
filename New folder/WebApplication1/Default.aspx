<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

     <h3>File Upload System for the PDF Editor </h3>
    <div>
        <table>
            <tr>
                <td>Select File : </td>
                <td>
                    <asp:FileUpload ID="FileUpload1" runat="server" /></td>
                <td>
                     <!-- Button to call the uploading function--> 
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" /></td>
            </tr>
        </table>
        <br />
        <div>
            <!-- Add a list of uploaded files to the server so that it the user can select a download -->
            <asp:DataList ID="DataList1" runat="server" RepeatColumns="4" RepeatDirection="Horizontal" OnItemCommand="DataList1_ItemCommand">
                <ItemTemplate>
                    <table>
                        <tr>
                            <td><%#Eval("FileName","File Name : {0}") %></td>
                        </tr>
                        <tr>
                            <td><%#String.Format("{0:0.00}",Convert.ToDecimal(Eval("FileSize"))/1024)%> KB</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lbtnDownload" runat="server" CommandName="Download" CommandArgument=<%#Eval("FileID") %>>Download</asp:LinkButton></td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
        </div>
    </div>

</asp:Content>
