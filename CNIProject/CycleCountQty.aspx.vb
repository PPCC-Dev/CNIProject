Imports System.Data
Imports System.Xml
Imports System.Collections
Imports System.Reflection
Imports Microsoft.VisualBasic.DateInterval

Public Class CycleCountQty
    Inherits System.Web.UI.Page

    Dim oWS As CNIService.DOWebServiceSoapClient
    Dim ds As DataSet
    Dim Filter As String
    Dim Parms As String
    Dim LenPointQty As Integer = 0

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
            BindData()

        End If

        txtCountQty.Focus()

    End Sub


    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click

        Dim PostURL As String = ""
        Dim strLoc As String = Request.QueryString("Loc").ToString
        Dim strWhse As String = Request.QueryString("Whse").ToString
        Dim strItem As String = Request.QueryString("Item").ToString
        Dim strLot As String = Request.QueryString("Lot").ToString
        Dim strProductCode As String = Request.QueryString("ProductCode").ToString
        Dim strLocation As String = Request.QueryString("Location").ToString

        PostURL = "?SessionID=" & Session("PSession").ToString & "&Item=" & strItem & "&Lot=" & strLot & ""
        PostURL = PostURL & "&Loc=" & strLoc & "&Whse=" & strWhse & ""
        PostURL = PostURL & "&ProductCode=" & strProductCode & "&Location=" & strLocation & ""

        Response.Redirect("CycleCount.aspx" & PostURL)

    End Sub

    Protected Sub btnback_Click(sender As Object, e As EventArgs) Handles btnback.Click

        Dim PostURL As String = ""
        Dim strLoc As String = Request.QueryString("Loc").ToString
        Dim strWhse As String = Request.QueryString("Whse").ToString
        Dim strItem As String = Request.QueryString("Item").ToString
        Dim strLot As String = Request.QueryString("Lot").ToString
        Dim strProductCode As String = Request.QueryString("ProductCode").ToString
        Dim strLocation As String = Request.QueryString("Location").ToString

        PostURL = "?SessionID=" & Session("PSession").ToString & "&Item=" & strItem & "&Lot=" & strLot & ""
        PostURL = PostURL & "&Loc=" & strLoc & "&Whse=" & strWhse & ""
        PostURL = PostURL & "&ProductCode=" & strProductCode & "&Location=" & strLocation & ""

        Response.Redirect("CycleCount.aspx" & PostURL)

    End Sub

    Protected Sub btncount_Click(sender As Object, e As EventArgs) Handles btncount.Click

        Dim Stat, MsgErr, MsgType As String
        Stat = "TRUE"
        MsgErr = ""
        MsgType = ""

        oWS = New CNIService.DOWebServiceSoapClient

        Parms = "<Parameters><Parameter>" & lblRowPointer.Text & "</Parameter>" &
                            "<Parameter>" & DBNull.Value & "</Parameter>" &
                            "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                            "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                            "<Parameter>" & "C" & "</Parameter>" &
                            "<Parameter>" & txtCountQty.Text & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_CycleCntDetail", "PPCC_Ex_DeleteCountTagCycleCountSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)

        Dim i As Integer = 1

        For Each node As XmlNode In doc.DocumentElement

            If i = 7 Then
                Stat = node.InnerText

            ElseIf i = 8 Then
                MsgType = node.InnerText

            ElseIf i = 9 Then
                MsgErr = node.InnerText

            End If

            i += 1

        Next

        If Stat = "TRUE" Then

            BindData()

            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Success','นับเรียบร้อยแล้ว', 'success');", True)
        Else

            MsgErr = MsgErr.Replace("'", "\'")
            MsgErr = MsgErr.Replace(vbLf, "<br />")

            MsgType = "Error [" & MsgType & "]"

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)

        End If



    End Sub

    Protected Sub btncancel_Click(sender As Object, e As EventArgs) Handles btncancel.Click
        txtCountQty.Text = String.Empty
        btnback_Click(sender, e)
    End Sub

#Region "Bind Data To Gridview"

    Sub BindData()

        Dim Filter As String
        Dim Propertie As String
        Dim strLoc As String = Request.QueryString("Loc").ToString
        Dim strWhse As String = Request.QueryString("Whse").ToString
        Dim strItem As String = Request.QueryString("Item").ToString
        Dim strLot As String = Request.QueryString("Lot").ToString

        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet
        Propertie = "Item, Loc, Lot, CountQty, Stat, Description, ProductCode, Uf_Item_LongDesc, Uf_loc_NonScanTag, RowPointer"


        Filter = "Whse = '" & strWhse & "' And Item = '" & strItem & "' And Loc = '" & strLoc & "' And Lot = '" & strLot & "' And Uf_item_Barcodecontrol = 1"
        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_SLCycles", Propertie, Filter, "RecordDate Desc", "", 0)

        'PanelList.DataSource = ds.Tables(0)
        'PanelList.DataBind()

        If ds.Tables(0).Rows.Count > 0 Then
            lblitem.Text = IIf(String.IsNullOrEmpty(ds.Tables(0).Rows(0)("Item").ToString), "", ds.Tables(0).Rows(0)("Item").ToString)
            lblLongDesc.Text = IIf(String.IsNullOrEmpty(ds.Tables(0).Rows(0)("Description").ToString), "", ds.Tables(0).Rows(0)("Description").ToString)
            lblLot.Text = IIf(String.IsNullOrEmpty(ds.Tables(0).Rows(0)("Lot").ToString), "", ds.Tables(0).Rows(0)("Lot").ToString)
            lblLoc.Text = IIf(String.IsNullOrEmpty(ds.Tables(0).Rows(0)("Loc").ToString), "", ds.Tables(0).Rows(0)("Loc").ToString)
            chkNonScanTag.Checked = IIf(String.IsNullOrEmpty(ds.Tables(0).Rows(0)("Uf_loc_NonScanTag").ToString), "0", ds.Tables(0).Rows(0)("Uf_loc_NonScanTag").ToString)
            txtCountQty.Text = IIf(String.IsNullOrEmpty(ds.Tables(0).Rows(0)("CountQty").ToString), "", ds.Tables(0).Rows(0)("CountQty").ToString)
            lblRowPointer.Text = IIf(String.IsNullOrEmpty(ds.Tables(0).Rows(0)("RowPointer").ToString), "", ds.Tables(0).Rows(0)("RowPointer").ToString)
        End If

        If txtCountQty.Text <> "" Then
            txtCountQty.Text = FormatNumber(txtCountQty.Text, LenPointQty)
        End If


    End Sub
#End Region

#Region "Function"

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

#End Region


End Class