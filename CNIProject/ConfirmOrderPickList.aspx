<%@ Page Title="Confirm Order PickList" Language="vb" AutoEventWireup="false" MasterPageFile="~/SRN.Master" CodeBehind="ConfirmOrderPickList.aspx.vb" Inherits="CNIProjet.ConfirmOrderPickList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
        .txt-margin { margin-bottom:5px;}
        .width-div { width: 50%;}

        .btn-customblue {
            color: #fff;
            background-color: #061355;
            border-color: #061355; 
        }
        .btn-customblue:hover, .btn-customblue:focus, .btn-customblue:active, .btn-customblue.active, .open>.dropdown-toggle.btn-customblue {
            color: #fff;
            background-color: #061355;
            border-color: #061355; 
        }

        .items-center {
            display: flex;
            justify-content: center;
            align-items: center;
        }

        img {
          display: block;
          max-width:220px;
          max-height:210px;
          width: auto;
          height: auto;
        }
    </style>


    <script type="text/javascript">

        function right(str, chr) {
          return str.slice(str.length-chr,str.length);
        }

        function scanbarcode(){

            var barcode = document.getElementById('<%=txtbarcode.ClientID%>').value;
            var scantag = document.getElementById('<%=lblbarcode.ClientID%>').innerHTML;
            var strim = scantag.trim();
            var substringbarcode = right(barcode, 13);
            var f = substringbarcode.substring(0, 1);
            var f2 = substringbarcode.substring(1, 2);



            if (strim == 'Scan Tag:') {

                if (f != '|') {

                    if (f == 'J' || f == 'I' || f == 'D') {

                        var s3 = substringbarcode.substring(0, 3);
                        //alert(s3);

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

                    }

                } else if (f == '|') {

                    if (f2 = 'P' || f2 == 'T') {

                        var s2 = substringbarcode.substring(1, 3);

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

                }

            } else {

                document.getElementById('<%=txtbarcode.ClientID%>').readOnly = false;
                document.getElementById("notifyWaiting").innerHTML = "Please Enter....";
                document.getElementById('<%=Button1.ClientID%>').disabled = false;

             }

        }

        function openModel(Item, CustItem, ErrorMsg) {

            $(document).ready(function () {

                document.getElementById("ItemModel").innerHTML = Item;
                document.getElementById("CustItemModel").innerHTML = CustItem;

                if (ErrorMsg == '') {

                    $('#myModel').modal('show');
                    setTimeout(function () {
                        $('#myModel').modal('hide');
                        document.getElementById('<%=txtbarcode.ClientID%>').focus();
                    }, 1000);

                } else {

                    $('#myModel').modal('show');
                }

                           

            });

        }

        $(document).ready(function () {
            var scantag = document.getElementById('<%=lblbarcode.ClientID%>').innerHTML;

            if (scantag == 'Scan Order Pick List: ') {
                $("#<%=txtbarcode.ClientID%>").attr("placeholder", "Scan Order Pick List");
            } else if (scantag == 'Scan Location: ') {
                $("#<%=txtbarcode.ClientID%>").attr("placeholder", "Scan Location");
            } else if (scantag == 'Scan Customer Tag: ') {
                $("#<%=txtbarcode.ClientID%>").attr("placeholder", "Scan Customer Tag");
            } else if (scantag == 'Scan CNI Tag: ') {
                $("#<%=txtbarcode.ClientID%>").attr("placeholder", "Scan CNI Tag");

            }

             
        });

        $(function () {
            $("#<%=RadioConfirm.ClientID%>").change(function () {
                var status = this.checked;
                if (status) {
                    $("#<%=txtbarcode.ClientID%>").focus();

                }
                
            })
        });

        $(function () {
            $("#<%=RadioChange.ClientID%>").change(function () {
                var status = this.checked;
                if (status) {
                    $("#<%=txtbarcode.ClientID%>").focus();

                }
                
            })
        });

        $(function () {
            $("#<%=RadioReject.ClientID%>").change(function () {
                var status = this.checked;
                if (status) {
                    $("#<%=txtbarcode.ClientID%>").focus();

                }
                
            })
        });


            <%--$(function () {
                $("#<%=Button2.ClientID%>").click(function (e) {

                    $('#ItemModel').text("xxxxx")

                    e.preventDefault();


                    $('#myModel').modal('show');
                        setTimeout(function() {
                            $('#myModel').modal('hide');
                        }, 2000);
               
                    });
        
            })--%>

        

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
                            <h6 class="m-0"><%--<a href="Menu.aspx">Menu</a><span class="mx-2 mb-0">/</span>--%><strong class="text-black"><%: Page.Title %></strong>  </h6>
                        </div>

                    </div>
                </div>

                <asp:ScriptManager ID="ScriptManager1" runat="server" ></asp:ScriptManager>
                 <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server" >
                <ContentTemplate>
                       <div class="card-body">

                            <div class="row align-items-center">                  
                            </div>

                            <table  width="100%">
                                <tr>
                                    <div class="row align-items-center">
                                        <%--<div class="col-sm-3 text-right width-div"></div>--%>
                                        <div class="col-sm-6 width-div">
                                            <p id="notifyWaiting"></p>
                                        </div>
                                    </div>
                                </tr>
                                <tr>
                                   <td>
                                        <div class="row mb-2">
                                            <div class="col-md-12">
                                                <div class="form-check form-check-inline">
                                                    <asp:RadioButton ID="RadioConfirm" class="form-check-input" Checked="true"  runat="server" GroupName="Confirm" />
                                                    <label class="form-check-label" for="RadioConfirm">Confirm&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                                                </div>
                                                <div class="form-check form-check-inline">
                                                    <asp:RadioButton ID="RadioChange" class="form-check-input" runat="server" GroupName="Confirm" />
                                                    <label class="form-check-label" for="RadioChange">Change&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                                                </div>
                                                <div class="form-check form-check-inline">
                                                    <asp:RadioButton ID="RadioReject" class="form-check-input" runat="server" GroupName="Confirm" />
                                                    <label class="form-check-label" for="RadioReject">Reject&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                                                </div>
                                            </div>
                                       </div>
                                   </td>
                                </tr>
                                <tr>
                                   <td>
                                       <div class="row mb-2">
                                            <div class="col-md-12">
                                                <asp:Button ID="btnconfirm" runat="server" class="btn btn-customblue btn-sm mx-2 mb-0" Text="Confirm Pick List" UseSubmitBehavior="false" />  
                                                <asp:Button ID="btnreset" runat="server" class="btn btn-danger btn-sm mx-2 mb-0" Text="Cancel" UseSubmitBehavior="false" />
                                    
                                            </div>
                                       </div>
                                   </td>
                               </tr>
                                <tr>
                                   <td>
                                       <div class="form-row">
                                            <div class="col-8">
                                                <div class="input-group mb-3">
                                                    <asp:TextBox ID="txtbarcode" runat="server" AutoComplete="off" class="form-control form-control-sm txt-margin"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="col-4">
                                                <div class="input-group mb-3 ml-4">
                                                    <asp:CheckBox ID="chkCancelTag" class="form-check-input txt-margin" runat="server" AutoPostBack="true" /><span>Cancel Tag</span>
                            
                                                </div>
                                                <div style="margin-top:20px;display: none;">
                                                    <asp:Button ID="Button1" runat="server" Text="Scan" UseSubmitBehavior="true" />
                                                </div>
                                            </div>

                                        </div>
                                    </td>
                               </tr>
                                <tr>
                                   <td>
                                       <div style="display:none;">
                                           <asp:Label ID="lblbarcode" runat="server"></asp:Label>
                                       </div>
                                        
                                   </td>
                               </tr>

                               <tr>
                                   <td>
                                       <div class="row mb-2">
                                            <div id="divScanDetail" runat="server" class="col-sm-12 text-left">
                                                <span><strong>Customer :</strong> <asp:Label ID="lblCustNum" runat="server" Font-Bold="False"></asp:Label>
                                                    &nbsp;&nbsp;<asp:Label ID="lblCustDesc" runat="server" Font-Bold="False"></asp:Label>
                                                    &nbsp;&nbsp;&nbsp;
                                                    <strong>Location :</strong> <asp:Label ID="lblLoc" runat="server" Font-Bold="False"></asp:Label>
                                                </span><br>
                                            </div>
                                       </div> 
                                   </td>
                               </tr>

                               <tr>
                                   <td>
                                       <div id="divPickListDetail" runat="server"  class="row mb-2">
                                            <div class="col-sm-12 text-left">
                                                <span><strong>Pick List No :</strong> <asp:Label ID="lblPickListNo" runat="server" Font-Bold="False"></asp:Label>
                                                    &nbsp;&nbsp;&nbsp;
                                                    <strong>Total Pick Line :</strong> <asp:Label ID="lblTotalPickLine" runat="server" Font-Bold="False"></asp:Label>
                                                    &nbsp;&nbsp;&nbsp;
                                                    <strong>Total Confirm Line :</strong> <asp:Label ID="lblTotalConfirm" runat="server" Font-Bold="False"></asp:Label>
                                                </span><br>
                                            </div>
                                       </div>
                                   </td>
                               </tr>

                                <tr>
                                   <td>
                                       <div class="row mb-2" style="display:none;">
                                            <div class="col-sm-12 text-left">
                                                <span><asp:Label ID="lblScanCustItem" runat="server" Font-Bold="False"></asp:Label>
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lblScanCustPO" runat="server" Font-Bold="False"></asp:Label>
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lblScanQty" runat="server" Font-Bold="False"></asp:Label>
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lblCustItemTag" runat="server" Font-Bold="False"></asp:Label>
                                                </span><br>
                                            </div>
                                       </div>
                                   </td>
                               </tr>
                        
                            </table>

                            <div class="row align-items-center">
                                 <div class="col-sm-12">
                                    <asp:ListView id="PanelList" runat="server">
                                     <ItemTemplate>
                                         <div class="col-sm-12 mt-2">
                                             <div class="card">
                                                 <div class="card-body" style="padding: 1.0rem;">
                                                    <h6 class="card-title text-primary">
                                                        <asp:Label ID="lblderconum" runat="server" Text='<%#Eval("DerCO")%>'></asp:Label>
                                                        <asp:Label ID="lblconum" runat="server" Font-Bold="False" Text='<%#Eval("CoNum")%>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblcoline" runat="server" Font-Bold="False" Text='<%#Eval("CoLine")%>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblcorelease" runat="server" Font-Bold="False" Text='<%#Eval("CoRelease")%>' Visible="false"></asp:Label>
                                                        <span style="font-size:40px;"class="text-success float-right mx-2 mb-0">
                                                            <asp:Label ID="lblconfirm" runat="server" Visible="false"><i class="fa fa-check-circle"></i></asp:Label>
                                                        </span>
                                                    </h6>
                                                 <span style="font-size:13px;">Item : <%#Eval("Item")%></span><br>
                                                 <span style="font-size:13px;">Customer Item : <%#Eval("CustItem")%></span><br>
                                                 <span style="font-size:13px;">Qty CO : <asp:Label ID="lblQtyOrdered" runat="server" Font-Bold="False" Text='<%#Eval("QtyOrder")%>'></asp:Label> | Qty Pick : <asp:Label ID="lblQtyPick" runat="server" Font-Bold="False" Text='<%#Eval("QtyPick")%>'></asp:Label> | Sum Qty : <asp:Label ID="lblSumQty" runat="server" Font-Bold="False" Text='<%#Eval("QtySum")%>'></asp:Label></span><br>
                                                 <span style="font-size:13px;"class="text-danger"><asp:LinkButton ID="lnkDetail" runat="server"  ToolTip="Click to view details" Text='Detail'></asp:LinkButton></span>
                                                </div>
                                             </div>
                                         </div>
                                     </ItemTemplate>
                                 </asp:ListView>
                                 </div>

                             </div>
                           <asp:Button ID="Button2" runat="server" Text="Button" style="display:none;" />
                      </div>
                </ContentTemplate>
                 <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="txtBarcode" EventName="TextChanged" />
                     <asp:PostBackTrigger ControlID="btnconfirm" />
                     <asp:PostBackTrigger ControlID="btnreset" />
                     <asp:PostBackTrigger ControlID="chkCancelTag" />
                     <asp:PostBackTrigger ControlID="Button1" />
                     <asp:PostBackTrigger ControlID="Button2" />
                 </Triggers>
                </asp:UpdatePanel>

                <asp:HiddenField ID="HiddenField1" runat="server" />
                <asp:HiddenField ID="HiddenField2" runat="server" />

                <div class="container">
                    <div class="modal fade" id="myModel" role="dialog" tabindex="-1" aria-hidden="true">
                        <div class="modal-dialog">

                            <div class="modal-content">
                            <div class="modal-header">

                                <h4>&nbsp;Tag Confirm</h4>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                  <span aria-hidden="true">&times;</span>
                                </button>
                            </div>

                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-sm-12">
                                            <div id="StatModelCorrect" class="lebel-text" runat="server" visible="false">
                                                <span class="text-success items-center">
                                                    <asp:Label ID="lblConrectTag" style="font-size:40px;" runat="server" ><i class="fa fa-check-circle"></i></asp:Label>
                                                    &nbsp;&nbsp;<strong><asp:Label ID="lblTextConrectTag" style="font-size:25px;" runat="server" Text="Item Tag Is Correct"></asp:Label></strong>
                                                </span>
                                            </div> 
                                            <div id="StatModelInCorrect" class="lebel-text" runat="server" visible="false">
                                                <span class="text-danger items-center">
                                                    <asp:Label ID="lblInConrectTag" style="font-size:40px;" runat="server" ><i class="fa fa-times-circle"></i></asp:Label>
                                                    &nbsp;&nbsp;<strong><asp:Label ID="lblTextInConrectTag" style="font-size:25px;" runat="server" Text="Item Tag InCorrect"></asp:Label></strong>
                                                </span>
                                                <br />
                                                <h6>
                                                    <span class="items-center">
                                                        <strong><asp:Label ID="lblmsg" runat="server"></asp:Label></strong>
                                                    </span>
                                                </h6>
                                                
                                            </div>
                                    
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-sm-3 lebel-text text-right width-div">
                                        <p>Item : </p>
                                    </div>
                                    <div class="col-sm-6 text-left width-div">
                                       <p id="ItemModel" class="lebel-text"></p>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 lebel-text text-right width-div">
                                        <p>Customer Item : </p>
                                    </div>
                                    <div class="col-sm-6 text-left width-div">
                                       <p id="CustItemModel" class="lebel-text"></p>
                                    </div>

                                </div>

                                <div class="row">
                                    <div class="col-sm-12 d-flex justify-content-center">
                                        <asp:Image ID="Image1" CssClass="img" runat="server"  />
                                    </div>
                                    
                                </div>
                                
                                <div class="row">
                                   <div class="col-sm-12 d-flex justify-content-center">
                                        <asp:Image ID="Image2" CssClass="img" runat="server" />
                                    </div>
                                </div>

                            </div>

                            </div>

                        </div>
                    </div>
                 </div>
            </div>

        </div>

        <asp:HiddenField ID="HidItemModel" runat="server" />
        <asp:HiddenField ID="HidCustItemModel" runat="server" />
</asp:Content>
