<%@ Page Title="Order Shipping" Language="vb" AutoEventWireup="false" MasterPageFile="~/SRN.Master" CodeBehind="OrderShipping.aspx.vb" Inherits="CNIProjet.OrderShipping" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .txt-margin { margin-bottom:5px;}
        .width-div { width: 50%;}
        .width-div3 { width: 15%;}

        img {
          display: block;
          max-width:220px;
          max-height:210px;
          width: auto;
          height: auto;
        }

        .items-center {
            display: flex;
            justify-content: center;
            align-items: center;
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
            var ShowPic = document.getElementById('<%=HiddenField1.ClientID%>').value;
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

                            <%--if (ShowPic == "1") {
                                document.getElementById('<%=Button2.ClientID%>').click();

                            }--%>
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

                            <%--if (ShowPic == "1") {
                                document.getElementById('<%=Button2.ClientID%>').click();

                            }--%>
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

                            <div class="row">
                                <div class="col-md-9">
                           
                                </div>

                                <div class="col-md-3 float-right">
                                    <asp:Button ID="btnreset" runat="server" class="btn btn-danger btn-sm float-right mx-2 mb-0"  Text="Reset" UseSubmitBehavior="false" />
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
                                            <div class="col-sm-3"></div>

                                            <div class="col-sm-6 text-right" style="margin-bottom:7px;"> 
                                                <asp:LinkButton runat="server" ID="btnstat" class="btn btn-outline-success btn-block btn-sm" aria-hidden="true" AutoPostBack="true">
                                                    <i class="fa fa-arrow-right" aria-hidden="true"></i> <strong>Order Shipping</strong>
                                                </asp:LinkButton>
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
                                                 &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkCancelTag" AutoPostBack="true" class="form-check-input txt-margin" runat="server" Visible="False" /><spen style="display:none;">Cancel Tag</spen>
                                            </div> 
                                             <div class="col-sm-3 width-div"></div>            
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="row">
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
                                        <div class="row">
                                            <div class="col-sm-3 text-right width-div">
                                                <span>Order Pick List No : </span>
                                            </div>
                                            <div class="col-sm-2 width-div">
                                                <asp:TextBox ID="txtpicklistno" runat="server" class="form-control form-control-sm txt-margin" ReadOnly="True"></asp:TextBox>                       
                                            </div>
                                            <div class="col-sm-3 text-right width-div">
                                            </div>
                                            <%--<div class="col-sm-2 width-div">
                                                <asp:CheckBox ID="ChkPreInv" runat="server" class="form-check-input txt-margin" Enabled="false" /> <spen>Pre-Invoice</spen>
               
                                            </div>--%>

                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="row">
                                            <div class="col-sm-3 text-right width-div">
                                                <span>Customer : </span>
                                            </div>
                                            <div class="col-sm-2 width-div">
                                            <asp:TextBox ID="txtcustnum" runat="server" class="form-control form-control-sm txt-margin" ReadOnly="True"></asp:TextBox>                       
               
                                            </div>

                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="row">
                                            <div class="col-sm-3 text-right width-div">
                                            </div>
                                            <div class="col-sm-3 width-div">
                                            <asp:TextBox ID="txtdescription" runat="server" class="form-control form-control-sm txt-margin" ReadOnly="True"></asp:TextBox>                       
               
                                            </div>

                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="row">
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
                                        <div class="row">
                                            <div class="col-sm-3 text-right width-div">
                                                <span>CO Return Code : </span>
                                            </div>
                                            <div class="col-sm-2 width-div" >
                                            <asp:DropDownList ID="ddlreturncode" runat="server" disabled = "disabled" AutoPostBack="true" class="form-control form-control-sm txt-margin"></asp:DropDownList>
                          
               
                                            </div>

                                        </div>
                                    </td>
                                </tr>           
                                <tr>
                                    <td>
                                        <div class="row">
                                            <div class="col-sm-3 text-right width-div">

                                            </div>
                                            <div class="col-sm-2 width-div" >
                                                <asp:Label ID="lblValidate" runat="server" Text="Validate" Visible="false"></asp:Label>
                                            </div>

                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="row">
                                            <div class="col-sm-3 text-right width-div">
                                                <asp:Label ID="lblValidateCustItem" runat="server" Text="Cust Item : " Visible="false"></asp:Label>
                                            </div>
                                            <div class="col-sm-2 width-div" >
                                                <asp:TextBox ID="txtValidateCustItem" runat="server" class="form-control form-control-sm txt-margin" ReadOnly="True" Visible="false"></asp:TextBox>
                                            </div>

                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="row">
                                            <div class="col-sm-3 text-right width-div">
                                                <asp:Label ID="lblValidateCustPo" runat="server" Text="Cust Po : " Visible="false"></asp:Label>
                                            </div>
                                            <div class="col-sm-2 width-div" >
                                                <asp:TextBox ID="txtValidateCustPo" runat="server" class="form-control form-control-sm txt-margin" ReadOnly="True" Visible="false"></asp:TextBox>
                                            </div>

                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="row">
                                            <div class="col-sm-3 text-right width-div">
                                                <asp:Label ID="lblValidateQty" runat="server" Text="Qty : " Visible="false"></asp:Label>
                                            </div>
                                            <div class="col-sm-2 width-div" >
                                                <asp:TextBox ID="txtValidateQty" runat="server" class="form-control form-control-sm txt-margin" ReadOnly="True" Visible="false"></asp:TextBox>
                                            </div>

                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="row">
                                            <div class="col-sm-3 text-right width-div">
                                                <span>Total Line Confirm : </span>
                                            </div>
                                            <div class="col-sm-2 width-div" >
                                                <asp:Label ID="lbltotallineconfirm" runat="server"></asp:Label>
                                            </div>

                                        </div>
                                    </td>
                                </tr>
                        
                            </table>

                            <div class="row align-items-center">
                                 <div class="col-sm-3"></div>
                                 <div class="col-sm-6">
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
                                                 <span style="font-size:13px;">Customer Item : <%#Eval("CustItem")%></span><br>
                                                 <%--<span style="font-size:13px;">Cust PO. : <%#Eval("CustPO")%></span><br>--%>
                                                 <span style="font-size:13px;">Qty Order : <asp:Label ID="lblQtyOrdered" runat="server" Font-Bold="False" Text='<%#Eval("QtyOrder")%>'></asp:Label> | Qty Pick : <asp:Label ID="lblQtyPick" runat="server" Font-Bold="False" Text='<%#Eval("QtyPick")%>'></asp:Label> | Sum Qty : <asp:Label ID="lblSumQty" runat="server" Font-Bold="False" Text='<%#Eval("QtySum")%>'></asp:Label> | Remain : <asp:Label ID="lblQtyRemain" runat="server" Font-Bold="False" Text='<%#Eval("QtyRemain")%>'></asp:Label></span><br>
                                                 <span style="font-size:13px;"class="text-danger">
                                                     <asp:LinkButton ID="lnkDetail" runat="server"  ToolTip="Click to view details" Text='Detail'></asp:LinkButton>
                                                 </span>
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
                     <asp:PostBackTrigger ControlID="btnprocess" />
                     <asp:PostBackTrigger ControlID="btnreset" />
                     <asp:PostBackTrigger ControlID="chkCancelTag" />
                     <asp:PostBackTrigger ControlID="Button1" />
                     <asp:PostBackTrigger ControlID="txtdate" />
                     <asp:PostBackTrigger ControlID="ddlreturncode" />
                     <asp:PostBackTrigger ControlID="Button2" />
                 </Triggers>
                </asp:UpdatePanel>

                


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

        <asp:HiddenField ID="HiddenField2" runat="server" />
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:HiddenField ID="HidItemModel" runat="server" />
        <asp:HiddenField ID="HidCustItemModel" runat="server" />
        <asp:HiddenField ID="HiddenField3" runat="server" />
        <asp:HiddenField ID="HiddenField4" runat="server" />
        
</asp:Content>
