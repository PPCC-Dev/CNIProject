Imports System.Data
Imports System.Xml

Public Class Signin
    Inherits System.Web.UI.Page

    Dim oWS As SRNService.DOWebServiceSoapClient
    Dim ds As DataSet
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

        If Not Page.IsPostBack Then

            oWS = New SRNService.DOWebServiceSoapClient
            For Each str As String In oWS.GetConfigurationNames
                ddlconfig.Items.Add(New ListItem(str, str))
            Next

            Dim SLConfig As String = System.Configuration.ConfigurationManager.AppSettings("Configuration")
            Dim lstConfig As ListItem = ddlconfig.Items.FindByValue(Convert.ToString(SLConfig))

            If SLConfig Is Nothing Or lstConfig Is Nothing Then
                ddlconfig.Enabled = True
            Else
                ddlconfig.SelectedValue = SLConfig.ToString
            End If

        End If

    End Sub

    Protected Sub btnlogin_Click(sender As Object, e As EventArgs) Handles btnlogin.Click

        Try
            Dim Token As String = ""
            Dim UserName As String = Trim(txtusername.Text)
            Dim Password As String = txtpassword.Text
            Dim Config As String = Convert.ToString(ddlconfig.SelectedValue)
            Dim Parms As String = ""
            Dim UserCodes As String = ""

            oWS = New SRNService.DOWebServiceSoapClient
            Token = oWS.CreateSessionToken(UserName, Password, Config)

            Session("Token") = Token
            Session("UserName") = UserName
            Session("Config") = Replace(Config, "_", " ")

            If Not IsNothing(Session("Token")) Then

                ParmSite = GetSite()

                'Parms = "<Parameters><Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & _
                '        "<Parameter>" & Session("UserName").ToString & "</Parameter>" & _
                '        "<Parameter>" & ParmSite.ToString & "</Parameter>" & _
                '        "</Parameters>"

                'oWS = New SRNService.DOWebServiceSoapClient
                'oWS.CallMethod(Session("Token").ToString, "PPCC_TagTransferShips", "PPCC_GetSessionIDSp", Parms)

                'Dim doc As XmlDocument = New XmlDocument()
                'doc.LoadXml(Parms)

                'Dim i As Integer = 1

                'For Each node As XmlNode In doc.DocumentElement

                '    If i = 1 Then
                '        Session("PSession") = node.InnerText
                '        Exit For
                '    End If

                '    i += 1
                'Next

            End If

            Dim time As String = System.Configuration.ConfigurationManager.AppSettings("PageTimeOut")

            If time Is Nothing Then
                time = "30"
            End If

            'MsgBox(Session("PSession").ToString)

            Session("Token") = Token
            Session.Timeout = time

            Response.Redirect("Menu.aspx")

        Catch ex As Exception

            NotificationPanel.Visible = True

            Dim end_pos As Integer = InStr(ex.Message, "at IDOWebService.ThrowSoapException")
            If end_pos > 0 And InStr(ex.Message, "System.Web.Services.Protocols.SoapException:") > 0 Then
                Literal1.Text = Left(ex.Message, IIf(end_pos = 0, 0, end_pos - 1)).Replace("System.Web.Services.Protocols.SoapException:", "")
            Else
                Literal1.Text = ex.Message
            End If

        End Try

    End Sub

    Function GetSite() As String

        GetSite = ""

        oWS = New SRNService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLParms", "Site", "", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetSite = ds.Tables(0).Rows(0)("Site").ToString
        End If

        Return GetSite

    End Function

End Class