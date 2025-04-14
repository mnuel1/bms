Imports System.Data
Imports MySql.Data.MySqlClient

Public Class AccountantPaymentControl
    Inherits UserControl

    Private dgvPayments As DataGridView

    ' Input controls
    Private txtAmountPaid, txtReference, txtBalance, txtDiscount, txtRefunded, txtRemarks, txtORNumber As TextBox

    Private cmbPaymentMethod, cmbPaymentStatus, cmbBookingID As ComboBox
    Private dtpPaymentDate, dtpPaymentTime As DateTimePicker


    Public Sub New()
        InitializeComponent()
        SetupLayout()
        LoadPayments()
    End Sub

    Private Sub SetupLayout()
        dgvPayments = New DataGridView With {
            .Dock = DockStyle.Top,
            .Height = 250,
            .ReadOnly = True,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AllowUserToAddRows = False
        }

        ' Inputs        
        txtAmountPaid = New TextBox()
        txtReference = New TextBox()
        txtBalance = New TextBox()
        txtDiscount = New TextBox()
        txtRefunded = New TextBox()
        txtRemarks = New TextBox()
        txtORNumber = New TextBox()


        cmbBookingID = New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList}
        LoadBookingDropdown()
        cmbBookingID.AutoSize = True

        cmbPaymentMethod = New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList}
        cmbPaymentMethod.Items.AddRange({"Cash", "Card", "Bank Transfer"})

        cmbPaymentStatus = New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList}
        cmbPaymentStatus.Items.AddRange({"Full", "Partial", "Overpaid"})

        dtpPaymentDate = New DateTimePicker()
        dtpPaymentTime = New DateTimePicker With {
            .Format = DateTimePickerFormat.Time,
            .ShowUpDown = True
        }

        ' Buttons
        Dim btnAddPayment As New Button With {.Text = "Record Payment"}
        AddHandler btnAddPayment.Click, AddressOf btnAddPayment_Click

        ' Layout Panel
        Dim layout As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 2,
            .RowCount = 13,
            .Padding = New Padding(10)
        }
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30))
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 70))

        layout.Controls.Add(New Label With {.Text = "Booking ID"}, 0, 0)
        layout.Controls.Add(cmbBookingID, 1, 0)

        layout.Controls.Add(New Label With {.Text = "Amount Paid"}, 0, 1)
        layout.Controls.Add(txtAmountPaid, 1, 1)

        layout.Controls.Add(New Label With {.Text = "Payment Method"}, 0, 2)
        layout.Controls.Add(cmbPaymentMethod, 1, 2)

        layout.Controls.Add(New Label With {.Text = "Reference Number"}, 0, 3)
        layout.Controls.Add(txtReference, 1, 3)

        layout.Controls.Add(New Label With {.Text = "Payment Status"}, 0, 4)
        layout.Controls.Add(cmbPaymentStatus, 1, 4)

        layout.Controls.Add(New Label With {.Text = "Payment Date"}, 0, 6)
        layout.Controls.Add(dtpPaymentDate, 1, 6)

        layout.Controls.Add(New Label With {.Text = "Payment Time"}, 0, 7)
        layout.Controls.Add(dtpPaymentTime, 1, 7)

        layout.Controls.Add(New Label With {.Text = "Balance"}, 0, 8)
        layout.Controls.Add(txtBalance, 1, 8)

        layout.Controls.Add(New Label With {.Text = "Discount Amount"}, 0, 9)
        layout.Controls.Add(txtDiscount, 1, 9)

        layout.Controls.Add(New Label With {.Text = "Refunded Amount"}, 0, 10)
        layout.Controls.Add(txtRefunded, 1, 10)

        layout.Controls.Add(New Label With {.Text = "OR Number"}, 0, 11)
        layout.Controls.Add(txtORNumber, 1, 11)

        layout.Controls.Add(New Label With {.Text = "Remarks"}, 0, 12)
        layout.Controls.Add(txtRemarks, 1, 12)

        ' Bottom panel
        Dim btnPanel As New FlowLayoutPanel With {.FlowDirection = FlowDirection.LeftToRight, .Dock = DockStyle.Bottom}
        btnPanel.Controls.Add(btnAddPayment)

        ' Add to UserControl
        Me.Controls.Add(layout)
        Me.Controls.Add(dgvPayments)
        Me.Controls.Add(btnPanel)
    End Sub

    Private Sub LoadPayments()
        Dim query As String = "SELECT PaymentID, BookingID, PaymentDate, AmountPaid, PaymentMethod, ReferenceNumber, PaymentStatus, ProcessedBy, PaymentTime,
                               Balance, DiscountAmount, RefundedAmount, ORNumber, Remarks 
                               FROM payment ORDER BY PaymentDate DESC"
        dgvPayments.DataSource = GetData(query)
    End Sub

    Private Sub btnAddPayment_Click(sender As Object, e As EventArgs)
        If String.IsNullOrWhiteSpace(CType(cmbBookingID.SelectedItem, ComboItem).Value) OrElse String.IsNullOrWhiteSpace(txtAmountPaid.Text) Then
            MessageBox.Show("Booking ID and Amount Paid are required.")
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
            {"@PaymentMethod", cmbPaymentMethod.Text},
            {"@ReferenceNumber", txtReference.Text},
            {"@PaymentStatus", cmbPaymentStatus.Text},
            {"@ProcessedBy", SessionInfo.LoggedInUserFullName},
            {"@PaymentTime", dtpPaymentTime.Value.ToString("HH:mm:ss")},
            {"@Balance", txtBalance.Text},
            {"@DiscountAmount", txtDiscount.Text},
            {"@RefundedAmount", txtRefunded.Text},
            {"@Remarks", txtRemarks.Text},
            {"@ORNumber", txtORNumber.Text}
        }

        If ExecuteQuery(query, parameters) Then
            MessageBox.Show("Payment recorded successfully.")
            LoadPayments()
            ClearFields()
        Else
            MessageBox.Show("Failed to record payment.")
        End If
    End Sub

    Private Sub LoadBookingDropdown()
        Dim query As String = "
        SELECT b.BookingID, c.FirstName, c.LastName, e.EventName, s.ServiceName 
        FROM booking b
        JOIN customer c ON b.CustomerID = c.CustomerID
        JOIN event e ON b.EventID = e.EventID
        JOIN service_availed s ON b.ServiceID = s.ServiceID"

        Dim dt As DataTable = GetData(query)

        cmbBookingID.Items.Clear()
        For Each row As DataRow In dt.Rows
            Dim bookingId = row("BookingID")
            Dim fullName = $"{row("FirstName")} {row("LastName")}"
            Dim eventName = Abbreviate(row("EventName").ToString())
            Dim serviceName = Abbreviate(row("ServiceName").ToString())
            Dim displayText = $"{fullName} - {eventName}/{serviceName}"
            cmbBookingID.Items.Add(New ComboItem(displayText, bookingId))
        Next
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
        cmbBookingID.SelectedIndex = -1
        cmbPaymentMethod.SelectedIndex = -1
        cmbPaymentStatus.SelectedIndex = -1
        dtpPaymentDate.Value = DateTime.Now
        dtpPaymentTime.Value = DateTime.Now
    End Sub
End Class
