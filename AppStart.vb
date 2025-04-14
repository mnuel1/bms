Module AppStart
    Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        ' Show Settings Form first
        Dim settingsForm As New ConnectSettingsForm()
        If settingsForm.ShowDialog() = DialogResult.OK Then
            ' If connection successful, show login
            Application.Run(New Login())
        Else
            ' Exit if user cancels
            Application.Exit()
        End If
    End Sub
End Module
