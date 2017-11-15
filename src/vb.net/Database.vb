'*
' Original file made by http://www.github.com/blockba5her
' File OPEN DOMAIN (No license)
' Free for public/private use in applications
'*

Imports System
Imports System.Data.SqlTypes
Imports System.IO
Imports System.IO.Compression
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary

Namespace EZDatabase
    ''' <summary>
    ''' Thread safe read and write to a file of serializable objects
    ''' </summary>
    Public NotInheritable Class Database
        ' The file to save the database stuff on
        Private file As String

        ''' <summary>
        ''' Initializes the class of <see cref="Database"/>
        ''' </summary>
        ''' <param name="file">The name of the file to save the data in</param>
        ''' <param name="createFile">Should the file be created if it doesn't already exist</param>
        Public Sub New(file As String, createFile As Boolean)
            Me.file = file
            If createFile Then
                SyncLock Me ' Locking so thread read/write
                    Call New FileStream(file, FileMode.OpenOrCreate).Dispose() ' Creating the database file (if not already created)
                End SyncLock
            End If
        End Sub

        ''' <summary>
        ''' Reads the information of the Datafile file, then returns the dynamic value
        ''' </summary>
        ''' <returns>Value of the database file</returns>
        Public Function Read() As Object
            SyncLock Me ' Locking so thread read/write
                Using compressed = New FileStream(file, FileMode.Open)
                    If Not compressed.CanWrite Or Not compressed.CanRead Then
                        Throw New IOException("Invalid permissions to read or write") ' Throw when perms bad
                    End If

                    Using uncompressed = New MemoryStream()
                        Using gzip = New GZipStream(compressed, CompressionMode.Decompress, True) ' creating decompresser stream
                            gzip.CopyTo(uncompressed) ' copy stream to the uncompressed stream for manipulation
                        End Using
                        uncompressed.Position = 0 ' Setting the position to 0 for deserialization
                        Return New BinaryFormatter().Deserialize(uncompressed) ' Deserialization of the bytes given
                    End Using
                End Using
            End SyncLock
        End Function

        ''' <summary>
        ''' Writes the data to the database file, from the starting point
        ''' </summary>
        ''' <param name="data">The data to write to the database</param>
        Public Sub Write(data As Object)
            SyncLock Me ' Locking so thread read/write
                If Not data.GetType().IsSerializable Then
                    Throw New SerializationException("The object trying to be serialized is not marked serializable", New NullReferenceException())
                End If

                Using fileStream = New FileStream(file, FileMode.Open)
                    If Not fileStream.CanRead Or Not fileStream.CanWrite Then
                        Throw New IOException("Invalid permissions to read or write") ' Throw when perms bad
                    End If

                    Dim bytes As Byte()
                    Using ms = New MemoryStream() ' Creating a memory stream for the bytes
                        Using gzip = New GZipStream(ms, CompressionMode.Compress) ' Creating a GZip stream for compression
                            Call New BinaryFormatter().Serialize(gzip, data) ' Serializing the bytes to the gzip stream

                            ' Closing the streams for read
                            gzip.Close()
                            ms.Close()

                            ' Setting the bytes in the class to the memorystream
                            bytes = ms.ToArray()
                        End Using
                    End Using

                    fileStream.Write(bytes, 0, bytes.Length) ' Writing the bytes to the stream, starting at 0
                End Using
            End SyncLock
        End Sub
    End Class
End Namespace