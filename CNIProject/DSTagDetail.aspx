<%@ Page Title="DS Receiving Detail" Language="vb" AutoEventWireup="false" MasterPageFile="~/SRN.Master" CodeBehind="DSTagDetail.aspx.vb" Inherits="CNIProjet.DSTagDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
        <div class="se-pre-con"></div>
        <div class="col-lg-12">
            <div class="card border-primary">
                <div class="card-header py-12">
                    <div class="row">
                        <div class="col-md-10">
                            <h6 class="m-0"><a href="Menu.aspx">Menu</a><span class="mx-2 mb-0">/</span><asp:LinkButton ID="LinkButton1" runat="server">DS Receiving</asp:LinkButton><span class="mx-2 mb-0">/</span><strong class="text-black"><%: Page.Title %></strong>  </h6>
                        </div>
                        
                    </div>
                 </div>
                
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-10">                            
                        </div>
                        
                        <div class="col-md-2 float-right">
                            <asp:Button ID="btnback" runat="server" class="btn btn-danger btn-sm float-right mx-2 mb-0" Text="Back" />
                           <%-- <asp:Button ID="btndeletetag" runat="server" class="btn btn-warning btn-sm float-right mx-2 mb-0" Text="Delete" />--%>
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

                    <div class="row">

                     
                        <asp:ListView id="PanelList" runat="server">
                         <ItemTemplate>
                             <div class="col-sm-12 mt-2">
                                 <div class="card">                                     
                                     <div class="card-body" style="padding: 1.0rem;">
                                     <h6 class="card-title text-primary">
                                         <asp:Label ID="lbltagID" runat="server" Font-Bold="False" Text='<%#Eval("tag_id")%>'></asp:Label>
                                     </h6>
                                     <span style="font-size:13px;">Item : <%#Eval("item")%></span><br>
                                     <span style="font-size:13px;">Description : <%#Eval("description")%></span><br>
                                     <span style="font-size:13px;">Tag Qty : <asp:Label ID="lblQtyTag" runat="server" Font-Bold="False" Text='<%#Eval("tag_qty")%>'></asp:Label></span><br>
                                     <span style="font-size:13px;">Tag Qty Convert : <asp:Label ID="lblQtyTagConv" runat="server" Font-Bold="False" Text='<%#Eval("tag_qty_conv")%>'></asp:Label></span><br>
                                     <span style="font-size:13px;">Lot : <%#Eval("lot")%></span><br>
                                     <span style="font-size:13px;"class="text-danger"><asp:LinkButton ID="lnkDeleteTag" CommandName="DeleteTag" class="text-danger" runat="server" Font-Bold="true" ToolTip="Click to Delete Tag" Text="Delete Tag"></asp:LinkButton></span>
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
                                                  <asp:TemplateField HeaderText="^" ItemStyle-HorizontalAlign="Center">  
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>  
                                                            <asp:CheckBox ID="chkSelect" runat="server" CssClass="form-check-input position-static" Checked= '<%# Eval("Selected") %>' AutoPostBack="true" OnCheckedChanged="SelectCheckBox_CheckedChanged" />  
                                                        </ItemTemplate>  
                                                    </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Item">                               
                                                    <ItemStyle HorizontalAlign="Left" />                                  
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPoNum" runat="server" Font-Bold="False" Text='<%#Eval("DerPo")%>'></asp:Label><br />
                                                        Item: <asp:Label ID="lblItem" runat="server" Font-Bold="False" Text='<%#Eval("Item")%>'></asp:Label><br />      
                                                    </ItemTemplate>

                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Tag ID" DataField="TagID" ReadOnly="true" />
                                                <asp:BoundField HeaderText="Lot" DataField="Lot" ReadOnly="true" />
                                                <asp:BoundField HeaderText="Vendor Lot" DataField="VendLot" ReadOnly="true" />
                                                <asp:BoundField HeaderText="Qty" DataField="Qty" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                             </Columns>
                                            </asp:GridView>
                                        
                            </td>
                        </tr>
                    </table>--%>
                  
                </div>
            </div>
        </div>

         <asp:HiddenField ID="AmtTotalFormat" runat="server" />
</asp:Content>
