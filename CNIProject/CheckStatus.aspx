<%@ Page Title="Check Order Pick List Status" Language="vb" AutoEventWireup="false" MasterPageFile="~/SRN.Master" CodeBehind="CheckStatus.aspx.vb" Inherits="CNIProjet.CheckStatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .txt-margin { margin-bottom:5px;}
        .txt-margin2 { margin-bottom:10px;}
        .width-div { width: 50%;} 
        .txt-center {text-align:center;}
        .display-col { display:none;}
    </style>

    <script>

        function ShowSweetAlert(type, msg, icon) {
            Swal.fire(
                type,
                msg,
                icon
            ).then(function() {
                window.setTimeout(function () { 
                    document.getElementById('<%=txtbarcode.ClientID%>').focus();
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
                    <div class="col-md-9">
                        <h6 class="m-0"><%--<a href="Menu.aspx">Menu</a><span class="mx-2 mb-0">/</span>--%><strong class="text-black"><%: Page.Title %></strong>  </h6>
                    </div>
                </div>
                  
            </div>

            <div class="card-body">
                <div class="row">
                    <div class="col-md-9">
                        <div style="margin-top:20px;display:none;">
                            <asp:Button ID="Button1" runat="server" Text="Scan" UseSubmitBehavior="true" />                                
                       </div>
                    </div>
                    
                </div>

                <table  width="100%">
                <tr>
                    <div class="row align-items-center">
                        <div class="col-sm-3 text-right width-div"></div>
                        <div class="col-sm-6 width-div">
                            <p id="notifyWaiting"></p>
                        </div>
                    </div>
                </tr>                      
                <tr>
                    <td>
                        <div class="row align-items-right">
                        <div class="col-sm-3 text-right width-div">
                            <asp:Label ID="lblbarcode" runat="server" Text="Scan Pick List: "></asp:Label>
                        </div>
                        <div class="col-sm-5 width-div">
                                <asp:TextBox ID="txtbarcode" runat="server" AutoComplete="off" class="form-control form-control-sm txt-margin"></asp:TextBox>
                                    
                            </div>
                            
                                    
                        </div>
                    </td>
                </tr>

             </table>
                <br />
                <div class="row">
                    <asp:ListView id="PanelList" runat="server">
                        <ItemTemplate>
                            <div class="col-sm-12 mt-2">
                                <div class="card">
                                    <div class="card-body">
                                        <h6 class="card-title text-primary">
                                            <div class="row"> 
                                                <div class="form-group col-sm-6 row mb-1">
                                                    <div class="col-sm-5 text-right width-div">
                                                        <span><asp:Label ID="lblstat" runat="server" Text='<%#Eval("Status")%>'></asp:Label></span>
                                                    </div>                                                                 
                                                                    
                                                </div>

                                            </div> 
                                        </h6>

                                        <div class="row"> 
                                                <div class="form-group col-sm-6 row mb-1">
                                                    <div class="col-sm-4 text-right width-div">
                                                        <span>Order Pick List NO. :</span>
                                                    </div> 
                                                    <div class="col-sm-8 text-left width-div">
                                                        <span><%#Eval("PickListNum")%></span>
                                                    </div> 
                                                </div>

                                         </div>

                                        <div class="row"> 
                                                <div class="form-group col-sm-6 row mb-1">
                                                    <div class="col-sm-4 text-right width-div">
                                                        <span>Total Pick List Line :</span>
                                                    </div> 
                                                    <div class="col-sm-8 text-left width-div">
                                                        <span><%#Eval("TotalLine")%></span>
                                                    </div> 
                                                </div>

                                         </div>
                                         <div class="row"> 
                                                <div class="form-group col-sm-6 row mb-1">
                                                    <div class="col-sm-4 text-right width-div">
                                                        <span>Total Qty Pick :</span>
                                                    </div> 
                                                    <div class="col-sm-8 text-left width-div">
                                                        <span><%#Eval("TotalPick")%>
                                                    </div> 
                                                </div>

                                         </div>
                                         <div class="row"> 
                                                <div class="form-group col-sm-6 row mb-1">
                                                    <div class="col-sm-4 text-right width-div">
                                                        <span>Total Qty Shipped :</span>
                                                    </div> 
                                                    <div class="col-sm-8 text-left width-div">
                                                        <span><%#Eval("TotalShipped")%></span>
                                                    </div> 
                                                </div>

                                         </div>
                                    
                                </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                </div>

            </div>

            

        </div>
    </div>
</asp:Content>
