Imports System.Data
Imports MySql.Data.MySqlClient

Public Class AccountantRefundControl
    Inherits UserControl

    Private dgvRefunds As DataGridView
    Private txtPaymentID, txtBookingID, txtRefundAmount, txtDiscountAmount, txtRemarks As TextBox
    Private btnProcess As Button

    Private Sub AccountantRefundControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public Sub New()
        InitializeComponent()
        SetupLayout()
        LoadRefunds()
    End Sub

    Private Sub SetupLayout()
        dgvRefunds = New DataGridView With {
            .Dock = DockStyle.Top,
            .Height = 250,
            .ReadOnly = True,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AllowUserToAddRows = False
        }
        AddHandler dgvRefunds.CellClick, AddressOf dgvRefunds_CellClick


        txtPaymentID = New TextBox()
        txtBookingID = New TextBox()
        txtRefundAmount = New TextBox()
        txtDiscountAmount = New TextBox()
        txtRemarks = New TextBox()
        txtPaymentID.ReadOnly = True
        txtBookingID.ReadOnly = True

        btnProcess = New Button With {.Text = "Apply Refund/Discount"}
        AddHandler btnProcess.Click, AddressOf btnProcess_Click

        ' Layout
        Dim layout As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 2,
            .RowCount = 7,
            .Padding = New Padding(10)
        }
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30))
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 70))

        layout.Controls.Add(New Label With {.Text = "Payment ID"}, 0, 0)
        layout.Controls.Add(txtPaymentID, 1, 0)

        layout.Controls.Add(New Label With {.Text = "Booking ID"}, 0, 1)
        layout.Controls.Add(txtBookingID, 1, 1)

        layout.Controls.Add(New Label With {.Text = "Refunded Amount"}, 0, 2)
        layout.Controls.Add(txtRefundAmount, 1, 2)

        layout.Controls.Add(New Label With {.Text = "Discount Amount"}, 0, 3)
        layout.Controls.Add(txtDiscountAmount, 1, 3)

        layout.Controls.Add(New Label With {.Text = "Remarks"}, 0, 4)
        layout.Controls.Add(txtRemarks, 1, 4)

        layout.Controls.Add(btnProcess, 1, 6)

        Me.Controls.Add(layout)
        Me.Controls.Add(dgvRefunds)
    End Sub

    Private Sub LoadRefunds()
        Dim query As String = "
            SELECT 
                PaymentID, BookingID, AmountPaid, RefundedAmount, DiscountAmount, Remarks, PaymentDate, ProcessedBy 
            FROM payment 
            WHERE RefundedAmount > 0 OR DiscountAmount > 0 
            ORDER BY PaymentDate DESC"
        dgvRefunds.DataSource = GetData(query)
    End Sub

    Private Sub btnProcess_Click(sender As Object, e As EventArgs)
        If dgvRefunds.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a payment record to apply refund/discount.")
            Return
        End If

        Dim selectedRow As DataGridViewRow = dgvRefunds.SelectedRows(0)
        Dim paymentID As Object = selectedRow.Cells("PaymentID").Value
        Dim bookingID As Object = selectedRow.Cells("BookingID").Value

        Dim refundAmount As Decimal = 0
        Dim discountAmount As Decimal = 0
        Decimal.TryParse(txtRefundAmount.Text, refundAmount)
        Decimal.TryParse(txtDiscountAmount.Text, discountAmount)

        Dim query As String = "
        UPDATE payment 
        SET RefundedAmount = @RefundedAmount, DiscountAmount = @DiscountAmount, Remarks = @Remarks, ProcessedBy = @ProcessedBy 
        WHERE PaymentID = @PaymentID"

        Dim parameters As New Dictionary(Of String, Object) From {
        {"@RefundedAmount", refundAmount},
        {"@DiscountAmount", discountAmount},
        {"@Remarks", txtRemarks.Text},
        {"@ProcessedBy", SessionInfo.LoggedInUserFullName},
        {"@PaymentID", paymentID}
    }

        If ExecuteQuery(query, parameters) Then
            MessageBox.Show("Refund/Discount processed.")
            LoadRefunds()
            ClearFields()
        Else
            MessageBox.Show("Failed to process refund/discount.")
        End If
    End Sub

    Private Sub dgvRefunds_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 AndAlso dgvRefunds.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = dgvRefunds.Rows(e.RowIndex)

            txtPaymentID.Text = selectedRow.Cells("PaymentID").Value.ToString()
            txtBookingID.Text = selectedRow.Cells("BookingID").Value.ToString()
            txtRefundAmount.Text = selectedRow.Cells("RefundedAmount").Value.ToString()
            txtDiscountAmount.Text = selectedRow.Cells("DiscountAmount").Value.ToString()
            txtRemarks.Text = selectedRow.Cells("Remarks").Value.ToString()
        End If
    End Sub



    Private Sub ClearFields()
        txtPaymentID.Clear()
        txtBookingID.Clear()
        txtRefundAmount.Clear()
        txtDiscountAmount.Clear()
        txtRemarks.Clear()
    End Sub
End Class
