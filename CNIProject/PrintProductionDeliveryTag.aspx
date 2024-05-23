<%@ Page Title="Print Production & Delivery Tag" Language="vb" AutoEventWireup="false" MasterPageFile="~/SRN.Master" CodeBehind="PrintProductionDeliveryTag.aspx.vb" Inherits="CNIProjet.PrintProductionDeliveryTag" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .txt-margin { margin-bottom:5px;}
        .width-div { width: 50%;}
        .txt-right{
            display:block;
            text-align:right;
        }

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

        .hide { display: none; }

    </style>

    <script type="text/javascript">
        window.onbeforeunload = function () {
            $("#<%=btnPrint.ClientID%>").prop('disabled', true);
        };
    </script>

    <script type="text/javascript">
        $(document).ready(function () {

            if (document.getElementById('<%=RadioFuncionNew.ClientID%>').checked == true) {

                document.getElementById("DivNewRePrint").style.display = "block";
                document.getElementById("DivSplitMerge").style.display = "none";
                document.getElementById('<%= lblbarcode.ClientID %>').innerHTML = "Barcode Job: ";
                //alert(document.getElementById('<%= lblbarcode.ClientID %>').innerHTML);

                if (document.getElementById('<%=chkTradingPart.ClientID%>').checked == true) {

                     $("#<%=ddlStartJob.ClientID%>").prop('disabled', true);
                     $("#<%=ddlStartJob.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlEndJob.ClientID%>").prop('disabled', true);
                     $("#<%=ddlEndJob.ClientID%>").selectpicker('refresh');
                     $("#<%=txtStartSuffix.ClientID%>").prop('disabled', true);
                     $("#<%=txtEndSuffix.ClientID%>").prop('disabled', true);
                     $("#<%=txtStartDate.ClientID%>").prop('disabled', true);
                     $("#<%=txtEndDate.ClientID%>").prop('disabled', true);
                     $("#<%=txtStartTagID.ClientID%>").prop('disabled', true);
                     $("#<%=txtEndTagID.ClientID%>").prop('disabled', true);
                     $("#<%=ddlStartPO.ClientID%>").prop('disabled', false);
                     $("#<%=ddlStartPO.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlStartPOLine.ClientID%>").prop('disabled', false);
                     $("#<%=ddlStartPORelease.ClientID%>").prop('disabled', false);
                     $("#<%=ddlEndPO.ClientID%>").prop('disabled', false);
                     $("#<%=ddlEndPO.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlEndPOLine.ClientID%>").prop('disabled', false);
                     $("#<%=ddlEndPORelease.ClientID%>").prop('disabled', false);
                     $("#<%=ddlStartTO.ClientID%>").prop('disabled', false);
                     $("#<%=ddlStartTO.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlStartTOLine.ClientID%>").prop('disabled', false);
                     $("#<%=ddlEndTO.ClientID%>").prop('disabled', false);
                     $("#<%=ddlEndTO.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlEndTOLine.ClientID%>").prop('disabled', false);
                } else {

                     $("#<%=ddlStartJob.ClientID%>").prop('disabled', false);
                     $("#<%=ddlStartJob.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlEndJob.ClientID%>").prop('disabled', true);
                     $("#<%=ddlEndJob.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlStartPOLine.ClientID%>").prop('disabled', false);                    
                     $("#<%=txtEndSuffix.ClientID%>").prop('disabled', false);
                     $("#<%=txtStartDate.ClientID%>").prop('disabled', false);
                     $("#<%=txtEndDate.ClientID%>").prop('disabled', false);
                     $("#<%=txtStartTagID.ClientID%>").prop('disabled', true);
                     $("#<%=txtEndTagID.ClientID%>").prop('disabled', true);
                     $("#<%=ddlStartPO.ClientID%>").prop('disabled', true);
                     $("#<%=ddlStartPO.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlStartPOLine.ClientID%>").prop('disabled', true);
                     $("#<%=ddlStartPORelease.ClientID%>").prop('disabled', true);
                     $("#<%=ddlEndPO.ClientID%>").prop('disabled', true);
                     $("#<%=ddlEndPO.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlEndPOLine.ClientID%>").prop('disabled', true);
                     $("#<%=ddlEndPORelease.ClientID%>").prop('disabled', true);
                     $("#<%=ddlStartTO.ClientID%>").prop('disabled', true);
                     $("#<%=ddlStartTO.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlStartTOLine.ClientID%>").prop('disabled', true);
                     $("#<%=ddlEndTO.ClientID%>").prop('disabled', true);
                     $("#<%=ddlEndTO.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlEndTOLine.ClientID%>").prop('disabled', true);

                }   
            }

            if (document.getElementById('<%=RadioFuncionRePrint.ClientID%>').checked == true) {

                document.getElementById("DivNewRePrint").style.display = "block";
                document.getElementById("DivSplitMerge").style.display = "none";
                document.getElementById('<%= lblbarcode.ClientID %>').innerHTML = "Barcode Job: ";
                //alert(document.getElementById('<%= lblbarcode.ClientID %>').innerHTML);

                if (document.getElementById('<%=chkTradingPart.ClientID%>').checked == true) {

                    $("#<%=ddlStartJob.ClientID%>").prop('disabled', true);
                    $("#<%=ddlStartJob.ClientID%>").selectpicker('refresh');
                    $("#<%=ddlEndJob.ClientID%>").prop('disabled', true);
                    $("#<%=ddlEndJob.ClientID%>").selectpicker('refresh');
                    $("#<%=txtStartSuffix.ClientID%>").prop('disabled', true);
                    $("#<%=txtEndSuffix.ClientID%>").prop('disabled', true);
                    $("#<%=txtStartDate.ClientID%>").prop('disabled', true);
                    $("#<%=txtEndDate.ClientID%>").prop('disabled', true);
                    $("#<%=txtStartTagID.ClientID%>").prop('disabled', false);
                    $("#<%=txtEndTagID.ClientID%>").prop('disabled', false);
                    $("#<%=ddlStartPO.ClientID%>").prop('disabled', false);
                    $("#<%=ddlStartPO.ClientID%>").selectpicker('refresh');
                    $("#<%=ddlStartPOLine.ClientID%>").prop('disabled', false);
                    $("#<%=ddlStartPORelease.ClientID%>").prop('disabled', false);
                    $("#<%=ddlEndPO.ClientID%>").prop('disabled', false);
                    $("#<%=ddlEndPO.ClientID%>").selectpicker('refresh');
                    $("#<%=ddlEndPOLine.ClientID%>").prop('disabled', false);
                    $("#<%=ddlEndPORelease.ClientID%>").prop('disabled', false);
                    $("#<%=ddlStartTO.ClientID%>").prop('disabled', false);
                    $("#<%=ddlStartTO.ClientID%>").selectpicker('refresh');
                    $("#<%=ddlStartTOLine.ClientID%>").prop('disabled', false);
                    $("#<%=ddlEndTO.ClientID%>").prop('disabled', false);
                    $("#<%=ddlEndTO.ClientID%>").selectpicker('refresh');
                    $("#<%=ddlEndTOLine.ClientID%>").prop('disabled', false);

                } else {

                     $("#<%=ddlStartJob.ClientID%>").prop('disabled', false);
                     $("#<%=ddlStartJob.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlEndJob.ClientID%>").prop('disabled', false);
                     $("#<%=ddlEndJob.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlStartPOLine.ClientID%>").prop('disabled', false);                    
                     $("#<%=txtEndSuffix.ClientID%>").prop('disabled', false);
                     $("#<%=txtStartDate.ClientID%>").prop('disabled', false);
                     $("#<%=txtEndDate.ClientID%>").prop('disabled', false);
                     $("#<%=txtStartTagID.ClientID%>").prop('disabled', false);
                     $("#<%=txtEndTagID.ClientID%>").prop('disabled', false);
                     $("#<%=ddlStartPO.ClientID%>").prop('disabled', true);
                     $("#<%=ddlStartPO.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlStartPOLine.ClientID%>").prop('disabled', true);
                     $("#<%=ddlStartPORelease.ClientID%>").prop('disabled', true);
                     $("#<%=ddlEndPO.ClientID%>").prop('disabled', true);
                     $("#<%=ddlEndPO.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlEndPOLine.ClientID%>").prop('disabled', true);
                     $("#<%=ddlEndPORelease.ClientID%>").prop('disabled', true);
                     $("#<%=ddlStartTO.ClientID%>").prop('disabled', true);
                     $("#<%=ddlStartTO.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlStartTOLine.ClientID%>").prop('disabled', true);
                     $("#<%=ddlEndTO.ClientID%>").prop('disabled', true);
                     $("#<%=ddlEndTO.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlEndTOLine.ClientID%>").prop('disabled', true);

                }

            }

            if (document.getElementById('<%=RadioFuncionSplitMerge.ClientID%>').checked == true) {

                document.getElementById("DivNewRePrint").style.display = "none";
                document.getElementById("DivSplitMerge").style.display = "block";
                document.getElementById('<%= lblbarcode.ClientID %>').innerHTML = "Barcode Job: ";
                //alert(document.getElementById('<%= lblbarcode.ClientID %>').innerHTML);

                if (document.getElementById('<%=RadioSplit.ClientID%>').checked == true) {

                    $("#<%=txtSplitTagID.ClientID%>").prop('disabled', false);
                    $("#<%=txtSplitQty.ClientID%>").prop('disabled', false);
                    $("#<%=txtMergeTagID.ClientID%>").prop('disabled', true);
                    $("#<%=ddlLocFraction.ClientID%>").prop('disabled', true);                    
                    $("#<%=btnAdd.ClientID%>").prop('disabled', true);
                    $("#<%=btnClear.ClientID%>").prop('disabled', true);
                    $("#<%=btnMerge.ClientID%>").prop('disabled', true);

                }

                if (document.getElementById('<%=RadioSplit.ClientID%>').checked == true) {

                    $("#<%=txtSplitTagID.ClientID%>").prop('disabled', true);
                    $("#<%=txtSplitQty.ClientID%>").prop('disabled', true);
                    $("#<%=txtMergeTagID.ClientID%>").prop('disabled', false);
                    $("#<%=ddlLocFraction.ClientID%>").prop('disabled', false); 
                    $("#<%=btnAdd.ClientID%>").prop('disabled', false);
                    $("#<%=btnClear.ClientID%>").prop('disabled', false);
                    $("#<%=btnMerge.ClientID%>").prop('disabled', false);

                }

                    
            }

        });

        $(function () {
            $("#<%=RadioFuncionNew.ClientID%>").change(function () {

                var status = this.checked;
                if (status) {

                    document.getElementById("DivNewRePrint").style.display = "block";
                    document.getElementById("DivSplitMerge").style.display = "none";
                    document.getElementById('<%= lblbarcode.ClientID %>').innerHTML = "Barcode Job: ";
                    //alert(document.getElementById('<%= lblbarcode.ClientID %>').innerHTML);

                    $("#<%=txtStartTagID.ClientID%>").prop('disabled', true);
                    $("#<%=txtEndTagID.ClientID%>").prop('disabled', true);
                    $("#<%=ddlStartPO.ClientID%>").prop('disabled', true);
                    $("#<%=ddlStartPO.ClientID%>").prop('disabled', true);
                    $("#<%=ddlStartPORelease.ClientID%>").prop('disabled', true);
                    $("#<%=ddlEndPO.ClientID%>").prop('disabled', true);
                    $("#<%=ddlEndPOLine.ClientID%>").prop('disabled', true);
                    $("#<%=ddlEndPORelease.ClientID%>").prop('disabled', true);
                    $("#<%=ddlStartTO.ClientID%>").prop('disabled', true);
                    $("#<%=ddlStartTO.ClientID%>").selectpicker('refresh');
                    $("#<%=ddlStartTOLine.ClientID%>").prop('disabled', true);
                    $("#<%=ddlEndTO.ClientID%>").prop('disabled', true);
                    $("#<%=ddlEndTO.ClientID%>").selectpicker('refresh');
                    $("#<%=ddlEndTOLine.ClientID%>").prop('disabled', true);
                    $("#<%=ddlEndJob.ClientID%>").prop('disabled', true);
                    $("#<%=ddlEndJob.ClientID%>").selectpicker('refresh');

                }

            });
        })

        $(function () {
            $("#<%=RadioFuncionRePrint.ClientID%>").change(function () {

                var status = this.checked;
                if (status) {

                    document.getElementById("DivNewRePrint").style.display = "block";
                    document.getElementById("DivSplitMerge").style.display = "none";
                    document.getElementById('<%= lblbarcode.ClientID %>').innerHTML = "Barcode Job: ";
                    //alert(document.getElementById('<%= lblbarcode.ClientID %>').innerHTML);

                    $("#<%=txtStartTagID.ClientID%>").prop('disabled', false);
                    $("#<%=txtEndTagID.ClientID%>").prop('disabled', false);
                    $("#<%=ddlStartPO.ClientID%>").prop('disabled', true);
                    $("#<%=ddlStartPO.ClientID%>").prop('disabled', true);
                    $("#<%=ddlStartPORelease.ClientID%>").prop('disabled', true);
                    $("#<%=ddlEndPO.ClientID%>").prop('disabled', true);
                    $("#<%=ddlEndPOLine.ClientID%>").prop('disabled', true);
                    $("#<%=ddlEndPORelease.ClientID%>").prop('disabled', true);
                    $("#<%=ddlStartTO.ClientID%>").prop('disabled', true);
                    $("#<%=ddlStartTO.ClientID%>").selectpicker('refresh');
                    $("#<%=ddlStartTOLine.ClientID%>").prop('disabled', true);
                    $("#<%=ddlEndTO.ClientID%>").prop('disabled', true);
                    $("#<%=ddlEndTO.ClientID%>").selectpicker('refresh');
                    $("#<%=ddlEndTOLine.ClientID%>").prop('disabled', true);
                    $("#<%=ddlEndJob.ClientID%>").prop('disabled', false);
                    $("#<%=ddlEndJob.ClientID%>").selectpicker('refresh');

                }

            });
        })

        $(function () {
            $("#<%=RadioFuncionSplitMerge.ClientID%>").change(function () {

                var status = this.checked;
                if (status) {

                    document.getElementById("DivNewRePrint").style.display = "none";
                    document.getElementById("DivSplitMerge").style.display = "block";
                    document.getElementById('<%= lblbarcode.ClientID %>').innerHTML = "Barcode Tag ID: ";
                    //alert(document.getElementById('<%= lblbarcode.ClientID %>').innerHTML);

                    $("#<%=txtMergeTagID.ClientID%>").prop('disabled', true);
                    $("#<%=ddlLocFraction.ClientID%>").prop('disabled', true);
                    $("#<%=btnAdd.ClientID%>").prop('disabled', true);
                    $("#<%=btnClear.ClientID%>").prop('disabled', true);
                    $("#<%=btnMerge.ClientID%>").prop('disabled', true);


                }

            });
        })

        $(function () {
            $("#<%=RadioSplit.ClientID%>").change(function () {

                var status = this.checked;
                if (status) {

                    $("#<%=txtMergeTagID.ClientID%>").prop('disabled', true);
                    $("#<%=ddlLocFraction.ClientID%>").prop('disabled', true);
                    $("#<%=btnAdd.ClientID%>").prop('disabled', true);
                    $("#<%=btnClear.ClientID%>").prop('disabled', true);
                    $("#<%=btnMerge.ClientID%>").prop('disabled', true);
                    $("#<%=txtSplitTagID.ClientID%>").prop('disabled', false);
                    $("#<%=txtSplitQty.ClientID%>").prop('disabled', false);

                }

            });
        })

        $(function () {
            $("#<%=RadioMerge.ClientID%>").change(function () {

                var status = this.checked;
                if (status) {

                    $("#<%=txtSplitTagID.ClientID%>").prop('disabled', true);
                    $("#<%=txtSplitQty.ClientID%>").prop('disabled', true);
                    $("#<%=txtMergeTagID.ClientID%>").prop('disabled', false);
                    $("#<%=ddlLocFraction.ClientID%>").prop('disabled', false);
                    $("#<%=btnAdd.ClientID%>").prop('disabled', false);
                    $("#<%=btnClear.ClientID%>").prop('disabled', false);
                    $("#<%=btnMerge.ClientID%>").prop('disabled', false);

                }

            });
        })

        $(function () {
             $("#<%=chkTradingPart.ClientID%>").change(function () {
                 var status = this.checked;
                 if (status) {
                     
                     $("#<%=ddlStartJob.ClientID%>").prop('disabled', true);
                     $("#<%=ddlStartJob.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlEndJob.ClientID%>").prop('disabled', true);
                     $("#<%=ddlEndJob.ClientID%>").selectpicker('refresh');
                     $("#<%=txtStartSuffix.ClientID%>").prop('disabled', true);
                     $("#<%=txtEndSuffix.ClientID%>").prop('disabled', true);
                     $("#<%=txtStartDate.ClientID%>").prop('disabled', true);
                     $("#<%=txtEndDate.ClientID%>").prop('disabled', true);
                     $("#<%=ddlStartPO.ClientID%>").prop('disabled', false);
                     $("#<%=ddlStartPO.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlStartPOLine.ClientID%>").prop('disabled', false);
                     $("#<%=ddlStartPORelease.ClientID%>").prop('disabled', false);
                     $("#<%=ddlEndPO.ClientID%>").prop('disabled', false);
                     $("#<%=ddlEndPO.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlEndPOLine.ClientID%>").prop('disabled', false);
                     $("#<%=ddlEndPORelease.ClientID%>").prop('disabled', false);
                     $("#<%=ddlStartTO.ClientID%>").prop('disabled', false);
                     $("#<%=ddlStartTO.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlStartTOLine.ClientID%>").prop('disabled', false);
                     $("#<%=ddlEndTO.ClientID%>").prop('disabled', false);
                     $("#<%=ddlEndTO.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlEndTOLine.ClientID%>").prop('disabled', false);
                 }
                 else {

                     $("#<%=ddlStartJob.ClientID%>").prop('disabled', false);
                     $("#<%=ddlStartJob.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlEndJob.ClientID%>").prop('disabled', false);
                     $("#<%=ddlEndJob.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlStartPOLine.ClientID%>").prop('disabled', false);                    
                     $("#<%=txtEndSuffix.ClientID%>").prop('disabled', false);
                     $("#<%=txtStartDate.ClientID%>").prop('disabled', false);
                     $("#<%=txtEndDate.ClientID%>").prop('disabled', false);
                     $("#<%=ddlStartPO.ClientID%>").prop('disabled', true);
                     $("#<%=ddlStartPO.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlStartPOLine.ClientID%>").prop('disabled', true);
                     $("#<%=ddlStartPORelease.ClientID%>").prop('disabled', true);
                     $("#<%=ddlEndPO.ClientID%>").prop('disabled', true);
                     $("#<%=ddlEndPO.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlEndPOLine.ClientID%>").prop('disabled', true);
                     $("#<%=ddlEndPORelease.ClientID%>").prop('disabled', true);
                     $("#<%=ddlStartTO.ClientID%>").prop('disabled', true);
                     $("#<%=ddlStartTO.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlStartTOLine.ClientID%>").prop('disabled', true);
                     $("#<%=ddlEndTO.ClientID%>").prop('disabled', true);
                     $("#<%=ddlEndTO.ClientID%>").selectpicker('refresh');
                     $("#<%=ddlEndTOLine.ClientID%>").prop('disabled', true);
                     

                 }
             })
        })

        $(document).ready(function () {
            $("#<%=txtWhse.ClientID%>").prop('disabled', true);
        });

        function EnablePrint() {
             $("#<%=btnPrint.ClientID%>").prop('disabled', false);
        }

        function DisabledPrint() {
            $("#<%=btnPrint.ClientID%>").prop('disabled', true);
        }
    </script>

    
    <script>

        function ShowSweetAlert(type, msg, icon) {
            Swal.fire(
                type,
                msg,
                icon
            )

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

             <%--<asp:ScriptManager ID="ScriptManager1" runat="server" ></asp:ScriptManager>
                 <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server" >
                        <ContentTemplate>--%>
                            <div class="card-body">

                            <div class="form-row mb-1">
                                <div class="col-12">
                                    <div class="row align-items-right">
                                        <div class="col-sm-3 text-right width-div">
                                            <asp:Label ID="lblbarcode" runat="server" Text="Barcode Job: "></asp:Label>
                                        </div>
                                        <div class="col-sm-5 width-div">
                                            <asp:TextBox ID="txtbarcode" runat="server" AutoComplete="off" class="form-control form-control-sm txt-margin"></asp:TextBox>
                                    
                                        </div>
                                        <div style="margin-top:20px;display:none;">
                                            <asp:Button ID="Button1" runat="server" Text="Scan" UseSubmitBehavior="true" />
                                       </div>
                                    
                                    </div>
                                </div>
                                
                            </div>

                            <div class="form-row mb-1">  
                                <div class="col-6">
                                    <div class="card">
                                        <div class="card-body pt-1 pb-2">
                                            <span>Funcion</span><br />
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-check form-check-inline">
                                                        <asp:RadioButton ID="RadioFuncionNew" class="form-check-input" Checked="true"  runat="server" GroupName="Function" />
                                                        <label class="form-check-label" for="RadioConfirm">New&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                                                    </div>
                                                    <div class="form-check form-check-inline">
                                                        <asp:RadioButton ID="RadioFuncionRePrint" class="form-check-input" runat="server" GroupName="Function" />
                                                        <label class="form-check-label" for="RadioChange">Re-Print&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                                                    </div>
                                                    <div class="form-check form-check-inline">
                                                        <asp:RadioButton ID="RadioFuncionSplitMerge" class="form-check-input" runat="server" GroupName="Function" />
                                                        <label class="form-check-label" for="RadioReject">Split-Merge&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-6">
                                    <div class="card">
                                        <div class="card-body pt-1 pb-2">
                                            <span>Format Tag</span><br />
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-check form-check-inline">
                                                        <asp:CheckBox ID="chkDelivery" class="form-check-input txt-margin" runat="server" /><span>Delivery</span>
                                                    </div>
                                                    <div class="form-check form-check-inline">
                                                        <asp:CheckBox ID="chkProductionTag" class="form-check-input txt-margin" runat="server" Checked="true" /><span>Production Tag</span>
                                                    </div>
                                                    <div class="form-check form-check-inline">
                                                        <asp:CheckBox ID="chkBoxTag" class="form-check-input txt-margin" runat="server" Checked="true" /><span>Box Tag</span>
                                                    </div>
                                                    <div class="form-check form-check-inline">
                                                        <asp:CheckBox ID="chkBagTag" class="form-check-input txt-margin" runat="server" Checked="true" /><span>Bag Tag</span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                            </div>

                            <div class="form-row mb-1">  
                                <div class="col-12">
                                        <div class="card-body pt-2 pb-1">
                                            <div class="row mb-1">
                                                <div class="col-5">
                                                    <div class="form-row mb-1">
                                                        <div class="col-3">
                                                            <div class="input-group txt-right">
                                                                <span></span>
                                                            </div>
                                                        </div>
                                                        <div class="col-4">
                                                            <div class="input-group">
                                                                <asp:CheckBox ID="chkTradingPart" class="form-check-input txt-margin" runat="server" /><span>Trading Part</span>
                                                            </div>
                                                        </div>
                                                        
                                                    </div>
                                                </div>
                                                <div class="col-5">
                                                    <div class="form-row mb-1">
                                                        <div class="col-3">
                                                            <div class="input-group txt-right">
                                                                <span>Whse : </span>
                                                            </div>
                                                        </div>

                                                        <div class="col-4">
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtWhse" runat="server" AutoComplete="off" class="form-control form-control-sm txt-margin"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                </div>

                            </div>

                            <div id="DivNewRePrint">
                                
                            <div class="form-row mb-1">
                                <div class="col-12">
                                    <div class="card">
                                        <div class="card-body pt-1 pb-2">
                                            <div class="row mb-1">
                                                <div class="col-md-6">
                                                    <div class="row d-flex justify-content-center">
                                                        <span>Starting</span>
                                                 </div>
                                                </div>

                                                <div class="col-md-6">
                                                    <div class="row d-flex justify-content-center">
                                                        <span>Ending</span>
                                                    </div>
                                                </div>
                                            </div>
                                            
                                            <div class="row mb-1">
                                                <div class="col-md-6">
                                                    <div class="form-row mb-1">
                                                        <div class="col-4 ">
                                                            <div class="input-group txt-right">
                                                                <span>Job : </span>
                                                            </div>
                                                        </div>

                                                        <div class="col-5">
                                                            <div class="input-group">
                                                                <asp:DropDownList ID="ddlStartJob" runat="server" class="form-control form-control-sm txt-margin selectpicker" data-live-search="true" AutoPostBack="true"></asp:DropDownList>
                                                            </div>
                                                        </div>

                                                        <div class="col-2">
                                                            <div class="input-group">
                                                                 <asp:TextBox ID="txtStartSuffix" runat="server" AutoComplete="off" class="form-control form-control-sm txt-margin">0000</asp:TextBox>
                                                                
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>

                                                <div class="col-md-6">
                                                    <div class="form-row">
                                                        <div class="col-4">
                                                            <div class="input-group">
                                                                <span></span>
                                                            </div>
                                                        </div>

                                                        <div class="col-5">
                                                            <div class="input-group">
                                                                <asp:DropDownList ID="ddlEndJob" runat="server" class="form-control form-control-sm txt-margin selectpicker" data-live-search="true" AutoPostBack="true"></asp:DropDownList>
                                                            </div>
                                                        </div>

                                                        <div class="col-2">
                                                            <div class="input-group">
                                                                 <asp:TextBox ID="txtEndSuffix" runat="server" AutoComplete="off" class="form-control form-control-sm txt-margin">9999</asp:TextBox>
                                                                
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row mb-1">
                                                <div class="col-md-6">
                                                    <div class="form-row mb-1">
                                                        <div class="col-4">
                                                            <div class="input-group txt-right">
                                                                <span>Job Date : </span>
                                                            </div>
                                                        </div>

                                                        <div class="col-5">
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtStartDate" runat="server" AutoComplete="off" class="form-control datepicker form-control-sm txt-margin"></asp:TextBox>
                                                            </div>
                                                        </div>


                                                    </div>
                                                </div>

                                                <div class="col-md-6">
                                                    <div class="form-row mb-1">
                                                        <div class="col-4">
                                                            <div class="input-group">
                                                                <span></span>
                                                            </div>
                                                        </div>

                                                        <div class="col-5">
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtEndDate" runat="server" AutoComplete="off" class="form-control datepicker form-control-sm txt-margin"></asp:TextBox>
                                                            </div>
                                                        </div>


                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row mb-1">
                                                <div class="col-md-6">
                                                    <div class="form-row mb-1">
                                                        <div class="col-4">
                                                            <div class="input-group txt-right">
                                                                <span>Tag ID : </span>
                                                            </div>
                                                        </div>

                                                        <div class="col-8">
                                                            <div class="input-group">
                                                                 <asp:TextBox ID="txtStartTagID" runat="server" AutoComplete="off" class="form-control form-control-sm txt-margin"></asp:TextBox>
                                                            </div>
                                                        </div>


                                                    </div>
                                                </div>

                                                <div class="col-md-6">
                                                    <div class="form-row mb-1">
                                                        <div class="col-4">
                                                            <div class="input-group">
                                                                <span></span>
                                                            </div>
                                                        </div>

                                                        <div class="col-8">
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtEndTagID" runat="server" AutoComplete="off" class="form-control form-control-sm txt-margin"></asp:TextBox>
                                                            </div>
                                                        </div>


                                                    </div>
                                                </div>
                                            </div>

                                            

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="form-row mb-1">
                                <div class="col-12">
                                    <div class="card">
                                        <div class="card-body pb-2">
                                            <div class="row mb-1">
                                                <div class="col-md-6">
                                                    <div class="form-row mb-1">
                                                        <div class="col-3 ">
                                                            <div class="input-group txt-right">
                                                                <span>PO : </span>
                                                            </div>
                                                        </div>

                                                        <div class="col-5">
                                                            <div class="input-group">
                                                                <asp:DropDownList ID="ddlStartPO" runat="server" class="form-control form-control-sm txt-margin selectpicker" data-live-search="true" AutoPostBack="true"></asp:DropDownList>
                                                            </div>
                                                        </div>

                                                        <div class="col-2">
                                                            <div class="input-group">
                                                                 <asp:DropDownList ID="ddlStartPOLine" runat="server" class="form-control form-control-sm txt-margin" AutoPostBack="true"></asp:DropDownList>
                                                            </div>
                                                        </div>

                                                        <div class="col-2">
                                                            <div class="input-group">
                                                                 <asp:DropDownList ID="ddlStartPORelease" runat="server" class="form-control form-control-sm txt-margin"></asp:DropDownList>
                                                                
                                                            </div>
                                                        </div>


                                                    </div>
                                                </div>

                                                <div class="col-md-6">
                                                    <div class="form-row">
                                                        <div class="col-3">
                                                            <div class="input-group">
                                                                <span></span>
                                                            </div>
                                                        </div>

                                                        <div class="col-5">
                                                            <div class="input-group">
                                                               <asp:DropDownList ID="ddlEndPO" runat="server" class="form-control form-control-sm txt-margin selectpicker" data-live-search="true" AutoPostBack="true"></asp:DropDownList>
                                                            </div>
                                                        </div>

                                                        <div class="col-2">
                                                            <div class="input-group">
                                                                 <asp:DropDownList ID="ddlEndPOLine" runat="server" class="form-control form-control-sm txt-margin" AutoPostBack="true"></asp:DropDownList>
                                                                
                                                            </div>
                                                        </div>

                                                        <div class="col-2">
                                                            <div class="input-group">
                                                                 <asp:DropDownList ID="ddlEndPORelease" runat="server" class="form-control form-control-sm txt-margin"></asp:DropDownList>
                                                                
                                                            </div>
                                                        </div>


                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row mb-1">
                                                <div class="col-md-6">
                                                    <div class="form-row mb-1">
                                                        <div class="col-3 ">
                                                            <div class="input-group txt-right">
                                                                <span>TO : </span>
                                                            </div>
                                                        </div>

                                                        <div class="col-5">
                                                            <div class="input-group">
                                                                <asp:DropDownList ID="ddlStartTO" runat="server" class="form-control form-control-sm txt-margin selectpicker" data-live-search="true" AutoPostBack="true"></asp:DropDownList>
                                                            </div>
                                                        </div>

                                                        <div class="col-2">
                                                            <div class="input-group">
                                                                 <asp:DropDownList ID="ddlStartTOLine" runat="server" class="form-control form-control-sm txt-margin"></asp:DropDownList>
                                                                
                                                            </div>
                                                        </div>

                                                        
                                                    </div>
                                                </div>

                                                <div class="col-md-6">
                                                    <div class="form-row">
                                                        <div class="col-3">
                                                            <div class="input-group">
                                                                <span></span>
                                                            </div>
                                                        </div>

                                                        <div class="col-5">
                                                            <div class="input-group">
                                                                 <asp:DropDownList ID="ddlEndTO" runat="server" class="form-control form-control-sm txt-margin selectpicker" data-live-search="true" AutoPostBack="true"></asp:DropDownList>
                                                            </div>
                                                        </div>

                                                        <div class="col-2">
                                                            <div class="input-group">
                                                                 <asp:DropDownList ID="ddlEndTOLine" runat="server" class="form-control form-control-sm txt-margin"></asp:DropDownList>
                                                                
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                            </div>

                            <div id="DivSplitMerge">

                                <div class="form-row mb-1">
                                    <div class="col-12">
                                        <div class="card">
                                            <div class="card-body pt-1 pb-2">
                                            
                                                <div class="row mb-1">
                                                    <div class="form-check form-check-inline">
                                                        <asp:RadioButton ID="RadioSplit" class="form-check-input" Checked="true"  runat="server" GroupName="SplitMerge" />
                                                        <label class="form-check-label" for="RadioSplit">Split&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                                                    </div>
                                                </div>

                                                <div class="row mb-1">
                                                <div class="col-3">
                                                        <div class="input-group txt-right">
                                                            <span>Tag ID : </span>
                                                        </div>
                                                    </div>

                                                    <div class="col-6">
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtSplitTagID" runat="server" AutoComplete="off" class="form-control form-control-sm txt-margin"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row mb-1">
                                                <div class="col-3">
                                                        <div class="input-group txt-right">
                                                            <span>Split Qty : </span>
                                                        </div>
                                                    </div>

                                                    <div class="col-6">
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtSplitQty" runat="server" AutoComplete="off" class="form-control form-control-sm txt-margin" TextMode="MultiLine" rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <div class="form-row mb-1">
                                    <div class="col-12">
                                        <div class="card">
                                            <div class="card-body pt-1 pb-2">
                                            
                                                <div class="row mb-1">
                                                    <div class="form-check form-check-inline">
                                                        <asp:RadioButton ID="RadioMerge" class="form-check-input"  runat="server" GroupName="SplitMerge" />
                                                        <label class="form-check-label" for="RadioMerge">Merge&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                                                    </div>
                                                </div>

                                                <div class="row mb-1">
                                                <div class="col-3">
                                                        <div class="input-group txt-right">
                                                            <span>Tag ID : </span>
                                                        </div>
                                                    </div>

                                                    <div class="col-6">
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtMergeTagID" runat="server" AutoComplete="off" class="form-control form-control-sm txt-margin"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row mb-1">
                                                <div class="col-3">
                                                        <div class="input-group txt-right">
                                                            <span>Location Fraction : </span>
                                                        </div>
                                                    </div>

                                                    <div class="col-6">
                                                        <div class="input-group">
                                                            <asp:DropDownList ID="ddlLocFraction" runat="server" class="form-control form-control-sm txt-margin"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row mb-1">
                                                   <div class="col-sm-12 mt-2 d-flex justify-content-center">
                                                        <asp:Button ID="btnAdd"  runat="server" Text="Add Tag ID" UseSubmitBehavior="false"  class="btn btn-customblue btn-sm mr-3" /> 
                                                        
                                                        <asp:Button ID="btnClearTag"  runat="server" Text="Clear Tag" UseSubmitBehavior="false" class="btn btn-customblue btn-sm mr-3" />
                                                        
                                                        <asp:Button ID="btnMerge"  runat="server" Text="Merge" UseSubmitBehavior="false" class="btn btn-customblue btn-sm" />
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>

                                </div>

                            </div>

                            <div class="form-row mb-1">
                                    <div class="col-12">
                                        <div class="card-body pt-1 pb-2">
                                            <div class="row mb-1">
                                                <div class="col-sm-12 mt-2 d-flex justify-content-center">
                                                    <asp:Button ID="btnPreview"  runat="server" Text="Preview" UseSubmitBehavior="false"  class="btn btn-customblue btn-sm mr-3" /> 
                                                        
                                                    <asp:Button ID="btnPrint"  runat="server" Text="Print" UseSubmitBehavior="false" class="btn btn-customblue btn-sm mr-3" />

                                                    <asp:Button ID="btnClear"  runat="server" Text="Clear" UseSubmitBehavior="false" class="btn btn-customblue btn-sm mr-3" />
                                                        
                                                </div>
                                            </div>


                                        </div>
                                    </div>

                                </div>

                            <div id="GridTag" style="font-size:13px">

                                    <div class="form-row mb-1">
                                        <div class="col-12">
                                            <div class="card">
                                                <div class="card-body pt-1 pb-2">
                                                    <div class="row mb-1">
                                                        <div class="col-sm-12 mt-2 d-flex justify-content-left">
                                                            <asp:Button ID="btnSelectAll"  runat="server" Text="Select All" UseSubmitBehavior="false"  class="btn btn-customblue btn-sm mr-3" /> 
                                                        
                                                            <asp:Button ID="btnDeSelectAll"  runat="server" Text="DeSelect All" UseSubmitBehavior="false" class="btn btn-customblue btn-sm mr-3" />
                                                        
                                                        </div>
                                                    </div>
                                                    
                                                    <div class="table-responsive">
                                                     <asp:GridView ID="GridView1" CssClass="table table-bordered" 
                                                                runat="server" AutoGenerateColumns="false"
                                                                EnableModelValidation="True"
                                                                DataKeyNames="TagID">
                                                         <Columns>
                                                             <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                     <asp:CheckBox ID="chkSelect" class="form-check-input txt-margin" runat="server" />
                                                                         
                                                                </ItemTemplate>
                                                             </asp:TemplateField>                                                             
                                                             <asp:BoundField HeaderText="Tag Format" DataField="DerTagformat" ItemStyle-HorizontalAlign="Left" />
                                                             <asp:BoundField HeaderText="Customer" DataField="CustNum" ItemStyle-HorizontalAlign="Left" />
                                                             <asp:BoundField HeaderText="Customer ShotName" DataField="cust_shotname" ItemStyle-HorizontalAlign="Left" />
                                                             <asp:BoundField HeaderText="Tag ID" DataField="TagId" ItemStyle-HorizontalAlign="Left" />
                                                             <asp:TemplateField HeaderText="Tag Qty" ItemStyle-Wrap="true" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                        <asp:Label ID="lblTagQty" runat="server" Text='<%# Eval("TagQty") %>'></asp:Label>                
                                                                </ItemTemplate>
                                                             </asp:TemplateField>
                                                             <asp:BoundField HeaderText="Location Fraction" DataField="LocExcess" ItemStyle-HorizontalAlign="Left"  />
                                                             <asp:TemplateField HeaderText="Qty Fraction" ItemStyle-Wrap="true" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                        <asp:Label ID="lblQtyExcess" runat="server" Text='<%# Eval("QtyExcess") %>'></asp:Label>                
                                                                </ItemTemplate>
                                                             </asp:TemplateField>
                                                             <asp:BoundField HeaderText="QR Code" DataField="qr_code" ItemStyle-HorizontalAlign="Left" />
                                                             <asp:TemplateField HeaderText="Package Weight" ItemStyle-Wrap="true" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                        <asp:Label ID="lblPackWeight" runat="server" Text='<%# Eval("PackageWeight") %>'></asp:Label>                
                                                                </ItemTemplate>
                                                             </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Part Weight" ItemStyle-Wrap="true" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                        <asp:Label ID="lblPartWeight" runat="server" Text='<%# Eval("PartWeight") %>'></asp:Label>                
                                                                </ItemTemplate>
                                                             </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Gross Weight" ItemStyle-Wrap="true" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                        <asp:Label ID="lblGrossWeight" runat="server" Text='<%# Eval("DerGrossWeight") %>'></asp:Label>                
                                                                </ItemTemplate>
                                                             </asp:TemplateField>
                                                             <asp:BoundField HeaderText="Error" DataField="Error" ItemStyle-HorizontalAlign="Left" />
                                                             <asp:BoundField HeaderText="Item" DataField="Item" ItemStyle-HorizontalAlign="Left" />
                                                             <asp:BoundField HeaderText="Item Description" DataField="ItemDescription" ItemStyle-HorizontalAlign="Left" />
                                                             <asp:BoundField HeaderText="Cust. Item for Tag" DataField="CustItem" ItemStyle-HorizontalAlign="Left" />
                                                             <asp:BoundField HeaderText="UM" DataField="UM" ItemStyle-HorizontalAlign="Left" />
                                                             <asp:BoundField HeaderText="Model" DataField="Model" ItemStyle-HorizontalAlign="Left" />
                                                             <asp:BoundField HeaderText="Job" DataField="Job" ItemStyle-HorizontalAlign="Left" />
                                                             <asp:BoundField HeaderText="Suffix" DataField="Suffix" ItemStyle-HorizontalAlign="Center" />
                                                             <asp:BoundField HeaderText="Lot" DataField="Lot" ItemStyle-HorizontalAlign="Left" />
                                                             <asp:BoundField HeaderText="Location" DataField="Loc" ItemStyle-HorizontalAlign="Left" />
                                                             <asp:BoundField HeaderText="Warehouse" DataField="Whse" ItemStyle-HorizontalAlign="Left" />
                                                             <asp:BoundField HeaderText="Transaction Date" DataField="TranDate" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:dd/MM/yyyy}" />
                                                             <asp:BoundField DataField="ref_tag_id" HeaderText="" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                                                             <asp:BoundField DataField="RowPointer" HeaderText="" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                                                         </Columns>
                                                     </asp:GridView>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            
                      </div>
             <%--</ContentTemplate>
                 <Triggers>

                 </Triggers>
                </asp:UpdatePanel>--%>

            </div>

        </div>
</asp:Content>
