Imports MySql.Data.MySqlClient

Module DatabaseHelper
    ' Set your MySQL connection string here
    Public connString As String = "host=127.0.0.1;user=root;password=;database=bms"

    ' Function to return a new connection
    Public Function GetConnection() As MySqlConnection
        Return New MySqlConnection(connString)
    End Function

    ' Execute SELECT queries and return as DataTable
    Public Function GetData(query As String, Optional parameters As Dictionary(Of String, Object) = Nothing) As DataTable
        Dim table As New DataTable()

        Using conn As MySqlConnection = GetConnection()
            Using cmd As New MySqlCommand(query, conn)
                If parameters IsNot Nothing Then
                    For Each param In parameters
                        cmd.Parameters.AddWithValue(param.Key, param.Value)
                    Next
                End If

                Try
                    conn.Open()
                    Using reader = cmd.ExecuteReader()
                        table.Load(reader)
                    End Using
                Catch ex As Exception
                    MessageBox.Show("Error loading data: " & ex.Message)
                End Try
            End Using
        End Using

        Return table
    End Function

    ' Execute INSERT, UPDATE, DELETE
    Public Function ExecuteQuery(query As String, Optional parameters As Dictionary(Of String, Object) = Nothing) As Boolean
        Using conn As MySqlConnection = GetConnection()
            Using cmd As New MySqlCommand(query, conn)
                If parameters IsNot Nothing Then
                    For Each param In parameters
                        cmd.Parameters.AddWithValue(param.Key, param.Value)
                    Next
                End If

                Try
                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return True
                Catch ex As Exception
                    MessageBox.Show("Query failed: " & ex.Message)
                    Return False
                End Try
            End Using
        End Using
    End Function
End Module
