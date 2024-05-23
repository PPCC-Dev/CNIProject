Imports System.Data
Imports System.Xml
Imports System.Collections
Imports System.Reflection

Public Class POTagDetail
    Inherits System.Web.UI.Page

    Dim oWS As CNIService.DOWebServiceSoapClient
    Dim ds As DataSet
    Dim Filter As String
    Dim LenPointQty As Integer = 0
    Dim Parms As String

    Private Shared Whse As String
    Private Shared ParmSite As String

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

        If Not Page.IsPostBack Then

            PSite = GetSite()
            PWhse = GetDefWhse()

            BindListDetail()

        End If

    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click

        Dim PostURL As String = ""
        Dim strAction As String = Request.QueryString("Action").ToString
        Dim strDSNum As String = Request.QueryString("DSNum").ToString
        Dim CountScanLine As String = Request.QueryString("CountScanLine").ToString

        PostURL = "?SessionID=" & Session("PSession").ToString _
                                & "&Action=" & strAction _
                                & "&DSNum=" & strDSNum _
                                & "&CountScanLine=" & CountScanLine

        Response.Redirect("POReceiving.aspx" & PostURL)

    End Sub

    Protected Sub btnback_Click(sender As Object, e As EventArgs) Handles btnback.Click

        Dim PostURL As String = ""
        Dim strAction As String = Request.QueryString("Action").ToString
        Dim strDSNum As String = Request.QueryString("DSNum").ToString
        Dim CountScanLine As String = Request.QueryString("CountScanLine").ToString

        PostURL = "?SessionID=" & Session("PSession").ToString _
                                & "&Action=" & strAction _
                                & "&DSNum=" & strDSNum _
                                & "&CountScanLine=" & CountScanLine

        Response.Redirect("POReceiving.aspx" & PostURL)

    End Sub

    Protected Sub PanelList_RowDataBound(sender As Object, e As ListViewItemEventArgs) Handles PanelList.ItemDataBound

        If e.Item.ItemType = ListViewItemType.DataItem Then

            Dim lblQtyTag As Label = CType(e.Item.FindControl("lblQtyTag"), Label)
            Dim lblQtyTagConv As Label = CType(e.Item.FindControl("lblQtyTagConv"), Label)

            lblQtyTag.Text = FormatNumber(lblQtyTag.Text, AmtTotalFormat.Value)
            lblQtyTagConv.Text = FormatNumber(lblQtyTagConv.Text, AmtTotalFormat.Value)

        End If

    End Sub

    Protected Sub PanelList_ItemCommand(ByVal sender As Object, ByVal e As ListViewCommandEventArgs) Handles PanelList.ItemCommand

        If e.CommandName = "DeleteTag" Then

            Parms = ""

            Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & "T" & "</Parameter>" &
                        "<Parameter>" & TryCast(e.Item.FindControl("lbltagID"), Label).Text & "</Parameter>" &
                        "<Parameter>" & "D" & "</Parameter>" &
                        "<Parameter>" & PSite.ToString & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "</Parameters>"

            oWS = New CNIService.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_EX_PO_Receives", "PPCC_EX_POReceivingSp", Parms)

            BindListDetail()

        End If



    End Sub

    Sub BindListDetail()

        Dim PropertyList As String = ""

        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet

        Filter = "SessionID = '" & Session("PSession").ToString & "'"

        PropertyList = "TagID, Item, Description, Lot, TagQty, TagQtyConv, PoNum, PoLine, PoRelease, Places, Round"

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_EX_PO_ReceiveDetails", PropertyList, Filter, "PoNum, PoLine, PoRelease", "", 0)

        'If ds.Tables(0).Rows.Count > 0 Then

        If ds.Tables(0).Rows.Count > 0 Then
            AmtTotalFormat.Value = ds.Tables(0).Rows(0)("Places").ToString
        End If

        PanelList.DataSource = ds
        PanelList.DataBind()

        'End If




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

End Class