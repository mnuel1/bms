Public Class ClerkDashboard
    Private Sub LoadContent(newContent As UserControl)
        SplitContainer1.Panel2.Controls.Clear()
        newContent.Dock = DockStyle.Fill
        SplitContainer1.Panel2.Controls.Add(newContent)
    End Sub

    Private Sub ClerkDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadContent(New accountantBookingControl()) ' Load Booking as default
    End Sub

    Private Sub bookingsNav_Click(sender As Object, e As EventArgs) Handles bookingsNav.Click
        LoadContent(New accountantBookingControl())
    End Sub

    Private Sub paymentNav_Click(sender As Object, e As EventArgs) Handles paymentNav.Click
        LoadContent(New accountantPaymentControl())
    End Sub

    Private Sub refundNav_Click(sender As Object, e As EventArgs) Handles refundNav.Click
        LoadContent(New accountantRefundControl())
    End Sub

    Private Sub reportsNav_Click(sender As Object, e As EventArgs) Handles reportsNav.Click
        LoadContent(New accountantReportControl())
    End Sub

    Private Sub logout_Click(sender As Object, e As EventArgs) Handles accountantLogout.Click
        Me.Hide()
        Login.Show()
    End Sub

    Private Sub SplitContainer1_Panel1_Paint(sender As Object, e As PaintEventArgs) Handles SplitContainer1.Panel1.Paint
        ' You can leave this empty or add custom design logic
    End Sub

    Private Sub SplitContainer1_Panel2_Paint(sender As Object, e As PaintEventArgs) Handles SplitContainer1.Panel2.Paint

    End Sub
End Class
