<%@ Page Title="Cycle Count" Language="vb" AutoEventWireup="false" MasterPageFile="~/SRN.Master" CodeBehind="CycleCount.aspx.vb" Inherits="CNIProjet.CycleCount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .txt-margin { margin-bottom:5px;}
        .width-div { width: 50%;}   
        .margin-head { margin-bottom:8px;}
    </style>

    <script type="text/javascript">

        function scanbarcode(){

            var barcode = document.getElementById('<%=txtbarcode.ClientID%>').value;
            var ArrayBarcode = barcode.split("|");
            var TagID = ArrayBarcode[5];
            var TagID_TD = ArrayBarcode[4];
            var TagID_PO = ArrayBarcode[8];

            if (TagID_TD != undefined && TagID_TD.substring(0,2) == 'TD') {
                var TagID = TagID_TD
            }

            if (TagID_PO != undefined && (TagID_PO.substring(0, 2) == 'PO' || TagID_PO.substring(0, 3) == 'INV')) {
                var TagID = TagID_PO

            }

            var f = TagID.substring(0, 1);
            var s3 = TagID.substring(0, 3);
            var s2 = TagID.substring(0, 2);


            if (f == 'J' || f == 'I' || f == 'D')  {

                if ((s3 == 'JOB' || s3 == 'INV' || s3 == 'DEL') && (barcode.length > 10)) {
                    document.getElementById('<%=txtbarcode.ClientID%>').readOnly = true;
                    document.getElementById("notifyWaiting").innerHTML = "Waiting....";
                    document.getElementById('<%=Button1.ClientID%>').click();
                    document.getElementById('<%=Button1.ClientID%>').disabled = true;

                } else {

                        document.getElementById('<%=txtbarcode.ClientID%>').readOnly = false;
                        document.getElementById("notifyWaiting").innerHTML = "Waiting....";
                        document.getElementById('<%=Button1.ClientID%>').disabled = false;
                }

            } else if (f == 'P' || f == 'T') {

                if ((s2 == 'PO' || s2 == 'TD') && (barcode.length > 10)) {

                    document.getElementById('<%=txtbarcode.ClientID%>').readOnly = true;
                    document.getElementById("notifyWaiting").innerHTML = "Waiting....";
                    document.getElementById('<%=Button1.ClientID%>').click();
                    document.getElementById('<%=Button1.ClientID%>').disabled = true;

                } else {

                         document.getElementById('<%=txtbarcode.ClientID%>').readOnly = false;
                        document.getElementById("notifyWaiting").innerHTML = "Waiting....";
                        document.getElementById('<%=Button1.ClientID%>').disabled = false;
                }

            } else {

                    document.getElementById('<%=txtbarcode.ClientID%>').readOnly = false;
                    document.getElementById("notifyWaiting").innerHTML = "Please Enter....";
                    document.getElementById('<%=Button1.ClientID%>').disabled = false;

            }

        }

    </script>

    <script>

        function ShowSweetAlert(type, msg, icon) {

            if (type === 'Prompt') {

                Swal.fire({
                    title: '',
                    text: msg,
                    type: "question",
                    icon: icon,
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Yes'
                }).then((result) => {
                    if (result.value) {
                        document.getElementById('<%=txtprocess.ClientID%>').value = "I";
                        scanbarcode();


                    }else if (result.dismiss === swal.DismissReason.cancel) {    
                        document.getElementById('<%=txtbarcode.ClientID%>').focus();
                        document.getElementById('<%=txtbarcode.ClientID%>').value = "";
 	            }
                })
            } else {
                Swal.fire(
                    type,
                    msg,
                    icon
                ).then(function () {
                    window.setTimeout(function () {
                        document.getElementById('<%=txtbarcode.ClientID%>').focus();
                }, 0);
                });
            }


        }

       </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <br />
        <div class="se-pre-con"></div>
        <div class="col-lg-12">
            <div class="card border-primary">
                <div class="card-header py-3">
                  <h6 class="m-0"><a href="Menu.aspx">Menu</a><span class="mx-2 mb-0">/</span> <strong class="text-black"><%: Page.Title %></strong>  </h6>
                </div>
                <div class="card-body">

                    <div class="row align-items-center">                  
                        <div class="col-sm-12" id="notify">

                            <%--<asp:Panel ID="NotPassNotifyPanel" runat="server" CssClass= "alert alert-danger alert-dismissable" Visible="false">
                                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                <asp:Literal ID="NotPassText" runat="server"></asp:Literal>
                            </asp:Panel>
                            <asp:Panel ID="PassNotifyPanel" runat="server" CssClass= "alert alert-success alert-dismissable" Visible="false">
                                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                <asp:Literal ID="PassText" runat="server"></asp:Literal>
                            </asp:Panel>
                            <asp:Panel ID="WarningNotifyPanel" runat="server" CssClass= "alert alert-warning alert-dismissable" Visible="false">
                                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                <asp:Literal ID="WarningText" runat="server"></asp:Literal>
                            </asp:Panel>--%>

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
                                <div class="row align-items-center">
                                <div class="col-sm-3 text-right width-div">
                                    <span>Warehouse : </span>
                                </div>
                                <div class="col-sm-2 width-div">
                                    <asp:DropDownList ID="ddlwhse" runat="server" class="form-control form-control-sm txt-margin" required></asp:DropDownList>
                                </div>
                                <div class="col-sm-2 text-right width-div">
                                    <span>Product Code : </span>
                                </div>
                                <div class="col-sm-2 width-div">
                                    <%--<asp:DropDownList ID="ddlloc" runat="server" class="form-control form-control-sm txt-margin" AutoPostBack="true" required></asp:DropDownList>--%>

                                    <asp:DropDownList ID="ddlProductCode" runat="server" class="form-control form-control-sm txt-margin" AutoPostBack="true" required></asp:DropDownList>
                                </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="row align-items-center">
                                <div class="col-sm-3 text-right width-div">
                                    <span>Scan Tag : </span>
                                </div>
                                <div class="col-sm-6 width-div">
                                    <asp:TextBox ID="txtbarcode" runat="server" AutoComplete="off" class="form-control form-control-sm txt-margin"></asp:TextBox>
                                </div>
                                <div style="margin-top:20px;display: none;">
                                    <asp:Button ID="Button1" runat="server" Text="Scan" UseSubmitBehavior="true" />
                                    <asp:TextBox ID="txtprocess" runat="server" AutoComplete="off" class="form-control form-control-sm txt-margin"></asp:TextBox>
                                </div>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <div class="row align-items-center">
                                <div class="col-sm-3 text-right width-div">
                                    <span>Date : </span>
                                </div>
                                <div class="col-sm-2 width-div">
                                    <asp:TextBox ID="txtdate" runat="server" class="form-control form-control-sm txt-margin datepicker" AutoComplete="off" disabled = "disabled"></asp:TextBox>
                                    
                                </div>
                                <div class="col-sm-2 text-right width-div">
                                    <span>Location : </span>
                                </div>
                                <div class="col-sm-2 width-div">
                                    <asp:DropDownList ID="ddlLoc" runat="server" class="form-control form-control-sm txt-margin" AutoPostBack="true" required>
                                        <asp:ListItem Text="ALL" Value="A"></asp:ListItem>
                                        <asp:ListItem Text="Non-Scan Tag" Value="N"></asp:ListItem>
                                        <asp:ListItem Text="Scan Tag" Value="Y"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                                </div>
                            </td>
                        </tr>
                        <%--<tr>
                            <td>
                                <div class="row align-items-center">
                                <div class="col-sm-3 text-right width-div">
                                </div>
                                <div class="col-sm-2 width-div">
                                    <asp:CheckBox ID="CheckBox1" AutoPostBack="true" class="form-check-input txt-margin" runat="server" /><span>Cancel Tag</span>
                                </div>
                                </div>
                            </td>
                        </tr>--%>
                        <%--<tr>
                            <td>
                                <div class="row align-items-center" style="margin-top:10px; margin-bottom:10px;">
                                    <div class="col-sm-3"> </div>
                                    <div class="col-sm-6 text-center">
                                        <asp:Button ID="btnprocess" runat="server" class="btn btn-success btn-sm" Text="Process" />&nbsp;&nbsp;

                                        <asp:Button ID="btnreset" runat="server" class="btn btn-info btn-sm"  Text="Reset" />&nbsp;&nbsp;
                                        <asp:Button ID="btndetail" runat="server" class="btn btn-warning btn-sm"  Text="Detail" />
                                    </div>
                                </div>
                            </td>
                        </tr>--%>
                    </table>
                    <br />

     <div class="row">

         <asp:ListView id="PanelList" runat="server">
             <ItemTemplate>
                 <div class="col-sm-12 mt-2">
                     <div class="card">
                         <div class="card-body" style="padding: 1.0rem;">
                         <h6 class="card-title text-primary"><asp:Label ID="lblItem" runat="server" Text='<%#Eval("Item")%>'></asp:Label></h6>
                         
                         <span style="font-size:13px;">Desc : <%#Eval("Description")%></span><br>
                         <span class="width-div" style="font-size:13px;">Location : <asp:Label ID="lblLoc" runat="server" Font-Bold="False" Text='<%#Eval("Loc")%>'></asp:Label></span>&nbsp;&nbsp;&nbsp;
                         <span class="width-div" style="font-size:13px;"><asp:CheckBox ID="chkNonScanTag" class="form-check-input float-right mx-2 mb-0" runat="server" Enabled="false" checked='<%# Eval("Uf_loc_NonScanTag") %>' /> <spen>Non-Scan Tag</spen></span><br>
                         <span style="font-size:13px;">Lot : <asp:Label ID="lblLot" runat="server" Font-Bold="False" Text='<%#Eval("Lot")%>'></asp:Label></span><br>
                         <span style="font-size:13px;">Qty : <asp:Label ID="lblqty" runat="server" Text='<%#Eval("CutOffQty")%>'></asp:Label>
                             | Qty Count : <asp:Label ID="lblqtycount" runat="server" Text='<%#Eval("CountQty")%>'></asp:Label>
                             | Remain : <asp:Label ID="lblremain" runat="server" Text='<%#Eval("CountQty")%>'></asp:Label></span><br>
                         <span style="font-size:13px;">Status : <strong><asp:Label ID="lblstat" runat="server" Text='<%#Eval("Stat")%>'></asp:Label></strong></span><br />
                         <span style="font-size:13px;"><asp:LinkButton ID="lnkTagDetail"  CssClass="mr-2" CommandName="TagDetail" Visible="false" OnClick="Display" runat="server" ToolTip="Click to view details" Text="Tag Detail"></asp:LinkButton></span>
                         <span style="font-size:13px;"><asp:LinkButton ID="lnkCount" CommandName="Counted" runat="server"></asp:LinkButton></span> 
                     </div>
                     </div>
                 </div>
             </ItemTemplate>
         </asp:ListView>

    </div>


                    <%--<div class="row align-items-center">
                        <div class="col-sm-2"></div>
                        <div class="col-sm-8 width-div">
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" 
                                    CssClass="table table-striped table-bordered table-hover" 
                                    ShowFooter="false" ShowHeader="true" Font-Size="Small">
                                <Columns>
                                <asp:TemplateField HeaderText="Item">  
                              
                                    <ItemStyle HorizontalAlign="Left" />                                  
                                    <ItemTemplate>
                                        <asp:Label ID="lblItem" runat="server" Font-Bold="False" Text='<%#Eval("Item")%>'></asp:Label><br />
                                        Desc: <asp:Label ID="lblItemDesc" runat="server" Font-Bold="False" Text='<%#Eval("Description")%>'></asp:Label><br />
                                        Lot: <asp:Label ID="lblLot" runat="server" Font-Bold="False" Text='<%#Eval("Lot")%>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Qty" DataField="CutOffQty" ReadOnly="true" ItemStyle-CssClass="text-right" />
                                <asp:BoundField HeaderText="Qty Count" DataField="CountQty" ReadOnly="true" ItemStyle-CssClass="text-right" />
                                <asp:TemplateField HeaderText="Remain">  
                              
                                    <ItemStyle HorizontalAlign="Right" />                                  
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemain" runat="server" Font-Bold="False"></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                    
                                <asp:TemplateField HeaderText="Status">  
                              
                                    <ItemStyle HorizontalAlign="Left" />                                  
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkStatus" OnClick="Display" runat="server"  ToolTip="Click to view details" Text='<%# Eval("Stat") %>'></asp:LinkButton>
                                        
                                    </ItemTemplate>

                                </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                                
                    </div>--%>
                  
                </div>
            </div>



        </div>


</asp:Content>
