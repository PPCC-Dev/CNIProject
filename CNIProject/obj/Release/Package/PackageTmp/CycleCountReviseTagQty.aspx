<%@ Page Title="Revise Tag Qty" Language="vb" AutoEventWireup="false" MasterPageFile="~/SRN.Master" CodeBehind="CycleCountReviseTagQty.aspx.vb" Inherits="CNIProjet.CycleCountReviseTagQty" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .txt-margin { margin-bottom:5px;}
        .width-div { width: 100%;}
        .margin-head { margin-bottom:8px;}
        .margin-top { margin-top:-24px;}
    </style>

    <script type="text/javascript">

        function scanbarcode(){

            var barcode = document.getElementById('<%=txtbarcode.ClientID%>').value;
            var f = barcode.substring(0, 1);
            var s3 = barcode.substring(0, 3);
            var s2 = barcode.substring(0, 2);

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

     <script type="text/javascript">
         $(document).ready(function () {
             $('#<%= txtCountQty.ClientID%>').keypress(function (e) {
               if (e.which != 45 && e.which != 46 && e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57) && e.which != 45) {
                    return false;
            }
        });

         });

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
                            <h6 class="m-0"><a href="Menu.aspx">Menu</a><span class="mx-2 mb-0">/</span><asp:LinkButton ID="LinkButton1" runat="server">Cycle Count</asp:LinkButton><span class="mx-2 mb-0">/</span><strong class="text-black"><%: Page.Title %></strong>  </h6>
                        </div>                       
                        
                    </div>
                 </div>
                <div class="card-body">
                    <div class="row margin-head">

                        <div class="col-md-10">
                        </div>

                        <div class="col-md-2 float-right">
                            <asp:Button ID="btnback" runat="server" class="btn btn-danger btn-sm float-right" Text="Back" UseSubmitBehavior="false"/>
                        </div>
                        
                    </div>

                    <div class="row">
                            <div class="col-sm-12 mt-2">
                                <div class="card">
                                    <div class="card-body" style="padding: 1.0rem;">
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
                                                <span>Scan Tag : </span>
                                            </div>
                                            <div class="col-sm-6 width-div">
                                                <asp:TextBox ID="txtbarcode" runat="server" AutoComplete="off" class="form-control form-control-sm txt-margin"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-2 width-div">
                                                <asp:Button ID="btnlasttag" runat="server" class="btn btn-info btn-sm txt-margin" Text="Tag ล่าสุด" UseSubmitBehavior="false" />
                                            </div>
                                            <div style="margin-top:20px;display: none;">
                                                <asp:Button ID="Button1" runat="server" Text="Scan" UseSubmitBehavior="true" />                                                    
                                            </div>
                                            </div>
                                        </td>
                                    </tr>
                                    </table>
                                    <h6 class="card-title text-primary txt-margin">Tag Detail</h6>
                                    <span style="font-size:13px;">Tag ID : <asp:Label ID="lblTagID" runat="server" Text='<%#Eval("tag_id")%>'></asp:Label></span>&nbsp;&nbsp;&nbsp;
                                    <span style="font-size:13px;"><asp:CheckBox ID="chkLastTag" class="form-check-input float-right mx-2 mb-0" runat="server" Enabled="false" /> <span>Tag ล่าสุด</span></span><br />
                                    <span style="font-size:13px;">Item : <asp:Label ID="lblitem" runat="server" Text='<%#Eval("item")%>'></asp:Label></span><br />
                                    <span style="font-size:13px;">Desc : <asp:Label ID="lblLongDesc" runat="server" Text='<%#Eval("item_description")%>'></asp:Label></span><br />      
                                    <span style="font-size:13px;">Lot : <asp:Label ID="lblLot" runat="server" Font-Bold="False" Text='<%#Eval("lot")%>'></asp:Label></span><br />                                    
                                    <span style="font-size:13px;">Location : <asp:Label ID="lblLoc" runat="server" Text='<%#Eval("loc")%>'></asp:Label></span><br />
                                    <span style="font-size:13px;">Old Tag Qty : <asp:Label ID="lblOldTagQty" runat="server" Text='<%#Eval("TagQty")%>'></asp:Label></span><br />
                                    <span style="font-size:13px;">Counted Qty : <asp:TextBox ID="txtCountQty" runat="server" Text='<%# Eval("count_qty") %>' class="form-control form-control-sm" style="width: 158px; font-size:13px; text-align:right;" AutoComplete="off"></asp:TextBox></span><br />
                                    <span style="font-size:13px;"><asp:Label ID="lblRowPointer" runat="server" style="display:none;" Font-Bold="False" Text='<%#Eval("RowPointer")%>'></asp:Label></span>
                                    <asp:Button ID="btncount" runat="server" class="btn btn-success btn-sm margin-top" Text="Count" UseSubmitBehavior="true" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btncancel" runat="server" class="btn btn-warning btn-sm margin-top" Text="Cancel" />

                                </div>
                                </div>
                            </div>

                    <asp:HiddenField ID="hidlasttag" runat="server" />
                    </div>
                    
                  
                </div>
            </div>



        </div>
</asp:Content>
