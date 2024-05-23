Imports System.Data
Imports System.Xml
Imports System.Collections
Imports System.Reflection
Imports Microsoft.VisualBasic.DateInterval
Imports System.Web.UI.WebControls.Expressions
Imports System.IO
Imports System.Linq
Imports System.Data.SqlClient

Public Class PrintProductionDeliveryTag
    Inherits System.Web.UI.Page

    Dim oWS As CNIService.DOWebServiceSoapClient
    Dim ds As DataSet
    Dim Filter As String
    Dim res As Object
    Dim SGUID As String
    Dim Parms As String
    Dim LenPointQty As Integer = 0
    Dim LenPointQtyFormat As Integer = 0
    Dim DateNow As String

    Private Shared ParmSite As String
    Private Shared JobStockTran As String
    Private Shared Whse As String

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

            GetJob()
            GetPO()
            GetTO()
            GetLocFraction()

            Parms = ""

            Parms = "<Parameters><Parameter>" & Session("UserName").ToString & "</Parameter>" &
                    "</Parameters>"

            oWS = New CNIService.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_TmpProductionTags", "PPCC_DeleteTmpProductionTag", Parms)

            'Link From Unposted
            If Request.QueryString("SessionID") <> "" Or Not String.IsNullOrEmpty(Request.QueryString("SessionID")) Then

                Dim Job, Suffix As String

                Job = ""
                Suffix = ""

                Job = Request.QueryString("Job").ToString

                If Len(Request.QueryString("Suffix").ToString) = 1 Then
                    Suffix = "000" & Request.QueryString("Suffix").ToString
                ElseIf Len(Request.QueryString("Suffix").ToString) = 2 Then
                    Suffix = "00" & Request.QueryString("Suffix").ToString
                ElseIf Len(Request.QueryString("Suffix").ToString) = 3 Then
                    Suffix = "0" & Request.QueryString("Suffix").ToString
                ElseIf Len(Request.QueryString("Suffix").ToString) = 4 Then
                    Suffix = Request.QueryString("Suffix").ToString
                End If

                ddlStartJob.SelectedIndex = ddlStartJob.Items.IndexOf(ddlStartJob.Items.FindByValue(Job))
                ddlEndJob.SelectedIndex = ddlEndJob.Items.IndexOf(ddlEndJob.Items.FindByValue(Job))
                txtStartSuffix.Text = Suffix
                txtEndSuffix.Text = Suffix

            End If

        End If

        If RadioFuncionNew.Checked = True Or RadioFuncionRePrint.Checked = True Then
            lblbarcode.Text = "Barcode Job: "
        ElseIf RadioFuncionSplitMerge.Checked = True Then
            lblbarcode.Text = "Barcode Tag ID: "
        End If

        txtWhse.Text = PWhse

        txtbarcode.Focus()

        txtbarcode.Attributes.Add("onchange", "javascript:scanbarcode();")

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim sBarcode, sJob, sSuffix, sOper, sTagID As String

        sBarcode = txtbarcode.Text
        sJob = ""
        sSuffix = ""
        sOper = ""
        sTagID = ""
        txtbarcode.Text = String.Empty

        Dim arrBarcode As String()
        arrBarcode = sBarcode.Split(New Char() {"|"c})

        If lblbarcode.Text = "Barcode Job: " Then

            If sBarcode.Contains("|") = True Then

                If arrBarcode.Length > 0 Then
                    sJob = arrBarcode(0)
                    sSuffix = arrBarcode(1)
                    sOper = arrBarcode(2)
                End If

                If sJob <> "" Then
                    ddlStartJob.SelectedIndex = ddlStartJob.Items.IndexOf(ddlStartJob.Items.FindByValue(sJob))
                    ddlEndJob.SelectedIndex = ddlEndJob.Items.IndexOf(ddlEndJob.Items.FindByValue(sJob))
                End If

                If sSuffix <> "" Then

                    If Len(sSuffix) = 1 Then
                        txtStartSuffix.Text = "000" & sSuffix
                        txtEndSuffix.Text = "000" & sSuffix
                    ElseIf Len(sSuffix) = 2 Then
                        txtStartSuffix.Text = "00" & sSuffix
                        txtEndSuffix.Text = "00" & sSuffix
                    ElseIf Len(sSuffix) = 3 Then
                        txtStartSuffix.Text = "0" & sSuffix
                        txtEndSuffix.Text = "0" & sSuffix
                    ElseIf Len(sSuffix) = 4 Then
                        txtStartSuffix.Text = sSuffix
                        txtEndSuffix.Text = sSuffix
                    End If

                End If

            End If


        ElseIf lblbarcode.Text = "Barcode Tag ID: " Then

            If sBarcode.Contains("|") = True Then

                Dim strPO As String = ""

                If arrBarcode.Length > 0 Then

                    strPO = arrBarcode(0)

                End If

                If Len(strPO) <> 10 Then
                    Dim lenBarcode As Integer = Len(sBarcode)
                    sBarcode = Right(Trim(sBarcode), lenBarcode - 1)
                End If

                If Left(arrBarcode(4), 2) = "TD" Then
                    sTagID = arrBarcode(4)
                Else
                    sTagID = arrBarcode(5)
                End If

                If RadioSplit.Checked = True Then
                    txtSplitTagID.Text = sTagID
                ElseIf RadioMerge.Checked = True Then
                    txtMergeTagID.Text = sTagID
                End If

            End If

        End If

    End Sub

    Protected Sub ddlStartPO_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlStartPO.SelectedIndexChanged

        If ddlStartPO.SelectedItem.Value <> "" Then
            GetStartPOLine(ddlStartPO.SelectedItem.Value)
        End If

    End Sub

    Protected Sub ddlEndPO_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlEndPO.SelectedIndexChanged

        If ddlEndPO.SelectedItem.Value <> "" Then
            GetEndPOLine(ddlEndPO.SelectedItem.Value)
        End If

    End Sub

    Protected Sub ddlStartPOLine_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlStartPOLine.SelectedIndexChanged

        If ddlStartPOLine.SelectedItem.Value <> "" Then
            GetStartPORelease(ddlStartPO.SelectedItem.Value, ddlStartPOLine.SelectedItem.Value)
        End If

    End Sub

    Protected Sub ddlEndPOLine_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlEndPOLine.SelectedIndexChanged

        If ddlEndPOLine.SelectedItem.Value <> "" Then
            GetEndPORelease(ddlEndPO.SelectedItem.Value, ddlEndPOLine.SelectedItem.Value)
        End If

    End Sub

    Protected Sub ddlStartTO_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlStartTO.SelectedIndexChanged

        If ddlStartTO.SelectedItem.Value <> "" Then
            GetStartTOLine(ddlStartTO.SelectedItem.Value)
        End If

    End Sub

    Protected Sub ddlEndTO_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlEndTO.SelectedIndexChanged

        If ddlEndTO.SelectedItem.Value <> "" Then
            GetEndTOLine(ddlEndTO.SelectedItem.Value)
        End If

    End Sub

    Protected Sub ddlStartJob_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlStartJob.SelectedIndexChanged

        If ddlStartJob.SelectedItem.Value <> "" Then
            txtStartSuffix.Text = Right(ddlStartJob.SelectedItem.Text, 4)

            ddlEndJob.SelectedIndex = ddlEndJob.Items.IndexOf(ddlEndJob.Items.FindByValue(ddlStartJob.SelectedItem.Value))
        End If

    End Sub

    Protected Sub ddlEndJob_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlEndJob.SelectedIndexChanged

        If ddlEndJob.SelectedItem.Value <> "" Then
            txtEndSuffix.Text = Right(ddlEndJob.SelectedItem.Text, 4)
        End If

    End Sub

    Protected Sub btnSelectAll_Click(sender As Object, e As EventArgs) Handles btnSelectAll.Click

        For Each row As GridViewRow In GridView1.Rows
            Dim chkSelect As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)
            chkSelect.Checked = True
        Next

    End Sub

    Protected Sub btnDeSelectAll_Click(sender As Object, e As EventArgs) Handles btnDeSelectAll.Click

        For Each row As GridViewRow In GridView1.Rows
            Dim chkSelect As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)
            chkSelect.Checked = False
        Next

    End Sub

    Protected Sub btnPreview_Click(sender As Object, e As EventArgs) Handles btnPreview.Click

        Dim sFunction, StartDate, EndDate, sSplitMerge, MsgErr As String

        sFunction = ""
        sSplitMerge = ""
        MsgErr = ""

        If RadioFuncionNew.Checked = True Then
            sFunction = "N"
        ElseIf RadioFuncionRePrint.Checked = True Then
            sFunction = "R"
        ElseIf RadioFuncionSplitMerge.Checked = True Then
            sFunction = "S"
        End If

        If RadioSplit.Checked = True Then
            sSplitMerge = "S"
        ElseIf RadioMerge.Checked = True Then
            sSplitMerge = "M"
        End If

        If String.IsNullOrEmpty(txtStartDate.Text) Then
            StartDate = Nothing
        Else
            StartDate = DateTime.Parse(txtStartDate.Text).ToString("yyyy-MM-dd")
        End If

        If String.IsNullOrEmpty(txtEndDate.Text) Then
            EndDate = Nothing
        Else
            EndDate = DateTime.Parse(txtEndDate.Text).ToString("yyyy-MM-dd")
        End If

        If ddlStartPOLine.Items.Count = 0 Then
            ddlStartPOLine.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlEndPOLine.Items.Count = 0 Then
            ddlEndPOLine.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlStartPORelease.Items.Count = 0 Then
            ddlStartPORelease.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlEndPORelease.Items.Count = 0 Then
            ddlEndPORelease.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlStartTOLine.Items.Count = 0 Then
            ddlStartTOLine.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlEndTOLine.Items.Count = 0 Then
            ddlEndTOLine.Items.Insert(0, New ListItem("", ""))
        End If

        If sFunction <> "S" Then

            If (ddlStartJob.SelectedItem.Value = "" Or ddlEndJob.SelectedItem.Value = "") And chkTradingPart.Checked = False Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','กรุณาเลือก Job ก่อน Preview Tag', 'error');", True)
                Exit Sub
            End If

            If chkTradingPart.Checked = False Then

                    Parms = ""

                    Parms = "<Parameters><Parameter>" & ddlStartJob.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & ddlEndJob.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & txtStartSuffix.Text & "</Parameter>" &
                            "<Parameter>" & txtEndSuffix.Text & "</Parameter>" &
                            "<Parameter>" & StartDate & "</Parameter>" &
                            "<Parameter>" & EndDate & "</Parameter>" &
                            "<Parameter>" & txtStartTagID.Text & "</Parameter>" &
                            "<Parameter>" & txtEndTagID.Text & "</Parameter>" &
                            "<Parameter>" & sFunction & "</Parameter>" &
                            "<Parameter>" & IIf(chkDelivery.Checked = True, "1", "0") & "</Parameter>" &
                            "<Parameter>" & IIf(chkProductionTag.Checked = True, "1", "0") & "</Parameter>" &
                            "<Parameter>" & IIf(chkTradingPart.Checked = True, "1", "0") & "</Parameter>" &
                            "<Parameter>" & ddlStartPO.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & ddlEndPO.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & ddlStartPOLine.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & ddlEndPOLine.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & ddlStartPORelease.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & ddlEndPORelease.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & ddlStartTO.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & ddlEndTO.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & ddlStartTOLine.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & ddlEndTOLine.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & Session("PSession").ToString & "</Parameter>" &
                            "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                            "<Parameter>" & PSite.ToString & "</Parameter>" &
                            "<Parameter>" & PWhse.ToString & "</Parameter>" &
                            "<Parameter>" & IIf(chkBoxTag.Checked = True, "1", "0") & IIf(chkBagTag.Checked = True, "1", "0") & "</Parameter>" &
                            "</Parameters>"

                    oWS = New CNIService.DOWebServiceSoapClient
                    oWS.CallMethod(Session("Token").ToString, "PPCC_TmpProductionTags", "PPCC_PreviewProductionTagSp", Parms)

                    BindGridview()

                ElseIf chkTradingPart.Checked = True Then

                    Parms = ""

                    Parms = "<Parameters><Parameter>" & ddlStartJob.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & ddlEndJob.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & txtStartSuffix.Text & "</Parameter>" &
                            "<Parameter>" & txtEndSuffix.Text & "</Parameter>" &
                            "<Parameter>" & StartDate & "</Parameter>" &
                            "<Parameter>" & EndDate & "</Parameter>" &
                            "<Parameter>" & txtStartTagID.Text & "</Parameter>" &
                            "<Parameter>" & txtEndTagID.Text & "</Parameter>" &
                            "<Parameter>" & sFunction & "</Parameter>" &
                            "<Parameter>" & IIf(chkDelivery.Checked = True, "1", "0") & "</Parameter>" &
                            "<Parameter>" & IIf(chkProductionTag.Checked = True, "1", "0") & "</Parameter>" &
                            "<Parameter>" & IIf(chkTradingPart.Checked = True, "1", "0") & "</Parameter>" &
                            "<Parameter>" & ddlStartPO.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & ddlEndPO.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & ddlStartPOLine.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & ddlEndPOLine.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & ddlStartPORelease.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & ddlEndPORelease.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & ddlStartTO.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & ddlEndTO.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & ddlStartTOLine.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & ddlEndTOLine.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & Session("PSession").ToString & "</Parameter>" &
                            "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                            "<Parameter>" & PSite.ToString & "</Parameter>" &
                            "<Parameter>" & PWhse.ToString & "</Parameter>" &
                            "<Parameter>" & IIf(chkBoxTag.Checked = True, "1", "0") & IIf(chkBagTag.Checked = True, "1", "0") & "</Parameter>" &
                            "</Parameters>"

                    oWS = New CNIService.DOWebServiceSoapClient
                    oWS.CallMethod(Session("Token").ToString, "PPCC_TmpProductionTags", "PPCC_PreviewProductionTag_PurchaseOrderSp", Parms)

                    BindGridview()

                End If


        Else

            If chkTradingPart.Checked = False Then

                Parms = ""

                Parms = "<Parameters><Parameter>" & txtSplitQty.Text & "</Parameter>" &
                        "<Parameter>" & txtSplitTagID.Text & "</Parameter>" &
                        "<Parameter>" & txtMergeTagID.Text & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & sSplitMerge & "</Parameter>" &
                        "<Parameter>" & PSite.ToString & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "</Parameters>"

                oWS = New CNIService.DOWebServiceSoapClient
                oWS.CallMethod(Session("Token").ToString, "PPCC_TmpProductionTags", "PPCC_ProductionTagSplitMergePreviewSp", Parms)

                Dim doc As XmlDocument = New XmlDocument()
                doc.LoadXml(Parms)

                Dim i As Integer = 1

                For Each node As XmlNode In doc.DocumentElement

                    If i = 8 Then
                        MsgErr = node.InnerText

                    End If

                    i += 1

                Next

                If MsgErr = "" Or String.IsNullOrEmpty(MsgErr) Then
                    BindGridview()
                    btnSelectAll_Click(sender, e)
                Else
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('PPCC','" & MsgErr & "', 'error');", True)
                End If

            ElseIf chkTradingPart.Checked = True Then

                Parms = ""

                Parms = "<Parameters><Parameter>" & txtSplitQty.Text & "</Parameter>" &
                        "<Parameter>" & txtSplitTagID.Text & "</Parameter>" &
                        "<Parameter>" & txtMergeTagID.Text & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & sSplitMerge & "</Parameter>" &
                        "<Parameter>" & PSite.ToString & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "</Parameters>"

                oWS = New CNIService.DOWebServiceSoapClient
                oWS.CallMethod(Session("Token").ToString, "PPCC_TmpProductionTags", "PPCC_ProductionTagSplitMergePreview_TradingSp", Parms)

                Dim doc As XmlDocument = New XmlDocument()
                doc.LoadXml(Parms)

                Dim i As Integer = 1

                For Each node As XmlNode In doc.DocumentElement

                    If i = 8 Then
                        MsgErr = node.InnerText

                    End If

                    i += 1

                Next

                If MsgErr = "" Or String.IsNullOrEmpty(MsgErr) Then
                    BindGridview()
                    btnSelectAll_Click(sender, e)
                Else
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('PPCC','" & MsgErr & "', 'error');", True)
                End If

            End If

        End If

    End Sub

    Protected Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click

        Dim sError, MsgErr, sSplitMerge, CheckError, RowPointer, sFunction, BGTaskName, TaskParms, Script As String
        sError = ""
        MsgErr = ""
        sSplitMerge = ""
        CheckError = "0"
        sFunction = ""
        BGTaskName = ""
        TaskParms = ""
        Script = ""

        If RadioSplit.Checked = True Then
            sSplitMerge = "S"
        ElseIf RadioMerge.Checked = True Then
            sSplitMerge = "M"
        End If

        If RadioFuncionNew.Checked = True Then
            sFunction = "N"
        ElseIf RadioFuncionRePrint.Checked = True Then
            sFunction = "R"
        ElseIf RadioFuncionSplitMerge.Checked = True Then
            sFunction = "S"
        End If

        If GridView1.Rows.Count > 0 Then

            For Each row As GridViewRow In GridView1.Rows

                Dim chkSelect As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)
                RowPointer = IIf(row.Cells(25).Text <> "", row.Cells(25).Text, "")

                Parms = ""

                Parms = "<Parameters><Parameter>" & RowPointer & "</Parameter>" &
                        "<Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter>" & PSite.ToString & "</Parameter>" &
                        "<Parameter>" & IIf(chkSelect.Checked = True, "1", "0") & "</Parameter>" &
                        "</Parameters>"

                oWS = New CNIService.DOWebServiceSoapClient
                oWS.CallMethod(Session("Token").ToString, "PPCC_EX_ListJobReleases", "PPCC_EX_UpdateSelectTmpProductionTagSp", Parms)

            Next

        End If

        Parms = ""

        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                "<Parameter>" & PSite.ToString & "</Parameter>" &
                "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                "<Parameter>" & sFunction & "</Parameter>" &
                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_TmpProductionTags", "PPCC_ProductionTagMergeMoveQtySp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)

        Dim i As Integer = 1

        For Each node As XmlNode In doc.DocumentElement

            If i = 5 Then
                MsgErr = node.InnerText

            End If

            i += 1

        Next

        If MsgErr = "" Or String.IsNullOrEmpty(MsgErr) Then

            TaskParms = ""

            TaskParms = "~LIT~(" & Session("PSession").ToString & "),"
            TaskParms = TaskParms & "~LIT~(" & PSite.ToString & "),"
            TaskParms = TaskParms & "~LIT~(" & Session("UserName").ToString & "),"
            TaskParms = TaskParms & "~LIT~(" & sFunction & ")"

            If chkTradingPart.Checked = False Then

                '"PPCC_DeliveryTag"
                '"PPCC_ProductionTag"

                If chkProductionTag.Checked = True And chkDelivery.Checked = True Then

                    Call BGTask("PPCC_ProductionTag", TaskParms, sFunction, Nothing)

                    Script = "window.open('" & Session("UrlReport").ToString & "', '_blank')"
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "response", Script, True)

                    If Not IsNothing(Session("UrlReport")) Then
                        Session.Remove("UrlReport")
                    End If

                    Call BGTask("PPCC_DeliveryTag", TaskParms, sFunction, Nothing)

                    Script = "window.open('" & Session("UrlReport").ToString & "', '_blank')"
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "response", Script, True)


                ElseIf chkProductionTag.Checked = True And chkDelivery.Checked = False Then

                    Call BGTask("PPCC_ProductionTag", TaskParms, sFunction, Nothing)

                    Script = "window.open('" & Session("UrlReport").ToString & "', '_blank')"
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "response", Script, True)

                ElseIf chkProductionTag.Checked = False And chkDelivery.Checked = True Then

                    Call BGTask("PPCC_DeliveryTag", TaskParms, sFunction, Nothing)

                    Script = "window.open('" & Session("UrlReport").ToString & "', '_blank')"
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "response", Script, True)

                End If

            ElseIf chkTradingPart.Checked = True Then

                '"PPCC_ProductionTag_TradingTD"
                '"PPCC_DeliveryTag_TradingDL"

                If chkProductionTag.Checked = True And chkDelivery.Checked = True Then

                    Call BGTask("PPCC_ProductionTag_TradingTD", TaskParms, sFunction, "SSRS")

                    Script = "window.open('" & Session("UrlReport").ToString & "', '_blank')"
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "response", Script, True)

                    If Not IsNothing(Session("UrlReport")) Then
                        Session.Remove("UrlReport")
                    End If

                    Call BGTask("PPCC_DeliveryTag_TradingDL", TaskParms, sFunction, "SSRS")

                    Script = "window.open('" & Session("UrlReport").ToString & "', '_blank')"
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "response", Script, True)

                ElseIf chkProductionTag.Checked = True And chkDelivery.Checked = False Then

                    Call BGTask("PPCC_ProductionTag_TradingTD", TaskParms, sFunction, "SSRS")

                    Script = "window.open('" & Session("UrlReport").ToString & "', '_blank')"
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "response", Script, True)

                ElseIf chkProductionTag.Checked = False And chkDelivery.Checked = True Then

                    Call BGTask("PPCC_DeliveryTag_TradingDL", TaskParms, sFunction, "SSRS")

                    Script = "window.open('" & Session("UrlReport").ToString & "', '_blank')"
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "response", Script, True)

                End If

            End If

            btnClearTag_Click(sender, e)

            If Not IsNothing(Session("UrlReport")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Success','Report Submitted', 'success');", True)
            Else
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','Report Filed', 'error');", True)
                Exit Sub
            End If

        Else
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('PPCC','" & MsgErr & "', 'error');", True)
            Exit Sub
        End If


    End Sub

    Protected Sub btnClearTag_Click(sender As Object, e As EventArgs) Handles btnClearTag.Click

        Parms = ""

        Parms = "<Parameters><Parameter>" & Session("UserName").ToString & "</Parameter>" &
                    "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_TmpProductionTags", "PPCC_DeleteTmpProductionTag", Parms)

        BindGridview()

        ddlLocFraction.SelectedIndex = ddlLocFraction.Items.IndexOf(ddlStartJob.Items.FindByValue(""))
        txtMergeTagID.Text = String.Empty

    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        Dim RefTagID, MsgErr, sSplitMerge As String
        RefTagID = ""
        MsgErr = ""
        sSplitMerge = ""

        If RadioSplit.Checked = True Then
            sSplitMerge = "S"
        ElseIf RadioMerge.Checked = True Then
            sSplitMerge = "M"
        End If

        If GridView1.Rows.Count > 0 Then

            For Each row As GridViewRow In GridView1.Rows

                RefTagID = IIf(row.Cells(24).Text <> "", row.Cells(24).Text, "")

                If RefTagID = "M" Then
                    MsgErr = "Tags cannot be merged. Because there are already merged tags to print"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('PPCC','" & MsgErr & "', 'error');", True)
                    Exit Sub
                End If

            Next

        End If

        If chkTradingPart.Checked = False Then

            Parms = ""

            Parms = "<Parameters><Parameter>" & txtSplitQty.Text & "</Parameter>" &
                    "<Parameter>" & txtSplitTagID.Text & "</Parameter>" &
                    "<Parameter>" & txtMergeTagID.Text & "</Parameter>" &
                    "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                    "<Parameter>" & Session("PSession").ToString & "</Parameter>" &
                    "<Parameter>" & sSplitMerge & "</Parameter>" &
                    "<Parameter>" & PSite.ToString & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter>" & ddlLocFraction.SelectedItem.Value & "</Parameter>" &
                    "</Parameters>"

            oWS = New CNIService.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_EX_ListJobReleases", "PPCC_EX_ProductionTagSplitMergePreviewSp", Parms)

            Dim doc As XmlDocument = New XmlDocument()
            doc.LoadXml(Parms)

            Dim i As Integer = 1

            For Each node As XmlNode In doc.DocumentElement

                If i = 8 Then
                    MsgErr = node.InnerText

                End If

                i += 1

            Next

            If MsgErr = "" Or String.IsNullOrEmpty(MsgErr) Then
                BindGridview()
                btnSelectAll_Click(sender, e)
            Else
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('PPCC','" & MsgErr & "', 'error');", True)
            End If

            ddlLocFraction.SelectedIndex = ddlLocFraction.Items.IndexOf(ddlStartJob.Items.FindByValue(""))

        ElseIf chkTradingPart.Checked = True Then

            Parms = ""

            Parms = "<Parameters><Parameter>" & txtSplitQty.Text & "</Parameter>" &
                        "<Parameter>" & txtSplitTagID.Text & "</Parameter>" &
                        "<Parameter>" & txtMergeTagID.Text & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & sSplitMerge & "</Parameter>" &
                        "<Parameter>" & PSite.ToString & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & ddlLocFraction.SelectedItem.Value & "</Parameter>" &
                        "</Parameters>"

            oWS = New CNIService.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_EX_ListJobReleases", "PPCC_EX_ProductionTagSplitMergePreview_TradingSp", Parms)

            Dim doc As XmlDocument = New XmlDocument()
            doc.LoadXml(Parms)

            Dim i As Integer = 1

            For Each node As XmlNode In doc.DocumentElement

                If i = 8 Then
                    MsgErr = node.InnerText

                End If

                i += 1

            Next

            If MsgErr = "" Or String.IsNullOrEmpty(MsgErr) Then
                BindGridview()
                btnSelectAll_Click(sender, e)
                txtMergeTagID.Text = String.Empty
                txtMergeTagID.Focus()
            Else
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('PPCC','" & MsgErr & "', 'error');", True)
            End If

            ddlLocFraction.SelectedIndex = ddlLocFraction.Items.IndexOf(ddlStartJob.Items.FindByValue(""))

        End If


    End Sub

    Protected Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click

        Parms = ""

        Parms = "<Parameters><Parameter>" & Session("UserName").ToString & "</Parameter>" &
                "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_TmpProductionTags", "PPCC_DeleteTmpProductionTag", Parms)

        BindGridview()
        txtMergeTagID.Text = String.Empty
        txtMergeTagID.Focus()

    End Sub

    Protected Sub btnMerge_Click(sender As Object, e As EventArgs) Handles btnMerge.Click

        Dim sError, MsgErr, sSplitMerge, CheckError, RowPointer As String
        sError = ""
        MsgErr = ""
        sSplitMerge = ""
        CheckError = "0"

        If RadioSplit.Checked = True Then
            sSplitMerge = "S"
        ElseIf RadioMerge.Checked = True Then
            sSplitMerge = "M"
        End If

        If GridView1.Rows.Count > 0 Then

            For Each row As GridViewRow In GridView1.Rows

                Dim chkSelect As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)

                If chkSelect.Checked = False Then
                    MsgErr = "Tags not merges and will be blank Tags"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('PPCC','" & MsgErr & "', 'error');", True)
                    Exit Sub
                End If

            Next

        End If

        If GridView1.Rows.Count > 0 Then

            For Each row As GridViewRow In GridView1.Rows

                Dim chkSelect As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)
                RowPointer = IIf(row.Cells(25).Text <> "", row.Cells(25).Text, "")

                Parms = ""

                Parms = "<Parameters><Parameter>" & RowPointer & "</Parameter>" &
                        "<Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter>" & PSite.ToString & "</Parameter>" &
                        "<Parameter>" & IIf(chkSelect.Checked = True, "1", "0") & "</Parameter>" &
                        "</Parameters>"

                oWS = New CNIService.DOWebServiceSoapClient
                oWS.CallMethod(Session("Token").ToString, "PPCC_EX_ListJobReleases", "PPCC_EX_UpdateSelectTmpProductionTagSp", Parms)

            Next

        End If

        If chkTradingPart.Checked = False Then

            Parms = ""

            Parms = "<Parameters><Parameter>" & Session("UserName").ToString & "</Parameter>" &
                    "<Parameter>" & Session("PSession").ToString & "</Parameter>" &
                    "<Parameter>" & PSite.ToString & "</Parameter>" &
                    "</Parameters>"

            oWS = New CNIService.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_TmpProductionTags", "PPCC_ProductionTagMergeProcessSp", Parms)

            BindGridview()
            btnSelectAll_Click(sender, e)

        ElseIf chkTradingPart.Checked = True Then

            Parms = ""

            Parms = "<Parameters><Parameter>" & Session("UserName").ToString & "</Parameter>" &
                    "<Parameter>" & Session("PSession").ToString & "</Parameter>" &
                    "<Parameter>" & PSite.ToString & "</Parameter>" &
                    "</Parameters>"

            oWS = New CNIService.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_TmpProductionTags", "PPCC_ProductionTagMergeProcessSp_TradingSp", Parms)

            BindGridview()
            btnSelectAll_Click(sender, e)

        End If

        If GridView1.Rows.Count > 0 Then

            For Each row As GridViewRow In GridView1.Rows

                sError = IIf(Replace(row.Cells(12).Text, "&nbsp;", "") <> "", row.Cells(12).Text, "")

                If sError <> "" Then
                    CheckError = "1"
                    Exit For
                End If

            Next

            If CheckError = "1" Then
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "DisabledPrint();", True)
            Else
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "EnablePrint();", True)
            End If

        End If


    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles GridView1.RowDataBound

        Dim PackWeight As Decimal = 0
        Dim PartWeight As Decimal = 0
        Dim GrossWeight As Decimal = 0
        Dim TagQty As Decimal = 0
        Dim QtyExcess As Decimal = 0

        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim lblPackWeight As Label = DirectCast(e.Row.FindControl("lblPackWeight"), Label)
            Dim lblPartWeight As Label = DirectCast(e.Row.FindControl("lblPartWeight"), Label)
            Dim lblGrossWeight As Label = DirectCast(e.Row.FindControl("lblGrossWeight"), Label)
            Dim lblTagQty As Label = DirectCast(e.Row.FindControl("lblTagQty"), Label)
            Dim lblQtyExcess As Label = DirectCast(e.Row.FindControl("lblQtyExcess"), Label)

            Decimal.TryParse(lblPackWeight.Text, PackWeight)
            Decimal.TryParse(lblPartWeight.Text, PartWeight)
            Decimal.TryParse(lblGrossWeight.Text, GrossWeight)
            Decimal.TryParse(lblTagQty.Text, TagQty)
            Decimal.TryParse(lblQtyExcess.Text, QtyExcess)

            lblPackWeight.Text = FormatNumber(PackWeight, LenPointQty)
            lblPartWeight.Text = FormatNumber(PartWeight, LenPointQty)
            lblGrossWeight.Text = FormatNumber(GrossWeight, LenPointQty)
            lblTagQty.Text = FormatNumber(TagQty, LenPointQty)
            lblQtyExcess.Text = FormatNumber(QtyExcess, LenPointQty)

        End If

    End Sub

    Sub BGTask(ByVal TaskName As String, ByVal TaskParms As String, ByVal sFunction As String, ByVal ReportType As String)

        Parms = "<Parameters><Parameter>" & TaskName & "</Parameter>" &
                "<Parameter>" & TaskParms & "</Parameter>" &
                "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                "<Parameter>" & "Preview" & "</Parameter>" &
                "<Parameter>" & PSite.ToString & "</Parameter>" &
                "<Parameter>" & ReportType & "</Parameter>" &
                "<Parameter ByRef='Y'>" & String.Empty & "</Parameter>" &
                "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_EX_ListJobReleases", "PPCC_EX_InsertActiveBGTaskForPrintBCSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)
        Dim TaskNumber As String = ""
        Dim i As Integer = 1
        Dim CallTaskCount As Integer = 0

        For Each node As XmlNode In doc.DocumentElement
            If i = 7 Then
                If Trim(node.InnerText) <> String.Empty Then
                    TaskNumber = node.InnerText
                    Call TaskManRunning(TaskNumber, "Preview")
                    CallTaskCount += 1
                End If
            End If
            i += 1
        Next


    End Sub

#Region "Bind Data To Gridview"

    Sub BindGridview()


        Dim ListProperty As String

        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet
        Filter = "SessionID = '" & Session("PSession").ToString & "'"
        ListProperty = "Selected, DerTagformat, CustNum, cust_shotname, TagId, TagQty, qr_code, "
        ListProperty = ListProperty & "PackageWeight, PartWeight, DerGrossWeight, Error, Item, ItemDescription, CustItem, UM, Model, "
        ListProperty = ListProperty & "Job, Suffix, Lot, Loc, Whse, TranDate, ref_tag_id, RowPointer, LocExcess, QtyExcess"

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_TmpProductionTags", ListProperty, Filter, "TagId", "", 0)

        'If ds.Tables(0).Rows.Count > 0 Then

        GridView1.DataSource = ds
        GridView1.DataBind()

        If RadioFuncionSplitMerge.Checked = True Then

            If RadioMerge.Checked = True Then
                GridView1.Columns(6).Visible = True
                GridView1.Columns(7).Visible = True
            End If

            If RadioSplit.Checked = True Then
                GridView1.Columns(6).Visible = False
                GridView1.Columns(7).Visible = False
            End If

        Else
            GridView1.Columns(6).Visible = False
            GridView1.Columns(7).Visible = False
        End If

        'End If


    End Sub

#End Region

#Region "Get Data Bind To Dropdownlist"

    Sub GetJob()

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_SLJobs", "Job, DerSuffix", "Type = 'J' AND DerConditionDate = '1'", "Job", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddlStartJob.Items.Add(New ListItem(dRow("Job") & HttpUtility.HtmlDecode("&nbsp;&nbsp;") & dRow("DerSuffix"), dRow("Job")))
            ddlEndJob.Items.Add(New ListItem(dRow("Job") & HttpUtility.HtmlDecode("&nbsp;&nbsp;") & dRow("DerSuffix"), dRow("Job")))

        Next

        ddlStartJob.Items.Insert(0, New ListItem("", ""))
        ddlEndJob.Items.Insert(0, New ListItem("", ""))

    End Sub

    Sub GetPO()

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_SLPos", "PoNum, VendorName", "Stat in ('O','F') AND Whse = 'MAIN' AND DerConditionDate = '1'", "PoNum", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddlStartPO.Items.Add(New ListItem(dRow("PoNum") & HttpUtility.HtmlDecode("&nbsp;&nbsp;") & dRow("VendorName"), dRow("PoNum")))
            ddlEndPO.Items.Add(New ListItem(dRow("PoNum") & HttpUtility.HtmlDecode("&nbsp;&nbsp;") & dRow("VendorName"), dRow("PoNum")))

        Next

        ddlStartPO.Items.Insert(0, New ListItem("", ""))
        ddlEndPO.Items.Insert(0, New ListItem("", ""))

    End Sub

    Sub GetStartPOLine(PONum As String)

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLPoItems", "PoLine", "PoNum = '" & PONum & "'", "PoNum", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddlStartPOLine.Items.Add(New ListItem(dRow("PoLine"), dRow("PoLine")))

        Next

        ddlStartPOLine.Items.Insert(0, New ListItem("", ""))

    End Sub

    Sub GetEndPOLine(PONum As String)

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLPoItems", "PoLine", "PoNum = '" & PONum & "'", "PoNum", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddlEndPOLine.Items.Add(New ListItem(dRow("PoLine"), dRow("PoLine")))

        Next

        ddlEndPOLine.Items.Insert(0, New ListItem("", ""))

    End Sub

    Sub GetStartPORelease(PONum As String, POLine As Integer)

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLPoItems", "PoRelease", "PoNum = '" & PONum & "' AND PoLine = " & POLine & "", "PoRelease", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddlStartPORelease.Items.Add(New ListItem(dRow("PoRelease"), dRow("PoRelease")))

        Next

        ddlStartPORelease.Items.Insert(0, New ListItem("", ""))

    End Sub

    Sub GetEndPORelease(PONum As String, POLine As Integer)

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLPoItems", "PoRelease", "PoNum = '" & PONum & "' AND PoLine = " & POLine & "", "PoRelease", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddlEndPORelease.Items.Add(New ListItem(dRow("PoRelease"), dRow("PoRelease")))

        Next

        ddlEndPORelease.Items.Insert(0, New ListItem("", ""))

    End Sub

    Sub GetTO()

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLTrnacts", "TrnNum, TraStat", "(QtyReceived <= QtyShipped) AND TraStat = 'T' AND ToWhse = 'MAIN'", "TrnNum", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddlStartTO.Items.Add(New ListItem(dRow("TrnNum") & HttpUtility.HtmlDecode("&nbsp;&nbsp;") & dRow("TraStat"), dRow("TrnNum")))
            ddlEndTO.Items.Add(New ListItem(dRow("TrnNum") & HttpUtility.HtmlDecode("&nbsp;&nbsp;") & dRow("TraStat"), dRow("TrnNum")))

        Next

        ddlStartTO.Items.Insert(0, New ListItem("", ""))
        ddlEndTO.Items.Insert(0, New ListItem("", ""))

    End Sub

    Sub GetStartTOLine(TrnNum As String)

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLTrnitems", "TrnLine", "TrnNum = '" & TrnNum & "'", "TrnLine", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddlStartTOLine.Items.Add(New ListItem(dRow("TrnLine"), dRow("TrnLine")))

        Next

        ddlStartTOLine.Items.Insert(0, New ListItem("", ""))

    End Sub

    Sub GetEndTOLine(TrnNum As String)

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLTrnitems", "TrnLine", "TrnNum = '" & TrnNum & "'", "TrnLine", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddlEndTOLine.Items.Add(New ListItem(dRow("TrnLine"), dRow("TrnLine")))

        Next

        ddlEndTOLine.Items.Insert(0, New ListItem("", ""))

    End Sub

    Sub GetLocFraction()

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLLocations", "Loc, Description", "locUf_location_scrap = 1", "Loc", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddlLocFraction.Items.Add(New ListItem(dRow("Loc") & HttpUtility.HtmlDecode("&nbsp;&nbsp;") & dRow("Description"), dRow("Loc")))

        Next

        ddlLocFraction.Items.Insert(0, New ListItem("", ""))

    End Sub

#End Region


#Region "Function"

    Function TaskManRunning(ByVal TaskNumber As String, ProcessType As String) As Boolean
        Dim StartDate As String = ""
        Dim CompletionDate As String = ""
        Dim ds As New Data.DataSet
        Dim i As Integer = 0
        oWS = New CNIService.DOWebServiceSoapClient

        ds = oWS.LoadDataSet(Session("Token").ToString, "BGTaskHistories", "TaskNumber, SubmissionDate, StartDate, CompletionDate", "TaskNumber= " & TaskNumber, "", "", 0)
        StartDate = ds.Tables(0).Rows(0)("StartDate").ToString

        Dim TaskInterval1 As String = System.Configuration.ConfigurationManager.AppSettings("TaskInterval1")
        Dim TaskInterval2 As String = System.Configuration.ConfigurationManager.AppSettings("TaskInterval2")

        If TaskInterval1 Is Nothing Then
            TaskInterval1 = "240" 'Max 2 Minutes
        End If

        If TaskInterval2 Is Nothing Then
            TaskInterval2 = "600" 'Max 5 Minutes
        End If

        While StartDate = "" And i < Convert.ToInt32(TaskInterval1)
            ds = New DataSet
            ds = oWS.LoadDataSet(Session("Token").ToString, "BGTaskHistories", "StartDate", "TaskNumber=" & TaskNumber, "", "", 0)
            StartDate = ds.Tables(0).Rows(0)("StartDate").ToString
            System.Threading.Thread.Sleep(500)
            i += 1
        End While

        If StartDate <> "" Then
            i = 0
            ds = New DataSet
            ds = oWS.LoadDataSet(Session("Token").ToString, "BGTaskHistories", "CompletionDate", "TaskNumber=" & TaskNumber, "", "", 0)

            While CompletionDate = "" And i < Convert.ToInt32(TaskInterval2)
                ds = New DataSet
                ds = oWS.LoadDataSet(Session("Token").ToString, "BGTaskHistories", "CompletionDate", "TaskNumber=" & TaskNumber, "", "", 0)
                CompletionDate = ds.Tables(0).Rows(0)("CompletionDate").ToString
                System.Threading.Thread.Sleep(500)
                i += 1
            End While
        End If


        Dim FileName As String = ""
        Dim OutputPath As String = ""
        If CompletionDate <> "" Then

            ds = New DataSet
            ds = oWS.LoadDataSet(Session("Token").ToString, "BGTaskHistories", "ReportOutputPath", "TaskNumber=" & TaskNumber, "", "", 0)
            OutputPath = ds.Tables(0).Rows(0)("ReportOutputPath").ToString
            FileName = OutputPath.Substring(OutputPath.LastIndexOf("\"c) + 1)

        End If

        If FileName <> "" Then
            Dim url As String = System.Configuration.ConfigurationManager.AppSettings("ReportAddress")

            If ProcessType = "Preview" Then
                Session("UrlReport") = url & Session("UserName").ToString & "/Preview/" & FileName
            Else
                Session("UrlReport") = url & Session("UserName").ToString & "/" & FileName
            End If

        End If

        Return True

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

End Class