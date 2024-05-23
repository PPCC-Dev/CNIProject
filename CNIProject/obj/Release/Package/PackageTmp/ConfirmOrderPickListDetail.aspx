<%@ Page Title="Confirm Order Pick List Detail" Language="vb" AutoEventWireup="false" MasterPageFile="~/SRN.Master" CodeBehind="ConfirmOrderPickListDetail.aspx.vb" Inherits="CNIProjet.ConfirmOrderPickListDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .txt-margin { margin-bottom:5px;}
        .width-div { width: 100%;}
      
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
        <div class="se-pre-con"></div>
        <div class="col-lg-12">
            <div class="card border-primary">
                <div class="card-header py-12">
                    <div class="row">
                        <div class="col-md-10">
                            <h6 class="m-0"><a href="Menu.aspx">Menu</a><span class="mx-2 mb-0">/</span><asp:LinkButton ID="LinkButton1" runat="server">Confirm Order Pick List</asp:LinkButton><span class="mx-2 mb-0">/</span><strong class="text-black"><%: Page.Title %></strong>  </h6>
                        </div>
                        
                    </div>
                 </div>
                
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-10">                           
                        </div>
                        
                        <div class="col-md-2 float-right">
                            <asp:Button ID="btnback" runat="server" class="btn btn-danger btn-sm float-right" Text="Back" />
                        </div>
                        
                    </div>
                    <table style="width:100%">
                        <tr>
                            <td>
                                <h6>Order No.: <asp:Label ID="lblderco" runat="server" Font-Bold="True"></asp:Label></h6>
                                <asp:Label ID="lblconum" runat="server" Font-Bold="True" Visible="false"></asp:Label>
                                <asp:Label ID="lblcoline" runat="server" Font-Bold="True" Visible="false"></asp:Label>
                                <asp:Label ID="lblcorelease" runat="server" Font-Bold="True" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                               <asp:ListView id="PanelList" runat="server">
                                <ItemTemplate>
                                    <div class="col-sm-12 mt-2">
                                        <div class="card">
                                            <div class="card-body" style="padding: 1.0rem;">
                                                <div class="row">
                                                    <h6 class="card-title text-primary">
                                                        <div class="col-sm-12">
                                                            <asp:Label ID="lblTagID" runat="server" Text='<%#Eval("TagID")%>'></asp:Label>
                                                        </div>
                                                    </h6> 
                                                </div>

                                                <div class="row"> 
                                                    <div class="col-sm-12">
                                                         <strong>Item: </strong><asp:Label ID="lblItem" runat="server" Text='<%#Eval("Item")%>'></asp:Label>
                                                        &nbsp;&nbsp;<asp:Label ID="lblDescription" runat="server" Text='<%#Eval("ItemDescription")%>'></asp:Label>
                                                    </div>           
                                                </div>

                                                <div class="row"> 
                                                    <div class="col-sm-12">
                                                         <strong>Lot: </strong> <asp:Label ID="lblLot" runat="server" Text='<%#Eval("Lot")%>'></asp:Label>
                                                    </div>
                                                </div>

                                                <div class="row">   
                                                    <div class="col-sm-12">
                                                         <strong>Quantity: </strong> <asp:Label ID="lblQtyTag" runat="server" Text='<%#Eval("QtyTag")%>'></asp:Label>
                                                    </div>
                                            
                                            
                                                </div>
                                        
                                            </div>
                                        </div>
                                    </div>
                            
                                </ItemTemplate>
                            </asp:ListView>         
                            </td>
                        </tr>
                    </table>
                  
                </div>
            </div>



        </div>
</asp:Content>
