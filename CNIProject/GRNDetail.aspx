<%@ Page Title="Generate GRN Detail" Language="vb" AutoEventWireup="false" MasterPageFile="~/SRN.Master" CodeBehind="GRNDetail.aspx.vb" Inherits="CNIProjet.GRNDetail" %>
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
                            <h6 class="m-0"><a href="Menu.aspx">Menu</a><span class="mx-2 mb-0">/</span><asp:LinkButton ID="LinkButton1" runat="server">Generate GRN</asp:LinkButton><span class="mx-2 mb-0">/</span><strong class="text-black"><%: Page.Title %></strong>  </h6>
                        </div>
                        
                        <%--<div class="col-md-2 float-right">
                            <asp:Button ID="btnback" runat="server" class="btn btn-danger btn-sm float-right mx-2 mb-0" Text="Back" />
                            <asp:Button ID="btndeletetag" runat="server" class="btn btn-warning btn-sm float-right mx-2 mb-0" Text="Delete" />
                        </div>--%>
                        
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
                                         <asp:Label ID="lblPoNum" runat="server" Font-Bold="False" Text='<%#Eval("DerPo")%>'></asp:Label></br>
                                         <asp:Label ID="lblItem" runat="server" Font-Bold="False" Text='<%#Eval("Item")%>'></asp:Label>
                                     </h6>
                                     <span style="font-size:13px;">Tag ID : <asp:Label ID="lbltagID" runat="server" Font-Bold="False" Text='<%#Eval("TagID")%>'></asp:Label></span><br>
                                     <span style="font-size:13px;">Lot : <%#Eval("Lot")%></span><br>
                                     <span style="font-size:13px;">Vendor Lot : <%#Eval("VendLot")%></span><br>
                                     <span style="font-size:13px;">Qty : <asp:Label ID="lblQtyTag" runat="server" Font-Bold="False" Text='<%#Eval("Qty")%>'></asp:Label></span><br>
                                     <span style="font-size:13px;"class="text-danger"><asp:LinkButton ID="lnkDeleteTag" class="text-danger" runat="server" Font-Bold="true" ToolTip="Click to Delete Tag" Text="Delete Tag"></asp:LinkButton></span>
                                 </div>
                                 </div>
                             </div>
                         </ItemTemplate>
                     </asp:ListView>


                  <%--<div class="col-sm-12 mt-2">
                    <div class="card">
                      <div class="card-body" style="padding: 1.0rem;">
                        <h6 class="card-title text-primary">FHL014BDB01</h6>
                        <span style="font-size:13px;">Desc : FLEECE HARD 014B DASH (1.0)</span><br>
                        <span style="font-size:13px;">Lot: 200408000000001</span><br>
                        <span style="font-size:13px;">Qty: 25.00 | Qty Count : 0.00 | Remain : 0:00</span><br>
                        <span style="font-size:13px;"class="text-danger">Status : Not Count</span>
                      </div>
                    </div>
                  </div>

                  <div class="col-sm-12 mt-2">
                    <div class="card">
                      <div class="card-body" style="padding: 1.0rem;">
                        <h6 class="card-title text-primary">FHL014BDB01</h6>
                        <span style="font-size:13px;">Desc : FLEECE HARD 014B DASH (1.0)</span><br>
                        <span style="font-size:13px;">Lot: 200408000000001</span><br>
                        <span style="font-size:13px;">Qty: 25.00 | Qty Count : 0.00 | Remain : 0:00</span><br>
                        <span style="font-size:13px;"class="text-danger">Status : Not Count</span>
                      </div>
                    </div>
                  </div>--%>

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
</asp:Content>
