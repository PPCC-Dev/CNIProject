﻿<%@ Page Title="Generate GRN Vendor Lot" Language="vb" AutoEventWireup="false" MasterPageFile="~/SRN.Master" CodeBehind="GRNVendorLot.aspx.vb" Inherits="SRNProject.GRNVendorLot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .txt-margin { margin-bottom:5px;}
        .display-col { display:none;}
        .width-div { width: 50%;}
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
                            <h6 class="m-0"><a href="Menu.aspx">Menu</a><span class="mx-2 mb-0">/</span><asp:LinkButton ID="LinkButton1" runat="server">Generate GRN</asp:LinkButton><span class="mx-2 mb-0">/</span><strong class="text-black"><%: Page.Title %></strong>  </h6>
                        </div>
                        
                        <%--<div class="col-md-2 float-right">
                            <asp:Button ID="btnback" runat="server" class="btn btn-danger btn-sm float-right mx-2 mb-0" Text="Back" />
                            <asp:Button ID="btnsave" runat="server" class="btn btn-warning btn-sm float-right mx-2 mb-0" Text="Save" />
                        </div>--%>
                        </div>
                        
                    </div>
                 
                
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-10"></div>
                        
                        <div class="col-md-2 float-right">
                            <asp:Button ID="btnback" runat="server" class="btn btn-danger btn-sm float-right mx-2 mb-0" Text="Back" />
                            <asp:Button ID="btnsave" runat="server" class="btn btn-success btn-sm float-right mx-2 mb-0" Text="Save" />
                        </div>

                    </div>

                    <div class="row">

                     <asp:ListView id="PanelList" runat="server">
                         <ItemTemplate>
                             <div class="col-sm-12 mt-2">
                                 <div class="card">                                     
                                     <div class="card-body" style="padding: 1.0rem;">
                                     <h6 class="card-title text-primary">
                                         <asp:Label ID="lblPoNum" runat="server" Font-Bold="False" Text='<%#Eval("DerPo")%>'></asp:Label></br>
                                         <asp:Label ID="lblItem" runat="server" Font-Bold="False" Text='<%#Eval("Item")%>'></asp:Label>
                                     </h6>                                     
                                     <span style="font-size:13px;">Lot : <%#Eval("Lot")%></span><br>
                                     <span style="font-size:13px;">Vendor Lot : </span><span class="width-div"><asp:TextBox ID="txtvendorlot" ReadOnly="false" runat="server" Text='<%# Eval("VendLot") %>' class="form-control form-control-sm mh-100 txt-margin" style="width:255px" AutoComplete="off"></asp:TextBox></span>
                                     <span style="font-size:13px;">Qty : <asp:Label ID="lblQtyTag" runat="server" Font-Bold="False" Text='<%#Eval("Qty")%>'></asp:Label></span><br>
                                     <span style="font-size:13px;">Expiration Date : <asp:TextBox ID="txtExpDate" runat="server" Text='<%# Eval("ExpDate") %>' class="form-control form-control-sm mh-100 txt-margin datepicker" style="width:255px; font-size:13px;" AutoComplete="off" MaxLength="10"></asp:TextBox></span>
                                     <span style="font-size:13px; display:none;"><asp:Label ID="lblRowPointer" runat="server" Text='<%# Eval("RowPointer") %>' ></asp:Label></span>
                                 </div>
                                 </div>
                             </div>
                         </ItemTemplate>
                     </asp:ListView>

                </div>

                    <%--<table style="width:100%">
                        <tr>
                            <td>

                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" 
                                        CssClass="table table-striped table-bordered table-hover"
                                    ShowFooter="false" ShowHeader="true" Font-Size="Small">
                                    <Columns>
                                    <asp:TemplateField HeaderText="Item">  
                              
                                        <ItemStyle HorizontalAlign="Left" />                                  
                                        <ItemTemplate>
                                            <asp:Label ID="lblPoNum" runat="server" Font-Bold="False" Text='<%#Eval("DerPO")%>'></asp:Label><br />
                                            Item: <asp:Label ID="lblItem" runat="server" Font-Bold="False" Text='<%#Eval("Item")%>'></asp:Label><br />      
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Lot" DataField="Lot" ReadOnly="true" />

                                    <asp:TemplateField HeaderText="Vendor Lot">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtvendorlot" ReadOnly="false" runat="server" Text='<%# Eval("VendLot") %>' class="form-control form-control-sm txt-margin" AutoComplete="off"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField HeaderText="Qty" DataField="Qty" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />

                                        <asp:TemplateField HeaderText="Expiration Date">
                                        <ItemStyle HorizontalAlign="left" />
                                        <HeaderStyle HorizontalAlign="center"/>
                                        <ItemTemplate>                      
                                            <asp:TextBox ID="txtExpDate" runat="server" Text='<%# Eval("ExpDate") %>' class="form-control form-control-sm txt-margin datepicker" AutoComplete="off"></asp:TextBox>                            
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                        <asp:TemplateField HeaderText="RowPointer">
                                        <ItemStyle HorizontalAlign="Left" CssClass="display-col" />
                                        <HeaderStyle CssClass="display-col" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblRowPointer" runat="server" Text='<%# Eval("RowPointer") %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                        
                            </td>
                        </tr>
                    </table>--%>
                  
                </div>
            </div>
            </div>

</asp:Content>
