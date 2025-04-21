Imports System.Data
Imports MySql.Data.MySqlClient
Imports System.Drawing
Imports System.ComponentModel

Public Class AccountantPaymentControl
    Inherits UserControl

    ' Main controls
    Private dgvPayments As DataGridView
    Private pnlInputForm As Panel
    Private pnlButtons As FlowLayoutPanel

    ' Input controls
    Private txtAmountPaid, txtReference, txtBalance, txtDiscount, txtRefunded, txtRemarks, txtORNumber As TextBox
    Private cmbPaymentMethod, cmbPaymentStatus, cmbBookingID As ComboBox
    Private dtpPaymentDate, dtpPaymentTime As DateTimePicker

    ' Buttons
    Private btnAddPayment As Button
    Private btnClear As Button

    ' Colors for modern theme
    Private ReadOnly colorPrimary As Color = Color.FromArgb(100, 181, 246)  ' Light blue
    Private ReadOnly colorAccent As Color = Color.FromArgb(255, 179, 200)   ' Soft pink
    Private ReadOnly colorText As Color = Color.FromArgb(73, 80, 87)        ' Dark gray
    Private ReadOnly colorLight As Color = Color.FromArgb(248, 249, 250)    ' Off white
    Private ReadOnly colorBorder As Color = Color.FromArgb(222, 226, 230)   ' Light gray

    Public Sub New()
        InitializeComponent()
        SetupLayout()
        LoadPayments()
        SetCustomStyles()
    End Sub

    Private Sub AccountantPaymentControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Any additional loading logic
    End Sub

    Private Sub SetupLayout()
        ' Main container
        Dim mainContainer As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .RowCount = 3,
            .ColumnCount = 1,
            .Padding = New Padding(15),
            .BackColor = Color.White
        }

        mainContainer.RowStyles.Add(New RowStyle(SizeType.Absolute, 50))  ' Header
        mainContainer.RowStyles.Add(New RowStyle(SizeType.Percent, 40))   ' DataGrid
        mainContainer.RowStyles.Add(New RowStyle(SizeType.Percent, 60))   ' Input Form

        ' Header
        Dim lblHeader As New Label With {
            .Text = "Payment Management",
            .Font = New Font("Segoe UI", 16, FontStyle.Regular),
            .ForeColor = colorText,
            .Dock = DockStyle.Fill,
            .TextAlign = ContentAlignment.MiddleLeft
        }

        ' DataGridView for payments
        dgvPayments = New DataGridView With {
            .Dock = DockStyle.Fill,
            .ReadOnly = True,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AllowUserToAddRows = False,
            .MultiSelect = False,
            .BackgroundColor = Color.White,
            .BorderStyle = BorderStyle.None,
            .RowHeadersVisible = False,
            .AllowUserToResizeRows = False
        }

        ' Input panel with rounded corners
        pnlInputForm = New Panel With {
            .Dock = DockStyle.Fill,
            .Padding = New Padding(20),
            .BackColor = colorLight
        }

        ' Create input controls
        CreateInputControls()

        ' Button panel
        pnlButtons = New FlowLayoutPanel With {
            .FlowDirection = FlowDirection.RightToLeft,
            .Dock = DockStyle.Bottom,
            .Height = 60,
            .Padding = New Padding(0, 10, 0, 0)
        }

        btnAddPayment = New Button With {
            .Text = "Record Payment",
            .Size = New Size(150, 40),
            .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Segoe UI", 10, FontStyle.Regular),
            .Cursor = Cursors.Hand,
            .Margin = New Padding(10, 3, 3, 3)
        }
        AddHandler btnAddPayment.Click, AddressOf btnAddPayment_Click

        btnClear = New Button With {
            .Text = "Clear Form",
            .Size = New Size(120, 40),
            .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Segoe UI", 10, FontStyle.Regular),
            .Cursor = Cursors.Hand
        }
        AddHandler btnClear.Click, AddressOf btnClear_Click

        ' Add buttons to panel
        pnlButtons.Controls.Add(btnAddPayment)
        pnlButtons.Controls.Add(btnClear)

        ' Add panels to main container
        mainContainer.Controls.Add(lblHeader, 0, 0)
        mainContainer.Controls.Add(dgvPayments, 0, 1)
        mainContainer.Controls.Add(pnlInputForm, 0, 2)

        ' Add button panel to input panel
        pnlInputForm.Controls.Add(pnlButtons)

        ' Add main container to UserControl
        Me.Controls.Add(mainContainer)
    End Sub

    Private Sub CreateInputControls()
        ' Create a TableLayoutPanel for the form
        Dim inputLayout As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 4,
            .RowCount = 7,
            .Padding = New Padding(5)
        }

        ' Set column styles for even spacing
        inputLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20))
        inputLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30))
        inputLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20))
        inputLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30))

        ' Initialize input controls
        cmbBookingID = New ComboBox With {
            .Dock = DockStyle.Fill,
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Font = New Font("Segoe UI", 9.5F)
        }
        LoadBookingDropdown()

        txtAmountPaid = CreateTextBox()
        txtReference = CreateTextBox()
        txtBalance = CreateTextBox()
        txtDiscount = CreateTextBox()
        txtRefunded = CreateTextBox()
        txtRemarks = New TextBox() With {
            .Multiline = True,
            .Height = 60,
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.5F),
            .BorderStyle = BorderStyle.FixedSingle
        }
        txtORNumber = CreateTextBox()

        cmbPaymentMethod = CreateComboBox()
        cmbPaymentMethod.Items.AddRange({"Cash", "Card", "Bank Transfer", "Mobile Payment", "Check"})

        cmbPaymentStatus = CreateComboBox()
        cmbPaymentStatus.Items.AddRange({"Full", "Partial", "Overpaid"})

        dtpPaymentDate = New DateTimePicker With {
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.5F),
            .Format = DateTimePickerFormat.Short,
            .Value = DateTime.Now
        }

        dtpPaymentTime = New DateTimePicker With {
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.5F),
            .Format = DateTimePickerFormat.Time,
            .ShowUpDown = True,
            .Value = DateTime.Now
        }

        ' Add controls to layout - First column
        AddFormField(inputLayout, "Booking ID", cmbBookingID, 0, 0)
        AddFormField(inputLayout, "Amount Paid", txtAmountPaid, 0, 1)
        AddFormField(inputLayout, "Payment Method", cmbPaymentMethod, 0, 2)
        AddFormField(inputLayout, "Reference Number", txtReference, 0, 3)
        AddFormField(inputLayout, "Payment Status", cmbPaymentStatus, 0, 4)
        AddFormField(inputLayout, "OR Number", txtORNumber, 0, 5)
        AddFormField(inputLayout, "Remarks", txtRemarks, 0, 6)

        ' Second column
        AddFormField(inputLayout, "Payment Date", dtpPaymentDate, 2, 0)
        AddFormField(inputLayout, "Payment Time", dtpPaymentTime, 2, 1)
        AddFormField(inputLayout, "Balance", txtBalance, 2, 2)
        AddFormField(inputLayout, "Discount Amount", txtDiscount, 2, 3)
        AddFormField(inputLayout, "Refunded Amount", txtRefunded, 2, 4)

        ' Add layout to panel
        pnlInputForm.Controls.Add(inputLayout)
    End Sub

    Private Function CreateTextBox() As TextBox
        Return New TextBox With {
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.5F),
            .BorderStyle = BorderStyle.FixedSingle
        }
    End Function

    Private Function CreateComboBox() As ComboBox
        Return New ComboBox With {
            .Dock = DockStyle.Fill,
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Font = New Font("Segoe UI", 9.5F)
        }
    End Function

    Private Sub AddFormField(layout As TableLayoutPanel, labelText As String, control As Control, column As Integer, row As Integer)
        Dim lbl As New Label With {
            .Text = labelText,
            .Font = New Font("Segoe UI", 9.5F),
            .Dock = DockStyle.Fill,
            .TextAlign = ContentAlignment.MiddleLeft,
            .ForeColor = colorText
        }

        layout.Controls.Add(lbl, column, row)
        layout.Controls.Add(control, column + 1, row)
    End Sub

    Private Sub SetCustomStyles()
        ' DataGridView styling
        dgvPayments.EnableHeadersVisualStyles = False
        dgvPayments.ColumnHeadersDefaultCellStyle.BackColor = colorPrimary
        dgvPayments.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvPayments.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Regular)
        dgvPayments.ColumnHeadersHeight = 40
        dgvPayments.RowTemplate.Height = 30
        dgvPayments.DefaultCellStyle.Font = New Font("Segoe UI", 9.5F)
        dgvPayments.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
        dgvPayments.GridColor = colorBorder
        dgvPayments.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal

        ' Button styling
        btnAddPayment.BackColor = colorPrimary
        btnAddPayment.ForeColor = Color.White
        btnAddPayment.FlatAppearance.BorderSize = 0

        btnClear.BackColor = Color.White
        btnClear.ForeColor = colorText
        btnClear.FlatAppearance.BorderColor = colorBorder
        btnClear.FlatAppearance.BorderSize = 1

        ' Set rounded corners for panels
        SetRoundedCorners(pnlInputForm)

        ' Set input borders
        SetControlBorders()
    End Sub

    Private Sub SetRoundedCorners(panel As Panel)
        AddHandler panel.Paint, Sub(sender As Object, e As PaintEventArgs)
                                    Dim radius As Integer = 10
                                    Dim rect As New Rectangle(0, 0, panel.Width - 1, panel.Height - 1)
                                    Dim path As New Drawing2D.GraphicsPath()

                                    ' Top left corner
                                    path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90)
                                    ' Top right corner
                                    path.AddArc(rect.Width - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90)
                                    ' Bottom right corner
                                    path.AddArc(rect.Width - radius * 2, rect.Height - radius * 2, radius * 2, radius * 2, 0, 90)
                                    ' Bottom left corner
                                    path.AddArc(rect.X, rect.Height - radius * 2, radius * 2, radius * 2, 90, 90)
                                    path.CloseAllFigures()

                                    e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                                    e.Graphics.DrawPath(New Pen(colorBorder, 1), path)
                                End Sub
    End Sub


    Private Sub SetControlBorders()
        ' Add event handlers for all text boxes to give focus effect
        For Each ctrl As Control In GetAllControls(pnlInputForm)
            If TypeOf ctrl Is TextBox Then
                Dim txt As TextBox = DirectCast(ctrl, TextBox)
                txt.BorderStyle = BorderStyle.FixedSingle

                AddHandler txt.Enter, Sub(sender As Object, e As EventArgs)
                                          txt.BackColor = Color.FromArgb(230, 241, 255)
                                      End Sub

                AddHandler txt.Leave, Sub(sender As Object, e As EventArgs)
                                          txt.BackColor = Color.White
                                      End Sub
            ElseIf TypeOf ctrl Is ComboBox Then
                Dim cmb As ComboBox = DirectCast(ctrl, ComboBox)

                AddHandler cmb.Enter, Sub(sender As Object, e As EventArgs)
                                          cmb.BackColor = Color.FromArgb(230, 241, 255)
                                      End Sub

                AddHandler cmb.Leave, Sub(sender As Object, e As EventArgs)
                                          cmb.BackColor = Color.White
                                      End Sub
            ElseIf TypeOf ctrl Is DateTimePicker Then
                Dim dtp As DateTimePicker = DirectCast(ctrl, DateTimePicker)

                AddHandler dtp.Enter, Sub(sender As Object, e As EventArgs)
                                          dtp.BackColor = Color.FromArgb(230, 241, 255)
                                      End Sub

                AddHandler dtp.Leave, Sub(sender As Object, e As EventArgs)
                                          dtp.BackColor = Color.White
                                      End Sub
            End If
        Next
    End Sub

    Private Function GetAllControls(container As Control) As List(Of Control)
        Dim allControls As New List(Of Control)
        For Each ctrl As Control In container.Controls
            allControls.Add(ctrl)
            If ctrl.HasChildren Then
                allControls.AddRange(GetAllControls(ctrl))
            End If
        Next
        Return allControls
    End Function

    Private Sub LoadPayments()
        Dim query As String = "SELECT 
                               PaymentID, 
                               BookingID, 
                               DATE_FORMAT(PaymentDate, '%d %b %Y') AS PaymentDate, 
                               AmountPaid, 
                               PaymentMethod, 
                               ReferenceNumber, 
                               PaymentStatus, 
                               ProcessedBy, 
                               PaymentTime,
                               Balance, 
                               DiscountAmount, 
                               RefundedAmount, 
                               ORNumber, 
                               Remarks 
                               FROM payment ORDER BY PaymentDate DESC, PaymentTime DESC"
        dgvPayments.DataSource = GetData(query)

        ' Customize column headers for better readability
        If dgvPayments.Columns.Count > 0 Then
            dgvPayments.Columns("PaymentID").HeaderText = "ID"
            dgvPayments.Columns("BookingID").HeaderText = "Booking ID"
            dgvPayments.Columns("PaymentDate").HeaderText = "Date"
            dgvPayments.Columns("AmountPaid").HeaderText = "Amount"
            dgvPayments.Columns("PaymentMethod").HeaderText = "Method"
            dgvPayments.Columns("ReferenceNumber").HeaderText = "Reference #"
            dgvPayments.Columns("PaymentStatus").HeaderText = "Status"
            dgvPayments.Columns("ProcessedBy").HeaderText = "Processed By"
            dgvPayments.Columns("PaymentTime").HeaderText = "Time"
            dgvPayments.Columns("Balance").HeaderText = "Balance"
            dgvPayments.Columns("DiscountAmount").HeaderText = "Discount"
            dgvPayments.Columns("RefundedAmount").HeaderText = "Refunded"
            dgvPayments.Columns("ORNumber").HeaderText = "OR #"
            dgvPayments.Columns("Remarks").HeaderText = "Remarks"

            ' Hide some columns to avoid cluttering
            dgvPayments.Columns("Remarks").Visible = False
        End If
    End Sub

    Private Sub btnAddPayment_Click(sender As Object, e As EventArgs)
        If cmbBookingID.SelectedItem Is Nothing OrElse String.IsNullOrWhiteSpace(txtAmountPaid.Text) Then
            ShowCustomMessage("Booking ID and Amount Paid are required fields.", "Validation Error")
            Return
        End If

        ' Validation for amount paid
        Dim amountPaid As Decimal
        If Not Decimal.TryParse(txtAmountPaid.Text, amountPaid) OrElse amountPaid <= 0 Then
            ShowCustomMessage("Please enter a valid amount paid.", "Validation Error")
            Return
        End If

        Dim query As String = "INSERT INTO payment (BookingID, PaymentDate, AmountPaid, PaymentMethod, ReferenceNumber, PaymentStatus, 
                               ProcessedBy, PaymentTime, Balance, DiscountAmount, RefundedAmount, Remarks, ORNumber, CreatedAt)
                               VALUES (@BookingID, @PaymentDate, @AmountPaid, @PaymentMethod, @ReferenceNumber, @PaymentStatus, 
                               @ProcessedBy, @PaymentTime, @Balance, @DiscountAmount, @RefundedAmount, @Remarks, @ORNumber, NOW())"

        Dim parameters As New Dictionary(Of String, Object) From {
            {"@BookingID", CType(cmbBookingID.SelectedItem, ComboItem).Value},
            {"@PaymentDate", dtpPaymentDate.Value},
            {"@AmountPaid", txtAmountPaid.Text},
            {"@PaymentMethod", If(cmbPaymentMethod.SelectedItem IsNot Nothing, cmbPaymentMethod.Text, "Cash")},
            {"@ReferenceNumber", txtReference.Text},
            {"@PaymentStatus", If(cmbPaymentStatus.SelectedItem IsNot Nothing, cmbPaymentStatus.Text, "Full")},
            {"@ProcessedBy", SessionInfo.LoggedInUserFullName},
            {"@PaymentTime", dtpPaymentTime.Value.ToString("HH:mm:ss")},
            {"@Balance", If(String.IsNullOrEmpty(txtBalance.Text), "0", txtBalance.Text)},
            {"@DiscountAmount", If(String.IsNullOrEmpty(txtDiscount.Text), "0", txtDiscount.Text)},
            {"@RefundedAmount", If(String.IsNullOrEmpty(txtRefunded.Text), "0", txtRefunded.Text)},
            {"@Remarks", txtRemarks.Text},
            {"@ORNumber", txtORNumber.Text}
        }

        If ExecuteQuery(query, parameters) Then
            ShowSuccessMessage("Payment recorded successfully!")
            LoadPayments()
            ClearFields()
        Else
            ShowCustomMessage("Failed to record payment. Please try again.", "Error")
        End If
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs)
        ClearFields()
    End Sub

    Private Sub ShowCustomMessage(message As String, title As String)
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub ShowSuccessMessage(message As String)
        MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub LoadBookingDropdown()
        Dim query As String = "
        SELECT b.BookingID, c.FirstName, c.LastName, e.EventName, s.ServiceName 
        FROM booking b
        JOIN customer c ON b.CustomerID = c.CustomerID
        JOIN event e ON b.EventID = e.EventID
        JOIN service_availed s ON b.ServiceID = s.ServiceID
        ORDER BY b.BookingID DESC"

        Dim dt As DataTable = GetData(query)

        cmbBookingID.Items.Clear()
        For Each row As DataRow In dt.Rows
            Dim bookingId = row("BookingID")
            Dim fullName = $"{row("FirstName")} {row("LastName")}"
            Dim eventName = Abbreviate(row("EventName").ToString())
            Dim serviceName = Abbreviate(row("ServiceName").ToString())
            Dim displayText = $"{bookingId} - {fullName} ({eventName}/{serviceName})"
            cmbBookingID.Items.Add(New ComboItem(displayText, bookingId))
        Next

        If cmbBookingID.Items.Count > 0 Then
            cmbBookingID.SelectedIndex = 0
        End If
    End Sub

    Private Function Abbreviate(name As String) As String
        If name.Length > 10 Then
            Return name.Substring(0, 10) & "..."
        End If
        Return name
    End Function

    Private Sub ClearFields()
        txtAmountPaid.Clear()
        txtReference.Clear()
        txtBalance.Clear()
        txtDiscount.Clear()
        txtRefunded.Clear()
        txtRemarks.Clear()
        txtORNumber.Clear()

        If cmbBookingID.Items.Count > 0 Then
            cmbBookingID.SelectedIndex = 0
        Else
            cmbBookingID.SelectedIndex = -1
        End If

        cmbPaymentMethod.SelectedIndex = 0
        cmbPaymentStatus.SelectedIndex = 0
        dtpPaymentDate.Value = DateTime.Now
        dtpPaymentTime.Value = DateTime.Now
    End Sub

    ' This is the ComboItem class needed for storing both display text and value
    Private Class ComboItem
        Public Property Text As String
        Public Property Value As Object

        Public Sub New(text As String, value As Object)
            Me.Text = text
            Me.Value = value
        End Sub

        Public Overrides Function ToString() As String
            Return Text
        End Function
    End Class
End Class