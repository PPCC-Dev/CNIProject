Imports System.Data
Imports System.Xml
Imports System.Collections
Imports System.Reflection
Imports Microsoft.VisualBasic.DateInterval
Imports System.Drawing
Imports System.IO

Public Class ConfirmOrderPickList
    Inherits System.Web.UI.Page

    Dim oWS As CNIService.DOWebServiceSoapClient
    Dim ds As DataSet
    Dim dt_match As DataTable
    Dim Filter As String
    Dim res As Object
    Dim SGUID As String
    Dim Parms As String
    Dim LenPointQty As Integer = 0
    Dim LenPointQtyFormat As Integer = 0
    Dim DateNow As String

    Private Shared ParmSite As String
    Private Shared Whse As String
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

        LenPointQty = UnitQtyFormat()

        If Not Page.IsPostBack Then

            PSite = GetSite()
            PWhse = GetDefWhse()
            PImagePath = GetImagePath()

            lblbarcode.Text = "Scan Order Pick List: "

            divScanDetail.Visible = False
            divPickListDetail.Visible = False

            If Request.QueryString("OrderPickList") <> "" Or Not String.IsNullOrEmpty(Request.QueryString("OrderPickList")) Then

                lblbarcode.Text = Session("Scan")

                divScanDetail.Visible = True
                divPickListDetail.Visible = True

                BindGridview()

                lblLoc.Text = Request.QueryString("Loc").ToString

                HiddenField1.Value = GetCheckCustDoc(lblCustNum.Text)
                'HiddenField2.Value = GetNotScanPickList(lblCustNum.Text)

                lblTotalPickLine.Text = FormatNumber(Session("lblTotalPickLine").ToString, LenPointQty)
                lblTotalConfirm.Text = FormatNumber(Session("lblTotalConfirm").ToString, LenPointQty)

                If Session("lblScanCustItem") IsNot Nothing Then
                    lblScanCustItem.Text = Session("lblScanCustItem").ToString
                End If

                If Session("lblScanCustPO") IsNot Nothing Then
                    lblScanCustPO.Text = Session("lblScanCustPO").ToString
                End If

                If Session("lblScanQty") IsNot Nothing Then
                    lblScanQty.Text = Session("lblScanQty").ToString
                End If

                If Request.QueryString("ModeType") <> "" Or Not String.IsNullOrEmpty(Request.QueryString("ModeType")) Then

                    If Request.QueryString("ModeType").ToString = "C" Then
                        RadioConfirm.Checked = True
                    ElseIf Request.QueryString("ModeType").ToString = "H" Then
                        RadioChange.Checked = True
                    ElseIf Request.QueryString("ModeType").ToString = "R" Then
                        RadioReject.Checked = True
                    Else
                        RadioConfirm.Checked = True
                    End If

                End If

            End If

        End If

        txtbarcode.Focus()

        txtbarcode.Attributes.Add("onchange", "javascript:scanbarcode();")

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim sBarcode, Stat, MsgErr, MsgType, CheckCustDoc, CustTagType As String
        Dim StartCustItem, LenghtCustItem, StartCustPO, LenghtCustPO, StartQty, LenghtQty, SepCustItemCol, SepCustPoCol, SepQtyCol As String
        Dim CustItem, CustPo, Qty, sTagID As String
        Dim CheckCustItem, CheckCustPO, CheckQty, CheckSepCustItem, CheckSepCustPo, CheckSepQty As String
        Dim TotalLine, TotalConfirm, ModeType As String

        Dim arrBarcode As String()
        sBarcode = txtbarcode.Text
        Stat = "TRUE"
        MsgErr = ""
        MsgType = ""
        CheckCustDoc = "0"
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
        TotalLine = "0"
        TotalConfirm = "0"

        If RadioConfirm.Checked = True Then
            ModeType = "C"
        ElseIf RadioChange.Checked = True Then
            ModeType = "H"
        ElseIf RadioReject.Checked = True Then
            ModeType = "R"
        Else
            ModeType = "C"
        End If

        If txtbarcode.Text <> "" Then

            If lblbarcode.Text = "Scan Order Pick List: " Then

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & ModeType & "</Parameter>" &
                        "<Parameter>" & "O" & "</Parameter>" &
                        "<Parameter>" & sBarcode & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter>" & IIf(chkCancelTag.Checked = True, 1, 0) & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & PWhse.ToString & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "</Parameters>"

            ElseIf lblbarcode.Text = "Scan Location: " Then

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & ModeType & "</Parameter>" &
                        "<Parameter>" & "L" & "</Parameter>" &
                        "<Parameter>" & sBarcode & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter>" & IIf(chkCancelTag.Checked = True, 1, 0) & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & lblCustNum.Text & "</Parameter>" &
                        "<Parameter>" & lblPickListNo.Text & "</Parameter>" &
                        "<Parameter>" & lblScanCustItem.Text & "</Parameter>" &
                        "<Parameter>" & lblScanCustPO.Text & "</Parameter>" &
                        "<Parameter>" & lblScanQty.Text & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & PWhse.ToString & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "</Parameters>"

            ElseIf lblbarcode.Text = "Scan Customer Tag: " Then

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & ModeType & "</Parameter>" &
                        "<Parameter>" & "C" & "</Parameter>" &
                        "<Parameter>" & sBarcode & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter>" & IIf(chkCancelTag.Checked = True, 1, 0) & "</Parameter>" &
                        "<Parameter>" & lblLoc.Text & "</Parameter>" &
                        "<Parameter>" & lblCustNum.Text & "</Parameter>" &
                        "<Parameter>" & lblPickListNo.Text & "</Parameter>" &
                        "<Parameter>" & lblScanCustItem.Text & "</Parameter>" &
                        "<Parameter>" & lblScanCustPO.Text & "</Parameter>" &
                        "<Parameter>" & lblScanQty.Text & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & PWhse.ToString & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "</Parameters>"

            ElseIf lblbarcode.Text = "Scan CNI Tag: " Then

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
                        "<Parameter>" & ModeType & "</Parameter>" &
                        "<Parameter>" & "T" & "</Parameter>" &
                        "<Parameter>" & Trim(sBarcode) & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter>" & IIf(chkCancelTag.Checked = True, 1, 0) & "</Parameter>" &
                        "<Parameter>" & lblLoc.Text & "</Parameter>" &
                        "<Parameter>" & lblCustNum.Text & "</Parameter>" &
                        "<Parameter>" & lblPickListNo.Text & "</Parameter>" &
                        "<Parameter>" & lblScanCustItem.Text & "</Parameter>" &
                        "<Parameter>" & lblScanCustPO.Text & "</Parameter>" &
                        "<Parameter>" & lblScanQty.Text & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & PWhse.ToString & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "</Parameters>"

            End If

            oWS = New CNIService.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_EX_PickListLines", "PPCC_EX_ConfirmOrderPickListSp", Parms)

            Dim doc As XmlDocument = New XmlDocument()
            doc.LoadXml(Parms)

            Dim i As Integer = 1

            For Each node As XmlNode In doc.DocumentElement

                If i = 15 Then
                    Stat = node.InnerText

                ElseIf i = 16 Then
                    MsgType = node.InnerText

                ElseIf i = 17 Then
                    MsgErr = node.InnerText

                ElseIf i = 18 Then
                    TotalLine = node.InnerText

                ElseIf i = 19 Then
                    TotalConfirm = node.InnerText

                End If

                i += 1

            Next

            If Stat = "TRUE" Then

                If lblbarcode.Text = "Scan Order Pick List: " Then

                    divScanDetail.Visible = True
                    divPickListDetail.Visible = True

                    BindGridview()

                    HiddenField1.Value = GetCheckCustDoc(lblCustNum.Text)
                    'HiddenField2.Value = GetNotScanPickList(lblCustNum.Text)

                    lblbarcode.Text = "Scan Location: "
                    Session("Scan") = "Scan Location: "

                ElseIf lblbarcode.Text = "Scan Location: " Then

                    lblLoc.Text = sBarcode

                    If HiddenField1.Value = "1" Then
                        lblbarcode.Text = "Scan Customer Tag: "
                        Session("Scan") = "Scan Customer Tag: "
                    Else
                        lblbarcode.Text = "Scan CNI Tag: "
                        Session("Scan") = "Scan CNI Tag: "
                    End If

                ElseIf lblbarcode.Text = "Scan Customer Tag: " Then

                    ds = New DataSet
                    ds = GetCheckInformation(lblCustNum.Text)

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
                            sBarcode = Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(sBarcode, "  ", ""), "-", ""), " ", ""), "/", ""), ",", ""), "|", ""), "&", ""), ";", ""), "[)>", "")

                            If CheckCustItem = "1" Then
                                lblScanCustItem.Text = Mid(sBarcode, StartCustItem, LenghtCustItem)
                                Session("lblScanCustItem") = lblScanCustItem.Text
                            End If

                            If CheckCustPO = "1" Then
                                lblScanCustPO.Text = Mid(sBarcode, StartCustPO, LenghtCustPO)
                                Session("lblScanCustPO") = lblScanCustPO.Text
                            End If

                            If CheckQty = "1" Then
                                lblScanQty.Text = CInt(Mid(sBarcode, StartQty, LenghtQty))
                                Session("lblScanQty") = lblScanQty.Text
                            End If

                        Else

                            SepCustItemCol = SepCustItemCol - 1
                            SepCustPoCol = SepCustPoCol - 1
                            SepQtyCol = SepQtyCol - 1

                            sBarcode = Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(sBarcode, "  ", ""), "-", ""), " ", ""), "/", ""), ",", ""), "&", ""), ";", ""), "[)>", "")

                            arrBarcode = sBarcode.Split(New Char() {"|"c})

                            If arrBarcode.Length > 0 Then
                                CustItem = arrBarcode(SepCustItemCol)
                                CustPo = arrBarcode(SepCustPoCol)
                                Qty = arrBarcode(SepQtyCol)
                            End If

                            If CheckCustItem = "1" Then
                                lblScanCustItem.Text = CustItem
                                Session("lblScanCustItem") = lblScanCustItem.Text
                            End If

                            If CheckCustPO = "1" Then
                                lblScanCustPO.Text = CustPo
                                Session("lblScanCustPO") = lblScanCustPO.Text
                            End If

                            If CheckQty = "1" Then
                                lblScanQty.Text = CInt(Qty)
                                Session("lblScanQty") = lblScanQty.Text
                            End If

                        End If

                    End If

                    lblbarcode.Text = "Scan CNI Tag: "
                    Session("Scan") = "Scan CNI Tag: "

                ElseIf lblbarcode.Text = "Scan CNI Tag: " Then

                    If sBarcode <> "OK" Then

                        arrBarcode = sBarcode.Split(New Char() {"|"c})

                        If arrBarcode.Length > 0 Then

                            If Left(arrBarcode(4), 2) = "TD" Then
                                sTagID = arrBarcode(4)
                            Else
                                sTagID = arrBarcode(5)
                            End If

                        End If

                        lblCustItemTag.Text = GetCustItemTag(sTagID)

                        Display(sTagID, Stat, "")

                        BindGridview()

                        If HiddenField1.Value = "1" Then
                            lblbarcode.Text = "Scan Customer Tag: "
                            Session("Scan") = "Scan Customer Tag: "
                        Else
                            lblbarcode.Text = "Scan CNI Tag: "
                            Session("Scan") = "Scan CNI Tag: "
                        End If

                        If chkCancelTag.Checked = True Then
                            chkCancelTag.Checked = False
                        End If

                    End If



                    'If sBarcode <> "OK" Then

                    '    arrBarcode = sBarcode.Split(New Char() {"|"c})

                    '    If arrBarcode.Length > 0 Then

                    '        If Left(arrBarcode(4), 2) = "TD" Then
                    '            sTagID = arrBarcode(4)
                    '        Else
                    '            sTagID = arrBarcode(5)
                    '        End If

                    '    End If

                    '    lblCustItemTag.Text = GetCustItemTag(sTagID)

                    '    Display(sTagID, Stat, "")

                    '    BindGridview()

                    'ElseIf sBarcode = "OK" Then

                    '    If HiddenField1.Value = "1" Then
                    '        lblbarcode.Text = "Scan Customer Tag: "
                    '        Session("Scan") = "Scan Customer Tag: "
                    '    Else
                    '        lblbarcode.Text = "Scan CNI Tag: "
                    '        Session("Scan") = "Scan CNI Tag: "
                    '    End If

                    'End If

                End If


            End If

            If Stat = "FALSE" Then


                If lblbarcode.Text = "Scan CNI Tag: " Then

                    arrBarcode = sBarcode.Split(New Char() {"|"c})

                    If arrBarcode.Length > 0 Then
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


            End If

        End If

        txtbarcode.Text = String.Empty

        lblTotalPickLine.Text = FormatNumber(TotalLine, LenPointQty)
        lblTotalConfirm.Text = FormatNumber(TotalConfirm, LenPointQty)

        Session("lblTotalPickLine") = lblTotalPickLine.Text
        Session("lblTotalConfirm") = lblTotalConfirm.Text

    End Sub

    Protected Sub btnreset_Click(sender As Object, e As EventArgs) Handles btnreset.Click

        Dim Stat, MsgErr, MsgType, ModeType As String
        Stat = "FALSE"
        MsgErr = ""
        MsgType = ""

        If RadioConfirm.Checked = True Then
            ModeType = "C"
        ElseIf RadioChange.Checked = True Then
            ModeType = "H"
        ElseIf RadioReject.Checked = True Then
            ModeType = "R"
        Else
            ModeType = "C"
        End If

        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                "<Parameter>" & ModeType & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                "<Parameter>" & IIf(chkCancelTag.Checked = True, 1, 0) & "</Parameter>" &
                "<Parameter>" & lblLoc.Text & "</Parameter>" &
                "<Parameter>" & lblCustNum.Text & "</Parameter>" &
                "<Parameter>" & lblPickListNo.Text & "</Parameter>" &
                "<Parameter>" & lblScanCustItem.Text & "</Parameter>" &
                "<Parameter>" & lblScanCustPO.Text & "</Parameter>" &
                "<Parameter>" & lblScanQty.Text & "</Parameter>" &
                "<Parameter>" & "R" & "</Parameter>" &
                "<Parameter>" & PWhse.ToString & "</Parameter>" &
                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_EX_PickListLines", "PPCC_EX_ConfirmOrderPickListSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)

        Dim i As Integer = 1

        For Each node As XmlNode In doc.DocumentElement

            If i = 15 Then
                Stat = node.InnerText

            ElseIf i = 16 Then
                MsgType = node.InnerText

            ElseIf i = 17 Then
                MsgErr = node.InnerText

            End If


            i += 1

        Next

        If Stat = "TRUE" Then

            Session("PSession") = NewSessionID()

            Response.Redirect("ConfirmOrderPickList.aspx")

        Else

            MsgErr = MsgErr.Replace("'", "\'")
            MsgErr = MsgErr.Replace(vbLf, "<br />")

            MsgType = "Error [" & MsgType & "]"

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)

        End If

    End Sub

    Protected Sub btnconfirm_Click(sender As Object, e As EventArgs) Handles btnconfirm.Click

        Dim Stat, MsgErr, MsgType, ModeType As String
        Stat = "FALSE"
        MsgErr = ""
        MsgType = ""

        If RadioConfirm.Checked = True Then
            ModeType = "C"
        ElseIf RadioChange.Checked = True Then
            ModeType = "H"
        ElseIf RadioReject.Checked = True Then
            ModeType = "R"
        Else
            ModeType = "C"
        End If

        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                "<Parameter>" & ModeType & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                "<Parameter>" & IIf(chkCancelTag.Checked = True, 1, 0) & "</Parameter>" &
                "<Parameter>" & lblLoc.Text & "</Parameter>" &
                "<Parameter>" & lblCustNum.Text & "</Parameter>" &
                "<Parameter>" & lblPickListNo.Text & "</Parameter>" &
                "<Parameter>" & lblScanCustItem.Text & "</Parameter>" &
                "<Parameter>" & lblScanCustPO.Text & "</Parameter>" &
                "<Parameter>" & lblScanQty.Text & "</Parameter>" &
                "<Parameter>" & "P" & "</Parameter>" &
                "<Parameter>" & PWhse.ToString & "</Parameter>" &
                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_EX_PickListLines", "PPCC_EX_ConfirmOrderPickListSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)

        Dim i As Integer = 1

        For Each node As XmlNode In doc.DocumentElement

            If i = 15 Then
                Stat = node.InnerText

            ElseIf i = 16 Then
                MsgType = node.InnerText

            ElseIf i = 17 Then
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

    Protected Sub PanelList_ItemDataBound(ByVal sender As Object, ByVal e As ListViewItemEventArgs) Handles PanelList.ItemDataBound

        Dim QtyPick As Decimal = 0
        Dim QtyOrdered As Decimal = 0
        Dim QtySum As Decimal = 0

        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim lblQtyOrdered As Label = CType(e.Item.FindControl("lblQtyOrdered"), Label)
            Dim lblQtyPick As Label = CType(e.Item.FindControl("lblQtyPick"), Label)
            Dim lblconfirm As Label = CType(e.Item.FindControl("lblconfirm"), Label)
            Dim lblSumQty As Label = CType(e.Item.FindControl("lblSumQty"), Label)

            Decimal.TryParse(lblQtyOrdered.Text, QtyOrdered)
            Decimal.TryParse(lblQtyPick.Text, QtyPick)
            Decimal.TryParse(lblSumQty.Text, QtySum)

            lblQtyOrdered.Text = FormatNumber(QtyOrdered, LenPointQty)
            lblQtyPick.Text = FormatNumber(QtyPick, LenPointQty)
            lblSumQty.Text = FormatNumber(QtySum, LenPointQty)


            If QtyOrdered = QtyPick And QtyPick <> 0 Then
                lblconfirm.Visible = True
            Else
                lblconfirm.Visible = False
            End If


        End If

    End Sub

    Protected Sub PanelList_ItemCommand(ByVal sender As Object, ByVal e As ListViewCommandEventArgs) Handles PanelList.ItemCommand

        Dim PostURL As String = ""
        Dim ModeType As String = ""

        If RadioConfirm.Checked = True Then
            ModeType = "C"
        ElseIf RadioChange.Checked = True Then
            ModeType = "H"
        ElseIf RadioReject.Checked = True Then
            ModeType = "R"
        Else
            ModeType = "C"
        End If

        PostURL = "?SessionID=" & Session("PSession").ToString & "&CoNum=" & CType(e.Item.FindControl("lblconum"), Label).Text & "&CoLine=" & CType(e.Item.FindControl("lblcoline"), Label).Text & ""
        PostURL = PostURL & "&CoRelease=" & CType(e.Item.FindControl("lblcorelease"), Label).Text & "&OrderPickList=" & lblPickListNo.Text & "&Loc=" & lblLoc.Text & "&ModeType=" & ModeType & ""

        Response.Redirect("ConfirmOrderPickListDetail.aspx" & PostURL)
    End Sub

    Sub Clear()

        txtbarcode.Text = String.Empty
        lblbarcode.Text = "Scan Order Pick List: "
        lblCustNum.Text = String.Empty
        lblCustDesc.Text = String.Empty
        lblPickListNo.Text = String.Empty
        lblTotalPickLine.Text = String.Empty
        lblTotalConfirm.Text = String.Empty
        divScanDetail.Visible = False
        divPickListDetail.Visible = False

        PanelList.DataSource = Nothing
        PanelList.DataBind()

        RadioConfirm.Checked = True
        RadioChange.Checked = False
        RadioReject.Checked = False
        chkCancelTag.Checked = False

        lblCustItemTag.Text = String.Empty
        lblScanCustItem.Text = String.Empty
        lblScanCustPO.Text = String.Empty
        lblScanQty.Text = String.Empty

        Session("Scan") = "Scan Order Pick List: "
        Session.Remove("lblScanCustItem")
        Session.Remove("lblScanCustPO")
        Session.Remove("lblScanQty")
        Session.Remove("lblTotalPickLine")
        Session.Remove("lblTotalConfirm")

        lblmsg.Text = String.Empty
        HidItemModel.Value = String.Empty
        HidCustItemModel.Value = String.Empty
        StatModelCorrect.Visible = False
        StatModelInCorrect.Visible = False

        HiddenField1.Value = ""

    End Sub

#Region "Bind Data To Gridview"

    Sub BindGridview()

        Dim Filter As String
        Dim Propertie As String

        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet
        Filter = "SessionID = '" & Session("PSession").ToString & "'"
        Propertie = "DerCO, CoNum, CoLine, CoRelease, Item, Description, CustNum, Name, CustItem, QtyOrder, QtyPick, PickListNum, PickListLine, QtySum"

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_EX_PickListLines", Propertie, Filter, "PickListNum, PickListLine", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then

            lblCustNum.Text = ds.Tables(0).Rows(0)("CustNum").ToString
            lblCustDesc.Text = ds.Tables(0).Rows(0)("Name").ToString
            lblPickListNo.Text = ds.Tables(0).Rows(0)("PickListNum").ToString

            PanelList.DataSource = ds.Tables(0)
            PanelList.DataBind()

        End If


    End Sub

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
            '        Dim imageUrl1 As String = "data:image/jpg;base64," & Convert.ToBase64String(CType(ds.Tables(0).Rows(0)("Picture"), Byte()))
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
            '    Dim imageUrl2 As String = "data:image/jpg;base64," & Convert.ToBase64String(CType(ds.Tables(0).Rows(0)("Picture"), Byte()))
            '    Image2.ImageUrl = imageUrl2
            'Else
            '    Image2.ImageUrl = "asset/image/No_picture_available.png"
            'End If

        End If


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

#End Region

    Function GetCheckInformation(CustNum As String) As DataSet

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLCustomers", "cusUf_Customer_TagType, cusUf_Customer_FixCustItem, cusUf_Customer_FixCustPO, cusUf_Customer_FixQty, cusUf_Customer_FixStartCustItem, cusUf_Customer_FixLenghtCustItem, cusUf_Customer_FixStartCustPO, cusUf_Customer_FixLenghtCustPO, cusUf_Customer_FixStartQty, cusUf_Customer_FixLenghtQty, cusUf_Customer_SepCustItem, cusUf_Customer_SepCustPO, cusUf_Customer_SepQty, cusUf_Customer_SepCustItemCol, cusUf_Customer_SepCustPoCol, cusUf_Customer_SepQtyCol", "CustNum = '" & CustNum & "'", "", "", 0)

        Return ds

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

    'Function GetNotScanPickList(CustNum As String) As String

    '    GetNotScanPickList = "0"

    '    oWS = New CNIService.DOWebServiceSoapClient

    '    ds = New DataSet

    '    ds = oWS.LoadDataSet(Session("Token").ToString, "SLCustomers", "cusUf_Customer_NotScanPickList", "CustNum = '" & CustNum & "'", "", "", 0)

    '    If ds.Tables(0).Rows.Count > 0 Then
    '        GetNotScanPickList = ds.Tables(0).Rows(0)("cusUf_Customer_NotScanPickList").ToString
    '    End If

    '    Return GetNotScanPickList

    'End Function

    Function GetCustItemTag(TagID As String) As String

        GetCustItemTag = "0"

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Tags", "cust_item", "TagID = '" & TagID & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetCustItemTag = ds.Tables(0).Rows(0)("cust_item").ToString
        End If

        Return GetCustItemTag

    End Function


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

End Class