<%@ Page Title="PO Receiving" Language="vb" AutoEventWireup="false" MasterPageFile="~/SRN.Master" CodeBehind="POReceiving.aspx.vb" Inherits="CNIProjet.POReceiving" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .txt-margin { margin-bottom:5px;}
        .width-div { width: 50%;}
        .width-div3 { width: 15%;}
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

    <script type="text/javascript">

        function scanbarcode(){

            var barcode = document.getElementById('<%=txtbarcode.ClientID%>').value;
            var scantag = document.getElementById('<%=lblbarcode.ClientID%>').innerHTML;
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
            var strim = scantag.trim();
            

            if (strim == 'Scan Tag :') {

                if (f == 'J' || f == 'I' || f == 'D') {

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

                }
            } else if (strim == 'Scan Delivery Sheet :') {

                document.getElementById('<%=txtbarcode.ClientID%>').readOnly = true;
                document.getElementById("notifyWaiting").innerHTML = "Waiting....";
                document.getElementById('<%=Button1.ClientID%>').click();
                document.getElementById('<%=Button1.ClientID%>').disabled = true;

            } else if (strim == 'Scan Lines :') {

                document.getElementById('<%=txtbarcode.ClientID%>').readOnly = true;
                document.getElementById("notifyWaiting").innerHTML = "Waiting....";
                document.getElementById('<%=Button1.ClientID%>').click();
                document.getElementById('<%=Button1.ClientID%>').disabled = true;
            }
            else {

                    document.getElementById('<%=txtbarcode.ClientID%>').readOnly = false;
                    document.getElementById("notifyWaiting").innerHTML = "Please Enter....";
                    document.getElementById('<%=Button1.ClientID%>').disabled = false;

            }

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
                        <div class="col-md-8">
                            <h6 class="m-0"><a href="Menu.aspx">Menu</a><span class="mx-2 mb-0">/</span> <strong class="text-black"><%: Page.Title %></strong>  </h6>
                        </div>

                        <%--<div class="col-md-4 float-right">
                            <asp:Button ID="btndetail" runat="server" class="btn btn-warning btn-sm float-right mx-2 mb-0"  Text="Detail" UseSubmitBehavior="false" />
                            <asp:Button ID="btnreset" runat="server" class="btn btn-info btn-sm float-right mx-2 mb-0"  Text="Reset" UseSubmitBehavior="false" />
                            <asp:Button ID="btnprocess" runat="server" class="btn btn-success btn-sm float-right mx-2 mb-0" Text="Process" UseSubmitBehavior="false" />
                            
                        </div>--%>
                    </div>
                
                </div>

             <div class="card-body">
                 <div class="row">
                        <div class="col-md-8">
                        </div>

                        <div class="col-md-4 float-right">
                            <asp:Button ID="btndetail" runat="server" class="btn btn-warning btn-sm float-right mx-2 mb-0"  Text="Detail" UseSubmitBehavior="false" />
                            <asp:Button ID="btnreset" runat="server" class="btn btn-info btn-sm float-right mx-2 mb-0"  Text="Reset" UseSubmitBehavior="false" />
                            <asp:Button ID="btnprocess" runat="server" class="btn btn-success btn-sm float-right mx-2 mb-0" Text="Process" UseSubmitBehavior="false" />
                             
                            
                        </div>
                 </div>
                 <div class="row align-items-center">                  
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
                                    <asp:Label ID="lblbarcode" runat="server"></asp:Label>
                                </div>
                                <div class="col-sm-6 width-div">
                                    <asp:TextBox ID="txtbarcode" runat="server" AutoComplete="off" class="form-control form-control-sm txt-margin"></asp:TextBox>
                                </div>
                                <div style="margin-top:20px;display: none;">
                                    <asp:Button ID="Button1" runat="server" Text="Scan" UseSubmitBehavior="true" />
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
                                    <div class="col-sm-6 width-div">
                                    <asp:TextBox ID="txtdate" runat="server" class="form-control form-control-sm txt-margin" ReadOnly="True" AutoComplete="off"></asp:TextBox>                       
               
                                    </div>

                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <div class="row align-items-center">
                                <div class="col-sm-3 text-right width-div">
                                    <span>PO No. : </span>
                                </div>
                                <div class="col-sm-2 width-div">
                                    <asp:TextBox ID="txtPONo" runat="server" class="form-control form-control-sm txt-margin" ReadOnly="True"></asp:TextBox>
                                </div>
                                <%--<div class="col-sm-2 text-right width-div">
                                    <span>Location : </span>
                                </div>
                                <div class="col-sm-2 width-div">
                                    <asp:TextBox ID="txtLoc" runat="server" class="form-control form-control-sm txt-margin"></asp:TextBox>
                                </div>--%>
                                <div class="col-sm-2 text-right width-div">
                                    <span>Whse : </span>
                                </div>
                                <div class="col-sm-2 width-div">
                                    <asp:TextBox ID="txtWhse" runat="server" class="form-control form-control-sm txt-margin" ReadOnly="True"></asp:TextBox>
                                </div>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <div class="row align-items-center">
                                    <div class="col-sm-3 text-right width-div">
                                        <span>Vendor : </span>
                                    </div>
                                    <div class="col-sm-6 width-div">
                                    <asp:TextBox ID="txtvendname" runat="server" class="form-control form-control-sm txt-margin" ReadOnly="True" AutoComplete="off"></asp:TextBox>  
                                  
               
                                    </div>

                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <div class="row align-items-center">
                                    <div class="col-sm-3 text-right width-div">
                                        <span>Vendor Invoice : </span>
                                    </div>
                                    <div class="col-sm-6 width-div">
                                    <asp:TextBox ID="txtvendVendInv" runat="server" class="form-control form-control-sm txt-margin" AutoComplete="off"></asp:TextBox>
               
                                    </div>

                                </div>
                            </td>
                        </tr>

                        <%--<tr>
                            <td>
                                <div class="row align-items-center">
                                    <div class="col-sm-3 text-right width-div">
                                        <span>Whse : </span>
                                    </div>
                                    <div class="col-sm-6 width-div">
                                    <asp:TextBox ID="txtWhse" runat="server" class="form-control form-control-sm txt-margin" ReadOnly="True"></asp:TextBox>
               
                                    </div>

                                </div>
                            </td>
                        </tr>--%>

                        <tr>
                            <td>
                                <div class="row align-items-center">
                                    <div class="col-sm-3 text-right width-div">
                                        <span>Total DS Line : </span>
                                    </div>
                                    <div class="col-sm-6 width-div">
                                     <asp:TextBox ID="txtTotalLine" runat="server" class="form-control form-control-sm txt-margin" ReadOnly="True" AutoComplete="off"></asp:TextBox> 
               
                                    </div>

                                </div>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                <div class="row align-items-center mt-2">
                                    <div class="col-sm-3 text-right width-div"></div>
                                    <div class="col-sm-9 text-left">
                                        <span><strong>Invoice Amt :</strong> <asp:Label ID="lblInvoceAmt" runat="server" Font-Bold="False"></asp:Label>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <strong>VAT :</strong> <asp:Label ID="lblVAT" runat="server" Font-Bold="False"></asp:Label>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <strong>Total Invoice :</strong> <asp:Label ID="lblTotalInvoice" runat="server" Font-Bold="False"></asp:Label>
                                        </span><br />
                                    </div>
                                </div>
                            </td>
                        </tr>
                           

                        <%--<tr>
                            <td>
                                <div class="row align-items-center">
                                    <div class="col-sm-3 text-right width-div">

                                    </div>
                                    <div class="col-sm-6 width-div">
                                        <asp:Button ID="btnvendorlot" runat="server" class="btn btn-primary btn-sm mb-0"  Text="VendorLot" UseSubmitBehavior="false" />
               
                                    </div>

                                </div>
                                
                            </td>
                        </tr>--%>
                    </table>
                    <br />

                    <div class="row align-items-center">

                <div class="col-sm-3"></div>

                    <div class="col-sm-6">
                        <asp:ListView id="PanelList" runat="server">
                        <ItemTemplate>
                            <asp:Panel ID="Panel1" class="card card-body mb-3" style="max-width: 100%; font-size:13px;" runat="server">
                                <div class="card-text card-body-font">
                                <div class="row">                        
                                    <div class="col-sm-3">
                                        <asp:Label ID="lblListItem" runat="server" Text='<%#Eval("Item")%>'></asp:Label>
                                    </div>  
                                    <div class="col-sm-9">
                                        <asp:Label ID="lblListItemDesc" runat="server" Text='<%#Eval("Description")%>'></asp:Label>
                                        <span style="font-size:27px;"class="text-success float-right mx-2 mb-0">
                                            <asp:Label ID="lblconfirm" runat="server" Visible="false"><i class="fa fa-check-circle"></i></asp:Label>
                                        </span>
                                    </div>                          
                                </div>

                                <div class="row"> 
                                    <div class="col-sm-6">
                                            <strong>Qty Order: </strong> <asp:Label ID="lblListQtyOrder" runat="server" Text='<%#Eval("QtyOrder")%>'></asp:Label> 
                                            <asp:Label ID="lblListPoUM" runat="server" Text='<%#Eval("PoUM")%>'></asp:Label>
                                    </div>
                                    <div class="col-sm-6">
                                            <strong>Qty Received: </strong> <asp:Label ID="lblListQtyReceived" runat="server" Text='<%#Eval("QtyReceived")%>'></asp:Label>
                                    </div>
                                </div>

                                <div class="row">                        
                                    <div class="col-sm-6">
                                        <strong>Qty Remain: </strong> <asp:Label ID="lblListRemain" runat="server" Text='<%#Eval("QtyRemain")%>'></asp:Label>
                                    </div>
                                    <div class="col-sm-6">
                                        <strong>Receive Qty: </strong> <asp:Label ID="lblListSumQty" runat="server" Text='<%#Eval("ReceiveQtyConv")%>'></asp:Label>
                                    </div>
                                </div>

                                <div class="row">                        
                                    <div class="col-sm-6">
                                        <strong>Lot No.: </strong> <asp:Label ID="lblListLot" runat="server" Text='<%#Eval("Lot")%>'></asp:Label>
                                    </div>
                                    <div class="col-sm-6">
                                        <strong>Loc: </strong> <asp:Label ID="lblListLoc" runat="server" Text='<%#Eval("Loc")%>'></asp:Label>
                                    </div>
                                    
                                </div>

                                <div class="row">                        
                                    <div class="col-sm-6">
                                        <strong>Qty Delivery: </strong> <asp:Label ID="lblQtyDelivery" runat="server" Text='<%#Eval("QtyDelivery")%>'></asp:Label>
                                    </div>
                                    <div class="col-sm-6">
                                        <%--<strong>Loc: </strong> <asp:Label ID="Label2" runat="server" Text='<%#Eval("Loc")%>'></asp:Label>--%>
                                    </div>
                                    
                                </div>

                                <div class="row">                        
                                    <div class="col-sm-6">
                                        <p class="text-primary"><strong>Material Cost: </strong> <asp:Label ID="lblListMatlCost" runat="server" Text='<%#Eval("UnitMatCostConv")%>'></asp:Label></p>
                                    </div>
                                    <div class="col-sm-6">
                                        <p class="text-primary"><strong>Line Amt: </strong> <asp:Label ID="lblListLineAmt" runat="server" Text='<%#Eval("LineAmt")%>'></asp:Label></p>
                                    </div>
                                </div>

                            </div>
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:ListView>

                    </div>
                    
                </div>
             </div>
        </div>
    </div>

    <asp:Label ID="lblBarcodeDSNum" runat="server" ForeColor="White"></asp:Label>
    <asp:Label ID="lblWebDeliveryReceive" runat="server" ForeColor="White"></asp:Label>
    <asp:HiddenField ID="CountScanLine" runat="server" Value="0" />
    <asp:HiddenField ID="AmtTotalFormat" runat="server" />
</asp:Content>
