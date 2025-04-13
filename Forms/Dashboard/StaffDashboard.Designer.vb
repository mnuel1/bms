<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class StaffDashboard
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.staffLogout = New System.Windows.Forms.Button()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.venueNav = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.bookingNav = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.customersNav = New System.Windows.Forms.Button()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.Panel5.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.staffLogout)
        Me.Panel5.Location = New System.Drawing.Point(3, 340)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(209, 45)
        Me.Panel5.TabIndex = 9
        '
        'staffLogout
        '
        Me.staffLogout.Location = New System.Drawing.Point(0, 4)
        Me.staffLogout.Name = "staffLogout"
        Me.staffLogout.Size = New System.Drawing.Size(206, 38)
        Me.staffLogout.TabIndex = 0
        Me.staffLogout.Text = "Logout"
        Me.staffLogout.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.venueNav)
        Me.Panel3.Location = New System.Drawing.Point(3, 141)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(209, 45)
        Me.Panel3.TabIndex = 7
        '
        'venueNav
        '
        Me.venueNav.Location = New System.Drawing.Point(0, 4)
        Me.venueNav.Name = "venueNav"
        Me.venueNav.Size = New System.Drawing.Size(206, 38)
        Me.venueNav.TabIndex = 0
        Me.venueNav.Text = "Venues"
        Me.venueNav.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.bookingNav)
        Me.Panel2.Location = New System.Drawing.Point(3, 90)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(209, 45)
        Me.Panel2.TabIndex = 6
        '
        'bookingNav
        '
        Me.bookingNav.Location = New System.Drawing.Point(0, 4)
        Me.bookingNav.Name = "bookingNav"
        Me.bookingNav.Size = New System.Drawing.Size(206, 38)
        Me.bookingNav.TabIndex = 0
        Me.bookingNav.Text = "Bookings"
        Me.bookingNav.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.customersNav)
        Me.Panel1.Location = New System.Drawing.Point(3, 39)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(209, 45)
        Me.Panel1.TabIndex = 5
        '
        'customersNav
        '
        Me.customersNav.Location = New System.Drawing.Point(0, 4)
        Me.customersNav.Name = "customersNav"
        Me.customersNav.Size = New System.Drawing.Size(206, 38)
        Me.customersNav.TabIndex = 0
        Me.customersNav.Text = "Customers"
        Me.customersNav.UseVisualStyleBackColor = True
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel5)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel3)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel1)
        Me.SplitContainer1.Panel1.Padding = New System.Windows.Forms.Padding(5)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Padding = New System.Windows.Forms.Padding(5)
        Me.SplitContainer1.Size = New System.Drawing.Size(800, 450)
        Me.SplitContainer1.SplitterDistance = 214
        Me.SplitContainer1.TabIndex = 1
        '
        'StaffDashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.SplitContainer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "StaffDashboard"
        Me.Text = "StaffDashboard"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel5.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel5 As Panel
    Friend WithEvents staffLogout As Button
    Friend WithEvents Panel3 As Panel
    Friend WithEvents venueNav As Button
    Friend WithEvents Panel2 As Panel
    Friend WithEvents bookingNav As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents customersNav As Button
    Friend WithEvents SplitContainer1 As SplitContainer
End Class
