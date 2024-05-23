Imports System.Data
Imports System.Xml
Imports System.Collections
Imports System.Reflection
Imports Microsoft.VisualBasic.DateInterval
Imports System.Drawing
Imports System.IO

Public Class OrderShipping
    Inherits System.Web.UI.Page

    Dim oWS As CNIService.DOWebServiceSoapClient
    Dim ds As DataSet
    Dim Filter As String
    Dim res As Object
    Dim SGUID As String
    Dim Parms As String
    Dim Propertie As String
    Dim DateNow As String
    Dim LenPointQty As Integer = 0
    Dim CheckCustItem As String
    Dim CheckCustPO As String
    Dim CheckQty As String
    Dim CheckSepCustItem As String
    Dim CheckSepCustPo As String
    Dim CheckSepQty As String

    Private Shared Whse As String
    Private Shared ParmSite As String
    Private Shared ImagePath As String

    Private Shared Property PSite() As String
        Get
            Return ParmSite
        End Get
        Set(value As String)
            ParmSite = value
        End Set
    End Property

    Private Shared Property PWhse() As String
        Get
            Return Whse
        End Get
        Set(value As String)
            Whse = value
        End Set
    End Property

    Private Shared Property PImagePath() As String
        Get
            Return ImagePath
        End Get
        Set(value As String)
            ImagePath = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("PSession") Is Nothing Then
            Response.Redirect("signin.aspx")
        Else
            If Session("PSession").ToString = "" Then
                Response.Redirect("signin.aspx")
            End If
        End If

        'Dim sLock As String = "0"
        'sLock = GetAccessOrderShipping()

        'If sLock = "1" Then
        '    Response.Redirect("Menu.aspx")
        'End If

        LenPointQty = UnitQtyFormat()

        If Not Page.IsPostBack Then

            PSite = GetSite()
            PWhse = GetDefWhse()
            PImagePath = GetImagePath()

            'DateNow = Date.Now.ToString("dd/MM/yyyy")

            'txtdate.Text = DateNow

            'GetReasonCode()
            lbltotallineconfirm.Text = 0

                If Request.QueryString("SessionID") <> "" Or Not String.IsNullOrEmpty(Request.QueryString("SessionID")) Then

                    BindGridview()
                    HiddenField1.Value = Request.QueryString("ShowPic")

                    If Session("Stat").ToString = "Return" Then
                        btnstat.CssClass = "btn btn btn-outline-danger btn-block btn-sm"
                        btnstat.Text = "<i class=""fa fa-repeat"" aria-hidden=""true""></i>" & " <strong>Return</strong>"
                    Else
                        btnstat.CssClass = "btn btn-outline-success btn-block btn-sm"
                        btnstat.Text = "<i class=""fa fa-arrow-right"" aria-hidden=""true""></i>" & " <strong>Order Shipping</strong>"
                    End If

                    ddlreturncode.SelectedIndex = ddlreturncode.Items.IndexOf(ddlreturncode.Items.FindByValue(Request.QueryString("CoReturn")))

                    HiddenField3.Value = GetNotScanPickList(txtcustnum.Text)

                    If Session("Scan") = "Scan Tag: " Or Session("Scan") = "Scan Cust Doc.: " Then

                        ds = New DataSet
                        ds = GetCheckInformation(txtcustnum.Text)

                        If ds.Tables(0).Rows.Count > 0 Then

                            CheckCustItem = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_FixCustItem")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_FixCustItem").ToString)
                            CheckCustPO = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_FixCustPO")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_FixCustPO").ToString)
                            CheckQty = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_FixQty")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_FixQty").ToString)
                            CheckSepCustItem = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_SepCustItem")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_SepCustItem").ToString)
                            CheckSepCustPo = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_SepCustPo")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_SepCustPo").ToString)
                            CheckSepQty = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_SepQty")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_SepQty").ToString)

                        End If

                        If CheckCustItem = "1" Or CheckSepCustItem = "1" Then
                            lblValidate.Visible = True
                            lblValidateCustItem.Visible = True
                            txtValidateCustItem.Visible = True
                        End If

                        If CheckCustPO = "1" Or CheckSepCustPo = "1" Then
                            lblValidate.Visible = True
                            lblValidateCustPo.Visible = True
                            txtValidateCustPo.Visible = True
                        End If

                        If CheckQty = "1" Or CheckSepQty = "1" Then
                            lblValidate.Visible = True
                            lblValidateQty.Visible = True
                            txtValidateQty.Visible = True
                        End If

                        If Not Session("txtValidateCustItem") Is Nothing Then
                            txtValidateCustItem.Text = Session("txtValidateCustItem").ToString
                        Else
                            txtValidateCustItem.Text = String.Empty
                        End If

                        If Not Session("txtValidateCustPo") Is Nothing Then
                            txtValidateCustPo.Text = Session("txtValidateCustPo").ToString
                        Else
                            txtValidateCustPo.Text = String.Empty
                        End If

                        If Not Session("txtValidateQty") Is Nothing Then
                            txtValidateQty.Text = Session("txtValidateQty").ToString
                        Else
                            txtValidateQty.Text = String.Empty
                        End If

                    End If

                Else
                    Session("Stat") = "Ship"
                End If

                GetReasonCode()

            End If

            If txtpicklistno.Text = "" Then
            lblbarcode.Text = "Scan Order PickList: "
        Else
            lblbarcode.Text = Session("Scan").ToString
        End If

        'If ChkPreInv.Checked = False Then
        '    Enablebtnprocess()
        'Else
        '    EnablebtnprocessPreInvoice()
        'End If

        If Session("Stat").ToString = "Return" Then
            ddlreturncode.Attributes.Remove("disabled")
        End If

        If PanelList.Items.Count > 0 Then

            oWS = New CNIService.DOWebServiceSoapClient
            ds = New DataSet

            ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Order_Picklist_Hdrs", "pick_list_date", "pick_list_num = '" & txtpicklistno.Text & "'", "", "", 0)

            If ds.Tables(0).Rows.Count > 0 Then
                txtdate.Text = Convert.ToDateTime(ds.Tables(0).Rows(0)("pick_list_date").ToString).ToString("dd/MM/yyyy")
            End If

        Else

            DateNow = Date.Now.ToString("dd/MM/yyyy")

            txtdate.Text = DateNow

        End If

        txtbarcode.Focus()

        txtbarcode.Attributes.Add("onchange", "javascript:scanbarcode();")


    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim sBarcode, Stat, MsgErr, MsgType, Prompt, CheckCustDoc, CheckScanPallet, CheckPreInvoice, CustTagType As String
        Dim StartCustItem, LenghtCustItem, StartCustPO, LenghtCustPO, StartQty, LenghtQty, SepCustItemCol, SepCustPoCol, SepQtyCol As String
        Dim CustItem, CustPo, Qty, sTagID As String

        Dim arrBarcode As String()
        sBarcode = txtbarcode.Text
        Stat = "TRUE"
        MsgErr = ""
        MsgType = ""
        Prompt = ""
        CheckCustDoc = "0"
        CheckScanPallet = "0"
        CheckPreInvoice = "0"
        CheckCustItem = "0"
        CheckCustPO = "0"
        CheckQty = "0"
        CheckSepCustItem = "0"
        CheckSepCustPo = "0"
        CheckSepQty = "0"
        StartCustItem = "0"
        LenghtCustItem = "0"
        StartCustPO = "0"
        LenghtCustPO = "0"
        StartQty = "0"
        LenghtQty = "0"
        SepCustItemCol = "0"
        SepCustPoCol = "0"
        SepQtyCol = "0"
        CustTagType = ""
        CustItem = ""
        CustPo = ""
        Qty = ""
        sTagID = ""

        If txtbarcode.Text <> "" Then

            If lblbarcode.Text = "Scan Order PickList: " Then

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & "O" & "</Parameter>" &
                        "<Parameter>" & sBarcode & "</Parameter>" &
                        "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                        "<Parameter>" & PWhse.ToString & "</Parameter>" &
                        "<Parameter>" & txtcustnum.Text & "</Parameter>" &
                        "<Parameter>" & ddlreturncode.SelectedItem.Value & "</Parameter>" &
                        "<Parameter>" & IIf(chkCancelTag.Checked = True, 1, 0) & "</Parameter>" &
                        "<Parameter>" & IIf(Session("Stat").ToString = "Ship", "I", "W") & "</Parameter>" &
                        "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "</Parameters>"


            ElseIf lblbarcode.Text = "Scan Tag: " Then

                Dim strPO As String = ""
                Dim strTagID As String = ""
                'Dim arrBarcode As String()
                arrBarcode = sBarcode.Split(New Char() {"|"c})

                If arrBarcode.Length > 0 Then

                    strPO = arrBarcode(0)
                    strTagID = arrBarcode(5)

                End If

                If Len(strPO) <> 10 Then

                    If Len(strTagID) = 17 Then
                        Dim lenBarcode As Integer = Len(sBarcode)
                        sBarcode = Right(Trim(sBarcode), lenBarcode - 1)
                    End If

                End If

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & "T" & "</Parameter>" &
                        "<Parameter>" & Trim(sBarcode) & "</Parameter>" &
                        "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                        "<Parameter>" & PWhse.ToString & "</Parameter>" &
                        "<Parameter>" & txtcustnum.Text & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & IIf(chkCancelTag.Checked = True, 1, 0) & "</Parameter>" &
                        "<Parameter>" & IIf(Session("Stat").ToString = "Ship", "I", "W") & "</Parameter>" &
                        "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & txtValidateCustItem.Text & "</Parameter>" &
                        "<Parameter>" & txtValidateCustPo.Text & "</Parameter>" &
                        "<Parameter>" & txtValidateQty.Text & "</Parameter>" &
                        "</Parameters>"

            ElseIf lblbarcode.Text = "Scan Cust Doc.: " Then

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                       "<Parameter>" & "C" & "</Parameter>" &
                       "<Parameter>" & sBarcode & "</Parameter>" &
                       "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                       "<Parameter>" & PWhse.ToString & "</Parameter>" &
                       "<Parameter>" & txtcustnum.Text & "</Parameter>" &
                       "<Parameter>" & DBNull.Value & "</Parameter>" &
                       "<Parameter>" & IIf(chkCancelTag.Checked = True, 1, 0) & "</Parameter>" &
                       "<Parameter>" & "I" & "</Parameter>" &
                       "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                       "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                       "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                       "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                       "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                       "<Parameter>" & DBNull.Value & "</Parameter>" &
                       "<Parameter>" & DBNull.Value & "</Parameter>" &
                       "<Parameter>" & DBNull.Value & "</Parameter>" &
                       "</Parameters>"

                'lblbarcode.Text = "Scan Tag: "

                '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error [PPCC]','Check Cust. Doc does not exits', 'warning');", True)
                'Else

                '    lblbarcode.Text = "Scan Tag: "

                'End If
                'ElseIf lblbarcode.Text = "Scan Label: " Then

                'CheckCustDoc = GetCheckCustDoc(txtcustnum.Text)

                'If CheckCustDoc = "0" Then

                'Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                '       "<Parameter>" & "L" & "</Parameter>" &
                '       "<Parameter>" & sBarcode & "</Parameter>" &
                '       "<Parameter>" & txtdate.Text & "</Parameter>" &
                '       "<Parameter>" & PWhse.ToString & "</Parameter>" &
                '       "<Parameter>" & txtcustnum.Text & "</Parameter>" &
                '       "<Parameter>" & DBNull.Value & "</Parameter>" &
                '       "<Parameter>" & IIf(chkCancelTag.Checked = True, 1, 0) & "</Parameter>" &
                '       "<Parameter>" & "I" & "</Parameter>" &
                '       "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                '       "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                '       "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                '       "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                '       "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                '       "</Parameters>"


            End If

            oWS = New CNIService.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_OrderShipSums", "PPCC_Ex_OrderShipSp", Parms)

            Dim doc As XmlDocument = New XmlDocument()
            doc.LoadXml(Parms)

            Dim i As Integer = 1

            For Each node As XmlNode In doc.DocumentElement

                If i = 12 Then
                    Stat = node.InnerText

                ElseIf i = 13 Then
                    MsgType = node.InnerText

                ElseIf i = 14 Then
                    MsgErr = node.InnerText

                End If

                i += 1

            Next

            If Stat = "TRUE" Then

                If lblbarcode.Text = "Scan Order PickList: " Then

                    BindGridview()
                    'DefaultOrderPicklistDate(sBarcode)

                    If PanelList.Items.Count > 0 Then

                        oWS = New CNIService.DOWebServiceSoapClient
                        ds = New DataSet

                        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Order_Picklist_Hdrs", "pick_list_date", "pick_list_num = '" & txtpicklistno.Text & "'", "", "", 0)

                        If ds.Tables(0).Rows.Count > 0 Then
                            txtdate.Text = Convert.ToDateTime(ds.Tables(0).Rows(0)("pick_list_date").ToString).ToString("dd/MM/yyyy")
                        End If

                    Else

                        DateNow = Date.Now.ToString("dd/MM/yyyy")

                        txtdate.Text = DateNow

                    End If


                    HiddenField4.Value = GetCheckCustDoc(txtcustnum.Text)
                    HiddenField1.Value = GetCheckDisPlayPic(txtcustnum.Text)
                    HiddenField3.Value = GetNotScanPickList(txtcustnum.Text)

                    Session.Remove("txtValidateCustItem")
                    Session.Remove("txtValidateCustPo")
                    Session.Remove("txtValidateQty")

                    txtValidateCustItem.Text = ""
                    txtValidateCustPo.Text = ""
                    txtValidateQty.Text = ""

                    'CheckPreInvoice = GetCheckPreInvoice(txtcustnum.Text)

                    'If CheckPreInvoice = "1" Then
                    '    ChkPreInv.Checked = True
                    'End If

                    If HiddenField4.Value = "0" Or HiddenField4.Value = "" Then

                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Warning','#732 : ไม่ได้กำหนด Check Cust. Doc', 'warning');", True)

                        If PanelList.Items.Count > 0 Then
                            lblbarcode.Text = "Scan Tag: "
                            Session("Scan") = "Scan Tag: "
                        End If

                    Else

                        If PanelList.Items.Count > 0 Then

                            ds = New DataSet
                            ds = GetCheckInformation(txtcustnum.Text)

                            If ds.Tables(0).Rows.Count > 0 Then

                                CheckCustItem = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_FixCustItem")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_FixCustItem").ToString)
                                CheckCustPO = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_FixCustPO")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_FixCustPO").ToString)
                                CheckQty = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_FixQty")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_FixQty").ToString)
                                CheckSepCustItem = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_SepCustItem")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_SepCustItem").ToString)
                                CheckSepCustPo = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_SepCustPo")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_SepCustPo").ToString)
                                CheckSepQty = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_SepQty")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_SepQty").ToString)

                            End If

                            lblValidate.Visible = True

                            If CheckCustItem = "1" Or CheckSepCustItem = "1" Then
                                lblValidateCustItem.Visible = True
                                txtValidateCustItem.Visible = True
                            End If

                            If CheckCustPO = "1" Or CheckSepCustPo = "1" Then
                                lblValidateCustPo.Visible = True
                                txtValidateCustPo.Visible = True
                            End If

                            If CheckQty = "1" Or CheckSepQty = "1" Then
                                lblValidateQty.Visible = True
                                txtValidateQty.Visible = True
                            End If

                            lblbarcode.Text = "Scan Cust Doc.: "
                            Session("Scan") = "Scan Cust Doc.: "

                        End If

                    End If


                ElseIf lblbarcode.Text = "Scan Tag: " Then

                    If sBarcode <> "OK" Then

                        HidItemModel.Value = ""
                        HidCustItemModel.Value = ""

                        arrBarcode = sBarcode.Split(New Char() {"|"c})

                        If Left(arrBarcode(4), 2) = "TD" Then
                            sTagID = arrBarcode(4)
                        Else
                            sTagID = arrBarcode(5)
                        End If

                        If HiddenField1.Value = "1" Then
                            Display(sTagID, Stat, "")
                        End If

                        'CheckCustDoc = GetCheckCustDoc(txtcustnum.Text)
                        'CheckScanPallet = GetCheckScanPallet(txtcustnum.Text)

                        BindGridview()

                        If HiddenField4.Value = "1" Then
                            lblbarcode.Text = "Scan Cust Doc.: "
                            Session("Scan") = "Scan Cust Doc.: "
                        Else
                            lblbarcode.Text = "Scan Tag: "
                            Session("Scan") = "Scan Tag: "
                        End If



                    End If

                    'If sBarcode = "OK" Then

                    '    lblbarcode.Text = "Scan Cust Doc.: "
                    '    Session("Scan") = "Scan Cust Doc.: "

                    'End If

                    'HiddenField2.Value = ""
                    'HiddenField2.Value = GetScanLabelItem(sBarcode) 'GetCoProductMix(sBarcode)

                    'If chkCancelTag.Checked = False Then

                    'If HiddenField2.Value = "1" Then
                    '    lblbarcode.Text = "Scan Label: "
                    '    Session("Scan") = "Scan Label: "
                    'Else

                    'If CheckCustDoc = "0" Or CheckCustDoc = "" Then

                    'If CheckScanPallet = "1" Then
                    '    lblbarcode.Text = "Scan Pallet: "
                    '    Session("Scan") = "Scan Pallet: "
                    'Else
                    'lblbarcode.Text = "Scan Tag: "
                    '        Session("Scan") = "Scan Tag: "
                    'End If

                    'Else

                    '            lblValidate.Visible = True
                    '            lblValidateCustItem.Visible = True
                    '            txtValidateCustItem.Visible = True
                    '            lblValidateCustPo.Visible = True
                    '            txtValidateCustPo.Visible = True
                    '            lblValidateQty.Visible = True
                    '            txtValidateQty.Visible = True

                    '            lblbarcode.Text = "Scan Cust Doc.: "
                    '            Session("Scan") = "Scan Cust Doc.: "

                    '        End If

                    'End If

                    'ElseIf chkCancelTag.Checked = True Then

                    '        lblbarcode.Text = "Scan Tag: "
                    '        Session("Scan") = "Scan Tag: "

                    '        chkCancelTag.Checked = False
                    '        lblValidate.Visible = False
                    '        lblValidateCustItem.Visible = False
                    '        txtValidateCustItem.Visible = False
                    '        lblValidateCustPo.Visible = False
                    '        txtValidateCustPo.Visible = False
                    '        lblValidateQty.Visible = False
                    '        txtValidateQty.Visible = False

                    '    End If




                    'If CheckCustDoc = "0" Or CheckCustDoc = "" Then

                    'If CheckScanPallet = "1" Then
                    '    lblbarcode.Text = "Scan Pallet: "
                    '    Session("Scan") = "Scan Pallet: "
                    'Else
                    '    lblbarcode.Text = "Scan Tag: "
                    '    Session("Scan") = "Scan Tag: "
                    'End If

                    'End If

                    'ElseIf lblbarcode.Text = "Scan Pallet: " Then

                    '        lblValidate.Visible = False
                    '        lblValidateCustItem.Visible = False
                    '        txtValidateCustItem.Visible = False
                    '        lblValidateCustPo.Visible = False
                    '        txtValidateCustPo.Visible = False
                    '        lblValidateQty.Visible = False
                    '        txtValidateQty.Visible = False

                    '        txtValidateCustItem.Text = ""
                    '        txtValidateCustPo.Text = ""
                    '        txtValidateQty.Text = ""

                    '        lblbarcode.Text = "Scan Tag: "
                    '        Session("Scan") = "Scan Tag: "

                ElseIf lblbarcode.Text = "Scan Cust Doc.: " Then

                    ds = New DataSet
                    ds = GetCheckInformation(txtcustnum.Text)

                    If ds.Tables(0).Rows.Count > 0 Then
                        CustTagType = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_TagType")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_TagType").ToString)
                        CheckCustItem = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_FixCustItem")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_FixCustItem").ToString)
                        CheckCustPO = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_FixCustPO")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_FixCustPO").ToString)
                        CheckQty = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_FixQty")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_FixQty").ToString)
                        StartCustItem = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_FixStartCustItem")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_FixStartCustItem").ToString)
                        LenghtCustItem = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_FixLenghtCustItem")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_FixLenghtCustItem").ToString)
                        StartCustPO = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_FixStartCustPO")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_FixStartCustPO").ToString)
                        LenghtCustPO = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_FixLenghtCustPO")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_FixLenghtCustPO").ToString)
                        StartQty = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_FixStartQty")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_FixStartQty").ToString)
                        LenghtQty = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_FixLenghtQty")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_FixLenghtQty").ToString)
                        CheckSepCustItem = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_SepCustItem")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_SepCustItem").ToString)
                        CheckSepCustPo = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_SepCustPo")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_SepCustPo").ToString)
                        CheckSepQty = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_SepQty")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_SepQty").ToString)
                        SepCustItemCol = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_SepCustItemCol")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_SepCustItemCol").ToString)
                        SepCustPoCol = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_SepCustPoCol")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_SepCustPoCol").ToString)
                        SepQtyCol = IIf(IsDBNull(ds.Tables(0).Rows(0)("cusUf_Customer_SepQtyCol")), "0", ds.Tables(0).Rows(0)("cusUf_Customer_SepQtyCol").ToString)
                    End If


                    If sBarcode.Length > 0 Then

                        If CustTagType = "F" Then

                            sBarcode = Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(sBarcode, "  ", ""), "-", ""), " ", ""), "/", ""), ",", ""), "|", ""), "&", ""), ";", "")

                            If CheckCustItem = "1" Then
                                txtValidateCustItem.Text = Mid(sBarcode, StartCustItem, LenghtCustItem)
                                Session("txtValidateCustItem") = txtValidateCustItem.Text
                            End If

                            If CheckCustPO = "1" Then
                                txtValidateCustPo.Text = Mid(sBarcode, StartCustPO, LenghtCustPO)
                                Session("txtValidateCustPo") = txtValidateCustPo.Text
                            End If

                            If CheckQty = "1" Then
                                txtValidateQty.Text = CInt(Mid(sBarcode, StartQty, LenghtQty))
                                Session("txtValidateQty") = txtValidateQty.Text
                            End If

                        Else

                            SepCustItemCol = SepCustItemCol - 1
                            SepCustPoCol = SepCustPoCol - 1
                            SepQtyCol = SepQtyCol - 1

                            sBarcode = Replace(Replace(Replace(Replace(Replace(Replace(Replace(sBarcode, "  ", ""), "-", ""), " ", ""), "/", ""), ",", ""), "&", ""), ";", "")

                            arrBarcode = sBarcode.Split(New Char() {"|"c})

                            If arrBarcode.Length > 0 Then
                                CustItem = arrBarcode(SepCustItemCol)
                                CustPo = arrBarcode(SepCustPoCol)
                                Qty = arrBarcode(SepQtyCol)
                            End If

                            If CheckSepCustItem = "1" Then
                                txtValidateCustItem.Text = CustItem
                                Session("txtValidateCustItem") = txtValidateCustItem.Text
                            End If

                            If CheckSepCustPo = "1" Then
                                txtValidateCustPo.Text = CustPo
                                Session("txtValidateCustPo") = txtValidateCustPo.Text
                            End If

                            If CheckSepQty = "1" Then
                                txtValidateQty.Text = CInt(Qty)
                                Session("txtValidateQty") = txtValidateQty.Text
                            End If


                        End If

                    End If

                    lblbarcode.Text = "Scan Tag: "
                    Session("Scan") = "Scan Tag: "

                    'CheckScanPallet = GetCheckScanPallet(txtcustnum.Text)


                    'If CheckScanPallet = "1" Then
                    '    lblbarcode.Text = "Scan Pallet: "
                    '    Session("Scan") = "Scan Pallet: "
                    'Else
                    '    lblbarcode.Text = "Scan Tag: "
                    '    Session("Scan") = "Scan Tag: "
                    'End If


                    'ElseIf lblbarcode.Text = "Scan Label: " Then

                    '        lblValidate.Visible = False
                    '    lblValidateCustItem.Visible = False
                    '    txtValidateCustItem.Visible = False
                    '    lblValidateCustPo.Visible = False
                    '    txtValidateCustPo.Visible = False
                    '    lblValidateQty.Visible = False
                    '    txtValidateQty.Visible = False

                    '    Session.Remove("txtValidateCustItem")
                    '    Session.Remove("txtValidateCustPo")
                    '    Session.Remove("txtValidateQty")

                    '    BindGridview()

                    '    CheckCustDoc = GetCheckCustDoc(txtcustnum.Text)
                    '    CheckScanPallet = GetCheckScanPallet(txtcustnum.Text)

                    '    If CheckCustDoc = "0" Or CheckCustDoc = "" Then

                    '        If CheckScanPallet = "1" Then
                    '            lblbarcode.Text = "Scan Pallet: "
                    '            Session("Scan") = "Scan Pallet: "
                    '        Else
                    '            lblbarcode.Text = "Scan Tag: "
                    '            Session("Scan") = "Scan Tag: "
                    '        End If

                    '    Else

                    '        lblbarcode.Text = "Scan Cust Doc.: "
                    '        Session("Scan") = "Scan Cust Doc.: "

                    '        lblValidate.Visible = True
                    '        lblValidateCustItem.Visible = True
                    '        txtValidateCustItem.Visible = True
                    '        lblValidateCustPo.Visible = True
                    '        txtValidateCustPo.Visible = True
                    '        lblValidateQty.Visible = True
                    '        txtValidateQty.Visible = True

                    '    End If

                    'lblbarcode.Text = "Scan Tag: "
                    'Session("Scan") = "Scan Tag: "

                    'If HiddenField2.Value = "1" Then

                    '    Dim sCheck As String = "0"

                    '    If GridView1.Rows.Count > 0 Then

                    '        For j As Integer = 0 To GridView1.Rows.Count - 1

                    '            If GridView1.Rows(j).Cells(6).Text = "1" Then
                    '                sCheck = "-1"
                    '                Exit For
                    '            End If

                    '        Next

                    '    End If

                    '    If sCheck = "0" Then
                    '        btnprocess.Attributes.Remove("disabled")
                    '    End If

                    'Else
                    '    btnprocess.Attributes.Remove("disabled")
                    'End If


                    ''btnprocess.Attributes.Remove("disabled")

                End If

                'If ChkPreInv.Checked = False Then
                '    Enablebtnprocess()
                'Else
                '    EnablebtnprocessPreInvoice()
                'End If


                'If lblbarcode.Text = "Scan Tag: " Then

                '    If sBarcode <> "OK" Then

                '        HiddenField2.Value = ""
                '        HiddenField2.Value = GetCoProductMix(sBarcode)

                '        If CheckCustDoc = "0" Then

                '            If CheckScanPallet = "1" Then
                '                lblbarcode.Text = "Scan Pallet: "
                '                Exit Sub
                '            End If

                '        Else

                '            Dim sSumTag As String = ""
                '            sSumTag = GetSumTag()

                '            If sSumTag = "1" Then
                '                lblValidate.Visible = True
                '                lblValidateCustItem.Visible = True
                '                txtValidateCustItem.Visible = True
                '                lblValidateCustPo.Visible = True
                '                txtValidateCustPo.Visible = True
                '                lblValidateQty.Visible = True
                '                txtValidateQty.Visible = True
                '            End If

                '            lblbarcode.Text = "Scan Cust Doc.: "

                '        End If

                '            'If CheckScanPallet = "0" Then

                '            '    If CheckCustDoc = "1" Then
                '            '        lblbarcode.Text = "Scan Cust Doc.: "
                '            '    End If

                '            'Else
                '            '    lblbarcode.Text = "Scan Pallet: "
                '            'End If

                '        End If

                '    If sBarcode = "OK" Then

                '        If HiddenField2.Value = "0" Then
                '            btnprocess.Attributes.Remove("disabled")
                '        Else
                '            lblbarcode.Text = "Scan Label: "
                '            Exit Sub
                '        End If

                '    End If

                'End If

                'If lblbarcode.Text = "Scan Pallet: " Then

                '    lblbarcode.Text = "Scan Tag: "

                '    'If CheckCustDoc = "1" Then
                '    '    lblbarcode.Text = "Scan Cust Doc.: "
                '    'Else
                '    '    lblbarcode.Text = "Scan Tag: "
                '    'End If


                'End If

                'If lblbarcode.Text = "Scan Cust Doc.: " Then

                '    Dim arrBarcode As String()
                '    arrBarcode = sBarcode.Split(New Char() {","c})

                '    If arrBarcode.Length > 0 Then
                '        txtValidateCustItem.Text = arrBarcode(0)
                '        txtValidateCustPo.Text = arrBarcode(1)
                '        txtValidateQty.Text = arrBarcode(2)
                '    End If

                '    If CheckScanPallet = "1" Then
                '        lblbarcode.Text = "Scan Pallet: "
                '    Else
                '        lblbarcode.Text = "Scan Tag: "
                '    End If

                '    'If CheckCustDoc = "1" Then

                '    lblbarcode.Text = "Scan Tag: "

                '    'End If

                'End If

                'If lblbarcode.Text = "Scan Label: " Then

                '    btnprocess.Attributes.Remove("disabled")

                'End If


            End If

            If Stat = "FALSE" Then

                If lblbarcode.Text <> "Scan Cust Doc.: " Then
                    lblValidate.Visible = False
                    lblValidateCustItem.Visible = False
                    txtValidateCustItem.Visible = False
                    lblValidateCustPo.Visible = False
                    txtValidateCustPo.Visible = False
                    lblValidateQty.Visible = False
                    txtValidateQty.Visible = False

                    'txtValidateCustItem.Text = ""
                    'txtValidateCustPo.Text = ""
                    'txtValidateQty.Text = ""
                End If

                If lblbarcode.Text = "Scan Tag: " Then

                    arrBarcode = sBarcode.Split(New Char() {"|"c})

                    If Left(arrBarcode(4), 2) = "TD" Then
                        sTagID = arrBarcode(4)
                    Else
                        sTagID = arrBarcode(5)
                    End If

                    MsgErr = MsgErr.Replace("'", "\'")
                    MsgErr = MsgErr.Replace(vbLf, "<br />")

                    Display(sTagID, Stat, MsgErr)

                Else

                    MsgErr = MsgErr.Replace("'", "\'")
                    MsgErr = MsgErr.Replace(vbLf, "<br />")

                    MsgType = "Error [" & MsgType & "]"

                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)

                End If


                'CheckCustDoc = GetCheckCustDoc(txtcustnum.Text)

                'If CheckCustDoc = "0" Or CheckCustDoc = "" Then

                '    lblbarcode.Text = "Scan Tag: "
                '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error [PPCC]','Check Cust. Doc does not exits', 'warning');", True)

                'End If

            End If

        End If

        txtbarcode.Text = String.Empty


    End Sub

    Protected Sub btnprocess_Click(sender As Object, e As EventArgs) Handles btnprocess.Click

        Dim Stat, MsgErr, MsgType As String
        Stat = "FALSE"
        MsgErr = ""
        MsgType = ""

        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                       "<Parameter>" & "" & "</Parameter>" &
                       "<Parameter>" & txtpicklistno.Text & "</Parameter>" &
                       "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                       "<Parameter>" & PWhse.ToString & "</Parameter>" &
                       "<Parameter>" & txtcustnum.Text & "</Parameter>" &
                       "<Parameter>" & ddlreturncode.SelectedItem.Value & "</Parameter>" &
                       "<Parameter>" & IIf(chkCancelTag.Checked = True, 1, 0) & "</Parameter>" &
                       "<Parameter>" & IIf(Session("Stat").ToString = "Ship", "P", "W") & "</Parameter>" &
                       "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                       "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                       "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                       "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                       "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                       "<Parameter>" & DBNull.Value & "</Parameter>" &
                       "<Parameter>" & DBNull.Value & "</Parameter>" &
                       "<Parameter>" & DBNull.Value & "</Parameter>" &
                       "</Parameters>"
        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_OrderShipSums", "PPCC_Ex_OrderShipSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)

        Dim i As Integer = 1

        For Each node As XmlNode In doc.DocumentElement

            If i = 12 Then
                Stat = node.InnerText

            ElseIf i = 13 Then
                MsgType = node.InnerText

            ElseIf i = 14 Then
                MsgErr = node.InnerText

            End If


            i += 1

        Next

        If Stat = "TRUE" Then

            Clear()

            MsgErr = MsgErr.Replace("'", "\'")
            MsgErr = MsgErr.Replace(vbLf, "<br />")

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Success','" & MsgErr & "', 'success');", True)

        Else

            MsgErr = MsgErr.Replace("'", "\'")
            MsgErr = MsgErr.Replace(vbLf, "<br />")

            MsgType = "Error [" & MsgType & "]"

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)


        End If

    End Sub

    Sub Clear()

        DateNow = Date.Now.ToString("dd/MM/yyyy")
        txtdate.Text = DateNow

        btnstat.CssClass = "btn btn-outline-success btn-block btn-sm"
        btnstat.Text = "<i class=""fa fa-arrow-right"" aria-hidden=""true""></i>" & " <strong>Order Shipping</strong>"
        Session("Stat") = "Ship"
        lblbarcode.Text = "Scan Order PickList: "

        chkCancelTag.Checked = False
        'ChkPreInv.Checked = False
        txtbarcode.Text = String.Empty
        txtpicklistno.Text = String.Empty
        'txtshippingno.Text = String.Empty
        txtcustnum.Text = String.Empty
        txtdescription.Text = String.Empty

        'ddlreturncode.SelectedIndex = ddlreturncode.Items.IndexOf(ddlreturncode.Items.FindByValue(""))
        'ddlreturncode.SelectedIndex = 0
        ddlreturncode.Items.Clear()
        GetReasonCode()

        PanelList.DataSource = Nothing
        PanelList.DataBind()

        'SGUID = System.Guid.NewGuid.ToString()
        Session("PSession") = NewSessionID()

        HiddenField2.Value = ""

        lblValidate.Visible = False
        lblValidateCustItem.Visible = False
        txtValidateCustItem.Visible = False
        lblValidateCustPo.Visible = False
        txtValidateCustPo.Visible = False
        lblValidateQty.Visible = False
        txtValidateQty.Visible = False

        txtValidateCustItem.Text = String.Empty
        txtValidateCustPo.Text = String.Empty
        txtValidateQty.Text = String.Empty

        Session("Scan") = "Scan Order PickList: "

        Session.Remove("txtValidateQty")
        Session.Remove("txtValidateCustPo")
        Session.Remove("txtValidateQty")

        HiddenField1.Value = ""
        HiddenField3.Value = ""
        HiddenField4.Value = ""

    End Sub

    Sub ClearChangeStatus()

        DateNow = Date.Now.ToString("dd/MM/yyyy")
        txtdate.Text = DateNow

        'btnstat.CssClass = "btn btn-outline-success btn-block btn-sm"
        'btnstat.Text = "<i class=""fa fa-arrow-right"" aria-hidden=""true""></i>" & " <strong>Order Shipping</strong>"
        'Session("Stat") = "Ship"
        'lblbarcode.Text = "Scan Order PickList: "

        chkCancelTag.Checked = False
        'ChkPreInv.Checked = False
        txtbarcode.Text = String.Empty
        txtpicklistno.Text = String.Empty
        'txtshippingno.Text = String.Empty
        txtcustnum.Text = String.Empty
        txtdescription.Text = String.Empty

        'ddlreturncode.SelectedIndex = ddlreturncode.Items.IndexOf(ddlreturncode.Items.FindByValue(""))
        'ddlreturncode.SelectedIndex = 0
        ddlreturncode.Items.Clear()
        GetReasonCode()

        PanelList.DataSource = Nothing
        PanelList.DataBind()

        'SGUID = System.Guid.NewGuid.ToString()
        Session("PSession") = NewSessionID()

        HiddenField2.Value = ""

        lblValidate.Visible = False
        lblValidateCustItem.Visible = False
        txtValidateCustItem.Visible = False
        lblValidateCustPo.Visible = False
        txtValidateCustPo.Visible = False
        lblValidateQty.Visible = False
        txtValidateQty.Visible = False

        txtValidateCustItem.Text = String.Empty
        txtValidateCustPo.Text = String.Empty
        txtValidateQty.Text = String.Empty

        Session("Scan") = "Scan Order PickList: "

        Session.Remove("txtValidateQty")
        Session.Remove("txtValidateCustPo")
        Session.Remove("txtValidateQty")

        HiddenField1.Value = ""
    End Sub

    Protected Sub btnreset_Click(sender As Object, e As EventArgs) Handles btnreset.Click

        Dim Stat, MsgErr, MsgType As String
        Stat = "FALSE"
        MsgErr = ""
        MsgType = ""

        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                       "<Parameter>" & "" & "</Parameter>" &
                       "<Parameter>" & DBNull.Value & "</Parameter>" &
                       "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                       "<Parameter>" & PWhse.ToString & "</Parameter>" &
                       "<Parameter>" & txtcustnum.Text & "</Parameter>" &
                       "<Parameter>" & DBNull.Value & "</Parameter>" &
                       "<Parameter>" & IIf(chkCancelTag.Checked = True, 1, 0) & "</Parameter>" &
                       "<Parameter>" & "R" & "</Parameter>" &
                       "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                       "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                       "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                       "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                       "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                       "<Parameter>" & DBNull.Value & "</Parameter>" &
                       "<Parameter>" & DBNull.Value & "</Parameter>" &
                       "<Parameter>" & DBNull.Value & "</Parameter>" &
                       "</Parameters>"
        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_OrderShipSums", "PPCC_Ex_OrderShipSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)

        Dim i As Integer = 1

        For Each node As XmlNode In doc.DocumentElement

            If i = 12 Then
                Stat = node.InnerText

            ElseIf i = 13 Then
                MsgType = node.InnerText

            ElseIf i = 14 Then
                MsgErr = node.InnerText

            End If


            i += 1

        Next

        If Stat = "TRUE" Then

            'SGUID = System.Guid.NewGuid.ToString()
            Session("PSession") = NewSessionID()

            Response.Redirect("OrderShipping.aspx")

        Else

            MsgErr = MsgErr.Replace("'", "\'")
            MsgErr = MsgErr.Replace(vbLf, "<br />")

            MsgType = "Error [" & MsgType & "]"

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)


            'NotPassNotifyPanel.Visible = True
            'NotPassText.Text = MsgErr

        End If

    End Sub

    Protected Sub btnstat_Click(sender As Object, e As EventArgs) Handles btnstat.Click

        If Session("Stat").ToString = "Ship" Then
            btnstat.CssClass = "btn btn btn-outline-danger btn-block btn-sm"
            btnstat.Text = "<i class=""fa fa-repeat"" aria-hidden=""true""></i>" & " <strong>Return</strong>"
            Session("Stat") = "Return"
        Else
            btnstat.CssClass = "btn btn-outline-success btn-block btn-sm"
            btnstat.Text = "<i class=""fa fa-arrow-right"" aria-hidden=""true""></i>" & " <strong>Order Shipping</strong>"
            Session("Stat") = "Ship"
        End If

        'ddlreturncode.Items.Clear()
        'GetReasonCode()

        ClearChangeStatus()

        If Session("Stat").ToString = "Ship" Then
            'ddlreturncode.SelectedIndex = ddlreturncode.Items.IndexOf(ddlreturncode.Items.FindByValue(""))
            ddlreturncode.SelectedIndex = 0
            ddlreturncode.Attributes.Add("disabled", "disabled")
        Else
            ddlreturncode.Attributes.Remove("disabled")
        End If

    End Sub

    'Protected Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound

    '    If e.Row.RowType = DataControlRowType.DataRow Then

    '        e.Row.Cells(3).Text = FormatNumber(e.Row.Cells(3).Text, LenPointQty)
    '        e.Row.Cells(4).Text = FormatNumber(e.Row.Cells(4).Text, LenPointQty)
    '        e.Row.Cells(5).Text = FormatNumber(e.Row.Cells(5).Text, LenPointQty)


    '    End If

    'End Sub

    Protected Sub PanelList_RowDataBound(sender As Object, e As ListViewItemEventArgs) Handles PanelList.ItemDataBound

        If e.Item.ItemType = ListViewItemType.DataItem Then

            Dim lblQtyOrdered As Label = CType(e.Item.FindControl("lblQtyOrdered"), Label)
            Dim lblSumQty As Label = CType(e.Item.FindControl("lblSumQty"), Label)
            Dim lblQtyRemain As Label = CType(e.Item.FindControl("lblQtyRemain"), Label)
            Dim lblQtyPick As Label = CType(e.Item.FindControl("lblQtyPick"), Label)
            Dim lblconfirm As Label = CType(e.Item.FindControl("lblconfirm"), Label)

            Dim QtyPick As Decimal = 0
            Dim SumQty As Decimal = 0
            Dim QtyOrdered As Decimal = 0
            Dim QtyRemain As Decimal = 0

            Decimal.TryParse(lblQtyPick.Text, QtyPick)
            Decimal.TryParse(lblSumQty.Text, SumQty)
            Decimal.TryParse(lblQtyOrdered.Text, QtyOrdered)
            Decimal.TryParse(lblQtyRemain.Text, QtyRemain)

            lblQtyOrdered.Text = FormatNumber(QtyOrdered, LenPointQty)
            lblSumQty.Text = FormatNumber(SumQty, LenPointQty)
            lblQtyRemain.Text = FormatNumber(QtyRemain, LenPointQty)
            lblQtyPick.Text = FormatNumber(QtyPick, LenPointQty)

            If HiddenField3.Value = "1" Then

                If SumQty = QtyOrdered Then
                    lblconfirm.Visible = True
                Else
                    lblconfirm.Visible = False
                End If

            Else

                If SumQty = QtyPick And QtyPick <> 0 Then
                    lblconfirm.Visible = True
                Else
                    lblconfirm.Visible = False
                End If

            End If




        End If

    End Sub

    Protected Sub ddlreturncode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlreturncode.SelectedIndexChanged
        txtbarcode.Focus()
    End Sub

    Sub Display(sTagID As String, Stat As String, ErrorMsg As String)

        If Stat = "TRUE" Then
            Me.StatModelCorrect.Visible = True
            Me.StatModelInCorrect.Visible = False
        Else
            Me.StatModelCorrect.Visible = False
            Me.StatModelInCorrect.Visible = True
            lblmsg.Text = ErrorMsg
        End If

        HidItemModel.Value = ""
        HidCustItemModel.Value = ""

        PopUpTagDetail(sTagID, HidItemModel.Value, HidCustItemModel.Value)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "Pop", "openModel('" & HidItemModel.Value & "', '" & HidCustItemModel.Value & "', '" & ErrorMsg & "');", True)

    End Sub

    Protected Sub PanelList_ItemCommand(ByVal sender As Object, ByVal e As ListViewCommandEventArgs) Handles PanelList.ItemCommand
        'Dim lblconum As Label = CType(e.Item.FindControl("lblconum"), Label)
        'Dim lblcoline As Label = CType(e.Item.FindControl("lblcoline"), Label)
        'Dim lblcorelease As Label = CType(e.Item.FindControl("lblcorelease"), Label)

        Dim PostURL As String = ""

        PostURL = "?SessionID=" & Session("PSession").ToString & "&CoNum=" & CType(e.Item.FindControl("lblconum"), Label).Text & "&CoLine=" & CType(e.Item.FindControl("lblcoline"), Label).Text & ""
        PostURL = PostURL & "&CoRelease=" & CType(e.Item.FindControl("lblcorelease"), Label).Text & "&OrderPickList=" & txtpicklistno.Text & "&CoReturn=" & ddlreturncode.SelectedItem.Value & "&ShowPic=" & HiddenField1.Value & ""

        Response.Redirect("OrderShippingDetail.aspx" & PostURL)
    End Sub



    Public Function UnitQtyFormat() As Integer

        Dim strUnitQtyFormat As String = ""
        Dim PointQty As String = ""

        UnitQtyFormat = 0

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLInvparms", "QtyUnitFormat", "", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            strUnitQtyFormat = ds.Tables(0).Rows(0)("QtyUnitFormat").ToString

            Dim words As String() = strUnitQtyFormat.Split(New Char() {"."c})

            For Each word As String In words
                PointQty = words(1)
                Exit For
            Next

            UnitQtyFormat = Len(PointQty)

        End If

        Return UnitQtyFormat

    End Function

    Function GetCheckCustDoc(CustNum As String) As String

        GetCheckCustDoc = "0"

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLCustomers", "cusUf_Customer_CheckCustDoc", "CustNum = '" & CustNum & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetCheckCustDoc = ds.Tables(0).Rows(0)("cusUf_Customer_CheckCustDoc").ToString
        End If

        Return GetCheckCustDoc

    End Function

    Function GetCheckDisPlayPic(CustNum As String) As String

        GetCheckDisPlayPic = "0"

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLCustomers", "cusUf_Customer_DisplayPic", "CustNum = '" & CustNum & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetCheckDisPlayPic = ds.Tables(0).Rows(0)("cusUf_Customer_DisplayPic").ToString
        End If

        Return GetCheckDisPlayPic

    End Function


    'Function GetCheckScanPallet(CustNum As String) As String

    '    GetCheckScanPallet = ""

    '    oWS = New CNIService.DOWebServiceSoapClient

    '    ds = New DataSet

    '    ds = oWS.LoadDataSet(Session("Token").ToString, "SLCustomers", "cusUf_Customer_ScanPallet", "CustNum = '" & CustNum & "'", "", "", 0)

    '    If ds.Tables(0).Rows.Count > 0 Then
    '        GetCheckScanPallet = ds.Tables(0).Rows(0)("cusUf_Customer_ScanPallet").ToString
    '    End If

    '    Return GetCheckScanPallet

    'End Function


    'Function GetCheckPreInvoice(CustNum As String) As String

    '    GetCheckPreInvoice = "0"

    '    oWS = New CNIService.DOWebServiceSoapClient

    '    ds = New DataSet

    '    ds = oWS.LoadDataSet(Session("Token").ToString, "SLCustomers", "cusUf_customer_InBeforeShip", "CustNum = '" & CustNum & "'", "", "", 0)

    '    If ds.Tables(0).Rows.Count > 0 Then
    '        GetCheckPreInvoice = ds.Tables(0).Rows(0)("cusUf_customer_InBeforeShip").ToString
    '    End If

    '    Return GetCheckPreInvoice

    'End Function

    Function GetCheckInformation(CustNum As String) As DataSet

        'GetCheckInformation = "0"

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLCustomers", "cusUf_Customer_TagType, cusUf_Customer_FixCustItem, cusUf_Customer_FixCustPO, cusUf_Customer_FixQty, cusUf_Customer_FixStartCustItem, cusUf_Customer_FixLenghtCustItem, cusUf_Customer_FixStartCustPO, cusUf_Customer_FixLenghtCustPO, cusUf_Customer_FixStartQty, cusUf_Customer_FixLenghtQty, cusUf_Customer_SepCustItem, cusUf_Customer_SepCustPO, cusUf_Customer_SepQty, cusUf_Customer_SepCustItemCol, cusUf_Customer_SepCustPoCol, cusUf_Customer_SepQtyCol", "CustNum = '" & CustNum & "'", "", "", 0)

        Return ds

    End Function

    Function GetSumTag() As String

        GetSumTag = "0"

        oWS = New CNIService.DOWebServiceSoapClient

        Dim Filter As String = ""

        ds = New DataSet

        Filter = "SessionID = '" & Session("PSession").ToString & "' And QtySum > 0 And QtySum = QtyOrder"
        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Ex_OrderShipSums", "QtySum", Filter, "RecordDate DESC", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetSumTag = "1"
        End If

        Return GetSumTag

    End Function

    Function GetCustomer(PickListNum As String) As DataSet

        Dim CustNum As String = ""

        Filter = "OrderpicklistNum = '" & PickListNum & "'"

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_OrderPickList_Lines", "CustNum", Filter, "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            CustNum = ds.Tables(0).Rows(0)("CustNum").ToString
        End If

        If CustNum <> "" Then

            oWS = New CNIService.DOWebServiceSoapClient

            ds = New DataSet

            ds = oWS.LoadDataSet(Session("Token").ToString, "SLCustomers", "CustNum, Name", Filter, "", "", 0)

        End If


        Return ds

    End Function

    Function GetSite() As String

        GetSite = ""

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLParms", "Site", "", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetSite = ds.Tables(0).Rows(0)("Site").ToString
        End If

        Return GetSite

    End Function

    Function GetDefWhse() As String

        GetDefWhse = ""
        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token"), "SLInvparms", "DefWhse", "", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetDefWhse = ds.Tables(0).Rows(0)("DefWhse").ToString
        End If

        Return GetDefWhse

    End Function

    Function GetScanLabelItem(sBarcode As String) As String

        GetScanLabelItem = "0"
        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token"), "PPCC_Tags", "ScanLabelItem", "TagID = '" & sBarcode & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetScanLabelItem = ds.Tables(0).Rows(0)("ScanLabelItem").ToString
        End If

        Return GetScanLabelItem

    End Function

    Function GetImagePath() As String

        GetImagePath = ""
        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token"), "UserDefinedTypeValues", "Description", "TypeName = 'PPCC_FilePath' AND Value = 'PPCC_ItemPicPath'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetImagePath = ds.Tables(0).Rows(0)("Description").ToString
        End If

        Return GetImagePath

    End Function

    Sub Enablebtnprocess()

        Dim LabelID As String = ""

        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token"), "PPCC_Ex_OrderShipDetails", "TagID, LabelID", "SessionID = '" & Session("PSession").ToString & "'", "Createdate desc", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then

            'If HiddenField2.Value = "1" Then

            '    LabelID = ds.Tables(0).Rows(0)("LabelID").ToString

            '    If LabelID.ToString <> "" Then
            '        btnprocess.Attributes.Remove("disabled")
            '    End If

            'Else
            btnprocess.Attributes.Remove("disabled")
            'End If

        End If


    End Sub

    Sub EnablebtnprocessPreInvoice()

        Dim LabelItem As String = "0"
        Dim sCount1 As Integer = 0
        Dim sCount2 As Integer = 0

        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet

        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet

        If Not IsNothing(Session("Stat")) Then

            If Session("Stat").ToString = "Ship" Then
                ds = oWS.LoadDataSet(Session("Token"), "PPCC_Ex_OrderShipSums", "CoNum, CoLine, CoRelease", "SessionID = '" & Session("PSession").ToString & "' And QtySum = QtyOrder", "Createdate desc", "", 0)
            Else
                ds = oWS.LoadDataSet(Session("Token"), "PPCC_Ex_OrderShipSums", "CoNum, CoLine, CoRelease", "SessionID = '" & Session("PSession").ToString & "' And (QtySum*-1) = QtyOrder", "Createdate desc", "", 0)
            End If

        End If

        If ds.Tables(0).Rows.Count > 0 Then

            sCount1 = ds.Tables(0).Rows.Count

            'If HiddenField2.Value = "1" Then

            '    ds = oWS.LoadDataSet(Session("Token"), "PPCC_Ex_OrderShipDetails", "TagID, LabelID, LabelItem", "SessionID = '" & Session("PSession").ToString & "'", "Createdate desc", "", 0)

            '    If ds.Tables(0).Rows.Count > 0 Then
            '        LabelItem = ds.Tables(0).Rows(0)("LabelItem").ToString

            '        If LabelItem.ToString = "1" Then

            '            ds = oWS.LoadDataSet(Session("Token"), "PPCC_Ex_OrderShipSums", "CoNum, CoLine, CoRelease", "SessionID = '" & Session("PSession").ToString & "'", "Createdate desc", "", 0)

            '            If ds.Tables(0).Rows.Count > 0 Then

            '                sCount2 = ds.Tables(0).Rows.Count

            '                If sCount1 = sCount2 Then
            '                    btnprocess.Attributes.Remove("disabled")
            '                End If

            '            End If

            '        End If

            '    End If

            'Else

            ds = oWS.LoadDataSet(Session("Token"), "PPCC_Ex_OrderShipSums", "CoNum, CoLine, CoRelease", "SessionID = '" & Session("PSession").ToString & "'", "Createdate desc", "", 0)

            If ds.Tables(0).Rows.Count > 0 Then

                sCount2 = ds.Tables(0).Rows.Count

                If sCount1 = sCount2 Then
                    btnprocess.Attributes.Remove("disabled")
                End If

            End If

            'End If

        End If


    End Sub


    Function GetCoProductMix(TagID As String) As String

        GetCoProductMix = "0"

        Dim sItem As String = ""

        Filter = "TagID = '" & TagID & "'"

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Tags", "Item", Filter, "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            sItem = ds.Tables(0).Rows(0)("Item").ToString
        End If

        If sItem <> "" Then

            Filter = "Item = '" & sItem & "'"

            oWS = New CNIService.DOWebServiceSoapClient

            ds = New DataSet

            ds = oWS.LoadDataSet(Session("Token").ToString, "SLProdMixItems", "Item", Filter, "", "", 0)

            If ds.Tables(0).Rows.Count > 0 Then
                GetCoProductMix = "1"
            End If

        End If


        Return GetCoProductMix

    End Function

    'Function GetAccessOrderShipping() As String

    '    GetAccessOrderShipping = "0"

    '    'Dim sUserUnLock As String = ""

    '    'Filter = "UserMissOper = '" & Session("UserName").ToString & "'"

    '    'oWS = New CNIService.DOWebServiceSoapClient

    '    'ds = New DataSet

    '    'ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Ex_LogMiss", "UserMissOper", Filter, "RecordDate desc", "", 0)

    '    'If ds.Tables(0).Rows.Count > 0 Then
    '    '    sUserUnLock = ds.Tables(0).Rows(0)("UserUnLock").ToString
    '    'End If

    '    'If sUserUnLock <> "" Then

    '    '    GetAccessOrderShipping = "Lock"

    '    'End If

    '    oWS = New CNIService.DOWebServiceSoapClient

    '    Parms = "<Parameters><Parameter>" & Session("UserName").ToString & "</Parameter>" &
    '                    "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
    '                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
    '                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
    '                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
    '                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
    '                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
    '                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
    '                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
    '                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
    '                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
    '                    "</Parameters>"

    '    oWS = New CNIService.DOWebServiceSoapClient
    '    oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobTranTags", "PPCC_EX_GetAccessFormSp", Parms)

    '    Dim doc As XmlDocument = New XmlDocument()
    '    doc.LoadXml(Parms)

    '    Dim i As Integer = 1

    '    For Each node As XmlNode In doc.DocumentElement


    '        If i = 11 Then
    '            GetAccessOrderShipping = node.InnerText
    '        End If

    '        i += 1

    '    Next

    '    Return GetAccessOrderShipping

    'End Function

    Public Function URLExists(ByVal url As String) As Boolean

        Dim webRequest As System.Net.WebRequest = System.Net.WebRequest.Create(url)
        webRequest.Method = "HEAD"

        Try
            Dim response As System.Net.HttpWebResponse = CType(webRequest.GetResponse, System.Net.HttpWebResponse)

            If (response.StatusCode.ToString = "OK") Then
                Return True
            End If

            Return False

        Catch
            Return False

        End Try

    End Function

    Sub PopUpTagDetail(ByVal TagID As String, ByRef Item As String, ByRef CustItem As String)

        Item = ""
        CustItem = ""

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Tags", "item, cust_item", "TagID = '" & TagID & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            Item = ds.Tables(0).Rows(0)("item").ToString
            CustItem = ds.Tables(0).Rows(0)("cust_item").ToString
        End If

        If Item <> "" Then

            Dim imageUrl1 As String = PImagePath.ToString & Item & ".jpg"
            Dim imageUrl2 As String = PImagePath.ToString & Item & "-1.jpg"

            If URLExists(imageUrl1) = True Then
                Image1.ImageUrl = imageUrl1
            Else
                Image1.ImageUrl = "asset/image/No_picture_available.png"
            End If

            If URLExists(imageUrl2) = True Then
                Image2.ImageUrl = imageUrl2
            Else
                Image2.ImageUrl = "asset/image/No_picture_available.png"
            End If

            'oWS = New CNIService.DOWebServiceSoapClient

            'ds = New DataSet

            'ds = oWS.LoadDataSet(Session("Token").ToString, "SLItems", "Picture", "Item = '" & Item & "'", "", "", 0)

            'If ds.Tables(0).Rows.Count > 0 Then

            '    If Not IsDBNull(ds.Tables(0).Rows(0)("Picture")) Then
            '        'Dim imageUrl1 As String = "data:image/jpg;base64," & Convert.ToBase64String(CType(ds.Tables(0).Rows(0)("Picture"), Byte()))
            '        Dim imageUrl1 As String = PImagePath.ToString & Item & ".jpg"
            '        Image1.ImageUrl = imageUrl1
            '    Else
            '        Image1.ImageUrl = "asset/image/No_picture_available.png"
            '    End If

            'Else
            '    Image1.ImageUrl = "asset/image/No_picture_available.png"
            'End If


            'oWS = New CNIService.DOWebServiceSoapClient

            'ds = New DataSet

            'ds = oWS.LoadDataSet(Session("Token").ToString, "SLItemPictures", "Picture", "Item = '" & Item & "' AND Seq = 1", "", "", 0)

            'If ds.Tables(0).Rows.Count > 0 Then
            '    'Dim imageUrl2 As String = "data:image/jpg;base64," & Convert.ToBase64String(CType(ds.Tables(0).Rows(0)("Picture"), Byte()))
            '    Dim imageUrl2 As String = PImagePath.ToString & Item & "-1.jpg"
            '    Image2.ImageUrl = imageUrl2
            'Else
            '    Image2.ImageUrl = "asset/image/No_picture_available.png"
            'End If

        End If


    End Sub



#Region "Get Data Bind To Dropdownlist"
    Sub GetReasonCode()

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLReasons", "ReasonCode, Description", "ReasonClass = 'CO RETURN'", "ReasonCode", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddlreturncode.Items.Add(New ListItem(dRow("ReasonCode") & IIf(IsDBNull(dRow("Description")), "", " : " & dRow("Description")), dRow("ReasonCode")))

        Next

        If Session("Stat") Is Nothing Then
            ddlreturncode.Items.Insert(0, New ListItem("", ""))
        Else
            If Session("Stat").ToString = "Ship" Then
                ddlreturncode.Items.Insert(0, New ListItem("", ""))
            End If
        End If

        'ddlreturncode.Items.Insert(0, New ListItem("", ""))

    End Sub
#End Region

#Region "Bind Data To Gridview"

    Sub BindGridview()

        Dim Filter As String
        Dim Propertie As String

        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet
        Filter = "SessionID = '" & Session("PSession").ToString & "'"
        Propertie = "DerCO, CoNum, CoLine, CoRelease, Item, CustPO, QtyOrder, QtySum, QtyRemain, OrderPickListNum, CustNum, CustName, TransDate, CoReturn, CustItem, LabelItem, QtyPick, DerCountConfirm"

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Ex_OrderShipSums", Propertie, Filter, "CoNum, CoLine, CoRelease  Desc", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then

            txtcustnum.Text = ds.Tables(0).Rows(0)("CustNum").ToString
            txtpicklistno.Text = ds.Tables(0).Rows(0)("OrderPickListNum").ToString
            txtdescription.Text = ds.Tables(0).Rows(0)("CustName").ToString
            lbltotallineconfirm.Text = ds.Tables(0).Rows(0)("DerCountConfirm").ToString
            'ddlreturncode.SelectedItem.Value = ddlreturncode.Items.IndexOf(ddlreturncode.Items.FindByValue(ds.Tables(0).Rows(0)("CoReturn").ToString))

            'GridView1.DataSource = ds.Tables(0)
            'GridView1.DataBind()

            PanelList.DataSource = ds.Tables(0)
            PanelList.DataBind()

        End If


    End Sub


    Function GetNotScanPickList(CustNum As String) As String

        GetNotScanPickList = "0"

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLCustomers", "cusUf_Customer_NotScanPickList", "CustNum = '" & CustNum & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetNotScanPickList = ds.Tables(0).Rows(0)("cusUf_Customer_NotScanPickList").ToString
        End If

        Return GetNotScanPickList

    End Function


    Function NewSessionID() As String

        NewSessionID = ""

        Parms = "<Parameters><Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_QtyMove", "PPCC_Ex_GetSessionIDSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)

        Dim i As Integer = 1

        For Each node As XmlNode In doc.DocumentElement

            If i = 1 Then
                NewSessionID = node.InnerText
                Exit For
            End If

            i += 1
        Next

        Return NewSessionID

    End Function

#End Region

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim item, custitem As String
        item = "Item"
        custitem = "custiten"

        Page.ClientScript.RegisterStartupScript(Me.GetType(), "Pop", "openModel('" & item & "', '" & custitem & "');", True)

    End Sub


End Class