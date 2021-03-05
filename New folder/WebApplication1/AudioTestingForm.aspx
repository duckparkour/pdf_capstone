<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AudioTestingForm.aspx.cs" Inherits="WebApplication1.AudioTestingForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
             <asp:Button ID="btn1" runat="server" Text="Record" OnClick="RecordButton" />

             <asp:Button ID="btn2" runat="server" Text="Stop" OnClick="StopButton" />

             <asp:Button ID="btn3" runat="server" Text="Play" OnClick="PlayButton" />

<audio id="audioPlayer" style="display:none" "autoplay=false">
  <source id="audioSrc" src="foo.wav" type="audio/mp3"/>
  Your browser does not support the <code>audio</code> element. 
</audio>

var audioSrc = document.getElementById("audioSrc")
audioSrc.setAttribte("src","http://xx.xx.xxx.x/.global/call_recording_archive/download.php?file=xxxxxxxxxxxxxx-all.mp3");

var audioPlayer = document.getElementById("audioPlayer")
audioPlayer.play()

        </div>
    </form>
</body>
</html>
