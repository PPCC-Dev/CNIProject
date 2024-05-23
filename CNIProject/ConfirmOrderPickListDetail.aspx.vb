Imports System.Data
Imports System.Xml
Imports System.Collections
Imports System.Reflection

Public Class ConfirmOrderPickListDetail
    Inherits System.Web.UI.Page

    Dim oWS As CNIService.DOWebServiceSoapClient
    Dim ds As DataSet
    Dim Filter As String
    Dim LenPointQty As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("PSession") Is Nothing Then
            Response.Redirect("signin.aspx")
        Else
            If Session("PSession").ToString = "" Then
                Response.Redirect("signin.aspx")
            End If
        End If

        If Not Page.IsPostBack Then

            lblconum.Text = Request.QueryString("CoNum").ToString
            lblcoline.Text = Request.QueryString("CoLine").ToString
            lblcorelease.Text = Request.QueryString("CoRelease").ToString

            lblderco.Text = lblconum.Text & "-" &
                            IIf(Len(lblcoline.Text) = 1, "000" & lblcoline.Text,
                            IIf(Len(lblcoline.Text) = 2, "00" & lblcoline.Text,
                            IIf(Len(lblcoline.Text) = 3, "0" & lblcoline.Text, lblcoline.Text))) & "-" &
                            IIf(Len(lblcorelease.Text) = 1, "000" & lblcorelease.Text,
                            IIf(Len(lblcorelease.Text) = 2, "00" & lblcorelease.Text,
                            IIf(Len(lblcorelease.Text) = 3, "0" & lblcorelease.Text, lblcorelease.Text)))

            LenPointQty = UnitQtyFormat()
            BindGridview()

        End If

    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click

        Dim PostURL As String = ""
        Dim strCoNum As String = Request.QueryString("CoNum").ToString
        Dim strCoLine As String = Request.QueryString("CoLine").ToString
        Dim strCoRelease As String = Request.QueryString("CoRelease").ToString
        Dim strOrderPickList As String = Request.QueryString("OrderPickList").ToString
        Dim strLoc As String = Request.QueryString("Loc").ToString
        Dim sModeType As String = Request.QueryString("ModeType").ToString

        PostURL = "?SessionID=" & Session("PSession").ToString & "&CoNum=" & strCoNum & "&CoLine=" & strCoLine & ""
        PostURL = PostURL & "&CoRelease=" & strCoRelease & "&OrderPickList=" & strOrderPickList & "&Loc=" & strLoc & "&ModeType=" & sModeType & ""

        Response.Redirect("ConfirmOrderPickList.aspx" & PostURL)

    End Sub

    Protected Sub btnback_Click(sender As Object, e As EventArgs) Handles btnback.Click

        Dim PostURL As String = ""
        Dim strCoNum As String = Request.QueryString("CoNum").ToString
        Dim strCoLine As String = Request.QueryString("CoLine").ToString
        Dim strCoRelease As String = Request.QueryString("CoRelease").ToString
        Dim strOrderPickList As String = Request.QueryString("OrderPickList").ToString
        Dim strLoc As String = Request.QueryString("Loc").ToString
        Dim sModeType As String = Request.QueryString("ModeType").ToString

        PostURL = "?SessionID=" & Session("PSession").ToString & "&CoNum=" & strCoNum & "&CoLine=" & strCoLine & ""
        PostURL = PostURL & "&CoRelease=" & strCoRelease & "&OrderPickList=" & strOrderPickList & "&Loc=" & strLoc & "&ModeType=" & sModeType & ""

        Response.Redirect("ConfirmOrderPickList.aspx" & PostURL)

    End Sub

    Protected Sub ListView1_ItemDataBound(ByVal sender As Object, ByVal e As ListViewItemEventArgs) Handles PanelList.ItemDataBound

        Dim QtyTag As Decimal = 0

        If e.Item.ItemType = ListViewItemType.DataItem Then

            Dim lblQtyTag As Label = CType(e.Item.FindControl("lblQtyTag"), Label)

            Decimal.TryParse(lblQtyTag.Text, QtyTag)

            lblQtyTag.Text = FormatNumber(QtyTag, LenPointQty)

        End If

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

#Region "Bind Data To Gridview"

    Sub BindGridview()

        Dim Filter As String
        Dim Propertie As String

        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet
        Filter = "SessionID='" & Session("PSession").ToString & "' and CoNum = '" & lblconum.Text & "' and CoLine = '" & lblcoline.Text & "' and CoRelease = '" & lblcorelease.Text & "'"
        Propertie = "DerCO, Item, QtyTag, Lot, TagID, ItemDescription"

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_EX_PickListTags", Propertie, Filter, "CreateDate Desc", "", 0)

        PanelList.DataSource = ds.Tables(0)
        PanelList.DataBind()


    End Sub
#End Region

End Class