<%@ Page Title="Count Qty" Language="vb" AutoEventWireup="false" MasterPageFile="~/SRN.Master" CodeBehind="CycleCountQty.aspx.vb" Inherits="CNIProjet.CycleCountQty" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
        .txt-margin { margin-bottom:5px;}
        .width-div { width: 100%;}
        .margin-head { margin-bottom:8px;}
        .margin-top { margin-top:-24px;}
    </style>

     <script type="text/javascript">
         $(document).ready(function () {
             $('#<%= txtCountQty.ClientID%>').keypress(function (e) {
               if (e.which != 45 && e.which != 46 && e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57) && e.which != 45) {
                    return false;
            }
        });

         });

     </script>

    <script>

        function ShowSweetAlert(type, msg, icon) {
            Swal.fire(
                type,
                msg,
                icon
            ).then(function() {
                window.setTimeout(function () { 
                document.getElementById('<%=txtCountQty.ClientID%>').focus(); 
            }, 0);});

        }

       </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <br />
        <div class="se-pre-con"></div>
        <div class="col-lg-12">
            <div class="card border-primary">
                <div class="card-header py-12">
                    <div class="row">
                        <div class="col-md-10">
                            <h6 class="m-0"><a href="Menu.aspx">Menu</a><span class="mx-2 mb-0">/</span><asp:LinkButton ID="LinkButton1" runat="server">Cycle Count</asp:LinkButton><span class="mx-2 mb-0">/</span><strong class="text-black"><%: Page.Title %></strong>  </h6>
                        </div>
                        
                    </div>
                 </div>
                <div class="card-body">
                    <div class="row margin-head">

                        <div class="col-md-10">
                        </div>

                        <div class="col-md-2 float-right">
                            <asp:Button ID="btnback" runat="server" class="btn btn-danger btn-sm float-right" Text="Back" />
                        </div>
                        
                    </div>

                    <div class="row">
                                <div class="col-sm-12 mt-2">
                                    <div class="card">
                                        <div class="card-body" style="padding: 1.0rem;">
                                        <h6 class="card-title text-primary"><asp:Label ID="lblitem" runat="server" Text='<%#Eval("Item")%>'></asp:Label>                                          
                                        </h6>   
                                        <span style="font-size:13px;">Desc : <asp:Label ID="lblLongDesc" runat="server" Text='<%#Eval("Description")%>'></asp:Label></span><br />      
                                        <span style="font-size:13px;">Lot : <asp:Label ID="lblLot" runat="server" Font-Bold="False" Text='<%#Eval("Lot")%>'></asp:Label></span><br />                                    
                                        <span style="font-size:13px;">Location : <asp:Label ID="lblLoc" runat="server" Text='<%#Eval("Loc")%>'></asp:Label></span>&nbsp;&nbsp;&nbsp;
                                        <span style="font-size:13px;"><asp:CheckBox ID="chkNonScanTag" class="form-check-input float-right mx-2 mb-0" runat="server" Enabled="false" checked='<%# Eval("Uf_loc_NonScanTag") %>' /> <span>Non-Scan Tag</span></span><br />
                                        <span style="font-size:13px;">Counted Qty : <asp:TextBox ID="txtCountQty" runat="server" Text='<%# Eval("CountQty") %>' class="form-control form-control-sm" style="width: 158px; font-size:13px; text-align:right;" AutoComplete="off"></asp:TextBox></span><br />
                                        <span style="font-size:13px;"><asp:Label ID="lblRowPointer" runat="server" style="display:none;" Font-Bold="False" Text='<%#Eval("RowPointer")%>'></asp:Label></span>
                                        <asp:Button ID="btncount" runat="server" class="btn btn-success btn-sm margin-top" Text="Count" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btncancel" runat="server" class="btn btn-warning btn-sm margin-top" Text="Cancel" />

                                    </div>
                                    </div>
                                </div>
                    </div>
                    
                  
                </div>
            </div>



        </div>

</asp:Content>
