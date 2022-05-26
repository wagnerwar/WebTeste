<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/WebFormTeste.Master" CodeBehind="Default.aspx.vb" Inherits="WebFormTestes._Default" %>
<asp:Content ContentPlaceHolderID="conteudo" ID="conteudo" runat="server">
    <script type="text/javascript">
        function AbrirDownload(){
            let url = "Download.aspx"
            window.open(url, 'Imagem', 'resizable=yes,scrollbars=yes,width=570,height=350')
            return false;
        }
    </script>
    <asp:Label runat="server" ID="teste" Text="Olá a todos"></asp:Label>
    <asp:Button runat="server" OnClientClick="javascript: AbrirDownload()" ID="btnDownload" Text="Download" />
</asp:Content>
