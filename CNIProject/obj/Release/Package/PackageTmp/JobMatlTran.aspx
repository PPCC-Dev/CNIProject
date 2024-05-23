<%@ Page Title="Job Material Transaction" Language="vb" AutoEventWireup="false" MasterPageFile="~/SRN.Master" CodeBehind="JobMatlTran.aspx.vb" Inherits="CNIProjet.JobMatlTran" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .txt-margin { margin-bottom:5px;}
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
                        <div class="col-md-9">
                            <h6 class="m-0"><a href="Menu.aspx">Menu</a><span class="mx-2 mb-0">/</span> <strong class="text-black"><%: Page.Title %></strong>  </h6>
                        </div>

                        <%--<div class="col-md-3 float-right">
                            <asp:Button ID="btnprocess" runat="server" class="btn btn-success btn-sm float-right mx-2 mb-0" Text="Process" UseSubmitBehavior="false" />
                            <asp:Button ID="btnreset" runat="server" class="btn btn-info btn-sm float-right mx-2 mb-0"  Text="Reset" UseSubmitBehavior="false" />
                            <asp:Button ID="btndetail" runat="server" class="btn btn-warning btn-sm float-right mx-2 mb-0"  Text="Detail" UseSubmitBehavior="false" />
                        </div>--%>
                    </div>
                  
                </div>
                <div class="card-body">

                    <div class="row">
                        <div class="col-md-9"></div>

                        <div class="col-md-3 float-right">
                            <asp:Button ID="btndetail" runat="server" class="btn btn-warning btn-sm float-right mx-2 mb-0"  Text="Detail" UseSubmitBehavior="false" />
                            <asp:Button ID="btnreset" runat="server" class="btn btn-info btn-sm float-right mx-2 mb-0"  Text="Reset" UseSubmitBehavior="false" />
                            <asp:Button ID="btnprocess" runat="server" class="btn btn-success btn-sm float-right mx-2 mb-0" disabled ="disabled" Text="Process" UseSubmitBehavior="false" />
                            
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
                                    <div class="col-sm-3"></div>

                                    <div class="col-sm-6 text-right" style="margin-bottom:7px;"> 
                                        <asp:LinkButton runat="server" ID="btnstat" class="btn btn-outline-success btn-block btn-sm" aria-hidden="true" AutoPostBack="true"> <i class="fa fa-arrow-right" aria-hidden="true"></i><strong> Issue</strong> </asp:LinkButton>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="row align-items-center checkbox-margin">
                                    <div class="col-sm-3 text-right width-div">               
                                    </div>
                                    <div class="col-sm-4 width-div">
                                         &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkCancelTag" AutoPostBack="true" class="form-check-input txt-margin" runat="server" /><spen>Cancel Tag</spen>
                                    </div> 
                                     <div class="col-sm-3 width-div"></div>            
                                </div>
                            </td>
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
                                <div class="col-sm-2 width-div">
                                    <asp:TextBox ID="txtdate" runat="server" class="form-control form-control-sm txt-margin datepicker" AutoComplete="off"></asp:TextBox>
                                </div>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <div class="row align-items-center">
                                    <div class="col-sm-3 text-right width-div">
                                        <span>Job Order : </span>
                                    </div>
                                    <div class="col-sm-2 width-div">
                                    <asp:TextBox ID="txtjob" runat="server" class="form-control form-control-sm txt-margin" ReadOnly="True"></asp:TextBox>
                          
               
                                    </div>
                                    <div class="col-sm-2 text-right width-div">
                                         <span>Suffix : </span>
                                    </div>
                                    <div class="col-sm-2 text-left width-div">
                                       <asp:TextBox ID="txtsuffix" runat="server" class="form-control form-control-sm txt-margin" ReadOnly="True"></asp:TextBox>
              
                                    </div>

                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="row align-items-center">
                                    <div class="col-sm-3 text-right width-div">
                                        <span>Operation : </span>
                                    </div>
                                    <div class="col-sm-2 width-div">
                                        <asp:TextBox ID="txtOperNum" runat="server" class="form-control form-control-sm txt-margin" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2 text-right width-div">
                                         <span>Work Center : </span>
                                    </div>
                                    <div class="col-sm-2 text-left width-div">
                                       <asp:TextBox ID="txtWC" runat="server" class="form-control form-control-sm txt-margin" ReadOnly="True"></asp:TextBox>
              
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                               
                            </td>
                        </tr>
                    </table>
                    <br />

                <div class="row align-items-center">

                <div class="col-sm-3"></div>

                <div class="col-sm-6">
                    <asp:ListView id="PanelList" runat="server">
                        <ItemTemplate>
                            <asp:Panel ID="Panel1" class="card card-body mb-3" style="max-width: 100%;" runat="server">
                                <div class="card-text card-body-font">

                                <div class="row">
                                    <div class="col-sm-12 float-right">
                                            <strong><asp:Label CssClass="float-right mx-2 mb-0 text-danger" ID="lblScanTagJobMalt" runat="server" Text='<%#Eval("ScanTag")%>'></asp:Label></strong>
                                    </div>
                                </div>
                                <div class="row">                        
                                    <%--<div class="col-sm-6">
                                        <strong>Operation: </strong><asp:Label ID="lblOperNum" runat="server" Text='<%#Eval("Oper")%>'></asp:Label>
                                    </div>--%>  
                                    <div class="col-sm-6">
                                            <strong>Item: </strong> <asp:Label ID="lblListItem" runat="server" Text='<%#Eval("Item")%>'></asp:Label>
                                    </div>
                                    <div class="col-sm-6">
                                            <asp:Label ID="lblReason" runat="server" Visible="false" Font-Bold="true" Text="Reason: "></asp:Label> <asp:Label ID="lblListReason" runat="server" Visible="false" Text='<%#Eval("Reason")%>'></asp:Label>
                                    </div> 
                                </div>

                                <div class="row"> 
                                    <div class="col-sm-6">
                                            <asp:Label ID="lblQtyRequire" runat="server" Visible="false" Font-Bold="true" Text="Qty Require: "></asp:Label> <asp:Label ID="lblListQtyReq" runat="server" Visible="false" Text='<%#Eval("QtyRequire")%>'></asp:Label>
                                            <asp:Label ID="lblLoc" runat="server" Visible="false" Font-Bold="true" Text="Loc: "></asp:Label> <asp:Label ID="lblListLoc" runat="server" Visible="false" Text='<%#Eval("Loc")%>'></asp:Label>
                                    </div>
                                    <div class="col-sm-6">
                                            <strong>Qty Issue: </strong> <asp:Label ID="lblListQtyIssue" runat="server" Text='<%#Eval("QtyIssue")%>'></asp:Label>
                                    </div>
                                </div>

                                <div class="row">                        
                                    <div class="col-sm-6">
                                        <asp:Label ID="lblQtyRemain" runat="server" Visible="false" Font-Bold="true" Text="Qty Remain: "></asp:Label> <asp:Label ID="lblListRemain" Visible="false" runat="server" Text='<%#Eval("QtyRemain")%>'></asp:Label>
                                        <asp:Label ID="lblLot" runat="server" Visible="false" Font-Bold="true" Text="Lot: "></asp:Label> <asp:Label ID="lblListLot" runat="server" Visible="false" Text='<%#Eval("Lot")%>'></asp:Label>
                                    </div>
                                    <div class="col-sm-6">
                                        <strong>Qty Sum: </strong> <asp:Label ID="lblListQtySum" runat="server" Text='<%#Eval("QtySum")%>'></asp:Label>
                                    </div>
                                </div>

                                <div class="row">                        
                                    <div class="col-sm-6">
                                        <strong><asp:LinkButton ID="lnkIssueQty" runat="server" PostBackUrl="#" Visible="false" CommandName="LinkIssueQty">Issue Qty</asp:LinkButton></strong>
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

</asp:Content>
