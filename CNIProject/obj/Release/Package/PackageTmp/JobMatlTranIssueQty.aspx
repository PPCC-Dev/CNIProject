<%@ Page Title="Job Material Transaction Issue Qty" Language="vb" AutoEventWireup="false" MasterPageFile="~/SRN.Master" CodeBehind="JobMatlTranIssueQty.aspx.vb" Inherits="CNIProjet.JobMatlTranIssueQty" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .txt-margin { margin-bottom:5px;}
        .margin-head { margin-bottom:8px;}
        .margin-top { margin-top:-24px;}
        .width-div { width: 50%;}
    </style>

    <script type="text/javascript">

        function scanbarcode(){

            var barcode = document.getElementById('<%=txtbarcode.ClientID%>').value;
            var scantag = document.getElementById('<%=lblbarcode.ClientID%>').innerHTML;
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
            var strim = scantag.trim();

            if (strim == 'Barcode Tag:') {

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
            } else {

                    document.getElementById('<%=txtbarcode.ClientID%>').readOnly = false;
                    document.getElementById("notifyWaiting").innerHTML = "Please Enter....";
                    document.getElementById('<%=Button1.ClientID%>').disabled = false;

            }

        }

    </script>

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
                    <div class="col-md-10">
                        <h6 class="m-0"><a href="Menu.aspx">Menu</a><span class="mx-2 mb-0">/</span><asp:LinkButton ID="LinkButton1" runat="server">Job Material Transaction</asp:LinkButton><span class="mx-2 mb-0">/</span><strong class="text-black"><%: Page.Title %></strong>  </h6>
                    </div>
                        
                </div>
            </div>

            <div class="card-body">
                <div class="row margin-head">

                    <div class="col-md-10">
                    </div>

                    <div class="col-md-2 float-right">
                        <asp:Button ID="btnback" runat="server" class="btn btn-danger btn-sm float-right" Text="Back" UseSubmitBehavior="false" />
                    </div>
                        
                </div>

                <div class="row align-items-center">
                    <div class="col-sm-3 text-right width-div"></div>
                    <div class="col-sm-6 width-div">
                        <p id="notifyWaiting"></p>
                    </div>
                </div>

                <div class="row align-items-center">
                    <div class="col-sm-3 text-right width-div">
                        <asp:Label ID="lblbarcode" runat="server" Text="Scan Location: "></asp:Label>
                    </div>
                    <div class="col-sm-6 width-div">
                        <asp:TextBox ID="txtbarcode" runat="server" AutoComplete="off" class="form-control form-control-sm txt-margin"></asp:TextBox>
                    </div>
                    <div style="margin-top:20px;display: none;">
                        <asp:Button ID="Button1" runat="server" Text="Scan" UseSubmitBehavior="true" />
                    </div>
                </div>

                <div class="row align-items-center mt-2">
                    
                    <div class="col-sm-2 text-right width-div">
                    <span>Location : </span>
                    </div>
                    <div class="col-sm-3 width-div">
                        <asp:DropDownList ID="ddlloc" runat="server" class="form-control form-control-sm txt-margin" AutoPostBack="true"></asp:DropDownList>
                    </div>

                    <div class="col-sm-2 text-right width-div">
                    <span>Lot : </span>
                    </div>
                    <div class="col-sm-3 width-div">
                        <asp:DropDownList ID="ddllot" runat="server" class="form-control form-control-sm txt-margin" AutoPostBack="true"></asp:DropDownList>
                    </div>
                        
                </div>

                <div class="row">
                        <div class="col-sm-12 mt-2">
                            <div class="card">
                                <div class="card-body" style="padding: 1.0rem;">
                                <h6 class="card-title text-primary"><asp:Label ID="lblItem" runat="server"></asp:Label>                                          
                                </h6>
                                <div class="row"> 
                                    <div class="col-sm-12">
                                             <span style="font-size:13px;">Desc : <asp:Label ID="lblItemDesc" runat="server"></asp:Label></span>
                                    </div>                                   
                                </div>   
                                <div class="row"> 
                                    <div class="col-sm-3">
                                             <span style="font-size:13px;">Lot : <asp:Label ID="lblLot" runat="server"></asp:Label></span>
                                    </div>
                                    <div class="col-sm-3">
                                            <span style="font-size:13px;">On Hand : <asp:Label ID="lblQtyOnHand" runat="server" ></asp:Label></span>
                                    </div>
                                </div>
                                <div class="row"> 
                                    <div class="col-sm-3">
                                             <span style="font-size:13px;">Location : <asp:Label ID="lblLoc" runat="server"></asp:Label></span>
                                    </div>
                                    <div class="col-sm-3">
                                            <span style="font-size:13px;">Qty Required : <asp:Label ID="lblQtyRequired" runat="server"></asp:Label></span>
                                    </div>
                                </div>
                                <div class="row"> 
                                    <div class="col-sm-3">
                                             <span style="font-size:13px;">Qty Issued : <asp:Label ID="lblQtyIssue" runat="server"></asp:Label></span>
                                    </div>
                                    <div class="col-sm-3">
                                            <span style="font-size:13px;">Quantity : <asp:Label ID="lblQuantity" runat="server"></asp:Label></span>
                                    </div>
                                </div>
                                <div class="row"> 
                                    <div class="col-sm-3">
                                             <span style="font-size:13px;">
                                                 <asp:TextBox ID="txtQty" runat="server" class="form-control form-control-sm" style="width: 158px; font-size:13px; text-align:right;" AutoComplete="off"></asp:TextBox>
                                             </span>
                                    </div>
                                </div>
                                <div class="row"> 
                                    <div class="col-sm-6 mt-2">
                                            <asp:Button ID="btnconfirm" runat="server" class="btn btn-success btn-sm" Text="Confirm" />&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btncancel" runat="server" class="btn btn-danger btn-sm" Text="Cancel" />
                                    </div>
                                </div>

                                </div>
                            </div>
                        </div>
                  </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="DerPreassignLots" runat="server" />
</asp:Content>
