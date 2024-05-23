Imports System.Data
Imports System.Xml
Imports System.Collections
Imports System.Reflection
Imports Microsoft.VisualBasic.DateInterval
Imports System.Web.UI.WebControls.Expressions
Imports System.IO
Imports System.Linq
Imports System.Data.SqlClient
Imports Microsoft.SqlServer

Public Class WIPTag
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
    Dim rdoNew As String


    Private Shared JobStockTran As String
    Private Shared Whse As String
    Private Shared PSite As String

    Private Shared Property ParmSite() As String
        Get
            Return PSite
        End Get
        Set(value As String)
            PSite = value
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

            Parms = ""

            Parms = "<Parameters><Parameter>" & Session("UserName").ToString & "</Parameter>" &
                    "</Parameters>"

            oWS = New CNIService.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_TmpWIPTags", "PPCC_DeleteWIPTagSp", Parms)

            '---------Dropdown------------'
            checkfunction1.Checked = True
            vCheckFunciotn.Value = "N"
            RadioFormat2.Checked = True
            vTagType.Value = "I"
            GetJob()
            'GetTagId()
            '---------End Dropdown------------'

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

                deljob1.SelectedIndex = deljob1.Items.IndexOf(deljob1.Items.FindByValue(Job))
                deljob2.SelectedIndex = deljob2.Items.IndexOf(deljob2.Items.FindByValue(Job))
                deljob1_suffix.Text = Suffix
                deljob2_suffix.Text = Suffix

            End If
        End If

        txtbarcode.Focus()

        txtbarcode.Attributes.Add("onchange", "javascript:scanbarcode();")

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim sBarcode, sJob, sSuffix, sOper As String

        sBarcode = txtbarcode.Text
        sJob = ""
        sSuffix = ""
        sOper = ""
        txtbarcode.Text = String.Empty

        If sBarcode.Contains("|") = True Then

            Dim arrBarcode As String()
            arrBarcode = sBarcode.Split(New Char() {"|"c})

            If arrBarcode.Length > 0 Then
                sJob = arrBarcode(0)
                sSuffix = arrBarcode(1)
                sOper = arrBarcode(2)
            End If

            If sJob <> "" Then
                deljob1.SelectedIndex = deljob1.Items.IndexOf(deljob1.Items.FindByValue(sJob))
                deljob2.SelectedIndex = deljob2.Items.IndexOf(deljob2.Items.FindByValue(sJob))
            End If

            If sSuffix <> "" Then

                If Len(sSuffix) = 1 Then
                    deljob1_suffix.Text = "000" & sSuffix
                    deljob2_suffix.Text = "000" & sSuffix
                ElseIf Len(sSuffix) = 2 Then
                    deljob1_suffix.Text = "00" & sSuffix
                    deljob2_suffix.Text = "00" & sSuffix
                ElseIf Len(sSuffix) = 3 Then
                    deljob1_suffix.Text = "0" & sSuffix
                    deljob2_suffix.Text = "0" & sSuffix
                ElseIf Len(sSuffix) = 4 Then
                    deljob1_suffix.Text = sSuffix
                    deljob2_suffix.Text = sSuffix
                End If

                GetOper()

            End If

            If sOper <> "" Then
                OperDd.SelectedIndex = OperDd.Items.IndexOf(OperDd.Items.FindByValue(sOper))
            End If

        End If

    End Sub

    Protected Sub deljob1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles deljob1.SelectedIndexChanged

        If deljob1.SelectedItem.Value <> "" Then
            deljob1_suffix.Text = Right(deljob1.SelectedItem.Text, 4)

            deljob2.SelectedIndex = deljob2.Items.IndexOf(deljob2.Items.FindByValue(deljob1.SelectedItem.Value))
        End If

        GetOper()
    End Sub
    Protected Sub deljob2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles deljob2.SelectedIndexChanged

        If deljob2.SelectedItem.Value <> "" Then
            deljob2_suffix.Text = Right(deljob2.SelectedItem.Text, 4)
        End If

        'GetOper()

    End Sub
    Protected Sub checkfunction1_CheckedChanged(sender As Object, e As EventArgs) Handles checkfunction1.CheckedChanged
        If checkfunction1.Checked = True Then

            vCheckFunciotn.Value = "N"

        End If
        'MsgBox(vCheckFunciotn.Value)
    End Sub
    Protected Sub checkfunction2_CheckedChanged(sender As Object, e As EventArgs) Handles checkfunction2.CheckedChanged
        If checkfunction2.Checked = True Then

            vCheckFunciotn.Value = "R"

        End If
        'MsgBox(vCheckFunciotn.Value)
    End Sub
    Protected Sub RadioFormat1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioFormat1.CheckedChanged
        If RadioFormat1.Checked = True Then

            vTagType.Value = "O"

        End If
        ' MsgBox(vTagType.Value)
    End Sub
    Protected Sub RadioFormat2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioFormat2.CheckedChanged
        If RadioFormat2.Checked = True Then

            vTagType.Value = "I"

        End If
        ' MsgBox(vTagType.Value)
    End Sub
    Protected Sub ckOper_CheckedChanged(sender As Object, e As EventArgs) Handles ckOper.CheckedChanged

        If ckOper.Checked = True Then
            vCkeckOper.Value = 1
        Else
            vCkeckOper.Value = 0
        End If

    End Sub
    Protected Sub preview_Click(sender As Object, e As EventArgs) Handles preview.Click

        Dim StartDateTime As String
        Dim EndDateTime As String

        If String.IsNullOrEmpty(startdate.Text) Then
            StartDateTime = Nothing
        Else
            StartDateTime = DateTime.Parse(startdate.Text).ToString("yyyy-MM-dd")
        End If

        If String.IsNullOrEmpty(enddate.Text) Then
            EndDateTime = Nothing
        Else
            EndDateTime = DateTime.Parse(enddate.Text).ToString("yyyy-MM-dd")
        End If

        'If tagidstart.Items.Count = 0 Then
        '    tagidstart.Items.Insert(0, New ListItem("", ""))
        'End If

        'If tagidend.Items.Count = 0 Then
        '    tagidend.Items.Insert(0, New ListItem("", ""))
        'End If

        If OperDd.Items.Count = 0 Then
            OperDd.Items.Insert(0, New ListItem("", ""))
        End If

        If String.IsNullOrEmpty(txtarea.Value) Then
            txtarea.Value = ""
        End If

        If vCkeckOper.Value = "" Then
            vCkeckOper.Value = "0"
        End If

        If deljob1.SelectedItem.Value = "" Or deljob2.SelectedItem.Value = "" Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','กรุณาเลือก Job ก่อน Preview Tag', 'error');", True)
            Exit Sub
        End If


        oWS = New CNIService.DOWebServiceSoapClient
        Parms = "<Parameters><Parameter>" & Session("UserName").ToString & "</Parameter>" &
                "</Parameters>"

        oWS.CallMethod(Session("Token").ToString, "PPCC_TmpWIPTags", "PPCC_DeleteWIPTagSp", Parms)

        Parms = ""

        Parms = "<Parameters><Parameter>" & RTrim(deljob1.SelectedItem.Value) & "</Parameter>" &
                            "<Parameter>" & RTrim(deljob2.SelectedItem.Value) & "</Parameter>" &
                            "<Parameter>" & deljob1_suffix.Text & "</Parameter>" &
                            "<Parameter>" & deljob2_suffix.Text & "</Parameter>" &
                            "<Parameter>" & StartDateTime & "</Parameter>" &
                            "<Parameter>" & EndDateTime & "</Parameter>" &
                            "<Parameter>" & OperDd.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & txtStartTagID.Text & "</Parameter>" &
                            "<Parameter>" & txtEndTagID.Text & "</Parameter>" &
                            "<Parameter>" & vCheckFunciotn.Value & "</Parameter>" &
                            "<Parameter>" & Session("PSession").ToString & "</Parameter>" &
                            "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                            "<Parameter>" & PSite.ToString & "</Parameter>" &
                            "<Parameter>" & txtarea.Value & "</Parameter>" &
                            "<Parameter>" & vTagType.Value & "</Parameter>" &
                            "<Parameter>" & vCkeckOper.Value & "</Parameter>" &
                            "</Parameters>"


        oWS = New CNIService.DOWebServiceSoapClient

        oWS.CallMethod(Session("Token").ToString, "PPCC_TmpWIPTags", "PPCC_PreviewWIPTagSp", Parms)

        PrintGridViews()

    End Sub

    Sub GetJob()
        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_SLJobs", "Job, DerSuffix", "Type = 'J' AND DerConditionDate = '1'", "", "", 0)

        Dim dt_distinct As DataTable = ds.Tables(0).DefaultView.ToTable(True, "Job", "DerSuffix")

        For Each dRow As DataRow In dt_distinct.Rows
            deljob1.Items.Add(New ListItem(dRow("Job") & HttpUtility.HtmlDecode("&nbsp;&nbsp;") & dRow("DerSuffix"), dRow("Job")))
            deljob2.Items.Add(New ListItem(dRow("Job") & HttpUtility.HtmlDecode("&nbsp;&nbsp;") & dRow("DerSuffix"), dRow("Job")))
        Next

        deljob1.Items.Insert(0, New ListItem("", ""))
        deljob2.Items.Insert(0, New ListItem("", ""))

    End Sub
    Sub GetOper()

        Dim Filter As String = ""
        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet
        OperDd.Items.Clear()

        Filter = "Job = '" & deljob1.SelectedItem.Value & "' and Suffix = " & deljob1_suffix.Text & " and jbrUf_jobroute_WIPTag = 1"

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLJobRoutes", "OperNum, Wc", Filter, "OperNum", "", 0)

        Dim dt_distinct As DataTable = ds.Tables(0).DefaultView.ToTable(True, "OperNum", "Wc")

        For Each dRow As DataRow In dt_distinct.Rows
            OperDd.Items.Add(New ListItem(dRow("OperNum") & HttpUtility.HtmlDecode("&nbsp;&nbsp;") & dRow("Wc"), UCase(dRow("OperNum"))))
        Next

        OperDd.Items.Insert(0, New ListItem("", ""))


    End Sub

    'Sub GetTagId()


    '    oWS = New CNIService.DOWebServiceSoapClient

    '    ds = New DataSet

    '    Filter = "RefType = 'W' And stat <> 'X' AND DerConditionDate = '1'"


    '    ds = oWS.LoadDataSet(Session("Token").ToString, "ppcc_tags", "TagID, item", Filter, "", "", 0)

    '    Dim dt_distinct As DataTable = ds.Tables(0).DefaultView.ToTable(True, "TagID", "item")

    '    For Each dRow As DataRow In dt_distinct.Rows
    '        tagidstart.Items.Add(New ListItem(dRow("TagID") & IIf(IsDBNull(dRow("item")), "", " : " & dRow("item")), UCase(dRow("TagID"))))
    '        tagidend.Items.Add(New ListItem(dRow("TagID") & IIf(IsDBNull(dRow("item")), "", " : " & dRow("item")), UCase(dRow("TagID"))))

    '    Next


    '    tagidstart.Items.Insert(0, New ListItem("", ""))
    '    tagidend.Items.Insert(0, New ListItem("", ""))



    'End Sub

    Sub PrintGridViews()

        Dim Propertie As String
        Dim Filter As String
        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet

        Filter = "Sessionid = '" & Session("PSession").ToString & "' and CreatedBy = '" & Session("UserName").ToString & "'"


        Propertie = "Selected,Error,TagFormat,Job,Suffix,OperNum,Item,Description,TagId,TagQty,UM,TotalWeight,PackageWeight,PartWeight,GrossWeight,NextOperNum,PoNum,PoLine,PoRelease,VendNum,VendName,Whse,RowPointer"

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_TmpWIPTags", Propertie, Filter, "", "", -1)

        GridView2.DataSource = ds
        GridView2.DataBind()
    End Sub


    Protected Sub BTSelectAll_Click(sender As Object, e As EventArgs) Handles BTSelectAll.Click

        For Each row As GridViewRow In GridView2.Rows
            Dim chkSelect As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)
            chkSelect.Checked = True
        Next

    End Sub

    Protected Sub BTDeSelectAll_Click(sender As Object, e As EventArgs) Handles BTDeSelectAll.Click

        For Each row As GridViewRow In GridView2.Rows
            Dim chkSelect As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)
            chkSelect.Checked = False
        Next

    End Sub

    Protected Sub print_Click(sender As Object, e As EventArgs) Handles print.Click

        Dim sError, MsgErr, sSplitMerge, CheckError, RowPointer, sFunction, BGTaskName, TaskParms, Script As String
        sError = ""
        MsgErr = ""
        CheckError = "0"
        sFunction = ""
        BGTaskName = ""
        TaskParms = ""
        Script = ""

        If GridView2.Rows.Count > 0 Then

            For Each row As GridViewRow In GridView2.Rows

                Dim chkSelect As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)
                RowPointer = IIf(row.Cells(22).Text <> "", row.Cells(22).Text, "")
                MsgErr = IIf(row.Cells(2).Text <> "", row.Cells(2).Text, "")
                Parms = ""

                Parms = "<Parameters><Parameter>" & RowPointer & "</Parameter>" &
                        "<Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter>" & PSite.ToString & "</Parameter>" &
                        "<Parameter>" & IIf(chkSelect.Checked = True, "1", "0") & "</Parameter>" &
                        "</Parameters>"

                oWS = New CNIService.DOWebServiceSoapClient
                oWS.CallMethod(Session("Token").ToString, "PPCC_TmpWIPTags", "PPCC_EX_UpdateSelectTmpWIPTagSp", Parms)

            Next

        End If


        If MsgErr = "" Or String.IsNullOrEmpty(MsgErr) Then

            TaskParms = ""
            'V(SessionIDVar),V(Parm_Site),V(UsernameVar),V(vStat)

            If checkfunction1.Checked = True Then

                TaskParms = "~LIT~(" & Session("PSession").ToString & "),"
                TaskParms = TaskParms & "~LIT~(" & PSite.ToString & "),"
                TaskParms = TaskParms & "~LIT~(" & Session("UserName").ToString & "),"
                TaskParms = TaskParms & "~LIT~(" & "N" & ")"

                Call BGTask("PPCC_WIPTag", TaskParms, sFunction, Nothing)

                Script = "window.open('" & Session("UrlReport").ToString & "', '_blank')"
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "response", Script, True)

            ElseIf checkfunction2.Checked = True Then
                TaskParms = "~LIT~(" & Session("PSession").ToString & "),"
                TaskParms = TaskParms & "~LIT~(" & PSite.ToString & "),"
                TaskParms = TaskParms & "~LIT~(" & Session("UserName").ToString & "),"
                TaskParms = TaskParms & "~LIT~(" & "R" & ")"

                Call BGTask("PPCC_WIPTag", TaskParms, sFunction, Nothing)

                Script = "window.open('" & Session("UrlReport").ToString & "', '_blank')"
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "response", Script, True)


            End If

            btnclear_Click(sender, e)

            If Not IsNothing(Session("UrlReport")) Then
                'ClearDate()
                'PrintGridViews()
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Success','Report Submitted', 'success');", True)
            Else
                'ClearDate()
                'PrintGridViews()
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','Report Filed', 'error');", True)
                Exit Sub
            End If



        Else
            'ClearDate()
            'PrintGridViews()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('PPCC','" & MsgErr & "', 'error');", True)

            Exit Sub
        End If

    End Sub

    Protected Sub btnclear_Click(sender As Object, e As EventArgs) Handles btnclear.Click

        Parms = ""

        Parms = "<Parameters><Parameter>" & Session("UserName").ToString & "</Parameter>" &
                "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_TmpWIPTags", "PPCC_DeleteWIPTagSp", Parms)

        PrintGridViews()

    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles GridView2.RowDataBound

        Dim TotalWeight As Decimal = 0
        Dim PackWeight As Decimal = 0
        Dim PartWeight As Decimal = 0
        Dim GrossWeight As Decimal = 0
        Dim TagQty As Decimal = 0
        Dim TagFormat As String = ""

        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim lblTagFormat As Label = DirectCast(e.Row.FindControl("lblTagFormat"), Label)

            Dim lblTotalWeight As Label = DirectCast(e.Row.FindControl("lblTotalWeight"), Label)
            Dim lblPackWeight As Label = DirectCast(e.Row.FindControl("lblPackWeight"), Label)
            Dim lblPartWeight As Label = DirectCast(e.Row.FindControl("lblPartWeight"), Label)
            Dim lblGrossWeight As Label = DirectCast(e.Row.FindControl("lblGrossWeight"), Label)
            Dim lblTagQty As Label = DirectCast(e.Row.FindControl("lblTagQty"), Label)

            Decimal.TryParse(lblTotalWeight.Text, TotalWeight)
            Decimal.TryParse(lblPackWeight.Text, PackWeight)
            Decimal.TryParse(lblPartWeight.Text, PartWeight)
            Decimal.TryParse(lblGrossWeight.Text, GrossWeight)
            Decimal.TryParse(lblTagQty.Text, TagQty)

            If lblTagFormat.Text = "O" Then
                lblTagFormat.Text = "Outside Tag"
            ElseIf lblTagFormat.Text = "I" Then
                lblTagFormat.Text = "In-House Tag"
            End If

            lblTotalWeight.Text = FormatNumber(TotalWeight, LenPointQty)
            lblPackWeight.Text = FormatNumber(PackWeight, LenPointQty)
            lblPartWeight.Text = FormatNumber(PartWeight, LenPointQty)
            lblGrossWeight.Text = FormatNumber(GrossWeight, LenPointQty)
            lblTagQty.Text = FormatNumber(TagQty, LenPointQty)

            CheckError()

        End If




    End Sub
    Function CheckError()
        Dim sError As String = ""
        Dim sCheckError As String = "0"
        If GridView2.Rows.Count > 0 Then

            For Each row As GridViewRow In GridView2.Rows

                sError = IIf(row.Cells(2).Text <> "", row.Cells(2).Text, "")

                If sError <> "" Then
                    sCheckError = "1"
                    Exit For
                End If

            Next

            If sCheckError = "1" Then
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "DisabledPrint();", True)
            Else
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "EnablePrint();", True)
            End If

        End If
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

    Sub ClearDate()
        Dim sError, MsgErr, CheckError, RowPointer, BGTaskName, TaskParms As String
        sError = ""
        MsgErr = ""
        CheckError = "0"

        BGTaskName = ""
        TaskParms = ""


        If GridView2.Rows.Count > 0 Then

            For Each row As GridViewRow In GridView2.Rows

                Dim chkSelect As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)
                RowPointer = IIf(row.Cells(22).Text <> "", row.Cells(22).Text, "")
                MsgErr = IIf(row.Cells(2).Text <> "", row.Cells(2).Text, "")
                Parms = ""

                Parms = "<Parameters><Parameter>" & RowPointer & "</Parameter>" &
                        "<Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter>" & PSite.ToString & "</Parameter>" &
                        "<Parameter>" & "0" & "</Parameter>" &
                        "</Parameters>"

                oWS = New CNIService.DOWebServiceSoapClient
                oWS.CallMethod(Session("Token").ToString, "PPCC_TmpWIPTags", "PPCC_EX_UpdateSelectTmpWIPTagSp", Parms)

            Next

        End If
    End Sub

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

End Class

