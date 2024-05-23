Public Class Signout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim Parms As String
        Dim oWS As CNIService.DOWebServiceSoapClient

        Parms = ""

        Parms = "<Parameters><Parameter>" & Session("UserName").ToString & "</Parameter>" &
                    "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_TmpProductionTags", "PPCC_DeleteTmpProductionTag", Parms)

        Parms = ""

        Parms = "<Parameters><Parameter>" & Session("UserName").ToString & "</Parameter>" &
                "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_TmpWIPTags", "PPCC_DeleteWIPTagSp", Parms)


        Session.Abandon()
        Response.Redirect("signin.aspx")
    End Sub

End Class