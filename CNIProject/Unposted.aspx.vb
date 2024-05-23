Imports System.Data
Imports System.Xml
Imports System.Collections
Imports System.Reflection
Imports Microsoft.VisualBasic.DateInterval

Public Class Unposted
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

    Private Shared Property PJobStockTran() As String
        Get
            Return JobStockTran
        End Get
        Set(value As String)
            JobStockTran = value
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
        LenPointQtyFormat = QtyFormat()

        If Not Page.IsPostBack Then

            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "LocalSet();", True)

            Session.Remove("LabelScan")

            PSite = GetSite()
            PJobStockTran = GetJobStockTran()
            PWhse = GetDefWhse()

            DateNow = Date.Now.ToString("dd/MM/yyyy")

            txtdate.Text = DateNow
            txtStartTime.Text = Date.Now.ToString("HH:mm")
            txtEndTime.Text = Date.Now.ToString("HH:mm")

            '---------Dropdown------------'
            GetEmployee()
            GetShift()
            GetReasonCode()
            GetReasonCodeDownTime()

            '---------End Dropdown------------'

            lbltotalscrap.Text = FormatNumber(0, LenPointQty)
            txtQtyScrap.Text = FormatNumber(0, LenPointQty)

            BflushMessage.Value = ""
            BflushError.Value = 0

            'link from Job Monitoring Status
            If Request.QueryString("sBarcode") <> "" Or Not String.IsNullOrEmpty(Request.QueryString("sBarcode")) Then

                Button1_Click(sender, e)

                If Request.QueryString("StartTime") <> "" Or Not String.IsNullOrEmpty(Request.QueryString("StartTime")) Then
                    txtStartTime.Text = Request.QueryString("StartTime").ToString
                Else
                    txtStartTime.Text = Date.Now.ToString("HH:mm")
                End If

                If Request.QueryString("EndTime") <> "" Or Not String.IsNullOrEmpty(Request.QueryString("EndTime")) Then
                    txtEndTime.Text = Request.QueryString("EndTime").ToString
                Else
                    txtEndTime.Text = Date.Now.ToString("HH:mm")
                End If

                txtStartTime.Attributes.Add("disabled", "disabled")
                txtEndTime.Attributes.Add("disabled", "disabled")

                GetSchedulingShift()

            End If



        End If


        txtbarcode.Focus()

        txtbarcode.Attributes.Add("onchange", "javascript:scanbarcode();")

        If ActiveTabTextBox.Value = "3" Then

            If ddlscrapcode.SelectedItem.Value = "" Then
                lblbarcode.Text = "Barcode Scrap Code: "
            Else
                lblbarcode.Text = "Barcode Scrapped: "
            End If

        ElseIf ActiveTabTextBox.Value = "5" Then

            If ddldowntime.SelectedItem.Value = "" Then
                lblbarcode.Text = "Barcode Downtime Code: "
            Else

                If Session("LabelScan") Is Nothing Or Session("LabelScan") = "" Then
                    lblbarcode.Text = "Barcode Downtime Start Time: "
                Else
                    lblbarcode.Text = Session("LabelScan").ToString
                End If

            End If

        ElseIf ActiveTabTextBox.Value = "1" Then

            If txtjob.Text = String.Empty Then

                lblbarcode.Text = "Barcode Job: "

            ElseIf ddlemployee.SelectedItem.Value = "" Then

                lblbarcode.Text = "Barcode Employee: "

            ElseIf ddlResource.SelectedItem.Value = "" Then

                lblbarcode.Text = "Barcode Resource: "

            End If


        ElseIf ActiveTabTextBox.Value = "2" Then

            lblbarcode.Text = "Barcode Operator: "

        End If

        If CntrlPoint.Value = "0" Then

            tab2.HRef = "javascript:void(0);"

        End If

        'If WipTag.Value = "0" Then
        '    btnPrintWIPTag.Attributes.Add("disabled", "disabled")
        'Else
        '    btnPrintWIPTag.Attributes.Remove("disabled")
        'End If

        txtDownTimeTransDate.Text = txtdate.Text

    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        ActiveTabTextBox.Value = "5"
        Label3.Text = "5"

        'If PanelList3.Items.Count = 0 And lbljob.Text <> String.Empty Then

        '    Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
        '                "<Parameter>" & Session("Username").ToString & "</Parameter>" &
        '                "<Parameter>" & PSite.ToString() & "</Parameter>" &
        '                "<Parameter>" & lbljob.Text & "</Parameter>" &
        '                "<Parameter>" & lblSuffix.Text & "</Parameter>" &
        '                "<Parameter>" & lbloper.Text & "</Parameter>" &
        '                "<Parameter>" & "I" & "</Parameter>" &
        '                "<Parameter>" & DBNull.Value & "</Parameter>" &
        '                "</Parameters>"

        '    oWS = New CNIService.DOWebServiceSoapClient
        '    oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobTranTags", "PPCC_EX_InsertDowmtimeDefaultSp", Parms)

        '    BindDownTime()


        'End If


    End Sub

    'Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

    '    ActiveTabTextBox.Value = "5"
    '    Label3.Text = "5"

    'End Sub

    Protected Sub btnPrintPDTag_Click(sender As Object, e As EventArgs) Handles btnPrintPDTag.Click

        Dim PostURL As String = ""

        PostURL = "?SessionID=" & Session("PSession").ToString & "&Job=" & lbljob.Text & "&Suffix=" & lblSuffix.Text & ""

        Response.Redirect("PrintProductionDeliveryTag.aspx" & PostURL)

    End Sub

    Protected Sub btnPrintWIPTag_Click(sender As Object, e As EventArgs) Handles btnPrintWIPTag.Click

        'If WipTag.Value = "1" Then

        Dim PostURL As String = ""

        PostURL = "?SessionID=" & Session("PSession").ToString & "&Job=" & lbljob.Text & "&Suffix=" & lblSuffix.Text & ""

        Response.Redirect("WIPTag.aspx" & PostURL)

        'End If



    End Sub


    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim sBarcode, sJob, sSuffix, sOper, sSchedDriver, sWc, sProdLine, sCoProductMix, sItem, sWhse, sCntrlPoint, sLabor, sMachine, sWipTag As String
        Dim sLoc, sLastOper, Stat, MsgType, MsgErr As String
        Dim sQtyReceive, sQtyComplete As Decimal
        Dim oDataset As DataSet
        Dim StartTime As DateTime
        Dim Endtime As DateTime
        Dim TotalHours As Decimal = 0
        Dim StartSec As Integer = 0
        Dim EndSac As Integer = 0
        Dim sDate As DateTime

        sJob = ""
        sSuffix = ""
        sOper = ""
        sSchedDriver = ""
        sWc = ""
        sProdLine = ""
        sQtyReceive = 0
        sQtyComplete = 0
        sCoProductMix = "0"
        sItem = ""
        sWhse = ""
        sLastOper = "10"
        sLoc = ""
        Parms = ""
        Stat = "TRUE"
        MsgType = ""
        MsgErr = ""
        sCntrlPoint = "0"
        sLabor = ""
        sMachine = ""
        sWipTag = "0"

        If Request.QueryString("sBarcode") <> "" Or Not String.IsNullOrEmpty(Request.QueryString("sBarcode")) Then

            If lblbarcode.Text = "Barcode Job: " Then
                txtbarcode.Text = Request.QueryString("sBarcode").ToString
            End If

        End If

        sBarcode = txtbarcode.Text

        If txtbarcode.Text <> "" Then

            If lblbarcode.Text = "Barcode Job: " Then

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                            "<Parameter>" & "J" & "</Parameter>" &
                            "<Parameter>" & sBarcode & "</Parameter>" &
                            "<Parameter>" & "" & "</Parameter>" &
                            "<Parameter>" & "" & "</Parameter>" &
                            "<Parameter>" & "" & "</Parameter>" &
                            "<Parameter>" & IIf(chkcancel.Checked = True, 1, 0) & "</Parameter>" &
                            "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                            "</Parameters>"

            ElseIf lblbarcode.Text = "Barcode Employee: " Then

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                            "<Parameter>" & "E" & "</Parameter>" &
                            "<Parameter>" & sBarcode & "</Parameter>" &
                            "<Parameter>" & lbljob.Text & "</Parameter>" &
                            "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                            "<Parameter>" & lbloper.Text & "</Parameter>" &
                            "<Parameter>" & IIf(chkcancel.Checked = True, 1, 0) & "</Parameter>" &
                            "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                            "</Parameters>"

            ElseIf lblbarcode.Text = "Barcode Resource: " Then

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                            "<Parameter>" & "R" & "</Parameter>" &
                            "<Parameter>" & sBarcode & "</Parameter>" &
                            "<Parameter>" & lbljob.Text & "</Parameter>" &
                            "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                            "<Parameter>" & lbloper.Text & "</Parameter>" &
                            "<Parameter>" & IIf(chkcancel.Checked = True, 1, 0) & "</Parameter>" &
                            "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                            "</Parameters>"

                'ElseIf lblbarcode.Text = "Barcode Scheduling Shift: " Then

                '    Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                '                "<Parameter>" & "S" & "</Parameter>" &
                '                "<Parameter>" & sBarcode & "</Parameter>" &
                '                "<Parameter>" & lbljob.Text & "</Parameter>" &
                '                "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                '                "<Parameter>" & lbloper.Text & "</Parameter>" &
                '                "<Parameter>" & IIf(chkcancel.Checked = True, 1, 0) & "</Parameter>" &
                '                "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                '                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                '                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                '                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                '                "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                '                "</Parameters>"

                'ElseIf lblbarcode.Text = "Barcode Completed: " Then

                '    Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                '                "<Parameter>" & "C" & "</Parameter>" &
                '                "<Parameter>" & sBarcode & "</Parameter>" &
                '                "<Parameter>" & lbljob.Text & "</Parameter>" &
                '                "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                '                "<Parameter>" & lbloper.Text & "</Parameter>" &
                '                "<Parameter>" & IIf(chkcancel.Checked = True, 1, 0) & "</Parameter>" &
                '                "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                '                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                '                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                '                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                '                "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                '                "</Parameters>"

            ElseIf lblbarcode.Text = "Barcode Start Time: " Then

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                            "<Parameter>" & "T" & "</Parameter>" &
                            "<Parameter>" & sBarcode & "</Parameter>" &
                            "<Parameter>" & lbljob.Text & "</Parameter>" &
                            "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                            "<Parameter>" & lbloper.Text & "</Parameter>" &
                            "<Parameter>" & IIf(chkcancel.Checked = True, 1, 0) & "</Parameter>" &
                            "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                            "</Parameters>"

            ElseIf lblbarcode.Text = "Barcode End Time: " Then

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                            "<Parameter>" & "T" & "</Parameter>" &
                            "<Parameter>" & sBarcode & "</Parameter>" &
                            "<Parameter>" & lbljob.Text & "</Parameter>" &
                            "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                            "<Parameter>" & lbloper.Text & "</Parameter>" &
                            "<Parameter>" & IIf(chkcancel.Checked = True, 1, 0) & "</Parameter>" &
                            "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                            "</Parameters>"

                'ElseIf lblbarcode.Text = "Barcode Total Hours: " Then

                '    Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                '                "<Parameter>" & "H" & "</Parameter>" &
                '                "<Parameter>" & sBarcode & "</Parameter>" &
                '                "<Parameter>" & lbljob.Text & "</Parameter>" &
                '                "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                '                "<Parameter>" & lbloper.Text & "</Parameter>" &
                '                "<Parameter>" & IIf(chkcancel.Checked = True, 1, 0) & "</Parameter>" &
                '                "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                '                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                '                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                '                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                '                "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                '                "</Parameters>"

            ElseIf lblbarcode.Text = "Barcode Scrap Code: " Then

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                            "<Parameter>" & "Z" & "</Parameter>" &
                            "<Parameter>" & sBarcode & "</Parameter>" &
                            "<Parameter>" & lbljob.Text & "</Parameter>" &
                            "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                            "<Parameter>" & lbloper.Text & "</Parameter>" &
                            "<Parameter>" & IIf(chkcancel.Checked = True, 1, 0) & "</Parameter>" &
                            "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                            "</Parameters>"

            ElseIf lblbarcode.Text = "Barcode Scrapped: " Then

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                            "<Parameter>" & "P" & "</Parameter>" &
                            "<Parameter>" & sBarcode & "</Parameter>" &
                            "<Parameter>" & lbljob.Text & "</Parameter>" &
                            "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                            "<Parameter>" & lbloper.Text & "</Parameter>" &
                            "<Parameter>" & IIf(chkcancel.Checked = True, 1, 0) & "</Parameter>" &
                            "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                            "</Parameters>"

            ElseIf lblbarcode.Text = "Barcode Downtime Code: " Then

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                            "<Parameter>" & "X" & "</Parameter>" &
                            "<Parameter>" & sBarcode & "</Parameter>" &
                            "<Parameter>" & lbljob.Text & "</Parameter>" &
                            "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                            "<Parameter>" & lbloper.Text & "</Parameter>" &
                            "<Parameter>" & IIf(chkcancel.Checked = True, 1, 0) & "</Parameter>" &
                            "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                            "</Parameters>"

            ElseIf lblbarcode.Text = "Barcode Downtime Code: " Then

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                            "<Parameter>" & "X" & "</Parameter>" &
                            "<Parameter>" & sBarcode & "</Parameter>" &
                            "<Parameter>" & lbljob.Text & "</Parameter>" &
                            "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                            "<Parameter>" & lbloper.Text & "</Parameter>" &
                            "<Parameter>" & IIf(chkcancel.Checked = True, 1, 0) & "</Parameter>" &
                            "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                            "</Parameters>"

            ElseIf lblbarcode.Text = "Barcode Downtime Start Time: " Then

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                            "<Parameter>" & "T" & "</Parameter>" &
                            "<Parameter>" & sBarcode & "</Parameter>" &
                            "<Parameter>" & lbljob.Text & "</Parameter>" &
                            "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                            "<Parameter>" & lbloper.Text & "</Parameter>" &
                            "<Parameter>" & IIf(chkcancel.Checked = True, 1, 0) & "</Parameter>" &
                            "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                            "</Parameters>"

            ElseIf lblbarcode.Text = "Barcode Downtime End Time: " Then

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                            "<Parameter>" & "T" & "</Parameter>" &
                            "<Parameter>" & sBarcode & "</Parameter>" &
                            "<Parameter>" & lbljob.Text & "</Parameter>" &
                            "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                            "<Parameter>" & lbloper.Text & "</Parameter>" &
                            "<Parameter>" & IIf(chkcancel.Checked = True, 1, 0) & "</Parameter>" &
                            "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                            "</Parameters>"

            ElseIf lblbarcode.Text = "Barcode Operator: " Then

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                            "<Parameter>" & "A" & "</Parameter>" &
                            "<Parameter>" & sBarcode & "</Parameter>" &
                            "<Parameter>" & lbljob.Text & "</Parameter>" &
                            "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                            "<Parameter>" & lbloper.Text & "</Parameter>" &
                            "<Parameter>" & IIf(chkcancel.Checked = True, 1, 0) & "</Parameter>" &
                            "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                            "</Parameters>"

                'ElseIf lblbarcode.Text = "Barcode Downtime Total Hours: " Then

                '    Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                '                "<Parameter>" & "U" & "</Parameter>" &
                '                "<Parameter>" & sBarcode & "</Parameter>" &
                '                "<Parameter>" & lbljob.Text & "</Parameter>" &
                '                "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                '                "<Parameter>" & lbloper.Text & "</Parameter>" &
                '                "<Parameter>" & IIf(chkcancel.Checked = True, 1, 0) & "</Parameter>" &
                '                "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                '                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                '                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                '                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                '                "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                '                "</Parameters>"

            End If

            oWS = New CNIService.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobTranTags", "PPCC_Ex_CheckValidateUnpostedSp", Parms)

            Dim doc As XmlDocument = New XmlDocument()
            doc.LoadXml(Parms)

            Dim i As Integer = 1

            For Each node As XmlNode In doc.DocumentElement

                If i = 9 Then
                    Stat = node.InnerText

                ElseIf i = 10 Then
                    MsgType = node.InnerText

                ElseIf i = 11 Then
                    MsgErr = node.InnerText

                End If

                i += 1

            Next

            If Stat = "FALSE" Then

                MsgErr = MsgErr.Replace("'", "\'")
                MsgErr = MsgErr.Replace(vbLf, "<br />")

                MsgType = "Error [" & MsgType & "]"

                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)

            End If

            If Stat = "TRUE" Then

                If lblbarcode.Text = "Barcode Job: " Then

                    Dim arrBarcode As String()
                    arrBarcode = sBarcode.Split(New Char() {"|"c})

                    If arrBarcode.Length > 0 Then
                        sJob = arrBarcode(0)
                        sSuffix = arrBarcode(1)
                        sOper = arrBarcode(2)
                    End If

                    lbljob.Text = sJob
                    lblSuffix.Text = sSuffix
                    lbloper.Text = sOper

                    txtjob.Text = lbljob.Text +
                                  "-" +
                                  IIf(Len(lblSuffix.Text) = 1, "000" + lblSuffix.Text, IIf(Len(lblSuffix.Text) = 2, "00" + lblSuffix.Text, IIf(Len(lblSuffix.Text) = 3, "0" + lblSuffix.Text, lblSuffix.Text))) +
                                  "-" +
                                  lbloper.Text

                    sCntrlPoint = GetContorlPoint(lbljob.Text, lblSuffix.Text, lbloper.Text)
                    CntrlPoint.Value = sCntrlPoint

                    If sCntrlPoint = "0" Then
                        tab2.HRef = "javascript:void(0);"
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Warning','#227 : Operation นี้ไม่ได้ Control Point', 'warning');", True)
                    End If

                    '---------GetValue------------'

                    sItem = GetItemJob(lbljob.Text, lblSuffix.Text)
                    sWhse = GetDefWhse()
                    sLastOper = GetLastOper(lbljob.Text, lblSuffix.Text)
                    sCoProductMix = GetCoProductMix(lbljob.Text, lblSuffix.Text)

                    GetResource(lbljob.Text, lblSuffix.Text, lbloper.Text)
                    GetItemScrap(sCoProductMix, lbljob.Text, lblSuffix.Text)
                    GetItemLoc(sItem, sWhse)
                    '---------End GetValue------------'


                    '---------SetValue------------'

                    oDataset = GetJobroute(lbljob.Text, lblSuffix.Text, lbloper.Text)

                    If oDataset.Tables(0).Rows.Count > 0 Then
                        sSchedDriver = oDataset.Tables(0).Rows(0)("JshSchedDrv").ToString
                        sWc = oDataset.Tables(0).Rows(0)("Wc").ToString
                        sProdLine = oDataset.Tables(0).Rows(0)("jbrUf_jobroute_line").ToString
                        sQtyReceive = oDataset.Tables(0).Rows(0)("QtyReceived").ToString
                        sQtyComplete = oDataset.Tables(0).Rows(0)("QtyComplete").ToString
                        sWipTag = oDataset.Tables(0).Rows(0)("jbrUf_jobroute_WIPTag").ToString
                    End If

                    WipTag.Value = sWipTag

                    GetResourceGroupsType(lbljob.Text, lblSuffix.Text, lbloper.Text, sLabor, sMachine)


                    If sSchedDriver = "M" Then

                        If sLabor = "L" And sMachine = "M" Then
                            ddltrantype.SelectedIndex = ddltrantype.Items.IndexOf(ddltrantype.Items.FindByValue("R"))
                        Else
                            ddltrantype.SelectedIndex = ddltrantype.Items.IndexOf(ddltrantype.Items.FindByValue("C"))
                        End If

                    Else
                        ddltrantype.SelectedIndex = ddltrantype.Items.IndexOf(ddltrantype.Items.FindByValue("R"))
                    End If

                    If Request.QueryString("Wc") <> "" Or Not String.IsNullOrEmpty(Request.QueryString("Wc")) Then
                        txtwc.Text = Request.QueryString("Wc").ToString
                    Else
                        txtwc.Text = sWc
                    End If

                    If Request.QueryString("Resource") <> "" Or Not String.IsNullOrEmpty(Request.QueryString("Resource")) Then
                        ddlResource.SelectedIndex = ddlResource.Items.IndexOf(ddlResource.Items.FindByValue(Request.QueryString("Resource")))
                    End If

                    txtProdLine.Text = sProdLine

                    QAQCPass(txtwc.Text)

                    If sLastOper = lbloper.Text Then

                        sLoc = GetMoveToLoc(sWhse, sItem)

                        ddlmovetoloc.SelectedIndex = ddlmovetoloc.Items.IndexOf(ddlmovetoloc.Items.FindByValue(UCase(sLoc)))
                    End If

                    BindItem()

                    '---------End SetValue------------'

                    If ddltrantype.SelectedItem.Value = "C" Then

                        ddlemployee.Attributes.Add("disabled", "disabled")
                        lblbarcode.Text = "Barcode Resource: "

                    ElseIf ddltrantype.SelectedItem.Value = "R" Then

                        ddlemployee.Attributes.Remove("disabled")
                        lblbarcode.Text = "Barcode Employee: "

                    End If

                ElseIf lblbarcode.Text = "Barcode Employee: " Then

                    ddlemployee.SelectedIndex = ddlemployee.Items.IndexOf(ddlemployee.Items.FindByValue(UCase(sBarcode)))

                    ddlemployee_SelectedIndexChanged(sender, e)

                ElseIf lblbarcode.Text = "Barcode Resource: " Then

                    ddlResource.SelectedIndex = ddlResource.Items.IndexOf(ddlResource.Items.FindByValue(UCase(sBarcode)))

                    ddlResource_SelectedIndexChanged(sender, e)


                ElseIf lblbarcode.Text = "Barcode Scheduling Shift: " Then

                    ddlSchedulingShift.SelectedIndex = ddlSchedulingShift.Items.IndexOf(ddlSchedulingShift.Items.FindByValue(UCase(sBarcode)))

                    ddlSchedulingShift_SelectedIndexChanged(sender, e)

                ElseIf lblbarcode.Text = "Barcode Start Time: " Then

                    DigitStartTime.Value = CInt(DigitStartTime.Value) + 1

                    If DigitStartTime.Value = 1 Then

                        txtStartTime.Text = sBarcode + "0:00"

                        StartTime = DateTime.ParseExact("1900-01-01 " & txtStartTime.Text, "yyyy-MM-dd HH:mm", Nothing)

                        If txtStartTime.Text = "00:00" Then
                            Endtime = Convert.ToDateTime("1900-01-01 " & txtEndTime.Text).ToString
                        Else
                            Endtime = Convert.ToDateTime("1900-01-02 " & txtEndTime.Text).ToString
                        End If

                        sDate = DateTime.ParseExact("1900-01-01 00:00", "yyyy-MM-dd HH:mm", Nothing)

                        StartSec = DateDiff(DateInterval.Second, sDate, StartTime)
                        EndSac = DateDiff(DateInterval.Second, sDate, Endtime)

                        txttotalhour.Text = ((EndSac - StartSec) / 60.0) / 60.0

                    ElseIf DigitStartTime.Value = 2 Then

                        txtStartTime.Text = Left(txtStartTime.Text, 1) + sBarcode + ":00"

                        StartTime = DateTime.ParseExact("1900-01-01 " & txtStartTime.Text, "yyyy-MM-dd HH:mm", Nothing)

                        If txtDTStartTime.Text = "00:00" Then
                            Endtime = Convert.ToDateTime("1900-01-01 " & txtEndTime.Text).ToString
                        Else
                            Endtime = Convert.ToDateTime("1900-01-02 " & txtEndTime.Text).ToString
                        End If

                        sDate = DateTime.ParseExact("1900-01-01 00:00", "yyyy-MM-dd HH:mm", Nothing)


                        StartSec = DateDiff(DateInterval.Second, sDate, StartTime)
                        EndSac = DateDiff(DateInterval.Second, sDate, Endtime)

                        txttotalhour.Text = ((EndSac - StartSec) / 60.0) / 60.0

                    ElseIf DigitStartTime.Value = 3 Then

                        txtStartTime.Text = Left(txtStartTime.Text, 2) + ":" + sBarcode + "0"

                        StartTime = DateTime.ParseExact("1900-01-01 " & txtStartTime.Text, "yyyy-MM-dd HH:mm", Nothing)

                        If txtDTStartTime.Text = "00:00" Then
                            Endtime = Convert.ToDateTime("1900-01-01 " & txtEndTime.Text).ToString
                        Else
                            Endtime = Convert.ToDateTime("1900-01-02 " & txtEndTime.Text).ToString
                        End If

                        sDate = DateTime.ParseExact("1900-01-01 00:00", "yyyy-MM-dd HH:mm", Nothing)

                        StartSec = DateDiff(DateInterval.Second, sDate, StartTime)
                        EndSac = DateDiff(DateInterval.Second, sDate, Endtime)

                        txttotalhour.Text = ((EndSac - StartSec) / 60.0) / 60.0

                    ElseIf DigitStartTime.Value = 4 Then

                        txtStartTime.Text = Left(txtStartTime.Text, 4) + sBarcode

                        StartTime = DateTime.ParseExact("1900-01-01 " & txtStartTime.Text, "yyyy-MM-dd HH:mm", Nothing)

                        If txtStartTime.Text = "00:00" Then
                            Endtime = Convert.ToDateTime("1900-01-01 " & txtEndTime.Text).ToString
                        Else
                            Endtime = Convert.ToDateTime("1900-01-02 " & txtEndTime.Text).ToString
                        End If

                        sDate = DateTime.ParseExact("1900-01-01 00:00", "yyyy-MM-dd HH:mm", Nothing)

                        StartSec = DateDiff(DateInterval.Second, sDate, StartTime)
                        EndSac = DateDiff(DateInterval.Second, sDate, Endtime)

                        txttotalhour.Text = ((EndSac - StartSec) / 60.0) / 60.0

                        lblbarcode.Text = "Barcode End Time: "
                        Session("LabelScan") = "Barcode End Time: "


                    End If


                ElseIf lblbarcode.Text = "Barcode End Time: " Then

                    DigitEndTime.Value = CInt(DigitEndTime.Value) + 1

                    If DigitEndTime.Value = 1 Then

                        txtEndTime.Text = sBarcode + "0:00"

                        StartTime = DateTime.ParseExact("1900-01-01 " & txtStartTime.Text, "yyyy-MM-dd HH:mm", Nothing)

                        Endtime = Convert.ToDateTime("1900-01-01 " & txtEndTime.Text).ToString

                        sDate = DateTime.ParseExact("1900-01-01 00:00", "yyyy-MM-dd HH:mm", Nothing)

                        StartSec = DateDiff(DateInterval.Second, sDate, StartTime)
                        EndSac = DateDiff(DateInterval.Second, sDate, Endtime)

                        txttotalhour.Text = ((EndSac - StartSec) / 60.0) / 60.0

                    ElseIf DigitEndTime.Value = 2 Then

                        txtEndTime.Text = Left(txtEndTime.Text, 1) + sBarcode + ":00"

                        StartTime = DateTime.ParseExact("1900-01-01 " & txtStartTime.Text, "yyyy-MM-dd HH:mm", Nothing)

                        Endtime = Convert.ToDateTime("1900-01-01 " & txtEndTime.Text).ToString

                        sDate = DateTime.ParseExact("1900-01-01 00:00", "yyyy-MM-dd HH:mm", Nothing)


                        StartSec = DateDiff(DateInterval.Second, sDate, StartTime)
                        EndSac = DateDiff(DateInterval.Second, sDate, Endtime)

                        txttotalhour.Text = ((EndSac - StartSec) / 60.0) / 60.0

                    ElseIf DigitEndTime.Value = 3 Then

                        txtEndTime.Text = Left(txtEndTime.Text, 2) + ":" + sBarcode + "0"

                        StartTime = DateTime.ParseExact("1900-01-01 " & txtStartTime.Text, "yyyy-MM-dd HH:mm", Nothing)

                        Endtime = Convert.ToDateTime("1900-01-01 " & txtEndTime.Text).ToString

                        sDate = DateTime.ParseExact("1900-01-01 00:00", "yyyy-MM-dd HH:mm", Nothing)

                        StartSec = DateDiff(DateInterval.Second, sDate, StartTime)
                        EndSac = DateDiff(DateInterval.Second, sDate, Endtime)

                        txttotalhour.Text = ((EndSac - StartSec) / 60.0) / 60.0

                    ElseIf DigitEndTime.Value = 4 Then

                        txtEndTime.Text = Left(txtEndTime.Text, 4) + sBarcode

                        StartTime = DateTime.ParseExact("1900-01-01 " & txtStartTime.Text, "yyyy-MM-dd HH:mm", Nothing)

                        Endtime = Convert.ToDateTime("1900-01-01 " & txtEndTime.Text).ToString

                        sDate = DateTime.ParseExact("1900-01-01 00:00", "yyyy-MM-dd HH:mm", Nothing)

                        StartSec = DateDiff(DateInterval.Second, sDate, StartTime)
                        EndSac = DateDiff(DateInterval.Second, sDate, Endtime)

                        txttotalhour.Text = ((EndSac - StartSec) / 60.0) / 60.0

                        lblbarcode.Text = "Barcode Operator: "

                    End If

                    'ElseIf lblbarcode.Text = "Barcode Total Hours: " Then

                    '    txttotalhour.Text = sBarcode

                    '    If CntrlPoint.Value = "1" Then
                    '        lblbarcode.Text = "Barcode Completed: "
                    '    End If

                ElseIf lblbarcode.Text = "Barcode Completed: " Then

                    'If sBarcode <> "OK" Then



                    'End If

                    'If sBarcode = "OK" Then

                    '    lblbarcode.Text = "Barcode Operator: "
                    '    Session("LabelScan") = "Barcode Operator: "

                    'End If

                ElseIf lblbarcode.Text = "Barcode Operator: " Then

                    txtOperator.Text = sBarcode

                    lblbarcode.Text = "Barcode Scrap Code: "

                ElseIf lblbarcode.Text = "Barcode Scrap Code: " Then

                    ddlscrapcode.SelectedIndex = ddlscrapcode.Items.IndexOf(ddlscrapcode.Items.FindByValue(UCase(sBarcode)))

                    lblbarcode.Text = "Barcode Scrapped: "

                ElseIf lblbarcode.Text = "Barcode Scrapped: " Then

                    txtQtyScrap.Text = FormatNumber(sBarcode, LenPointQty)

                    lblbarcode.Text = "Barcode Scrap Code: "

                    btnAddScrap_Click(sender, e)

                    'ddlscrapcode.SelectedIndex = ddlscrapcode.Items.IndexOf(ddlscrapcode.Items.FindByValue(""))

                ElseIf lblbarcode.Text = "Barcode Downtime Code: " Then

                    ddldowntime.SelectedIndex = ddldowntime.Items.IndexOf(ddldowntime.Items.FindByValue(UCase(sBarcode)))

                    lblbarcode.Text = "Barcode Downtime Start Time: "

                ElseIf lblbarcode.Text = "Barcode Downtime Start Time: " Then

                    DigitStartTimeDt.Value = CInt(DigitStartTimeDt.Value) + 1

                    If DigitStartTimeDt.Value = 1 Then

                        txtDTStartTime.Text = sBarcode + "0:00"

                        StartTime = DateTime.ParseExact("1900-01-01 " & txtDTStartTime.Text, "yyyy-MM-dd HH:mm", Nothing)

                        If txtDTStartTime.Text = "00:00" Then
                            Endtime = Convert.ToDateTime("1900-01-01 " & txtDTEndTime.Text).ToString
                        Else
                            Endtime = Convert.ToDateTime("1900-01-02 " & txtDTEndTime.Text).ToString
                        End If

                        sDate = DateTime.ParseExact("1900-01-01 00:00", "yyyy-MM-dd HH:mm", Nothing)

                        StartSec = DateDiff(DateInterval.Second, sDate, StartTime)
                        EndSac = DateDiff(DateInterval.Second, sDate, Endtime)

                        txttotalhrsDT.Text = ((EndSac - StartSec) / 60.0) / 60.0

                    ElseIf DigitStartTimeDt.Value = 2 Then

                        txtDTStartTime.Text = Left(txtDTStartTime.Text, 1) + sBarcode + ":00"

                        StartTime = DateTime.ParseExact("1900-01-01 " & txtDTStartTime.Text, "yyyy-MM-dd HH:mm", Nothing)

                        If txtDTStartTime.Text = "00:00" Then
                            Endtime = Convert.ToDateTime("1900-01-01 " & txtDTEndTime.Text).ToString
                        Else
                            Endtime = Convert.ToDateTime("1900-01-02 " & txtDTEndTime.Text).ToString
                        End If

                        sDate = DateTime.ParseExact("1900-01-01 00:00", "yyyy-MM-dd HH:mm", Nothing)


                        StartSec = DateDiff(DateInterval.Second, sDate, StartTime)
                        EndSac = DateDiff(DateInterval.Second, sDate, Endtime)

                        txttotalhrsDT.Text = ((EndSac - StartSec) / 60.0) / 60.0

                    ElseIf DigitStartTimeDt.Value = 3 Then

                        txtDTStartTime.Text = Left(txtDTStartTime.Text, 2) + ":" + sBarcode + "0"

                        StartTime = DateTime.ParseExact("1900-01-01 " & txtDTStartTime.Text, "yyyy-MM-dd HH:mm", Nothing)

                        If txtDTStartTime.Text = "00:00" Then
                            Endtime = Convert.ToDateTime("1900-01-01 " & txtDTEndTime.Text).ToString
                        Else
                            Endtime = Convert.ToDateTime("1900-01-02 " & txtDTEndTime.Text).ToString
                        End If

                        sDate = DateTime.ParseExact("1900-01-01 00:00", "yyyy-MM-dd HH:mm", Nothing)

                        StartSec = DateDiff(DateInterval.Second, sDate, StartTime)
                        EndSac = DateDiff(DateInterval.Second, sDate, Endtime)

                        txttotalhrsDT.Text = ((EndSac - StartSec) / 60.0) / 60.0

                    ElseIf DigitStartTimeDt.Value = 4 Then

                        txtDTStartTime.Text = Left(txtDTStartTime.Text, 4) + sBarcode

                        StartTime = DateTime.ParseExact("1900-01-01 " & txtDTStartTime.Text, "yyyy-MM-dd HH:mm", Nothing)

                        If txtDTStartTime.Text = "00:00" Then
                            Endtime = Convert.ToDateTime("1900-01-01 " & txtDTEndTime.Text).ToString
                        Else
                            Endtime = Convert.ToDateTime("1900-01-02 " & txtDTEndTime.Text).ToString
                        End If

                        sDate = DateTime.ParseExact("1900-01-01 00:00", "yyyy-MM-dd HH:mm", Nothing)

                        StartSec = DateDiff(DateInterval.Second, sDate, StartTime)
                        EndSac = DateDiff(DateInterval.Second, sDate, Endtime)

                        txttotalhrsDT.Text = ((EndSac - StartSec) / 60.0) / 60.0

                        lblbarcode.Text = "Barcode Downtime End Time: "
                        Session("LabelScan") = "Barcode Downtime End Time: "


                    End If


                ElseIf lblbarcode.Text = "Barcode Downtime End Time: " Then

                    DigitEndTimeDt.Value = CInt(DigitEndTimeDt.Value) + 1

                    If DigitEndTimeDt.Value = 1 Then

                        txtDTEndTime.Text = sBarcode + "0:00"

                        StartTime = DateTime.ParseExact("1900-01-01 " & txtDTStartTime.Text, "yyyy-MM-dd HH:mm", Nothing)

                        Endtime = Convert.ToDateTime("1900-01-01 " & txtDTEndTime.Text).ToString

                        sDate = DateTime.ParseExact("1900-01-01 00:00", "yyyy-MM-dd HH:mm", Nothing)

                        StartSec = DateDiff(DateInterval.Second, sDate, StartTime)
                        EndSac = DateDiff(DateInterval.Second, sDate, Endtime)

                        txttotalhrsDT.Text = ((EndSac - StartSec) / 60.0) / 60.0

                    ElseIf DigitEndTimeDt.Value = 2 Then

                        txtDTEndTime.Text = Left(txtDTEndTime.Text, 1) + sBarcode + ":00"

                        StartTime = DateTime.ParseExact("1900-01-01 " & txtDTStartTime.Text, "yyyy-MM-dd HH:mm", Nothing)

                        Endtime = Convert.ToDateTime("1900-01-01 " & txtDTEndTime.Text).ToString

                        sDate = DateTime.ParseExact("1900-01-01 00:00", "yyyy-MM-dd HH:mm", Nothing)


                        StartSec = DateDiff(DateInterval.Second, sDate, StartTime)
                        EndSac = DateDiff(DateInterval.Second, sDate, Endtime)

                        txttotalhrsDT.Text = ((EndSac - StartSec) / 60.0) / 60.0

                    ElseIf DigitEndTimeDt.Value = 3 Then

                        txtDTEndTime.Text = Left(txtDTEndTime.Text, 2) + ":" + sBarcode + "0"

                        StartTime = DateTime.ParseExact("1900-01-01 " & txtDTStartTime.Text, "yyyy-MM-dd HH:mm", Nothing)

                        Endtime = Convert.ToDateTime("1900-01-01 " & txtDTEndTime.Text).ToString

                        sDate = DateTime.ParseExact("1900-01-01 00:00", "yyyy-MM-dd HH:mm", Nothing)

                        StartSec = DateDiff(DateInterval.Second, sDate, StartTime)
                        EndSac = DateDiff(DateInterval.Second, sDate, Endtime)

                        txttotalhrsDT.Text = ((EndSac - StartSec) / 60.0) / 60.0

                    ElseIf DigitEndTimeDt.Value = 4 Then

                        txtDTEndTime.Text = Left(txtDTEndTime.Text, 4) + sBarcode

                        StartTime = DateTime.ParseExact("1900-01-01 " & txtDTStartTime.Text, "yyyy-MM-dd HH:mm", Nothing)

                        Endtime = Convert.ToDateTime("1900-01-01 " & txtDTEndTime.Text).ToString

                        sDate = DateTime.ParseExact("1900-01-01 00:00", "yyyy-MM-dd HH:mm", Nothing)

                        StartSec = DateDiff(DateInterval.Second, sDate, StartTime)
                        EndSac = DateDiff(DateInterval.Second, sDate, Endtime)

                        txttotalhrsDT.Text = ((EndSac - StartSec) / 60.0) / 60.0

                        lblbarcode.Text = "Barcode Downtime Total Hours: "
                        Session("LabelScan") = "Barcode Downtime Total Hours: "


                    End If

                    'ElseIf lblbarcode.Text = "Barcode Downtime Total Hours: " Then

                    '    txttotalhrsDT.Text = sBarcode

                    '    StartTime = DateTime.ParseExact("1900-01-01 " & txtDTStartTime.Text, "yyyy-MM-dd HH:mm", Nothing)

                    '    Endtime = DateAdd(Hour, CDec(txttotalhrsDT.Text), StartTime)

                    '    txtDTEndTime.Text = Mid(Endtime, 12, 5)

                    '    'InsertDownTime()

                    '    lblbarcode.Text = "Barcode Downtime Code: "
                    '    Session.Remove("LabelScan")

                End If

            End If


        End If

        txtbarcode.Text = String.Empty

    End Sub

    Protected Sub btnprocess_Click(sender As Object, e As EventArgs) Handles btnprocess.Click

        Dim StartTimeSec As String = ""
        Dim EndTimeSec As String = ""
        Dim DefWhse As String = ""
        Dim UserCode As String = ""
        Dim Stat, MsgType, MsgErr, PromptButtons As String
        Dim StatPreProcess, MsgTypePreProcess, MsgErrPreProcess As String
        Dim doc As XmlDocument = New XmlDocument()
        Dim SelectedCount As Integer = 0
        Dim RowCount As Integer = 0

        DefWhse = GetDefWhse()
        UserCode = GetUserCode(Session("Username").ToString)
        Stat = "FALSE"
        MsgErr = ""
        MsgType = ""
        PromptButtons = ""
        StatPreProcess = "FALSE"
        MsgTypePreProcess = ""
        MsgErrPreProcess = ""

        If BflushError.Value <> "0" Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','" & BflushMessage.Value & "', 'error');", True)
            Exit Sub
        End If

        RowCount = PanelList5.Items.Count

        For Each item As ListViewItem In PanelList5.Items

            Dim chkMatchSelect As CheckBox = DirectCast(item.FindControl("chkMatchSelect"), CheckBox)

            If chkMatchSelect.Checked Then
                SelectedCount += 1
            End If

        Next


        If RowCount = SelectedCount Then

            Parms = ""
            Parms = "<Parameters><Parameter>" & Session("Username").ToString & "</Parameter>" &
                    "<Parameter>" & PSite.ToString & "</Parameter>" &
                    "<Parameter>" & lbljob.Text & "</Parameter>" &
                    "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                    "<Parameter>" & lbloper.Text & "</Parameter>" &
                    "<Parameter>" & 0 & "</Parameter>" &
                    "<Parameter>" & ddlmovetoloc.SelectedItem.Value & "</Parameter>" &
                    "<Parameter>" & Session("PSession").ToString & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "</Parameters>"

            oWS = New CNIService.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobTranTags", "PPCC_EX_ValidateUnpostPreProcessSp", Parms)

            doc.LoadXml(Parms)

            Dim k As Integer = 1

            For Each node As XmlNode In doc.DocumentElement

                If k = 9 Then
                    StatPreProcess = node.InnerText

                ElseIf k = 10 Then
                    MsgTypePreProcess = node.InnerText

                ElseIf k = 11 Then
                    MsgErrPreProcess = node.InnerText

                ElseIf k = 12 Then
                    PromptButtons = node.InnerText

                End If

                k += 1

            Next

            If StatPreProcess = "TRUE" And PromptButtons = "OK" Then

                MsgErrPreProcess = MsgErrPreProcess.Replace("'", "\'")
                MsgErrPreProcess = MsgErrPreProcess.Replace(vbLf, "<br />")


            ElseIf StatPreProcess = "FALSE" Then

                MsgErrPreProcess = MsgErrPreProcess.Replace("'", "\'")
                MsgErrPreProcess = MsgErrPreProcess.Replace(vbLf, "<br />")

                MsgTypePreProcess = "Error [" & MsgTypePreProcess & "]"

                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgTypePreProcess & "','" & MsgErrPreProcess & "', 'error');", True)

                Exit Sub

            End If


            If StatPreProcess = "TRUE" Then

                If PanelList1.Items.Count > 0 Then

                    Dim QtyComplete As Decimal

                    For Each item As ListViewItem In PanelList1.Items

                        QtyComplete = 0

                        Dim lblItemJobRowPointer As Label = DirectCast(item.FindControl("lblItemJobRowPointer"), Label)
                        Dim txtQtyComplete As TextBox = DirectCast(item.FindControl("txtQtyComplete"), TextBox)

                        Decimal.TryParse(txtQtyComplete.Text, QtyComplete)

                        Parms = ""
                        Parms = "<Parameters><Parameter>" & lblItemJobRowPointer.Text & "</Parameter>" &
                                "<Parameter>" & Session("PSession").ToString & "</Parameter>" &
                                "<Parameter>" & QtyComplete & "</Parameter>" &
                                "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                                "<Parameter>" & PSite.ToString & "</Parameter></Parameters>"

                        oWS = New CNIService.DOWebServiceSoapClient
                        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobTranTags", "PPCC_EX_UpdateQtyCompleteSp", Parms)

                    Next

                End If

                StartTimeSec = DateDiff(DateInterval.Second, Convert.ToDateTime("1900-01-01"), Convert.ToDateTime("1900-01-01 " & txtStartTime.Text)).ToString
                EndTimeSec = DateDiff(DateInterval.Second, Convert.ToDateTime("1900-01-01"), Convert.ToDateTime("1900-01-01 " & txtEndTime.Text)).ToString


                Parms = ""
                Parms = "<Parameters><Parameter>" & Session("Username").ToString & "</Parameter>" &
                        "<Parameter>" & PSite.ToString & "</Parameter>" &
                        "<Parameter>" & lbljob.Text & "</Parameter>" &
                        "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                        "<Parameter>" & ddltrantype.SelectedItem.Value & "</Parameter>" &
                        "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                        "<Parameter>" & lbloper.Text & "</Parameter>" &
                        "<Parameter>" & txttotalhour.Text & "</Parameter>" &
                        "<Parameter>" & StartTimeSec & "</Parameter>" &
                        "<Parameter>" & EndTimeSec & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & ddlemployee.SelectedItem.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & "R" & "</Parameter>" &
                        "<Parameter>" & DefWhse & "</Parameter>" &
                        "<Parameter>" & ddlmovetoloc.SelectedItem.Value & "</Parameter>" &
                        "<Parameter>" & UserCode & "</Parameter>" &
                        "<Parameter>" & 0 & "</Parameter>" &
                        "<Parameter>" & 0 & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & 0 & "</Parameter>" &
                        "<Parameter>" & 0 & "</Parameter>" &
                        "<Parameter>" & 0 & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & 0 & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & "J" & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & txtwc.Text & "</Parameter>" &
                        "<Parameter>" & 0 & "</Parameter>" &
                        "<Parameter>" & 0 & "</Parameter>" &
                        "<Parameter>" & 0 & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & ddlSchedulingShift.SelectedItem.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & ddlResource.SelectedItem.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & txtProdLine.Text & "</Parameter>" &
                        "<Parameter>" & "P" & "</Parameter>" &
                        "<Parameter>" & txtOperator.Text & "</Parameter>" &
                        "<Parameter>" & IIf(ChkQAQC.Checked = True, 1, 0) & "</Parameter>" &
                        "</Parameters>"

                oWS = New CNIService.DOWebServiceSoapClient
                oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobTranTags", "ppcc_ex_unpostedprocessSp", Parms)


                doc.LoadXml(Parms)

                Dim i As Integer = 1

                For Each node As XmlNode In doc.DocumentElement

                    If i = 43 Then
                        Stat = node.InnerText

                    ElseIf i = 44 Then
                        MsgType = node.InnerText

                    ElseIf i = 45 Then
                        MsgErr = node.InnerText

                    End If

                    i += 1

                Next

                If Stat = "TRUE" Then

                    '    Dim PostCompleteVar As String = "0"
                    '    Dim PostNegativeInventoryVar As String = "0"
                    '    Dim StartJobVar As String = lbljob.Text
                    '    Dim EndJobVar As String = lbljob.Text
                    '    Dim StartSuffixVar As String = lblSuffix.Text
                    '    Dim EndSuffixVar As String = lblSuffix.Text
                    '    Dim StartTransDateVar As String = ""
                    '    Dim EndTransDateVar As String = ""
                    '    Dim StartEmpNumVar As String = ""
                    '    Dim EndEmpNumVar As String = ""
                    '    Dim StartDeptVar As String = ""
                    '    Dim EndDeptVar As String = ""
                    '    Dim StartShiftVar As String = ""
                    '    Dim EndShiftVar As String = ""
                    '    Dim StartUserCodeVar As String = ""
                    '    Dim EndUserCodeVar As String = ""
                    '    Dim EmployeeTypeVar As String = "H S N"
                    '    Dim FormCurWhse As String = PWhse.ToString
                    '    Dim BlankVar1 As String = ""
                    '    Dim BlankVar2 As String = ""
                    '    Dim BlankVar3 As String = ""
                    '    Dim WCVar As String = ""

                    '    StartUserCodeVar = GetUserInitial(Session("Username").ToString)
                    '    EndUserCodeVar = StartUserCodeVar


                    '    Parms = ""
                    '    Parms = "<Parameters><Parameter>" & PostCompleteVar & "</Parameter>" &
                    '            "<Parameter>" & PostNegativeInventoryVar & "</Parameter>" &
                    '            "<Parameter>" & StartJobVar & "</Parameter>" &
                    '            "<Parameter>" & EndJobVar & "</Parameter>" &
                    '            "<Parameter>" & StartSuffixVar & "</Parameter>" &
                    '            "<Parameter>" & EndSuffixVar & "</Parameter>" &
                    '            "<Parameter>" & StartTransDateVar & "</Parameter>" &
                    '            "<Parameter>" & EndTransDateVar & "</Parameter>" &
                    '            "<Parameter>" & StartEmpNumVar & "</Parameter>" &
                    '            "<Parameter>" & EndEmpNumVar & "</Parameter>" &
                    '            "<Parameter>" & StartDeptVar & "</Parameter>" &
                    '            "<Parameter>" & EndDeptVar & "</Parameter>" &
                    '            "<Parameter>" & StartShiftVar & "</Parameter>" &
                    '            "<Parameter>" & EndShiftVar & "</Parameter>" &
                    '            "<Parameter>" & StartUserCodeVar & "</Parameter>" &
                    '            "<Parameter>" & EndUserCodeVar & "</Parameter>" &
                    '            "<Parameter>" & EmployeeTypeVar & "</Parameter>" &
                    '            "<Parameter>" & FormCurWhse & "</Parameter>" &
                    '            "<Parameter>" & DBNull.Value & "</Parameter>" &
                    '            "<Parameter>" & DBNull.Value & "</Parameter>" &
                    '            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    '            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    '            "<Parameter>" & DBNull.Value & "</Parameter>" &
                    '            "<Parameter>" & DBNull.Value & "</Parameter></Parameters>"

                    '    'Debug.WriteLine("JobJobP" & Parms)
                    '    Dim res As Object
                    '    res = New Object
                    '    oWS = New CNIService.DOWebServiceSoapClient
                    '    res = oWS.CallMethod(Session("Token").ToString, "SLJobTrans", "JobJobP", Parms)

                    '    If res = "0" Then

                    '        doc.LoadXml(Parms)

                    '        Dim j As Integer = 1
                    '        Dim MsgPost As String = ""

                    '        For Each node As XmlNode In doc.DocumentElement

                    '            If j = 21 Then
                    '                MsgPost = node.InnerText

                    '            End If

                    '            j += 1

                    '        Next

                    '        Parms = ""
                    '        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                    '                "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                    '                "<Parameter>" & PSite.ToString & "</Parameter></Parameters>"
                    '        oWS = New CNIService.DOWebServiceSoapClient
                    '        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobTranTags", "PPCC_Ex_PostedSetupJobTran2Sp", Parms)

                    Clear()

                    'MsgPost = MsgPost.Replace("'", "\'")
                    'MsgPost = MsgPost.Replace(vbLf, "<br />")

                    'MsgPost = MsgErrPreProcess + "<br />" + MsgPost

                    MsgErr = MsgErr.Replace("'", "\'")
                    MsgErr = MsgErr.Replace(vbLf, "<br />")

                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Success','" & MsgErr & "', 'success');", True)

                    '    Else

                    '        doc.LoadXml(Parms)

                    '        Dim j As Integer = 1
                    '        Dim MsgPost As String = ""

                    '        For Each node As XmlNode In doc.DocumentElement

                    '            If j = 21 Then
                    '                MsgPost = node.InnerText

                    '            End If

                    '            j += 1

                    '        Next

                    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','" & MsgPost & "', 'error');", True)

                    '    End If

                Else

                    MsgErr = MsgErr.Replace("'", "\'")
                    MsgErr = MsgErr.Replace(vbLf, "<br />")

                    MsgType = "Error [" & MsgType & "]"

                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)


                End If

            End If

        Else

            MsgType = "Error [STD]"

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','Target Qty must match Selected Qty', 'error');", True)

        End If


    End Sub


    Protected Sub btnreset_Click(sender As Object, e As EventArgs) Handles btnreset.Click

        Dim Stat, MsgErr, MsgType As String
        Stat = "FALSE"
        MsgErr = ""
        MsgType = ""

        Parms = ""
        Parms = "<Parameters><Parameter>" & Session("Username").ToString & "</Parameter>" &
                "<Parameter>" & PSite.ToString & "</Parameter>" &
                "<Parameter>" & lbljob.Text & "</Parameter>" &
                "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                "<Parameter>" & lbloper.Text & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & 0 & "</Parameter>" &
                "<Parameter>" & 0 & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & 0 & "</Parameter>" &
                "<Parameter>" & 0 & "</Parameter>" &
                "<Parameter>" & 0 & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & 0 & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & 0 & "</Parameter>" &
                "<Parameter>" & 0 & "</Parameter>" &
                "<Parameter>" & 0 & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & Session("PSession").ToString & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & "R" & "</Parameter>" &
                "<Parameter>" & txtOperator.Text & "</Parameter>" &
                "<Parameter>" & IIf(ChkQAQC.Checked = True, 1, 0) & "</Parameter>" &
                "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobTranTags", "ppcc_ex_unpostedprocessSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)

        Dim i As Integer = 1

        For Each node As XmlNode In doc.DocumentElement

            If i = 43 Then
                Stat = node.InnerText

            ElseIf i = 44 Then
                MsgType = node.InnerText

            ElseIf i = 45 Then
                MsgErr = node.InnerText

            End If

            i += 1

        Next

        If Stat = "TRUE" Then

            Session("PSession") = NewSessionID()

            Response.Redirect("Unposted.aspx")

        Else

            MsgErr = MsgErr.Replace("'", "\'")
            MsgErr = MsgErr.Replace(vbLf, "<br />")

            MsgType = "Error [" & MsgType & "]"

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)


        End If

    End Sub

    Sub InsertDownTime()

        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                                "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                                "<Parameter>" & DBNull.Value & "</Parameter>" &
                                "<Parameter>" & lbljob.Text & "</Parameter>" &
                                "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                                "<Parameter>" & lbloper.Text & "</Parameter>" &
                                "<Parameter>" & PSite.ToString() & "</Parameter>" &
                                "<Parameter>" & "D" & "</Parameter>" &
                                "<Parameter>" & ddldowntime.SelectedItem.Value & "</Parameter>" &
                                "<Parameter>" & DBNull.Value & "</Parameter>" &
                                "<Parameter>" & DBNull.Value & "</Parameter>" &
                                "<Parameter>" & txtDTStartTime.Text & "</Parameter>" &
                                "<Parameter>" & txtDTEndTime.Text & "</Parameter>" &
                                "<Parameter>" & IIf(chkcancel.Checked = True, 1, 0) & "</Parameter>" &
                                "<Parameter>" & DBNull.Value & "</Parameter>" &
                                "<Parameter>" & txtCauseBy.Text & "</Parameter>" &
                                "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobTranTags", "PPCC_Ex_InsertJobTranTagSp", Parms)
        BindDownTime()

        ddldowntime.SelectedIndex = ddldowntime.Items.IndexOf(ddldowntime.Items.FindByValue(""))
        txtDTStartTime.Text = "00:00"
        txtDTEndTime.Text = "00:00"
        txttotalhrsDT.Text = "0"
        DigitStartTime.Value = "0"
        DigitEndTime.Value = "0"
        DigitStartTimeDt.Value = "0"
        DigitEndTimeDt.Value = "0"
        Session.Remove("LabelScan")

    End Sub

    Sub InsertScrap()

        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & lbljob.Text & "</Parameter>" &
                "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                "<Parameter>" & lbloper.Text & "</Parameter>" &
                "<Parameter>" & PSite.ToString() & "</Parameter>" &
                "<Parameter>" & "S" & "</Parameter>" &
                "<Parameter>" & ddlscrapcode.SelectedItem.Value & "</Parameter>" &
                "<Parameter>" & ddlItemScrapped.SelectedItem.Value & "</Parameter>" &
                "<Parameter>" & txtQtyScrap.Text & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & IIf(chkcancel.Checked = True, 1, 0) & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobTranTags", "PPCC_Ex_InsertJobTranTagSp", Parms)

        BindScrap()
        GetBackflushLots(lbltotalscrap.Text, False)

        ddlscrapcode.SelectedIndex = ddlscrapcode.Items.IndexOf(ddlscrapcode.Items.FindByValue(""))
        ddlItemScrapped.SelectedIndex = ddlItemScrapped.Items.IndexOf(ddlItemScrapped.Items.FindByValue(""))
        txtQtyScrap.Text = FormatNumber(0, LenPointQty)

        Session.Remove("LabelScan")

    End Sub

    'Protected Sub txtscrapqty_TextChanged(sender As Object, e As EventArgs) Handles txtscrapqty.TextChanged

    '    If ddlscrapcode.SelectedItem.Value <> "" Then

    '        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
    '                        "<Parameter>" & Session("Username").ToString & "</Parameter>" &
    '                        "<Parameter>" & DBNull.Value & "</Parameter>" &
    '                        "<Parameter>" & lbljob.Text & "</Parameter>" &
    '                        "<Parameter>" & lblSuffix.Text & "</Parameter>" &
    '                        "<Parameter>" & lbloper.Text & "</Parameter>" &
    '                        "<Parameter>" & PSite.ToString() & "</Parameter>" &
    '                        "<Parameter>" & "S" & "</Parameter>" &
    '                        "<Parameter>" & ddlscrapcode.SelectedItem.Value & "</Parameter>" &
    '                        "<Parameter>" & ddlitem.SelectedItem.Value & "</Parameter>" &
    '                        "<Parameter>" & txtscrapqty.Text & "</Parameter>" &
    '                        "<Parameter>" & DBNull.Value & "</Parameter>" &
    '                        "<Parameter>" & DBNull.Value & "</Parameter>" &
    '                        "</Parameters>"

    '        oWS = New CNIService.DOWebServiceSoapClient
    '        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobTranTags", "PPCC_Ex_InsertJobTranTagSp", Parms)

    '        BindScrap()

    '    End If

    'End Sub

    'Protected Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound

    '    If e.Row.RowType = DataControlRowType.DataRow Then

    '        Dim lblitemComplete As Label = CType(e.Row.FindControl("lblitemComplete"), Label)
    '        Dim ddlLot As DropDownList = CType(e.Row.FindControl("ddlLot"), DropDownList)
    '        'Dim total As Double = Convert.ToDecimal(lblqtyscan.Text)

    '        Dim Qty As Decimal = CDec(lblitemComplete.Text)

    '        lblitemComplete.Text = FormatNumber(Qty, LenPointQty)

    '        'total = total - Convert.ToDecimal(lblitemComplete.Text)
    '        'lblqtyscan.Text = Convert.ToString(total)

    '        If PJobStockTran = "0" Then
    '            ddlLot.Attributes.Add("disabled", "disabled")
    '        Else

    '            ddlLot.Attributes.Add("disabled", "disabled")
    '            ddlLot.Items.Clear()

    '            oWS = New CNIService.DOWebServiceSoapClient

    '            ds = New DataSet

    '            ds = oWS.LoadDataSet(Session("Token").ToString, "SLPreassignedLots", "Lot", "RefType = 'J' and RefNum = '" & lbljob.Text & "' and RefLineSuf = '" & lblSuffix.Text & "'", "Lot", "", 0)

    '            For Each dRow As DataRow In ds.Tables(0).Rows
    '                ddlLot.Items.Add(New ListItem(dRow("Lot"), dRow("Lot")))

    '            Next

    '        End If

    '    End If

    'End Sub

    Protected Sub PanelList1_RowDataBound(sender As Object, e As ListViewItemEventArgs) Handles PanelList1.ItemDataBound

        If e.Item.ItemType = ListViewItemType.DataItem Then

            Dim lblItemJobRowPointer As Label = CType(e.Item.FindControl("lblItemJobRowPointer"), Label)
            Dim txtQtyComplete As TextBox = CType(e.Item.FindControl("txtQtyComplete"), TextBox)

            Dim QtyCompleted As Decimal = 0

            Decimal.TryParse(txtQtyComplete.Text, QtyCompleted)

            txtQtyComplete.Text = FormatNumber(QtyCompleted, LenPointQty)

            '20220214 ADD If ddlLot.Items.Count = 0 Then
            'If ddlLot.Items.Count = 0 Then

            '    ddlLot.Items.Insert(0, New ListItem("", ""))

            'End If

            'If PJobStockTran = "0" Then
            '    ddlLot.Attributes.Add("disabled", "disabled")
            'Else

            '    ddlLot.Attributes.Add("disabled", "disabled")
            '    ddlLot.Items.Clear()

            '    oWS = New CNIService.DOWebServiceSoapClient

            '    ds = New DataSet

            '    ds = oWS.LoadDataSet(Session("Token").ToString, "SLPreassignedLots", "Lot", "RefType = 'J' and RefNum = '" & lbljob.Text & "' and RefLineSuf = '" & lblSuffix.Text & "'", "Lot", "", 0)

            '    For Each dRow As DataRow In ds.Tables(0).Rows
            '        ddlLot.Items.Add(New ListItem(dRow("Lot"), dRow("Lot")))

            '    Next

            'End If

        End If

    End Sub

    Protected Sub PanelList4_RowDataBound(sender As Object, e As ListViewItemEventArgs) Handles PanelList4.ItemDataBound

        If e.Item.ItemType = ListViewItemType.DataItem Then

            Dim txtqtyReq As TextBox = CType(e.Item.FindControl("txtqtyReq"), TextBox)
            Dim lblQtyNeeded As Label = CType(e.Item.FindControl("lblQtyNeeded"), Label)
            Dim lblQtyOnHand As Label = CType(e.Item.FindControl("lblQtyOnHand"), Label)

            Dim QtyReq As Double = 0
            Dim QtyNeeded As Double = 0
            Dim QtyOnHand As Double = 0

            Double.TryParse(txtqtyReq.Text, QtyReq)
            Double.TryParse(lblQtyNeeded.Text, QtyNeeded)
            Double.TryParse(lblQtyOnHand.Text, QtyOnHand)

            txtqtyReq.Text = FormatNumber(QtyReq, LenPointQtyFormat)
            lblQtyNeeded.Text = FormatNumber(QtyNeeded, LenPointQtyFormat)
            lblQtyOnHand.Text = FormatNumber(QtyOnHand, LenPointQty)

        End If

    End Sub

    Protected Sub PanelList2_ItemCommand(ByVal sender As Object, ByVal e As ListViewCommandEventArgs) Handles PanelList2.ItemCommand

        If e.CommandName = "DeleteScrap" Then

            Dim RowPointer As String = e.CommandArgument

            Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                                "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                                "<Parameter>" & DBNull.Value & "</Parameter>" &
                                "<Parameter>" & lbljob.Text & "</Parameter>" &
                                "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                                "<Parameter>" & lbloper.Text & "</Parameter>" &
                                "<Parameter>" & PSite.ToString() & "</Parameter>" &
                                "<Parameter>" & "C" & "</Parameter>" &
                                "<Parameter>" & DBNull.Value & "</Parameter>" &
                                "<Parameter>" & DBNull.Value & "</Parameter>" &
                                "<Parameter>" & DBNull.Value & "</Parameter>" &
                                "<Parameter>" & DBNull.Value & "</Parameter>" &
                                "<Parameter>" & DBNull.Value & "</Parameter>" &
                                "<Parameter>" & IIf(chkcancel.Checked = True, 1, 0) & "</Parameter>" &
                                "<Parameter>" & RowPointer & "</Parameter>" &
                                "<Parameter>" & DBNull.Value & "</Parameter>" &
                                "</Parameters>"

            oWS = New CNIService.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobTranTags", "PPCC_Ex_InsertJobTranTagSp", Parms)
            BindScrap()

        End If

    End Sub

    Protected Sub PanelList3_ItemCommand(ByVal sender As Object, ByVal e As ListViewCommandEventArgs) Handles PanelList3.ItemCommand

        If e.CommandName = "DeleteDowntime" Then

            Dim RowPointer As String = e.CommandArgument

            Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                                "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                                "<Parameter>" & DBNull.Value & "</Parameter>" &
                                "<Parameter>" & lbljob.Text & "</Parameter>" &
                                "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                                "<Parameter>" & lbloper.Text & "</Parameter>" &
                                "<Parameter>" & PSite.ToString() & "</Parameter>" &
                                "<Parameter>" & "C" & "</Parameter>" &
                                "<Parameter>" & DBNull.Value & "</Parameter>" &
                                "<Parameter>" & DBNull.Value & "</Parameter>" &
                                "<Parameter>" & DBNull.Value & "</Parameter>" &
                                "<Parameter>" & DBNull.Value & "</Parameter>" &
                                "<Parameter>" & DBNull.Value & "</Parameter>" &
                                "<Parameter>" & IIf(chkcancel.Checked = True, 1, 0) & "</Parameter>" &
                                "<Parameter>" & RowPointer & "</Parameter>" &
                                "<Parameter>" & DBNull.Value & "</Parameter>" &
                                "</Parameters>"

            oWS = New CNIService.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobTranTags", "PPCC_Ex_InsertJobTranTagSp", Parms)
            BindDownTime()

        End If

    End Sub

    Protected Sub PanelList3_RowDataBound(sender As Object, e As ListViewItemEventArgs) Handles PanelList3.ItemDataBound

        If e.Item.ItemType = ListViewItemType.DataItem Then

            Dim lnkDeleteDowntime As LinkButton = CType(e.Item.FindControl("lnkDeleteDowntime"), LinkButton)
            ScriptManager1.RegisterPostBackControl(lnkDeleteDowntime)

            Dim lblAhrs As Label = CType(e.Item.FindControl("lblAhrs"), Label)

            Dim QtyAhrs As Double = 0
            Double.TryParse(lblAhrs.Text, QtyAhrs)

            lblAhrs.Text = Decimal.Round(QtyAhrs.ToString, LenPointQty, MidpointRounding.AwayFromZero)

        End If

    End Sub


    Protected Sub PanelList5_RowDataBound(sender As Object, e As ListViewItemEventArgs) Handles PanelList5.ItemDataBound

        If e.Item.ItemType = ListViewItemType.DataItem Then

            Dim lblTargetQty As Label = CType(e.Item.FindControl("lblTargetQty"), Label)
            Dim lblSelectedQty As Label = CType(e.Item.FindControl("lblSelectedQty"), Label)

            Dim TargetQty As Double = 0
            Dim SelectedQty As Double = 0

            Double.TryParse(lblTargetQty.Text, TargetQty)
            Double.TryParse(lblSelectedQty.Text, SelectedQty)

            lblTargetQty.Text = Decimal.Round(TargetQty.ToString, LenPointQtyFormat, MidpointRounding.AwayFromZero)
            lblSelectedQty.Text = Decimal.Round(SelectedQty.ToString, LenPointQtyFormat, MidpointRounding.AwayFromZero)

        End If

    End Sub

    Protected Sub PanelList2_RowDataBound(sender As Object, e As ListViewItemEventArgs) Handles PanelList2.ItemDataBound

        If e.Item.ItemType = ListViewItemType.DataItem Then

            Dim lnkDeleteScrap As LinkButton = CType(e.Item.FindControl("lnkDeleteScrap"), LinkButton)
            ScriptManager1.RegisterPostBackControl(lnkDeleteScrap)

            Dim lblQty As Label = CType(e.Item.FindControl("lblQty"), Label)

            Dim QtyTagScrap As Double = CDec(lblQty.Text)

            lblQty.Text = FormatNumber(QtyTagScrap, LenPointQty)



        End If

    End Sub


    Protected Sub btnAddScrap_Click(sender As Object, e As EventArgs) Handles btnAddScrap.Click

        If ddlscrapcode.SelectedItem.Value <> "" And ddlItemScrapped.SelectedItem.Value <> "" And txtQtyScrap.Text <> 0 Then

            InsertScrap()

        End If

    End Sub

    Protected Sub btnAddDowntime_Click(sender As Object, e As EventArgs) Handles btnAddDowntime.Click

        If txtDTStartTime.Text <> "" And txtDTEndTime.Text <> "" And txttotalhrsDT.Text <> "" And ddldowntime.SelectedItem.Value <> "" Then

            InsertDownTime()

        End If

    End Sub

    Sub GetBackflushLots(ByVal QtyScrapped As Decimal, ByVal Selected As Boolean)

        Dim QtyCompleted As Double = 0

        QtyCompleted = IIf(lblQtyCompleted.Text = "", 0, lblQtyCompleted.Text)

        'If PanelList1.Items.Count > 0 Then

        '    For Each item As ListViewItem In PanelList1.Items

        '        Dim txtQtyComplete As TextBox = DirectCast(item.FindControl("txtQtyComplete"), TextBox)

        '        QtyCompleted = QtyCompleted + Convert.ToDecimal(txtQtyComplete.Text)

        '    Next

        'Else

        '    QtyCompleted = 0

        'End If


        If Not Selected Then

            Parms = "<Parameters><Parameter>" & "1" & "</Parameter>" &
                        "<Parameter>" & "J" & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & lbljob.Text & "</Parameter>" &
                        "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                        "<Parameter>" & lbloper.Text & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                        "<Parameter>" & PWhse.ToString & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & QtyCompleted & "</Parameter>" &
                        "<Parameter>" & QtyScrapped & "</Parameter>" &
                        "<Parameter>" & ddlemployee.SelectedItem.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter>" & ddlemployee.SelectedItem.Value & "</Parameter>" &
                        "<Parameter>" & ParmSite.ToString & "</Parameter>" &
                        "<Parameter>" & Session("PSession").ToString & "</Parameter></Parameters>"

            oWS = New CNIService.DOWebServiceSoapClient
            res = oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_Backflushs", "PPCC_EX_CLM_LoadBackflushSp", Parms)


            If res <> "0" Then

                Dim doc As XmlDocument = New XmlDocument()
                doc.LoadXml(Parms)

                Dim j As Integer = 1

                For Each node As XmlNode In doc.DocumentElement

                    If j = 17 Then
                        BflushMessage.Value = node.InnerText
                        BflushError.Value = "1"
                    End If

                    j += 1

                Next

                BflushMessage.Value = BflushMessage.Value.Replace("'", "\'")
                BflushMessage.Value = BflushMessage.Value.Replace(vbLf, "<br />")

                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','" & BflushMessage.Value & "', 'error');", True)

                Exit Sub

            End If

            Filter = "UserID='" & Session("UserName").ToString & "' And Job= '" & lbljob.Text & "' And Suffix= '" & lblSuffix.Text & "' And QtyReq > 0 And SessionID='" & Session("PSession").ToString & "'"

            Dim List As String = "Selected, OperNum, Seq, Lot, Qty, UM, Item, ItemDesc, QtyOnHand, QtyNeeded, RowPointer, Loc, Whse, TransNum, TransSeq, EmpNum"
            oWS = New CNIService.DOWebServiceSoapClient
            ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Ex_Backflushs", List, Filter, "OperNum, Seq, Lot", "", 0)

            Dim dQty As Decimal = 0
            Dim dQtyNeeded As Decimal = 0
            Dim dQtyOnHand As Decimal = 0
            Dim i As Integer
            Dim MaterialSeq As Integer = 0
            Dim LastMaterialSeq As Integer = 0
            Dim iOperNum As Integer = 0
            Dim LastiOperNum As Integer = 0

            If ds.Tables(0).Rows.Count > 0 Then

                For i = 0 To ds.Tables(0).Rows.Count - 1
                    Integer.TryParse(ds.Tables(0).Rows(i)("Seq").ToString(), MaterialSeq)
                    Decimal.TryParse(ds.Tables(0).Rows(i)("QtyOnHand").ToString(), dQtyOnHand)
                    Integer.TryParse(ds.Tables(0).Rows(i)("OperNum").ToString(), iOperNum)

                    If MaterialSeq <> LastMaterialSeq Or iOperNum <> LastiOperNum Then
                        Decimal.TryParse(ds.Tables(0).Rows(i)("QtyNeeded").ToString(), dQtyNeeded)
                    End If

                    If dQtyOnHand < 0 Then
                        dQty = 0
                    ElseIf dQtyNeeded - dQtyOnHand > 0 Then
                        dQty = dQtyOnHand
                        dQtyNeeded = dQtyNeeded - dQtyOnHand
                    ElseIf dQtyNeeded - dQtyOnHand <= 0 Then
                        dQty = dQtyNeeded
                        dQtyNeeded = 0
                    End If

                    ds.Tables(0).Rows(i).Item(4) = dQty
                    LastMaterialSeq = MaterialSeq
                    LastiOperNum = iOperNum

                Next


                PanelList4.DataSource = ds
                PanelList4.DataBind()


                For Each item As ListViewItem In PanelList4.Items

                    Dim txtqtyReq As TextBox = DirectCast(item.FindControl("txtqtyReq"), TextBox)
                    Dim lblRowPointer As Label = DirectCast(item.FindControl("lblRowPointer"), Label)

                    Parms = ""
                    Parms = "<Parameters><Parameter>" & lblRowPointer.Text & "</Parameter>" &
                                    "<Parameter>" & Session("PSession").ToString & "</Parameter>" &
                                    "<Parameter>" & txtqtyReq.Text & "</Parameter>" &
                                    "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                                    "<Parameter>" & PSite.ToString & "</Parameter>" &
                                    "</Parameters>"

                    oWS = New CNIService.DOWebServiceSoapClient
                    oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_Backflushs", "PPCC_EX_BackflushUpdateQtySp", Parms)

                Next

            Else

                PanelList4.DataSource = Nothing
                PanelList4.DataBind()
                Exit Sub

            End If


            dt_match = New Data.DataTable

            With dt_match.Columns
                .Add("Matched", Type.GetType("System.Int32"))
                .Add("OperNum", Type.GetType("System.Int32"))
                .Add("Job", Type.GetType("System.String"))
                .Add("Seq", Type.GetType("System.Int32"))
                .Add("TargetQty", Type.GetType("System.Decimal"))
                .Add("SelectedQty", Type.GetType("System.Decimal"))
                .Add("IssueQty", Type.GetType("System.Decimal"))
            End With

        Else

            ds = New DataSet
            ds = GetBFLot(lbljob.Text, lblSuffix.Text, lbloper.Text)

            If ds.Tables(0).Rows.Count > 0 Then

                PanelList4.DataSource = ds
                PanelList4.DataBind()

            End If

            dt_match = New Data.DataTable

            With dt_match.Columns
                .Add("Matched", Type.GetType("System.Int32"))
                .Add("OperNum", Type.GetType("System.Int32"))
                .Add("Job", Type.GetType("System.String"))
                .Add("Seq", Type.GetType("System.Int32"))
                .Add("TargetQty", Type.GetType("System.Decimal"))
                .Add("SelectedQty", Type.GetType("System.Decimal"))
                .Add("IssueQty", Type.GetType("System.Decimal"))
            End With

        End If

        Dim sJob As String = lbljob.Text
        Dim sOperNum As String = "'"
        Dim sSeq As String = ""
        Dim sNeeded As String = ""
        Dim sRequired As String = ""
        Dim sSelect As String = ""
        Dim sLot As String = ""
        Dim sWhse As String = ""
        Dim sLoc As String = ""
        Dim sItem As String = ""
        Dim bClearSum As Boolean = False

        If PanelList4.Items.Count > 0 Then

            Call RefreshBflushLotsSum("", "", 0, 0, 0, 0, bClearSum, dt_match)

            For Each item As ListViewItem In PanelList4.Items

                Dim lblOperNum As Label = DirectCast(item.FindControl("lblOperNum"), Label)
                Dim lblSeq As Label = DirectCast(item.FindControl("lblSeq"), Label)
                Dim lblQtyNeeded As Label = DirectCast(item.FindControl("lblQtyNeeded"), Label)
                Dim txtqtyReq As TextBox = DirectCast(item.FindControl("txtqtyReq"), TextBox)
                Dim lblLot As Label = DirectCast(item.FindControl("lblLot"), Label)
                Dim chkSelect As CheckBox = DirectCast(item.FindControl("chkSelect"), CheckBox)
                Dim lblWhse As Label = DirectCast(item.FindControl("lblWhse"), Label)
                Dim lblLoc As Label = DirectCast(item.FindControl("lblLoc"), Label)
                Dim lblItem As Label = DirectCast(item.FindControl("lblItem"), Label)

                sOperNum = lblOperNum.Text
                sSeq = lblSeq.Text
                sNeeded = lblQtyNeeded.Text
                sRequired = txtqtyReq.Text
                sLot = lblLot.Text
                sSelect = IIf(chkSelect.Checked, "1", "0")
                sWhse = lblWhse.Text
                sLoc = lblLoc.Text
                sItem = lblItem.Text

                If sSelect = "1" Then
                    Parms = "<Parameters><Parameter>" & sWhse & "</Parameter>" &
                                "<Parameter>" & sLot & "</Parameter>" &
                                "<Parameter>" & "1" & "</Parameter>" &
                                "<Parameter>" & sItem & "</Parameter>" &
                                "<Parameter>" & sLoc & "</Parameter>" &
                                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                                "<Parameter>" & lbljob.Text & "</Parameter>" &
                                "<Parameter>" & lblSuffix.Text & "</Parameter></Parameters>"

                    oWS = New CNIService.DOWebServiceSoapClient
                    oWS.CallMethod(Session("Token").ToString, "SLJobtMats", "BflushLotValSp", Parms)

                End If

                Call RefreshBflushLotsSum(sJob, sOperNum, sSeq, sNeeded, sRequired, sSelect, bClearSum, dt_match)
                bClearSum = False

            Next

            'For Each Row As GridViewRow In GridView3.Rows
            '    Dim OperNum As String = Row.Cells(1).Text
            '    Dim Seq As String = Row.Cells(2).Text
            '    Dim Needed As String = Row.Cells(9).Text
            '    Dim txtqtyReq As TextBox = DirectCast(Row.FindControl("txtqtyReq"), TextBox)
            '    Dim Lot As String = Row.Cells(3).Text
            '    Dim chkSelect As CheckBox = DirectCast(Row.FindControl("chkSelect"), CheckBox)
            '    Dim Whse As String = Row.Cells(14).Text
            '    Dim Location As String = Row.Cells(13).Text
            '    Dim Item As String = Row.Cells(6).Text

            '    sOperNum = OperNum.ToString
            '    sSeq = Seq.ToString
            '    sNeeded = Needed.ToString
            '    sRequired = txtqtyReq.Text
            '    sLot = Lot.ToString
            '    sSelect = IIf(chkSelect.Checked, "1", "0")
            '    sWhse = Whse.ToString
            '    sLoc = Location.ToString
            '    sItem = Item.ToString

            '    If sSelect = "1" Then
            '        Parms = "<Parameters><Parameter>" & sWhse & "</Parameter>" &
            '                "<Parameter>" & sLot & "</Parameter>" &
            '                "<Parameter>" & "1" & "</Parameter>" &
            '                "<Parameter>" & sItem & "</Parameter>" &
            '                "<Parameter>" & sLoc & "</Parameter>" &
            '                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
            '                "<Parameter>" & lbljob.Text & "</Parameter>" &
            '                "<Parameter>" & lblSuffix.Text & "</Parameter></Parameters>"

            '        oWS = New CNIService.DOWebServiceSoapClient
            '        oWS.CallMethod(Session("Token").ToString, "SLJobtMats", "BflushLotValSp", Parms)

            '    End If

            '    Call RefreshBflushLotsSum(sJob, sOperNum, sSeq, sNeeded, sRequired, sSelect, bClearSum, dt_match)
            '    bClearSum = False
            'Next

            'GridView4.DataSource = dt_match
            'GridView4.DataBind()

            PanelList5.DataSource = dt_match
            PanelList5.DataBind()

        End If

    End Sub

    Sub RefreshBflushLotsSum(ByVal sJob As String, ByVal sOperNum As String, ByVal iSeq As Integer, ByVal dNeeded As Decimal,
                             ByVal dRequired As Decimal, ByVal iSelect As Integer, ByVal bClearSum As Boolean, ByRef dt_match As DataTable)

        Dim sSumJob As String = ""
        Dim sSumOperNum As String = ""
        Dim iSumSeq As Integer = 0
        Dim dQtyOnHand As Decimal = 0D
        Dim dSumRequired As Decimal = 0D
        Dim dSumNeeded As Decimal = 0D
        Dim bExists As Boolean
        Dim iRowCount As Integer = 0

        bExists = False

        iRowCount = dt_match.Rows.Count

        If sJob = "" Or sJob = String.Empty Then

            For i As Integer = 0 To iRowCount - 1
                dt_match.Rows(i)("Matched") = "0"
                dt_match.Rows(i)("SelectedQty") = "0"
            Next

        End If

        If sJob = "" Or sJob = String.Empty Then
            Exit Sub
        End If

        For i As Integer = 0 To iRowCount - 1

            sSumJob = dt_match.Rows(i)("Job")

            If sSumJob = "" Or sSumJob = String.Empty Then
                Exit Sub
            End If

            sSumOperNum = dt_match.Rows(i)("OperNum")
            Integer.TryParse(dt_match.Rows(i)("Seq"), iSumSeq)
            Decimal.TryParse(dt_match.Rows(i)("TargetQty"), dSumNeeded)

            If dt_match.Rows(i)("SelectedQty").ToString = "" Then
                dSumRequired = 0
            Else
                Decimal.TryParse(dt_match.Rows(i)("SelectedQty"), dSumRequired)
            End If

            If sSumJob = sJob And sSumOperNum = sOperNum And iSumSeq = iSeq Then

                If iSelect = 1 Then
                    dSumRequired = dSumRequired + dRequired
                    dt_match.Rows(i)("SelectedQty") = dSumRequired
                End If

                If dSumNeeded = dSumRequired Then
                    dt_match.Rows(i)("Matched") = "1"
                Else
                    dt_match.Rows(i)("Matched") = "0"
                End If

                bExists = True

                Exit For

            End If
        Next

        If Not bExists Then

            Dim row As Data.DataRow
            row = dt_match.NewRow
            row("Matched") = "0"
            row("Job") = sJob
            row("OperNum") = sOperNum
            row("Seq") = CStr(iSeq)
            row("TargetQty") = CStr(dNeeded)
            row("SelectedQty") = "0"

            If iSelect = 1 Then
                row("SelectedQty") = CStr(dRequired)
                If dNeeded = dRequired Then
                    row("Matched") = "1"
                Else
                    row("Matched") = "0"
                End If
            End If

            dt_match.Rows.Add(row)

            'Session("dt_match") = dt_match

        End If

    End Sub

    Protected Sub txtqtyReq_TextChanged(ByVal sender As Object, ByVal e As EventArgs)

        For Each item As ListViewItem In PanelList4.Items

            'If row.RowType = DataControlRowType.DataRow Then

            Dim txtqtyReq As TextBox = DirectCast(item.FindControl("txtqtyReq"), TextBox)
            Dim lblRowPointer As Label = DirectCast(item.FindControl("lblRowPointer"), Label)

            If Not String.IsNullOrEmpty(txtqtyReq.Text) Then

                If CDec(txtqtyReq.Text) > 0 Then

                    Parms = "<Parameters><Parameter>" & lblRowPointer.Text & "</Parameter>" &
                            "<Parameter>" & txtqtyReq.Text & "</Parameter>" &
                            "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                            "<Parameter>" & PSite.ToString & "</Parameter></Parameters>"


                    oWS = New CNIService.DOWebServiceSoapClient
                    oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_Backflushs", "PPCC_EX_UpdateQtyReqBackflushSp", Parms)

                End If

                ds = New DataSet

                ds = GetBFLot(lbljob.Text, lblSuffix.Text, lbloper.Text)


                If ds.Tables(0).Rows.Count > 0 Then
                    GetBackflushLots(lbltotalscrap.Text, True)
                End If

            End If

            'End If

        Next

    End Sub

    Protected Sub txtQtyComplete_TextChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim QtyComplete As Decimal = 0

        For Each item As ListViewItem In PanelList1.Items

            Dim txtQtyComplete As TextBox = DirectCast(item.FindControl("txtQtyComplete"), TextBox)

            If Not String.IsNullOrEmpty(txtQtyComplete.Text) Then

                If CDec(txtQtyComplete.Text) > 0 Then

                    QtyComplete = QtyComplete + txtQtyComplete.Text

                End If

            End If

        Next

        lblQtyCompleted.Text = QtyComplete

        If lblQtyCompleted.Text <> "" Then

            Parms = "<Parameters><Parameter>" & "1" & "</Parameter>" &
                        "<Parameter>" & "J" & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & lbljob.Text & "</Parameter>" &
                        "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                        "<Parameter>" & lbloper.Text & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                        "<Parameter>" & PWhse.ToString & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & lblQtyCompleted.Text & "</Parameter>" &
                        "<Parameter>" & lbltotalscrap.Text & "</Parameter>" &
                        "<Parameter>" & ddlemployee.SelectedItem.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter>" & ddlemployee.SelectedItem.Value & "</Parameter>" &
                        "<Parameter>" & ParmSite.ToString & "</Parameter>" &
                        "<Parameter>" & Session("PSession").ToString & "</Parameter></Parameters>"

            oWS = New CNIService.DOWebServiceSoapClient
            res = oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_Backflushs", "PPCC_EX_CLM_LoadBackflushSp", Parms)


            If res <> "0" Then

                Dim doc As XmlDocument = New XmlDocument()
                doc.LoadXml(Parms)

                Dim j As Integer = 1

                For Each node As XmlNode In doc.DocumentElement

                    If j = 17 Then
                        BflushMessage.Value = node.InnerText
                        BflushError.Value = "1"
                    End If

                    j += 1

                Next

                BflushMessage.Value = BflushMessage.Value.Replace("'", "\'")
                BflushMessage.Value = BflushMessage.Value.Replace(vbLf, "<br />")

                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','" & BflushMessage.Value & "', 'error');", True)

                Exit Sub

            End If

            ds = New DataSet

            ds = GetBFLot(lbljob.Text, lblSuffix.Text, lbloper.Text)

            Dim dQty As Decimal = 0
            Dim dQtyNeeded As Decimal = 0
            Dim dQtyOnHand As Decimal = 0
            Dim i As Integer
            Dim MaterialSeq As Integer = 0
            Dim LastMaterialSeq As Integer = 0
            Dim iOperNum As Integer = 0
            Dim LastiOperNum As Integer = 0

            If ds.Tables(0).Rows.Count > 0 Then

                For i = 0 To ds.Tables(0).Rows.Count - 1
                    Integer.TryParse(ds.Tables(0).Rows(i)("Seq").ToString(), MaterialSeq)
                    Decimal.TryParse(ds.Tables(0).Rows(i)("QtyOnHand").ToString(), dQtyOnHand)
                    Integer.TryParse(ds.Tables(0).Rows(i)("OperNum").ToString(), iOperNum)

                    If MaterialSeq <> LastMaterialSeq Or iOperNum <> LastiOperNum Then
                        Decimal.TryParse(ds.Tables(0).Rows(i)("QtyNeeded").ToString(), dQtyNeeded)
                    End If

                    If dQtyOnHand < 0 Then
                        dQty = 0
                    ElseIf dQtyNeeded - dQtyOnHand > 0 Then
                        dQty = dQtyOnHand
                        dQtyNeeded = dQtyNeeded - dQtyOnHand
                    ElseIf dQtyNeeded - dQtyOnHand <= 0 Then
                        dQty = dQtyNeeded
                        dQtyNeeded = 0
                    End If

                    ds.Tables(0).Rows(i).Item(4) = dQty
                    LastMaterialSeq = MaterialSeq
                    LastiOperNum = iOperNum

                Next


                PanelList4.DataSource = ds
                PanelList4.DataBind()


                For Each item As ListViewItem In PanelList4.Items

                    Dim txtqtyReq As TextBox = DirectCast(item.FindControl("txtqtyReq"), TextBox)
                    Dim lblRowPointer As Label = DirectCast(item.FindControl("lblRowPointer"), Label)

                    Parms = ""
                    Parms = "<Parameters><Parameter>" & lblRowPointer.Text & "</Parameter>" &
                                    "<Parameter>" & Session("PSession").ToString & "</Parameter>" &
                                    "<Parameter>" & txtqtyReq.Text & "</Parameter>" &
                                    "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                                    "<Parameter>" & PSite.ToString & "</Parameter>" &
                                    "</Parameters>"

                    oWS = New CNIService.DOWebServiceSoapClient
                    oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_Backflushs", "PPCC_EX_BackflushUpdateQtySp", Parms)

                Next

            Else

                PanelList4.DataSource = Nothing
                PanelList4.DataBind()
                Exit Sub

            End If

            dt_match = New Data.DataTable

            With dt_match.Columns
                .Add("Matched", Type.GetType("System.Int32"))
                .Add("OperNum", Type.GetType("System.Int32"))
                .Add("Job", Type.GetType("System.String"))
                .Add("Seq", Type.GetType("System.Int32"))
                .Add("TargetQty", Type.GetType("System.Decimal"))
                .Add("SelectedQty", Type.GetType("System.Decimal"))
                .Add("IssueQty", Type.GetType("System.Decimal"))
            End With

            ds = New DataSet

            ds = GetBFLot(lbljob.Text, lblSuffix.Text, lbloper.Text)

            If ds.Tables(0).Rows.Count > 0 Then
                PanelList4.DataSource = ds
                PanelList4.DataBind()
            End If

            Dim sJob As String = lbljob.Text
            Dim sOperNum As String = "'"
            Dim sSeq As String = ""
            Dim sNeeded As String = ""
            Dim sRequired As String = ""
            Dim sSelect As String = ""
            Dim sLot As String = ""
            Dim sWhse As String = ""
            Dim sLoc As String = ""
            Dim sItem As String = ""
            Dim bClearSum As Boolean = False

            If PanelList4.Items.Count > 0 Then

                Call RefreshBflushLotsSum("", "", 0, 0, 0, 0, bClearSum, dt_match)

                For Each item As ListViewItem In PanelList4.Items

                    Dim lblOperNum As Label = DirectCast(item.FindControl("lblOperNum"), Label)
                    Dim lblSeq As Label = DirectCast(item.FindControl("lblSeq"), Label)
                    Dim lblQtyNeeded As Label = DirectCast(item.FindControl("lblQtyNeeded"), Label)
                    Dim txtqtyReq As TextBox = DirectCast(item.FindControl("txtqtyReq"), TextBox)
                    Dim lblLot As Label = DirectCast(item.FindControl("lblLot"), Label)
                    Dim chkSelect As CheckBox = DirectCast(item.FindControl("chkSelect"), CheckBox)
                    Dim lblWhse As Label = DirectCast(item.FindControl("lblWhse"), Label)
                    Dim lblLoc As Label = DirectCast(item.FindControl("lblLoc"), Label)
                    Dim lblItem As Label = DirectCast(item.FindControl("lblItem"), Label)

                    sOperNum = lblOperNum.Text
                    sSeq = lblSeq.Text
                    sNeeded = lblQtyNeeded.Text
                    sRequired = txtqtyReq.Text
                    sLot = lblLot.Text
                    sSelect = IIf(chkSelect.Checked, "1", "0")
                    sWhse = lblWhse.Text
                    sLoc = lblLoc.Text
                    sItem = lblItem.Text

                    If sSelect = "1" Then
                        Parms = "<Parameters><Parameter>" & sWhse & "</Parameter>" &
                                    "<Parameter>" & sLot & "</Parameter>" &
                                    "<Parameter>" & "1" & "</Parameter>" &
                                    "<Parameter>" & sItem & "</Parameter>" &
                                    "<Parameter>" & sLoc & "</Parameter>" &
                                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                                    "<Parameter>" & lbljob.Text & "</Parameter>" &
                                    "<Parameter>" & lblSuffix.Text & "</Parameter></Parameters>"

                        oWS = New CNIService.DOWebServiceSoapClient
                        oWS.CallMethod(Session("Token").ToString, "SLJobtMats", "BflushLotValSp", Parms)

                    End If

                    Call RefreshBflushLotsSum(sJob, sOperNum, sSeq, sNeeded, sRequired, sSelect, bClearSum, dt_match)
                    bClearSum = False

                Next

                PanelList5.DataSource = dt_match
                PanelList5.DataBind()

            End If

        End If

    End Sub

    Protected Sub ddlemployee_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlemployee.SelectedIndexChanged

        If ddlemployee.SelectedItem.Value <> "" Then
            lblbarcode.Text = "Barcode Resource: "
        Else
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','#220 : กรุณาสแกน หรือเลือก Employee ก่อน', 'error');", True)
        End If

    End Sub

    Protected Sub ddlResource_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlResource.SelectedIndexChanged

        If ddlResource.SelectedItem.Value <> "" Then
            'lblbarcode.Text = "Barcode Scheduling Shift: "
            lblbarcode.Text = "Barcode Start Time: "

            GetSchedulingShift()
        Else
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','#221 : กรุณาสแกน หรือเลือก Resource ก่อน', 'error');", True)
        End If

    End Sub

    Protected Sub ddlSchedulingShift_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSchedulingShift.SelectedIndexChanged

        If ddlSchedulingShift.SelectedItem.Value <> "" Then

            'lblbarcode.Text = "Barcode Start Time: "

            'If Left(ddlSchedulingShift.SelectedItem.Value, 1) = "D" Then
            '    rdDayNight.SelectedValue = "D"
            'Else
            '    rdDayNight.SelectedValue = "N"
            'End If

        Else

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','#222 : กรุณาสแกน หรือเลือก Scheduling Shift ก่อน', 'error');", True)

        End If


    End Sub

    Protected Sub txtStartTime_TextChanged(sender As Object, e As EventArgs) Handles txtStartTime.TextChanged

        If txtStartTime.Text <> "" Then
            lblbarcode.Text = "Barcode End Time: "
            Session("LabelScan") = "Barcode End Time: "
        End If

    End Sub

    Protected Sub txtEndTime_TextChanged(sender As Object, e As EventArgs) Handles txtEndTime.TextChanged

        If txtStartTime.Text <> "" Then
            lblbarcode.Text = "Barcode Operator: "
        End If

    End Sub

    Protected Sub txtOperator_TextChanged(sender As Object, e As EventArgs) Handles txtOperator.TextChanged

        If txtStartTime.Text <> "" Then
            lblbarcode.Text = "Barcode Scrap Code: "
        End If

    End Sub

    Protected Sub txtDTStartTime_TextChanged(sender As Object, e As EventArgs) Handles txtDTStartTime.TextChanged

        If txtDTStartTime.Text <> "" Then
            Dim StartTime As DateTime
            Dim Endtime As DateTime
            Dim TotalHours As Decimal = 0
            Dim StartSec As Integer = 0
            Dim EndSac As Integer = 0
            Dim sDate As DateTime

            StartTime = DateTime.ParseExact("1900-01-01 " & txtDTStartTime.Text, "yyyy-MM-dd HH:mm", Nothing)

            If txtDTStartTime.Text = "00:00" Then
                Endtime = Convert.ToDateTime("1900-01-01 " & txtDTEndTime.Text).ToString
            Else
                Endtime = Convert.ToDateTime("1900-01-02 " & txtDTEndTime.Text).ToString
            End If

            sDate = DateTime.ParseExact("1900-01-01 00:00", "yyyy-MM-dd HH:mm", Nothing)

            StartSec = DateDiff(DateInterval.Second, sDate, StartTime)
            EndSac = DateDiff(DateInterval.Second, sDate, Endtime)

            txttotalhrsDT.Text = ((EndSac - StartSec) / 60.0) / 60.0

            lblbarcode.Text = "Barcode Downtime End Time: "
            Session("LabelScan") = "Barcode Downtime End Time: "

        End If

    End Sub

    Protected Sub txtDTEndTime_TextChanged(sender As Object, e As EventArgs) Handles txtDTEndTime.TextChanged

        If txtDTEndTime.Text <> "" Then

            Dim StartTime As DateTime
            Dim Endtime As DateTime
            Dim TotalHours As Decimal = 0
            Dim StartSec As Integer = 0
            Dim EndSac As Integer = 0
            Dim sDate As DateTime

            StartTime = DateTime.ParseExact("1900-01-01 " & txtDTStartTime.Text, "yyyy-MM-dd HH:mm", Nothing)

            Endtime = Convert.ToDateTime("1900-01-01 " & txtDTEndTime.Text).ToString

            sDate = DateTime.ParseExact("1900-01-01 00:00", "yyyy-MM-dd HH:mm", Nothing)

            StartSec = DateDiff(DateInterval.Second, sDate, StartTime)
            EndSac = DateDiff(DateInterval.Second, sDate, Endtime)

            'TotalHours = DateDiff(DateInterval.Hour, StartTime, Endtime)
            txttotalhrsDT.Text = ((EndSac - StartSec) / 60.0) / 60.0
            'Button2_Click(sender, e)
            'InsertDownTime()

            'lblbarcode.Text = "Barcode Downtime Total Hours: "
            'Session("LabelScan") = "Barcode Downtime Total Hours: "

        End If

    End Sub

    Protected Sub txttotalhrsDT_TextChanged(sender As Object, e As EventArgs) Handles txttotalhrsDT.TextChanged

        Dim StartTime As DateTime
        Dim Endtime As DateTime

        If txttotalhrsDT.Text <> "" And CDec(txttotalhrsDT.Text) < 0 Then

            StartTime = DateTime.ParseExact("1900-01-01 " & txtDTStartTime.Text, "yyyy-MM-dd HH:mm", Nothing)

            Endtime = DateAdd(Hour, CDec(txttotalhrsDT.Text), StartTime)

            txtDTEndTime.Text = Mid(Endtime, 12, 5)

            lblbarcode.Text = "Barcode Downtime Code: "
            Session.Remove("LabelScan")

        End If


    End Sub


    Protected Sub ddlscrapcode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlscrapcode.SelectedIndexChanged

        If ddlscrapcode.SelectedItem.Value <> "" Then
            lblbarcode.Text = "Barcode Scrapped: "
        End If


    End Sub

    Protected Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click

        If lblbarcode.Text = "Barcode Downtime End Time: " Then

            lblbarcode.Text = "Barcode Downtime Start Time: "
            Session("LabelScan") = "Barcode Downtime Start Time: "

        ElseIf lblbarcode.Text = "Barcode Downtime Start Time: " Then
            lblbarcode.Text = "Barcode Downtime Code: "

        ElseIf lblbarcode.Text = "Barcode Downtime Code: " Then
            lblbarcode.Text = "Barcode Scrapped: "

        ElseIf lblbarcode.Text = "Barcode Scrapped: " Then
            lblbarcode.Text = "Barcode Scrap Code: "

        ElseIf lblbarcode.Text = "Barcode Scrap Code: " Then
            lblbarcode.Text = "Barcode Operator: "

        ElseIf lblbarcode.Text = "Barcode Operator: " Then
            lblbarcode.Text = "Barcode End Time: "
            Session("LabelScan") = "Barcode End Time: "

        ElseIf lblbarcode.Text = "Barcode End Time: " Then

            lblbarcode.Text = "Barcode Start Time: "
            Session("LabelScan") = "Barcode Start Time: "

        ElseIf lblbarcode.Text = "Barcode Start Time: " Then
            lblbarcode.Text = "Barcode Resource: "

        ElseIf lblbarcode.Text = "Barcode Resource: " Then
            lblbarcode.Text = "Barcode Employee: "

        ElseIf lblbarcode.Text = "Barcode Employee: " Then
            lblbarcode.Text = "Barcode Job: "

        ElseIf lblbarcode.Text = "Barcode Job: " Then
            lblbarcode.Text = "Barcode Downtime End Time: "
            Session("LabelScan") = "Barcode Downtime End Time: "
        End If

    End Sub

    Protected Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click

        Dim MsgErr As String = ""
        Dim MsgType As String = ""

        If lblbarcode.Text = "Barcode Job: " Then

            If ddltrantype.SelectedItem.Value = "C" Then
                lblbarcode.Text = "Barcode Resource: "
            ElseIf ddltrantype.SelectedItem.Value = "R" Then
                lblbarcode.Text = "Barcode Employee: "
            End If

        ElseIf lblbarcode.Text = "Barcode Employee: " Then

            If ddlemployee.SelectedItem.Value = "" Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','#220 : กรุณาสแกน หรือเลือก Employee ก่อน', 'error');", True)
            Else
                lblbarcode.Text = "Barcode Resource: "
            End If

        ElseIf lblbarcode.Text = "Barcode Resource: " Then

            If ddlResource.SelectedItem.Value = "" Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','#221 : กรุณาสแกน หรือเลือก Resource ก่อน', 'error');", True)
            Else
                lblbarcode.Text = "Barcode Start Time: "
                Session("LabelScan") = "Barcode Start Time: "
            End If

        ElseIf lblbarcode.Text = "Barcode Start Time: " Then

            lblbarcode.Text = "Barcode End Time: "
            Session("LabelScan") = "Barcode End Time: "

        ElseIf lblbarcode.Text = "Barcode End Time: " Then
            lblbarcode.Text = "Barcode Operator: "

        ElseIf lblbarcode.Text = "Barcode Operator: " Then
            lblbarcode.Text = "Barcode Scrap Code: "

        ElseIf lblbarcode.Text = "Barcode Scrap Code: " Then
            lblbarcode.Text = "Barcode Scrapped: "

        ElseIf lblbarcode.Text = "Barcode Scrapped: " Then

            lblbarcode.Text = "Barcode Downtime Code: "

        ElseIf lblbarcode.Text = "Barcode Downtime Code: " Then

            lblbarcode.Text = "Barcode Downtime Start Time: "
            Session("LabelScan") = "Barcode Downtime Start Time: "

        ElseIf lblbarcode.Text = "Barcode Downtime Start Time: " Then

            lblbarcode.Text = "Barcode Downtime End Time: "
            Session("LabelScan") = "Barcode Downtime End Time: "

        ElseIf lblbarcode.Text = "Barcode Downtime End Time: " Then

            lblbarcode.Text = "Barcode Job: "

        End If

    End Sub

    'Protected Sub btnpreviousdt_Click(sender As Object, e As EventArgs) Handles btnpreviousdt.Click

    '    If lblbarcode.Text = "Barcode Downtime Total Hours: " Then

    '        DigitEndTime.Value = "0"

    '        lblbarcode.Text = "Barcode Downtime End Time: "
    '        Session("LabelScan") = "Barcode Downtime End Time: "

    '    ElseIf lblbarcode.Text = "Barcode Downtime End Time: " Then

    '        DigitStartTime.Value = "0"

    '        lblbarcode.Text = "Barcode Downtime Start Time: "
    '        Session("LabelScan") = "Barcode Downtime Start Time: "

    '    ElseIf lblbarcode.Text = "Barcode Downtime Start Time: " Then

    '        lblbarcode.Text = "Barcode Downtime Code: "
    '        Session("LabelScan") = "Barcode Downtime Code: "

    '    End If

    'End Sub

    'Protected Sub btnnextdt_Click(sender As Object, e As EventArgs) Handles btnnextdt.Click

    '    Dim MsgErr As String = ""
    '    Dim MsgType As String = ""

    '    If lblbarcode.Text = "Barcode Downtime Code: " Then

    '        If ddldowntime.SelectedItem.Value = "" Then
    '            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','#223 : กรุณาแสกน หรือเลือก Down Time Code ก่อน', 'error');", True)
    '        Else
    '            DigitStartTime.Value = "0"
    '            lblbarcode.Text = "Barcode Downtime Start Time: "

    '        End If

    '    ElseIf lblbarcode.Text = "Barcode Downtime Start Time: " Then

    '        If txtDTStartTime.Text = "" Then
    '            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','#224 : กรุณาสแกน หรือระบุ Start Time ก่อน', 'error');", True)
    '        Else
    '            DigitEndTime.Value = "0"
    '            lblbarcode.Text = "Barcode Downtime End Time: "

    '            Session("LabelScan") = "Barcode Downtime End Time: "

    '        End If

    '    ElseIf lblbarcode.Text = "Barcode Downtime End Time: " Then

    '        If txtDTEndTime.Text = "" Then
    '            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','#225 : กรุณาสแกน หรือระบุ End Time ก่อน', 'error');", True)
    '        Else

    '            Dim StartTime As DateTime
    '            Dim Endtime As DateTime
    '            Dim TotalHours As Decimal = 0
    '            Dim StartSec As Integer = 0
    '            Dim EndSac As Integer = 0
    '            Dim sDate As DateTime

    '            StartTime = Convert.ToDateTime("1900-01-01 " & txtDTStartTime.Text).ToString

    '            If txtDTEndTime.Text = "00:00" Then
    '                Endtime = Convert.ToDateTime("1900-01-02 " & txtDTEndTime.Text).ToString
    '            Else
    '                Endtime = Convert.ToDateTime("1900-01-01 " & txtDTEndTime.Text).ToString
    '            End If

    '            sDate = DateTime.ParseExact("1900-01-01 00:00", "yyyy-MM-dd HH:mm", Nothing)

    '            StartSec = DateDiff(DateInterval.Second, sDate, StartTime)
    '            EndSac = DateDiff(DateInterval.Second, sDate, Endtime)

    '            txttotalhrsDT.Text = ((EndSac - StartSec) / 60.0) / 60.0

    '            lblbarcode.Text = "Barcode Downtime Total Hours: "

    '            Session("LabelScan") = "Barcode Downtime Total Hours: "

    '        End If

    '    ElseIf lblbarcode.Text = "Barcode Downtime Total Hours: " Then

    '        'InsertDownTime()

    '        lblbarcode.Text = "Barcode Downtime Code: "
    '        Session.Remove("LabelScan")

    '    End If

    'End Sub

    'Protected Sub txtCauseBy_TextChanged(ByVal sender As Object, ByVal e As EventArgs)

    '    For Each item As ListViewItem In PanelList3.Items

    '        Dim txtCauseBy As TextBox = DirectCast(item.FindControl("txtCauseBy"), TextBox)
    '        Dim lblRowPointer As Label = DirectCast(item.FindControl("lblRowPointer"), Label)

    '        If Not (String.IsNullOrEmpty(txtCauseBy.Text)) Then

    '            Parms = "<Parameters><Parameter>" & lblRowPointer.Text & "</Parameter>" &
    '                    "<Parameter>" & Session("PSession").ToString & "</Parameter>" &
    '                    "<Parameter>" & txtCauseBy.Text & "</Parameter>" &
    '                    "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
    '                    "<Parameter>" & ParmSite.ToString & "</Parameter></Parameters>"


    '            oWS = New CNIService.DOWebServiceSoapClient
    '            oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobTranTags", "PPCC_EX_UpdateCauseByDowntimeSp", Parms)

    '        End If

    '    Next

    'End Sub

    Sub SelectCheckBox_CheckedChanged()

        Dim Selected As Integer = 0

        For Each item As ListViewItem In PanelList4.Items

            Dim chkSelect As CheckBox = DirectCast(item.FindControl("chkSelect"), CheckBox)

            Selected = IIf(chkSelect.Checked = True, 1, 0)

            Parms = "<Parameters><Parameter>" & DirectCast(item.FindControl("lblRowPointer"), Label).Text & "</Parameter>" &
                    "<Parameter>" & Selected & "</Parameter>" &
                    "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                    "<Parameter>" & ParmSite.ToString & "</Parameter></Parameters>"

            oWS = New CNIService.DOWebServiceSoapClient
            res = oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_Backflushs", "PPCC_EX_BackflushSelectSp", Parms)

        Next

        Call GetBackflushLots(lbltotalscrap.Text, True)

    End Sub

    Sub Clear()

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "LocalSet();", True)
        Session.Remove("LabelScan")
        DateNow = Date.Now.ToString("dd/MM/yyyy")
        txtdate.Text = DateNow

        lblbarcode.Text = "Barcode Job: "
        txtbarcode.Text = String.Empty
        txtjob.Text = String.Empty
        lbljob.Text = String.Empty
        lblSuffix.Text = String.Empty
        lbloper.Text = String.Empty
        chkcancel.Checked = False
        ddlemployee.Attributes.Remove("disabled")
        ddlemployee.SelectedIndex = ddlemployee.Items.IndexOf(ddlemployee.Items.FindByValue(""))
        txtwc.Text = String.Empty
        ddlResource.SelectedIndex = ddlResource.Items.IndexOf(ddlResource.Items.FindByValue(""))
        txtProdLine.Text = String.Empty
        ddlSchedulingShift.SelectedIndex = ddlSchedulingShift.Items.IndexOf(ddlSchedulingShift.Items.FindByValue(""))
        'rdDayNight.ClearSelection()
        txtStartTime.Text = "00:00"
        txtEndTime.Text = "00:00"
        txttotalhour.Text = "0"
        ddlmovetoloc.SelectedIndex = ddlmovetoloc.Items.IndexOf(ddlmovetoloc.Items.FindByValue(""))

        PanelList1.DataSource = Nothing
        PanelList1.DataBind()

        ddlscrapcode.SelectedIndex = ddlscrapcode.Items.IndexOf(ddlscrapcode.Items.FindByValue(""))
        lbltotalscrap.Text = FormatNumber(0, LenPointQty)

        PanelList2.DataSource = Nothing
        PanelList2.DataBind()

        PanelList4.DataSource = Nothing
        PanelList4.DataBind()

        PanelList5.DataSource = Nothing
        PanelList5.DataBind()

        ddldowntime.SelectedIndex = ddldowntime.Items.IndexOf(ddldowntime.Items.FindByValue(""))
        txtDTStartTime.Text = "00:00"
        txtDTEndTime.Text = "00:00"
        txttotalhrsDT.Text = "0"
        DigitStartTime.Value = "0"
        DigitEndTime.Value = "0"
        DigitStartTimeDt.Value = "0"
        DigitEndTimeDt.Value = "0"

        PanelList3.DataSource = Nothing
        PanelList3.DataBind()

        txtbarcode.Focus()
        Session("PSession") = NewSessionID()

        CntrlPoint.Value = "1"
        BflushMessage.Value = ""
        BflushError.Value = "0"
        lblQtyCompleted.Text = ""

        txtOperator.Text = ""
        ddlItemScrapped.SelectedIndex = ddlItemScrapped.Items.IndexOf(ddlItemScrapped.Items.FindByValue(""))
        txtQtyScrap.Text = FormatNumber(0, LenPointQty)

        HiddenField1.Value = "0"
        WipTag.Value = "0"

        txtStartTime.Attributes.Remove("disabled")
        txtEndTime.Attributes.Remove("disabled")

    End Sub

    Sub BindItem()

        Parms = ""
        Parms = "<Parameters><Parameter>" & DBNull.Value & "</Parameter>" &
                                "<Parameter>" & lbljob.Text & "</Parameter>" &
                                "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                                "<Parameter>" & lbloper.Text & "</Parameter>" &
                                "<Parameter>" & DBNull.Value & "</Parameter>" &
                                "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                                "<Parameter>" & PSite.ToString & "</Parameter>" &
                                "<Parameter>" & Session("PSession").ToString & "</Parameter></Parameters>"

        res = New Object
        oWS = New CNIService.DOWebServiceSoapClient
        res = oWS.CallMethod(Session("Token").ToString, "PPCC_EX_CoProducts", "PPCC_EX_LoadJobtranitemSp", Parms)

        Filter = "Job = '" & lbljob.Text & "' And Suffix = '" & lblSuffix.Text & "' And OperNum = '" & lbloper.Text & "' And SessionID = '" & Session("PSession").ToString & "'"

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_EX_CoProducts", "Item, QtyComplete, Lot, OperNum, DerQtyRemain, JobQtyRelease, JobQtyComplete, RowPointer", Filter, "Item", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then

            PanelList1.DataSource = ds.Tables(0)
            PanelList1.DataBind()

        End If

    End Sub

    Sub BindItemGrid()

        Filter = "Job = '" & lbljob.Text & "' And Suffix = '" & lblSuffix.Text & "' And OperNum = '" & lbloper.Text & "' And SessionID = '" & Session("PSession").ToString & "'"

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_EX_CoProducts", "Item, QtyComplete, Lot, OperNum, DerQtyRemain, JobQtyRelease, JobQtyComplete, RowPointer", Filter, "Item", "", 0)

        'GridView1.DataSource = ds.Tables(0)
        'GridView1.DataBind()

        If ds.Tables(0).Rows.Count > 0 Then

            PanelList1.DataSource = ds.Tables(0)
            PanelList1.DataBind()

        End If

    End Sub

    Sub BindScrap()

        Filter = "SessionID = '" & Session("PSession").ToString & "' And UserID = '" & Session("Username").ToString & "' And Type = 'S'"

        Dim List As String
        List = "ReasonCode, DerDescription, Qty, Job, Suffix, OperNum, Item, RowPointer"

        ds = New DataSet

        oWS = New CNIService.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Ex_Scrappeds", List, Filter, "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then

            PanelList2.DataSource = ds
            PanelList2.DataBind()

            Dim dt As DataTable = ds.Tables(0)
            Dim sum As Decimal = FormatNumber(Convert.ToDecimal(dt.Compute("SUM(Qty)", "Job = '" & lbljob.Text & "' And Suffix = '" & lblSuffix.Text & "' And OperNum = '" & lbloper.Text & "'")), LenPointQty)

            lbltotalscrap.Text = Decimal.Round(sum.ToString, LenPointQty, MidpointRounding.AwayFromZero)
        Else

            PanelList2.DataSource = ds
            PanelList2.DataBind()

            lbltotalscrap.Text = Decimal.Round(0, LenPointQty, MidpointRounding.AwayFromZero)
        End If

    End Sub

    Sub BindDownTime()

        Filter = "SessionID = '" & Session("PSession").ToString & "' And UserID = '" & Session("Username").ToString & "' And Type = 'D'"

        Dim List As String
        List = "ReasonCode, DerDescription, StartTime, EndTime, AHrs, Job, Suffix, OperNum, RowPointer, CauseBy"

        ds = New DataSet

        oWS = New CNIService.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Ex_Scrappeds", List, Filter, "CreateDate desc", "", 0)

        'If ds.Tables(0).Rows.Count > 0 Then

        'GridView6.DataSource = ds
        'GridView6.DataBind()

        'If ds.Tables(0).Rows.Count > 0 Then

        PanelList3.DataSource = ds
        PanelList3.DataBind()

        'End If

        'Dim dt As DataTable = ds.Tables(0)
        'Dim sum As Decimal = FormatNumber(Convert.ToDecimal(dt.Compute("SUM(Qty)", "Job = '" & Session("Job").ToString & "' And Suffix = '" & Session("Suffix").ToString & "' And OperNum = '" & Session("OperNum").ToString & "'")), LenPointQty)

        'lbltotalscrap.Text = Decimal.Round(sum.ToString, LenPointQty, MidpointRounding.AwayFromZero)

        'End If

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

    Public Function QtyFormat() As Integer

        Dim strQtyFormat As String = ""
        Dim PointQty As String = ""

        QtyFormat = 0

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLInvparms", "QtyPerFormat", "", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            strQtyFormat = ds.Tables(0).Rows(0)("QtyPerFormat").ToString

            Dim words As String() = strQtyFormat.Split(New Char() {"."c})

            For Each word As String In words
                PointQty = words(1)
                Exit For
            Next

            QtyFormat = Len(PointQty)

        End If

        Return QtyFormat

    End Function

    Function GetStrShift(Employee As String) As String

        GetStrShift = ""

        Filter = "EmpNum = '" & Employee & "'"

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLEmployees", "Shift", Filter, "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetStrShift = ds.Tables(0).Rows(0)("Shift").ToString
        End If

        Return GetStrShift

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

    Function GetJobStockTran() As String

        GetJobStockTran = ""

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLSfcparms", "JobStockrm", "", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetJobStockTran = ds.Tables(0).Rows(0)("JobStockrm").ToString
        End If

        Return GetJobStockTran

    End Function

    Function GetCoProductMix(Job As String, Suffix As String) As String

        GetCoProductMix = ""

        Filter = "Job = '" & Job & "' And Suffix = '" & Suffix & "'"

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLJobs", "CoProductMix", Filter, "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetCoProductMix = ds.Tables(0).Rows(0)("CoProductMix").ToString
        End If

        Return GetCoProductMix

    End Function

    Function GetItemJob(Job As String, Suffix As String) As String

        GetItemJob = ""

        Filter = "Job = '" & Job & "' And Suffix = '" & Suffix & "'"

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLJobs", "Item", Filter, "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetItemJob = ds.Tables(0).Rows(0)("Item").ToString
        End If

        Return GetItemJob

    End Function

    Function GetJobroute(Job As String, Suffix As String, OperNum As String) As DataSet

        Filter = "Job = '" & Job & "' And Suffix = '" & Suffix & "' And OperNum = '" & OperNum & "'"

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLJobRoutes", "JshSchedDrv, Wc, jbrUf_jobroute_line, QtyReceived, QtyComplete, jbrUf_jobroute_WIPTag", Filter, "", "", 0)

        Return ds

    End Function

    Function GetContorlPoint(Job As String, Suffix As String, OperNum As String) As String

        GetContorlPoint = "0"

        Filter = "Job = '" & Job & "' And Suffix = '" & Suffix & "' And OperNum = '" & OperNum & "'"

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLJobRoutes", "CntrlPoint", Filter, "", "", 0)


        If ds.Tables(0).Rows.Count > 0 Then
            GetContorlPoint = ds.Tables(0).Rows(0)("CntrlPoint").ToString

        End If


        Return GetContorlPoint

    End Function

    Function GetMoveToLoc(Whse As String, Item As String) As String

        GetMoveToLoc = ""
        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet

        Filter = "Whse = '" & Whse & "' AND Item = '" & Item & "' AND LocType <> 'T'"

        ds = oWS.LoadDataSet(Session("Token"), "SLItemLocs", "Loc", Filter, "Rank", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then

            GetMoveToLoc = ds.Tables(0).Rows(0)("Loc").ToString

        End If

        Return GetMoveToLoc

    End Function

    Function GetLastOper(Job As String, Suffix As String) As String

        GetLastOper = ""

        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet

        Filter = "Job = '" & Job & "' And Suffix = '" & Suffix & "'"

        ds = oWS.LoadDataSet(Session("Token"), "SLJobRoutes", "OperNum", Filter, "OperNum DESC", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetLastOper = ds.Tables(0).Rows(0)("OperNum").ToString
        End If

        Return GetLastOper

    End Function

    Function GetUserInitial(UserName As String) As String

        GetUserInitial = ""
        Dim UserID As String = ""

        ds = New DataSet
        oWS = New CNIService.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "UserNames", "UserId", "Username='" & UserName & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            UserID = ds.Tables(0).Rows(0)("UserId").ToString
        End If

        ds = New DataSet
        oWS = New CNIService.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "SLUserLocals", "UserCode", "UserId='" & UserID & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetUserInitial = ds.Tables(0).Rows(0)("UserCode").ToString
        End If

        Return GetUserInitial

    End Function

    Function GetFirstOper(Job As String, Suffix As String) As String

        GetFirstOper = ""
        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet

        Filter = "Job = '" & Job & "' And Suffix = '" & Suffix & "'"

        ds = oWS.LoadDataSet(Session("Token"), "SLJobRoutes", "OperNum", "", "OperNum", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetFirstOper = ds.Tables(0).Rows(0)("OperNum").ToString
        End If

        Return GetFirstOper

    End Function

    Function GetDefWhse() As String

        GetDefWhse = ""
        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token"), "PPCC_SLUserNames", "UserLocalWhse", "Username = '" & Session("UserName").ToString & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetDefWhse = ds.Tables(0).Rows(0)("UserLocalWhse").ToString
        End If

        Return GetDefWhse

    End Function

    Function GetUserCode(UserID As String) As String

        GetUserCode = ""

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "UserNames", "UserId", "Username = '" & UserID & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then

            Filter = "UserId = '" & ds.Tables(0).Rows(0)("UserId").ToString & "'"

            oWS = New CNIService.DOWebServiceSoapClient

            ds = New DataSet

            ds = oWS.LoadDataSet(Session("Token").ToString, "SLUserLocals", "UserCode", Filter, "", "", 0)

            If ds.Tables(0).Rows.Count > 0 Then
                GetUserCode = ds.Tables(0).Rows(0)("UserCode").ToString
            End If

        End If

        Return GetUserCode

    End Function

    Function GetBFLot(Job As String, Suffix As String, Oper As String) As DataSet

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        Dim sList As String

        sList = "Selected, OperNum, Seq, Lot, Qty, UM, Item, ItemDesc, QtyOnHand, QtyNeeded, RowPointer, Loc, Whse, TransNum, TransSeq, EmpNum"

        Filter = "Job = '" & Job & "' And Suffix = '" & Suffix & "' And UserID = '" & Session("Username").ToString & "' and QtyReq > 0 and SessionID = '" & Session("PSession").ToString & "'"

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Ex_Backflushs", sList, Filter, "", "", 0)

        Return ds

    End Function

    Sub GetSchedulingShift()

        Dim SchedulingShift As String = ""

        Parms = "<Parameters><Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                "<Parameter>" & ddlResource.SelectedItem.Value & "</Parameter>" &
                "<Parameter>" & txtStartTime.Text & "</Parameter>" &
                "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobTranTags", "PPCC_Ex_GetSchedulingShiftSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)

        Dim i As Integer = 1

        For Each node As XmlNode In doc.DocumentElement

            If i = 1 Then
                SchedulingShift = node.InnerText
                Exit For
            End If

            i += 1
        Next

        If SchedulingShift <> "" Then
            ddlSchedulingShift.SelectedIndex = ddlSchedulingShift.Items.IndexOf(ddlSchedulingShift.Items.FindByValue(SchedulingShift))
        End If

    End Sub

    Sub QAQCPass(ByVal Wc As String)

        Filter = "TypeName = 'WorkCenter-QAQC' AND Value = '" & Wc & "'"

        ds = New DataSet

        oWS = New CNIService.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "UserDefinedTypeValues", "Value", Filter, "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            ChkQAQC.Enabled = True
            HiddenField1.Value = "1"
        Else
            ChkQAQC.Enabled = False
            HiddenField1.Value = "0"
        End If


    End Sub

    Sub GetResourceGroupsType(ByVal Job As String, ByVal Suffix As String, ByVal OperNum As String, ByRef Labor As String, ByRef Machine As String)

        Labor = ""
        Machine = ""

        Filter = ""

        'Find RgrpSLTYPE Labor

        Filter = "Job = '" & Job & "' AND Suffix = " & Suffix & " And OperNum = " & OperNum & " AND RgrpSLTYPE = 'L'"

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLJrtResourceGroups", "RgrpSLTYPE", Filter, "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            Labor = ds.Tables(0).Rows(0)("RgrpSLTYPE").ToString
        End If

        'Find RgrpSLTYPE Machine

        Filter = ""

        Filter = "Job = '" & Job & "' AND Suffix = " & Suffix & " And OperNum = " & OperNum & " AND RgrpSLTYPE = 'M'"

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLJrtResourceGroups", "RgrpSLTYPE", Filter, "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            Machine = ds.Tables(0).Rows(0)("RgrpSLTYPE").ToString
        End If


    End Sub

#Region "Get Data Bind To Dropdownlist"

    Sub GetEmployee()

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLEmployees", "EmpNum, Name", "", "EmpNum", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddlemployee.Items.Add(New ListItem(dRow("EmpNum") & IIf(IsDBNull(dRow("Name")), "", " : " & dRow("Name")), UCase(dRow("EmpNum").ToString.Trim)))

        Next

        ddlemployee.Items.Insert(0, New ListItem("", ""))

    End Sub

    Sub GetResource(Job As String, Suffix As String, Oper As String)

        ddlResource.Items.Clear()

        Parms = ""
        Parms = "<Parameters><Parameter>" & Job & "</Parameter>" &
                                "<Parameter>" & Suffix & "</Parameter>" &
                                "<Parameter>" & Oper & "</Parameter>" &
                                "<Parameter>" & DBNull.Value & "</Parameter>" &
                                "<Parameter>" & DBNull.Value & "</Parameter>" &
                                "<Parameter  ByRef='Y'></Parameter>" &
                                "<Parameter>" & Session("Username").ToString & "</Parameter>" &
                                "<Parameter>" & PSite.ToString & "</Parameter></Parameters>"

        res = New Object
        oWS = New CNIService.DOWebServiceSoapClient
        res = oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_TmpRresources", "PPCC_EX_CLM_ResourceSp", Parms)

        Filter = "Job = '" & Job & "' And Suffix = '" & Suffix & "' And OperNum = '" & Oper & "'"

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Ex_TmpRresources", "RESID, DESCR", Filter, "SEQNO", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddlResource.Items.Add(New ListItem(dRow("RESID") & IIf(IsDBNull(dRow("DESCR")), "", " : " & dRow("DESCR")), UCase(dRow("RESID"))))
        Next

        ddlResource.Items.Insert(0, New ListItem("", ""))

    End Sub


    Sub GetItemLoc(Item As String, Whse As String)

        ddlmovetoloc.Items.Clear()

        Filter = "Item = '" & Item & "' And Whse = '" & Whse & "'"

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLItemLocs", "Loc, LocDescription", Filter, "Rank", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddlmovetoloc.Items.Add(New ListItem(dRow("Loc") & IIf(IsDBNull(dRow("LocDescription")), "", " : " & dRow("LocDescription")), UCase(dRow("Loc"))))
        Next

        ddlmovetoloc.Items.Insert(0, New ListItem("", ""))

    End Sub

    Sub GetShift()

        ddlSchedulingShift.Items.Clear()

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLSHIFTnnns", "SHIFTID, DESCR", Filter, "SHIFTID", "", 0)

        Dim dt_distinct As DataTable = ds.Tables(0).DefaultView.ToTable(True, "SHIFTID", "DESCR")

        For Each dRow As DataRow In dt_distinct.Rows
            ddlSchedulingShift.Items.Add(New ListItem(dRow("SHIFTID") & IIf(IsDBNull(dRow("DESCR")), "", " : " & dRow("DESCR")), UCase(dRow("SHIFTID"))))
        Next

        ddlSchedulingShift.Items.Insert(0, New ListItem("", ""))

    End Sub

    Sub GetReasonCode()

        ddlscrapcode.Items.Clear()

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLReasons", "ReasonCode, Description", "ReasonClass = 'MFG SCRAP'", "ReasonCode", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddlscrapcode.Items.Add(New ListItem(dRow("ReasonCode") & IIf(IsDBNull(dRow("Description")), "", " : " & dRow("Description")), UCase(dRow("ReasonCode"))))

        Next

        ddlscrapcode.Items.Insert(0, New ListItem("", ""))

    End Sub

    Sub GetReasonCodeDownTime()

        ddldowntime.Items.Clear()

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "ppcc_downtime_reasoncodes", "reason_code, description", "", "reason_code", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddldowntime.Items.Add(New ListItem(dRow("reason_code") & IIf(IsDBNull(dRow("description")), "", " : " & dRow("description")), UCase(dRow("reason_code"))))

        Next

        ddldowntime.Items.Insert(0, New ListItem("", ""))

    End Sub

    Sub GetItemScrap(CoProductMix As String, Job As String, Suffix As String)

        ddlItemScrapped.Items.Clear()

        Filter = ""

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        If CoProductMix = "0" Then
            Filter = "Job = '" & Job & "' AND Suffix = '" & Suffix & "'"
            ds = oWS.LoadDataSet(Session("Token").ToString, "SLJobs", "Item, ItemDescription", Filter, "Item", "", 0)
        Else
            Filter = "Job = '" & Job & "' AND Suffix = '" & Suffix & "'"
            ds = oWS.LoadDataSet(Session("Token").ToString, "SLJobitems", "Item, ItemDescription", Filter, "Item", "", 0)
        End If

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddlItemScrapped.Items.Add(New ListItem(dRow("Item") & IIf(IsDBNull(dRow("ItemDescription")), "", " : " & dRow("ItemDescription")), dRow("Item")))
        Next

        ddlItemScrapped.Items.Insert(0, New ListItem("", ""))

    End Sub


#End Region

#Region "Bind Data To Gridview Tag"

    Sub BindGridviewTag()

        'Dim Filter As String
        'Dim Propertie As String

        'oWS = New CNIService.DOWebServiceSoapClient
        'ds = New DataSet
        'Filter = "SessionID = '" & Session("PSession").ToString & "'"
        'Propertie = "TagID, Lot, Qty"

        'ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Ex_JobTranTags", Propertie, Filter, "CreateDate Desc", "", 0)

        'GridView2.DataSource = ds.Tables(0)
        'GridView2.DataBind()

        'If GridView2.Rows.Count > 0 Then

        '    Filter = "Job = '" & lbljob.Text & "' And Suffix = '" & lblSuffix.Text & "' And OperNum = '" & lbloper.Text & "' And SessionID = '" & Session("PSession").ToString & "'"

        '    oWS = New CNIService.DOWebServiceSoapClient

        '    ds = New DataSet

        '    ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_EX_CoProducts", "Item, QtyComplete, Lot", Filter, "Item", "", 0)

        '    GridView1.DataSource = ds.Tables(0)
        '    GridView1.DataBind()

        'End If


    End Sub

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