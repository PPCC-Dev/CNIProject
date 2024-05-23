<%@ Page Title="Order Shipping Detail" Language="vb" AutoEventWireup="false" MasterPageFile="~/SRN.Master" CodeBehind="OrderShippingDetail.aspx.vb" Inherits="CNIProjet.OrderShippingDetail" %>
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
                            <h6 class="m-0"><a href="Menu.aspx">Menu</a><span class="mx-2 mb-0">/</span><asp:LinkButton ID="LinkButton1" runat="server">Order shipping</asp:LinkButton><span class="mx-2 mb-0">/</span><strong class="text-black"><%: Page.Title %></strong>  </h6>
                        </div>
                        
                        <%--<div class="col-md-2 float-right">
                            <asp:Button ID="btnback" runat="server" class="btn btn-danger btn-sm float-right" Text="Back" />
                        </div>--%>
                        
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
                    <%--<table>
                        <tr>
                            <td>
                                <div class="row align-items-center">
                                <div class="col-sm-2 width-div">
                                    <asp:Button ID="btnback" runat="server" class="btn btn-danger btn-sm" Text="Back" />
                                </div>
                                
                                </div>
                            </td>
                        </tr>
                    </table>--%>
                    <table style="width:100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblderco" runat="server" Font-Bold="True"></asp:Label>
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
                                        <h6 class="card-title text-primary"><%#Eval("TagID")%></h6>
                                        <span style="font-size:13px;">Item : <%#Eval("Item")%>&nbsp;&nbsp;<%#Eval("ItemDescription")%></span><br>
                                        <span style="font-size:13px;">Lot : <%#Eval("Lot")%></span><br>
                                        <span style="font-size:13px;">Quantity : <asp:Label ID="lblListQtyReq" runat="server" Text='<%#Eval("QtyTag")%>'></asp:Label></span><br>                                        
                                        
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