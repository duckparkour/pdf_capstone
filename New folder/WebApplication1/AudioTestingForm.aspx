<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AudioTestingForm.aspx.cs" Inherits="WebApplication1.AudioTestingForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
             <asp:Button ID="1" runat="server" Text="Record" OnClick="RecordButton" />

             <asp:Button ID="2" runat="server" Text="Stop" OnClick="StopButton" />

             <asp:Button ID="3" runat="server" Text="Stop" OnClick="PlayButton" />

        </div>
    </form>
</body>
</html>
