Option Explicit On
Imports System.Windows.Interop
Imports Microsoft.VisualBasic.FileIO
Imports System.IO

Class MainWindow
    Protected Overrides Sub OnSourceInitialized(ByVal e As System.EventArgs)
        MyBase.OnSourceInitialized(e)
        ExtendGlass()
        GridLogin.Visibility = Windows.Visibility.Hidden
        If FileIO.FileSystem.FileExists("config") Then
            Dim sr = FileIO.FileSystem.OpenTextFileReader("config")
            CheckBox01.IsChecked = True
            TextBox01.Text = sr.ReadLine
            PasswordBox1.Password = sr.ReadLine
            sr.Close()
        End If
    End Sub
    Protected Overrides Sub OnMouseLeftButtonDown(e As MouseButtonEventArgs)
        MyBase.OnMouseLeftButtonDown(e)
        Me.DragMove()
    End Sub
    Private Declare Sub DwmIsCompositionEnabled Lib "dwmapi.dll" (ByRef b As Boolean)
    Private Declare Sub DwmExtendFrameIntoClientArea Lib "dwmapi.dll" (ByVal hWnd As IntPtr, ByRef pMarInset As Margins)
    Private Structure Margins
        Public Sub New(ByVal t As Thickness)
            Left = CInt(t.Left)
            Right = CInt(t.Right)
            Top = CInt(t.Top)
            Bottom = CInt(t.Bottom)
        End Sub
        Public Left As Integer
        Public Right As Integer
        Public Top As Integer
        Public Bottom As Integer
    End Structure
    Private Sub ExtendGlass()
        Try
            Dim b As Boolean
            DwmIsCompositionEnabled(b)
            If b Then
                Dim hWnd As IntPtr = New WindowInteropHelper(Me).Handle
                If hWnd <> IntPtr.Zero Then
                    Me.Background = Brushes.Transparent
                    HwndSource.FromHwnd(hWnd).CompositionTarget.BackgroundColor = Colors.Transparent
                    Dim m As New Margins(New Thickness(-1))
                    DwmExtendFrameIntoClientArea(hWnd, m)
                End If
            End If
        Catch
        End Try
    End Sub

    Dim account As String
    Dim lesson As String
    Dim scoreo0 As String
    Dim scoreo1 As String
    Dim scoreo2 As String
    Dim scoreo3 As String
    Dim scoreo4 As String
    Dim score0 As String
    Dim score1 As String
    Dim score2 As String
    Dim score3 As String
    Dim score4 As String
    Dim time As String
    Dim testmode As String
    Public cookiesend As String
    Dim dataet As String

    Dim username As String
    Dim password As String
    Dim cookieweb As String
    Dim cookieweb2 As String
    Dim accountid As String
    Dim accountid0 As String
    Dim aspsession As String
    Dim lastsessionid As String
    Dim ecpacid0() As String
    Dim ecpacid As String
    Dim data As String

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        scoreo0 = TextBox4.Text
        scoreo1 = TextBox5.Text
        scoreo2 = TextBox6.Text
        scoreo3 = TextBox7.Text
        scoreo4 = TextBox8.Text
        scoreo0 = (Val(scoreo1) + Val(scoreo2) + Val(scoreo3) + Val(scoreo4)) / 4

        score1 = Val(scoreo1) + Format(Rnd(), ".00")
        score2 = Val(scoreo2) + Format(Rnd(), ".00")
        score3 = Val(scoreo3) + Format(Rnd(), ".00")
        score4 = Val(scoreo4) + Format(Rnd(), ".00")
        score0 = Format((Val(score1) + Val(score2) + Val(score3) + Val(score4)) / 4, ".00")

        TextBox4.Text = scoreo0
        TextBox9.Text = score0
        TextBox10.Text = score1
        TextBox11.Text = score2
        TextBox12.Text = score3
        TextBox13.Text = score4
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        cookiesend = TextBox14.Text
        Dim web As New System.Net.WebClient()
        With web
            .Headers.Clear()
            .Headers.Add("Accept", "*/*")
            .Headers.Add("Accept-Language", "zh-CN")
            .Headers.Add("Referer", "http://cn.myet.com/Upload/ElizaWeb4Content/FlashSettings/LessonSelfTest.swf?CultureName=zh-CN&MyETURLPrefix=http://cn.myet.com/ElizaWeb&Sock")
            .Headers.Add("x-flash-version", "11,5,502,149")
            .Headers.Add("Content-Type", "application/x-www-form-urlencoded")
            .Headers.Add("Accept-Encoding", "gzip, deflate")
            .Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/6.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; WWTClient2; .NET4.0C; .NET4.0E; BRI/2; InfoPath.3; MASP)")
            .Headers.Add("Pragma", "no-cache")
            .Headers.Add("Cookie", cookiesend)
        End With
        account = TextBox2.Text
        lesson = TextBox3.Text
        score0 = TextBox9.Text
        score1 = TextBox10.Text
        score2 = TextBox11.Text
        score3 = TextBox12.Text
        score4 = TextBox13.Text
        time = TextBox15.Text
        If CheckBox1.IsChecked = True Then
            testmode = 1
        Else
            testmode = 0
        End If

        Dim d As String
        d = "<ScoreInfo AccountID=""" + account + """ LessonID=""" + lesson + """ Score=""" + score0 + """ PronunciationScore=""" + score1 + """ PitchScore=""" + score2 + """ TimingScore=""" + score3 + """ IntensityScore=""" + score4 + """ TimeElapsed=""" + time + """ WhereFrom="""" TestMode=""" + testmode + """> <ignoreWhitespace>false</ignoreWhitespace> </ScoreInfo>"
        Dim randomid As String
        randomid = Format(Rnd(), ".000000000") + Rnd() / 8
        dataet = "RandomID=" + randomid + "&FlashData=" + d
        Dim ok As Integer
        ok = MsgBox("Are you sure?", MsgBoxStyle.OkCancel, "Confirm")
        If ok = 1 Then
            Try
                web.UploadString("http://cn.myet.com/ElizaWeb/LessonSelfTestServices.aspx?op=UploadSpeakingScore", "POST", dataet)
            Catch ex As Exception
                MsgBox("..." & vbCrLf & ex.Message)
            End Try
            MsgBox("Success", MsgBoxStyle.Information, "YourET")
        Else
            MsgBox("Canceled", MsgBoxStyle.Information, "YourET")
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As RoutedEventArgs) Handles Button3.Click
        Dim about = New AboutWindow
        about.ShowDialog()
    End Sub

    Private Sub ButtonL1_Click(sender As Object, e As RoutedEventArgs) Handles ButtonL1.Click
        GridAccount.Visibility = Windows.Visibility.Hidden
        GridLogin.Visibility = Windows.Visibility.Visible
    End Sub

    Private Sub ButtonL3_Click(sender As Object, e As RoutedEventArgs) Handles ButtonL3.Click
        GridAccount.Visibility = Windows.Visibility.Visible
        GridLogin.Visibility = Windows.Visibility.Hidden
    End Sub

    Private Sub ButtonL4_Click(sender As Object, e As RoutedEventArgs) Handles ButtonL4.Click
        username = TextBox01.Text
        password = PasswordBox1.Password
        If CheckBox01.IsChecked = True Then
            Try
                Dim sw = FileIO.FileSystem.OpenTextFileWriter("config", False)
                sw.WriteLine(username)
                sw.WriteLine(password)
                sw.Close()
            Catch ex As Exception
            End Try
        End If
        If CheckBox01.IsChecked = False Then
            Try
                FileIO.FileSystem.DeleteFile("config")
            Catch ex As Exception
            End Try
        End If

        data = "__VIEWSTATE=&hdnCultureName=zh-CN&hdnSysAdminBulletinStatus=None&ReturnUrl=LoginPost.aspx&hdnPassword=&hdnWarningMsg=&hdnAutoLogin=N&hdnIsFirstLogin=Y&hdnLoginVerifyUrl=&hdnClientIP=&UserName=" + username + "&Password=" + password + "&btnLogin=%E7%99%BB%E5%BD%95"
        Dim web As New System.Net.WebClient()
        With web
            .Headers.Add("Accept", "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, application/x-shockwave-flash, */*")
            .Headers.Add("Accept-Language", "zh-CN")
            .Headers.Add("Accept-Encoding", "gzip, deflate")
            .Headers.Add("Content-Type", "application/x-www-form-urlencoded")
            .Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/6.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; WWTClient2; .NET4.0C; .NET4.0E; BRI/2; InfoPath.3; MASP)")
            .Headers.Add("Referer", "http://cn.myet.com/ElizaWeb/Alliance/MyET_Login.aspx?EptParams=BTKH2sQnFyVfRAldwWOL7CVXYIplizhUheIcGB2ULMs=&LoginVerifyUrl=")
        End With
        web.UploadString("http://cn.myet.com/ElizaWeb/Authentication/ValidateMyETUsernameNPassword.aspx?SaveAccount=N", data)
        aspsession = web.ResponseHeaders.Get("Set-Cookie").Substring(18, 24)

        Dim url0 As String
        cookieweb = "LLang=EN; TargetServerKey=CN1-LLabs; APVersion=5509; WPort=49156; LastSessionID=" + aspsession + "; RememberMyAccount=N; ASP.NET_SessionId=" + aspsession + "; ContentProvider=; IsCookieSupported=Y; IsAllianceAccount=N; TmpRememberMyAccount=N"
        Dim web2 As New System.Net.WebClient()
        With web2
            .Headers.Add("Accept", "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, application/x-shockwave-flash, */*")
            .Headers.Add("Accept-Language", "zh-CN")
            .Headers.Add("Accept-Encoding", "gzip, deflate")
            .Headers.Add("Content-Type", "application/x-www-form-urlencoded")
            .Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/6.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; WWTClient2; .NET4.0C; .NET4.0E; BRI/2; InfoPath.3; MASP)")
            .Headers.Add("Referer", "http://cn.myet.com/ElizaWeb/Authentication/ValidateMyETUsernameNPassword.aspx?SaveAccount=N")
            .Headers.Add("Cookie", cookieweb)
        End With
        url0 = "http://cn.myet.com/ElizaWeb/Authentication/LoginPost.aspx?ESID=" + aspsession + "&UserName=" + username
        web2.DownloadString(url0)
        Dim errorstate As Integer = 0
        Try
            ecpacid0 = web2.ResponseHeaders.Get("Set-Cookie").Split("; path=/")
            ecpacid = ecpacid0(2).Substring(16, 24)
        Catch ex As Exception
            MsgBox("Error", MsgBoxStyle.Critical, "YourET")
            errorstate = 1
        End Try
        If errorstate = 1 Then
        Else
            Dim web3 As New System.Net.WebClient()
            With web3
                .Headers.Add("Accept", "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, application/x-shockwave-flash, */*")
                .Headers.Add("Accept-Language", "zh-CN")
                .Headers.Add("Accept-Encoding", "gzip, deflate")
                .Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/6.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; WWTClient2; .NET4.0C; .NET4.0E; BRI/2; InfoPath.3; MASP)")
                .Headers.Add("Referer", "http://cn.myet.com/ElizaWeb/Home.aspx")
                .Headers.Add("Cookie", cookieweb)
                .Headers.Add("EcpACID", ecpacid)
                .Headers.Add("OnlinePurchaseEntrance", "ELIZAWEB")
                .Headers.Add("IsWebVersion", "false")
                .Headers.Add("TmpPassword", password)
            End With
            accountid0 = web3.DownloadString("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx")
            accountid = accountid0.Substring(195, 6)

            cookieweb2 = "LLang=EN; TargetServerKey=CN1-LLabs; APVersion=5509; WPort=49155; LastSessionID=" + aspsession + "; MyETAccountID=" + username + "; RememberMyAccount=N; LastLessonID=CN-PEP-XXQ-00033; ASP.NET_SessionId=" + aspsession + "; ContentProvider=; IsCookieSupported=Y; IsAllianceAccount=N; TmpRememberMyAccount=Y; TmpPassword=" + password + "; EcpACID=" + ecpacid + "; CourseIDInSessionInfo=PEP-XXQ-005"
            TextBox14.Text = cookieweb2
            TextBox2.Text = accountid
        End If
        GridAccount.Visibility = Windows.Visibility.Visible
        GridLogin.Visibility = Windows.Visibility.Hidden
    End Sub

    Private Sub ButtonL2_Click(sender As Object, e As RoutedEventArgs) Handles ButtonL2.Click
        Dim web4 As New System.Net.WebClient()
        With web4
            .Headers.Add("Accept", "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, application/x-shockwave-flash, */*")
            .Headers.Add("Accept-Language", "zh-CN")
            .Headers.Add("Accept-Encoding", "gzip, deflate")
            .Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/6.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; WWTClient2; .NET4.0C; .NET4.0E; BRI/2; InfoPath.3; MASP)")
            .Headers.Add("Referer", "http://cn.myet.com/ElizaWeb/Home.aspx")
            .Headers.Add("Cookie", cookieweb)
        End With
        web4.DownloadString("http://cn.myet.com/ElizaWeb/Logout.aspx")
        TextBox2.Text = ""
        TextBox14.Text = ""
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        Dim WebBrowser1 As New WebBrowser
        WebBrowser1.aspsession = aspsession
        WebBrowser1.ecpacid = ecpacid
        WebBrowser1.ShowDialog()
        TextBox3.Text = WebBrowser1.lesson
    End Sub
End Class


